#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
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
			CloseableDialogContentViewModelHelper.TryAddCloseCallback(this.ContentView as FrameworkElement, _closeCallback);
			OptionsAwareDialogContentViewModelHelper.TryAddDialogOptions(this.ContentView as FrameworkElement, this.Options);
			this.TryAddExternalCancellationCallback();
		}

		internal void Cancel(DialogResult dialogResult)
		{
			CloseableDialogContentViewModelHelper.TryRemoveCloseCallback(this.ContentView as FrameworkElement);
			OptionsAwareDialogContentViewModelHelper.TryRemoveDialogOptions(this.ContentView as FrameworkElement);
			this.TryRemoveExternalCancellationCallback();

			_dialogTaskFactory.Result = dialogResult;

			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}

		#region External Cancellation Token Revoking

		/// <summary>
		/// Links the <see cref="ExternalCancellationToken"/> of this dialog to the <see cref="_closeCallback"/>.
		/// </summary>
		private void TryAddExternalCancellationCallback()
		{
			_tokenRegistration = this.ExternalCancellationToken.CanBeCanceled
				? this.ExternalCancellationToken.Register(() => _closeCallback(DialogResult.Killed))
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