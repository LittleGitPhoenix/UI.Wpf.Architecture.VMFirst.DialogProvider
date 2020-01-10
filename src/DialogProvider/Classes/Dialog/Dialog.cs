#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Phoenix.UI.Wpf.DialogProvider.Models;
using Phoenix.UI.Wpf.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Wrapper class that holds all necessary information for a dialog. Instance of this class are handled by <see cref="DialogHandler"/>.
	/// </summary>
	internal class Dialog
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		private readonly Action<DialogResult> _closeCallback;

		private readonly CancellationTokenSource _cancellationTokenSource;

		private readonly DialogTaskFactory _dialogTaskFactory;

		/// <summary> The <see cref="CancellationTokenRegistration"/> used to remove the revoke callback from the <see cref="ExternalCancellationToken"/> if the dialog was closed normally. </summary>
		private CancellationTokenRegistration? _tokenRegistration;

		#endregion

		#region Properties

		public Visual ContentView { get; }

		public ICollection<Button> Buttons { get; }
		
		public DialogOptions Options { get; }

		/// <summary> An external <see cref="CancellationToken"/> that can revoke the dialog. </summary>
		public CancellationToken ExternalCancellationToken { get; }

		public DialogTask DialogTask { get; }

		#endregion

		#region (De)Constructors

		public Dialog
		(
			Visual contentView,
			ICollection<Button> buttons,
			Action<DialogResult> closeCallback,
			DialogOptions options,
			CancellationToken externalCancellationToken
		)
		{
			// Save parameters.
			_closeCallback = closeCallback;
			this.ContentView = contentView;
			this.Buttons = buttons;
			this.Options = options;
			this.ExternalCancellationToken = externalCancellationToken;

			// Initialize fields.
			_cancellationTokenSource = new CancellationTokenSource();
			_dialogTaskFactory = new DialogTaskFactory();
			this.DialogTask = _dialogTaskFactory.Create(closeCallback, _cancellationTokenSource.Token);
		}

		#endregion

		#region Methods

		internal void PrepareBeforeShown()
		{
			this.TryAddCloseCallback();
			this.TryAddExternalCancellationCallback();
		}

		internal void Cancel(DialogResult dialogResult)
		{
			this.TryRemoveCloseCallback();
			this.TryRemoveExternalCancellationCallback();

			_dialogTaskFactory.Result = dialogResult;

			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}

		#region Revoke Forwarding

		/// <summary>
		/// Request the dialog to be revoked by invoking the internal <see cref="_closeCallback"/>.
		/// </summary>
		/// <param name="result"> The result as a <see cref="bool"/> that will be converted to either <see cref="DialogResult.Yes"/> or <see cref="DialogResult.No"/>. </param>
		internal void RequestRevoke(bool result)
			=> this.RequestRevoke(result ? DialogResult.Yes : DialogResult.No);

		/// <summary>
		/// Request the dialog to be revoked by invoking the internal <see cref="_closeCallback"/>.
		/// </summary>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> of this dialog. </param>
		internal void RequestRevoke(DialogResult dialogResult)
			=> _closeCallback.Invoke(dialogResult);

		#endregion

		#region ViewModel Close Callback

		/// <summary>
		/// Tries to add the <see cref="RequestRevoke(DialogResult)"/> or <see cref="RequestRevoke(bool)"/> method to a <see cref="ICloseableViewModel.RequestClose"/> callback in the <see cref="ContentView"/>s view model.
		/// </summary>
		private void TryAddCloseCallback()
		{
			if (!(this.ContentView is FrameworkElement frameworkElement))
			{
				Trace.WriteLine($"{this.GetType().Name.ToUpper()}: No close callback found because the view '{this.ContentView}' is not a {nameof(FrameworkElement)} and therefore has no {nameof(FrameworkElement.DataContext)}.");
				return;
			}

			frameworkElement.Dispatcher.Invoke(() =>
			{
				// Get the view model.
				var viewModel = frameworkElement.DataContext;
				if (viewModel is null)
				{
					Trace.WriteLine($"{this.GetType().Name.ToUpper()}: No close callback found because the view '{this.ContentView}'s {nameof(FrameworkElement.DataContext)} is null.");
					return;
				}

				try
				{
					if (viewModel is ICloseableViewModel closeableViewModel)
					{
						Trace.WriteLine($"{this.GetType().Name.ToUpper()}: View model '{closeableViewModel}' is {nameof(ICloseableViewModel)}, close callback will be set.");
						closeableViewModel.RequestClose = this.RequestRevoke;
					}
					else
					{
						var closeCallbackProperty = viewModel.GetType().GetProperty(nameof(ICloseableViewModel.RequestClose), BindingFlags.Public | BindingFlags.Instance);
						if (closeCallbackProperty != null)
						{
							var propertyType = closeCallbackProperty.PropertyType;
							if (typeof(Action<DialogResult>).IsAssignableFrom(propertyType))
							{
								Trace.WriteLine($"{this.GetType().Name.ToUpper()}: View model '{viewModel}' has proper '{nameof(ICloseableViewModel.RequestClose)}' property, close callback will be set.");
								closeCallbackProperty.SetValue(viewModel, (Action<DialogResult>) this.RequestRevoke);
							}
							else if (typeof(Action<bool>).IsAssignableFrom(propertyType))
							{
								Trace.WriteLine($"{this.GetType().Name.ToUpper()}: View model '{viewModel}' has proper '{nameof(ICloseableViewModel.RequestClose)}' property, close callback will be set.");
								closeCallbackProperty?.SetValue(viewModel, (Action<bool>) this.RequestRevoke);
							}
						}
						else
						{
							Trace.WriteLine($"{this.GetType().Name.ToUpper()}: No close callback found for view model '{viewModel}'.");
						}
					}
				}
				catch
				{
					/* ignore */
				}
			});
		}

		/// <summary>
		/// Tries to remove the <see cref="ICloseableViewModel.RequestClose"/> callback from the <see cref="ContentView"/>s view model
		/// </summary>
		private void TryRemoveCloseCallback()
		{
			if (!(this.ContentView is FrameworkElement frameworkElement)) return;

			try
			{
				frameworkElement.Dispatcher.Invoke(() =>
				{
					var viewModel = frameworkElement.DataContext;
					if (viewModel is ICloseableViewModel closeableViewModel)
					{
						closeableViewModel.RequestClose = null;
					}
					else
					{
						var closeCallbackProperty = viewModel.GetType().GetProperty(nameof(ICloseableViewModel.RequestClose), BindingFlags.Public | BindingFlags.Instance);
						closeCallbackProperty?.SetValue(viewModel, null);
					}
				});
			}
			catch { /* ignore */ }
		}

		#endregion
		
		#region External Cancellation Token Revoking

		/// <summary>
		/// Links the <see cref="ExternalCancellationToken"/> of this dialog to the <see cref="RequestRevoke(DialogResult)"/> method.
		/// </summary>
		private void TryAddExternalCancellationCallback()
		{
			_tokenRegistration = this.ExternalCancellationToken.CanBeCanceled
				? this.ExternalCancellationToken.Register(() => this.RequestRevoke(DialogResult.Killed))
				: (CancellationTokenRegistration?) null;
		}

		/// <summary>
		/// Removes the callback for the <see cref="ExternalCancellationToken"/> of this dialog.
		/// </summary>
		private void TryRemoveExternalCancellationCallback()
		{
			_tokenRegistration?.Dispose();
		}

		#endregion

		#endregion
	}
}