#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels;
using Phoenix.UI.Wpf.Architecture.VMFirst.ViewProvider;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Manager responsible for showing dialogs.
	/// </summary>
	public class DialogManager : IDialogManager
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		private int _isInitialized;

		private readonly DialogAssemblyViewProvider _dialogAssemblyViewProvider;

		private readonly WrappingViewProvider _viewProvider;

		private readonly DialogHandlers _dialogHandlers;

		#endregion

		#region Properties

		/// <inheritdoc />
		public bool IsInitialized => _dialogHandlers.All(handler => handler.IsInitialized);

		#endregion

		#region (De)Constructors
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dialogAssemblyViewProvider"> An instance of a <see cref="DialogAssemblyViewProvider"/> used to resolve views for the direct dialog view models. </param>
		/// <param name="viewProviders"> Optional <see cref="IViewProvider"/>s used to resolve views for content view models. Default is only a <see cref="DefaultViewProvider"/>. </param>
		public DialogManager(DialogAssemblyViewProvider dialogAssemblyViewProvider, params IViewProvider[] viewProviders)
		{
			// Save parameters.
			_dialogAssemblyViewProvider = dialogAssemblyViewProvider ?? throw new ArgumentNullException(nameof(dialogAssemblyViewProvider));

			// Initialize fields.
			_isInitialized = 0;

			// Create a default view provider if no others are specified.
			if (!viewProviders?.Any() ?? true)
			{
				var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
				var defaultViewProvider = new DefaultViewProvider(setupCallback);
				viewProviders = new IViewProvider[] {defaultViewProvider};
			}

			// Save the view providers into a wrapper instance.
			_viewProvider = new WrappingViewProvider(viewProviders);
			
			// Build all necessary dialog handlers.
			var dialogHandlers = DialogManager.GetDialogHandlers(_dialogAssemblyViewProvider);
			_dialogHandlers = new DialogHandlers(dialogHandlers);
		}

		#endregion

		#region Methods

		#region Initialization

		/// <inheritdoc />
		public IDialogManager Initialize(FrameworkElement view)
		{
			// Allow initialization only once.
			if (Interlocked.CompareExchange(ref _isInitialized, 1, 0) == 1) return this;
			
			// Initialize all dialog handlers. Changes to the element will be made only by the handler, because the handler will wait for the element to be loaded and therefore it has more chances to find related element if needed (e.g. The element will most certainly not have a Window if it is not yet loaded).
			_dialogHandlers.ForEach(handler => handler.Initialize(view));
			return this;
		}

		/// <summary>
		/// Creates <see cref="DialogHandler"/>s for every member in <see cref="DialogDisplayLocation"/>.
		/// </summary>
		/// <param name="dialogAssemblyViewProvider"> An instance of a <see cref="DialogAssemblyViewProvider"/> used to resolve views for the direct dialog view models. </param>
		/// <returns> A collection of <see cref="DialogHandler"/>s. </returns>
		private static DialogHandler[] GetDialogHandlers(DialogAssemblyViewProvider dialogAssemblyViewProvider)
		{
			return Enum
				.GetValues(typeof(DialogDisplayLocation))
				.Cast<DialogDisplayLocation>()
				.Select(displayLocation => DialogManager.CreateDialogHandler(displayLocation, dialogAssemblyViewProvider))
				.ToArray()
				;
		}

		/// <summary>
		/// Creates a new instance of a <see cref="DialogHandler"/> based on <paramref name="displayLocation"/>.
		/// </summary>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> used later on to identify the <see cref="DialogHandler"/>. </param>
		/// <param name="dialogAssemblyViewProvider"> An instance of a <see cref="DialogAssemblyViewProvider"/> used to resolve views for the direct dialog view models. </param>
		/// <returns> A new <see cref="DialogHandler"/> instance. </returns>
		private static DialogHandler CreateDialogHandler(DialogDisplayLocation displayLocation, DialogAssemblyViewProvider dialogAssemblyViewProvider)
		{
			return new DialogHandler(displayLocation, dialogAssemblyViewProvider);
		}

		#endregion

		#region Messages

		/// <inheritdoc />
		public DialogTask ShowMessage
		(
			string title = null,
			string message = null,
			object contentViewModel = null,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowMessage(new MessageDialogModel(identifier: null, title: title, message: message, contentViewModel: contentViewModel), buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowMessage
		(
			MessageDialogModel messageModel,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowMessage(new[] {messageModel}, buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowMessage
		(
			ICollection<MessageDialogModel> messageModels,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowMessage(messageModels, DefaultButtonConfigurations.GetConfiguration(buttons), displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowMessage
		(
			ICollection<MessageDialogModel> messageModels,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowMessage(new MessageDialogViewModel(messageModels), buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);
		
		private DialogTask ShowMessage
		(
			MessageDialogViewModel viewModel,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation,
			DialogDisplayBehavior displayBehavior,
			DialogOptions dialogOptions,
			CancellationToken cancellationToken = default
		)
		{
			// Try to resolve the content views of all message models if necessary.
			foreach (var messageDialogModel in viewModel.MessageModels.Where(model => model.ContentViewModel != null))
			{
				messageDialogModel.ContentView = _viewProvider.GetViewInstance(messageDialogModel.ContentViewModel);
			}

			return this.Show(viewModel, _dialogAssemblyViewProvider, buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);
		}

		#endregion

		#region Warnings

		/// <inheritdoc />
		public DialogTask ShowWarning
		(
			string title = null,
			string message = null,
			object contentViewModel = null,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowWarning(new MessageDialogModel(identifier: null, title: title, message: message, contentViewModel: contentViewModel), buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowWarning
		(
			MessageDialogModel messageModel,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowWarning(new[] { messageModel }, buttons, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowWarning
		(
			ICollection<MessageDialogModel> messageModels,
			DialogButtons buttons = DialogButtons.Ok,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowWarning(messageModels, DefaultButtonConfigurations.GetConfiguration(buttons), displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowWarning
		(
			ICollection<MessageDialogModel> messageModels,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowMessage(new WarningDialogViewModel(messageModels), buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		#endregion

		#region Exceptions

		/// <inheritdoc />
		public DialogTask ShowException
		(
			string title = null,
			string message = null,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowExceptions(new Exception[0], title, message, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowException
		(
			Exception exception,
			string title = null,
			string message = null,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
			=> this.ShowExceptions(exception is AggregateException aggregateException ? aggregateException.InnerExceptions.ToArray() : new[] {exception}, title, message, displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowExceptions
		(
			ICollection<Exception> exceptions,
			string title = null,
			string message = null,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.None,
			CancellationToken cancellationToken = default
		)
		{
			var viewModel = new ExceptionDialogViewModel(title, message, exceptions);
			var buttonConfigurations = new[] {DefaultButtonConfigurations.OkButtonConfiguration};
			return this.Show(viewModel, _dialogAssemblyViewProvider, buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);
		}

		#endregion

		#region Content

		/// <inheritdoc />
		public DialogTask ShowContent
		(
			object viewModel,
			DialogButtons buttons = DialogButtons.None,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.HideTransparencyToggle,
			CancellationToken cancellationToken = default
		)
			=> this.ShowContent(viewModel, DefaultButtonConfigurations.GetConfiguration(buttons), displayLocation, displayBehavior, dialogOptions, cancellationToken);

		/// <inheritdoc />
		public DialogTask ShowContent
		(
			object viewModel,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation = DialogDisplayLocation.Window,
			DialogDisplayBehavior displayBehavior = DialogDisplayBehavior.Show,
			DialogOptions dialogOptions = DialogOptions.HideTransparencyToggle,
			CancellationToken cancellationToken = default
		)
		{
			return this.Show(viewModel, _viewProvider, buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);
		}

		#endregion

		#region Close

		/// <inheritdoc />
		public void Revoke(DialogResult dialogResult)
		{
			Enum
				.GetValues(typeof(DialogDisplayLocation))
				.Cast<DialogDisplayLocation>()
				.ToList()
				.ForEach(location => this.Revoke(location, dialogResult))
				;
		}

		/// <inheritdoc />
		public void Revoke(DialogDisplayLocation displayLocation, DialogResult dialogResult)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				// Find the matching handler for the display location.
				var dialogHandler = _dialogHandlers.GetDialogHandler(displayLocation);

				// Revoke the currently displayed view.
				dialogHandler.Revoke(dialogResult);
			});
		}

		/// <inheritdoc />
		public void Close(DialogResult dialogResult)
		{
			Enum
				.GetValues(typeof(DialogDisplayLocation))
				.Cast<DialogDisplayLocation>()
				.ToList()
				.ForEach(location => this.Close(location, dialogResult))
				;
		}

		/// <inheritdoc />
		public void Close(DialogDisplayLocation displayLocation, DialogResult dialogResult)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				// Find the matching handler for the display location.
				var dialogHandler = _dialogHandlers.GetDialogHandler(displayLocation);
				
				// Close the dialog.
				dialogHandler.Close(dialogResult);
			});
		}

		#endregion

		#region Helper

		private DialogTask Show
		(
			object viewModel,
			IViewProvider viewProvider,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayLocation displayLocation,
			DialogDisplayBehavior displayBehavior,
			DialogOptions dialogOptions,
			CancellationToken cancellationToken
		)
		{
			if (cancellationToken.IsCancellationRequested) return DialogTask.Killed;

			DialogTask dialogTask = null;

			Application.Current.Dispatcher.Invoke(() =>
			{
				var view = viewProvider.GetViewInstance(viewModel);

				// Find the matching handler for the display location.
				var dialogHandler = _dialogHandlers.GetDialogHandler(displayLocation);

				// Tell it to show the content.
				dialogTask = dialogHandler.ShowView(view, buttonConfigurations, displayBehavior, dialogOptions, cancellationToken);
			});

			if (dialogTask is null) return null;

			if (cancellationToken.IsCancellationRequested) return DialogTask.Killed;

			dialogTask.Start();
			return dialogTask;
		}

		#endregion

		/// <summary> Returns a string representation of the object. </summary>
		public override string ToString() => $"[<{this.GetType().Name}> :: Initialized: {this.IsInitialized} | DialogHandlers: {_dialogHandlers.Count}]";

		#endregion
	}
}