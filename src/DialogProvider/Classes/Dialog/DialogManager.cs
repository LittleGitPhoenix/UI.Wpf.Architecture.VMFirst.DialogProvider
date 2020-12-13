#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
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

		private readonly WrappingViewProvider _wrappingViewProvider;

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
			: this(dialogAssemblyViewProvider, new WrappingViewProvider(viewProviders), true) { }

		/// <summary>
		/// Constructor for creating a new instance based on another one. It is called by <see cref="DialogManagerViewModelHelper.SetupViewModel"/> and prevents attaching event handlers to the internal <see cref="_wrappingViewProvider"/> again. />
		/// </summary>
		/// <param name="other"> The other <see cref="DialogManager"/> instance. </param>
		internal DialogManager(DialogManager other)
			: this(other._dialogAssemblyViewProvider, other._wrappingViewProvider, false) { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dialogAssemblyViewProvider"> An instance of a <see cref="DialogAssemblyViewProvider"/> used to resolve views for the direct dialog view models. </param>
		/// <param name="wrappingViewProvider"> The <see cref="WrappingViewProvider"/>s used to resolve views for content view models. </param>
		/// <param name="attachToViewProviderEvent"> Flag if an event handler should be attached to the <see cref="WrappingViewProvider.ViewLoaded"/>event of <paramref name="wrappingViewProvider"/>. </param>
		private DialogManager(DialogAssemblyViewProvider dialogAssemblyViewProvider, WrappingViewProvider wrappingViewProvider, bool attachToViewProviderEvent)
		{
			// Save parameters.
			_dialogAssemblyViewProvider = dialogAssemblyViewProvider ?? throw new ArgumentNullException(nameof(dialogAssemblyViewProvider));

			// Initialize fields.
			_isInitialized = 0;
			
			// If the wrapper does not contain any elements, that create one containing a default view provider.
			if (!wrappingViewProvider.Any()) wrappingViewProvider = new WrappingViewProvider(new IViewProvider[] {new DefaultViewProvider()});
			_wrappingViewProvider = wrappingViewProvider;
			
			// Attach to the view providers view loaded event.
			if (attachToViewProviderEvent) _wrappingViewProvider.ViewLoaded += this.NewViewLoaded;

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
			
			// Initialize all dialog handlers. Changes to the element will be made only by the handler, because the handler will wait for the element to be loaded and therefore it has more chances to find the related element if needed (e.g. The element will most certainly not have a Window if it is not yet loaded).
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

		#region View Loading

		/// <summary>
		/// This is called by <see cref="_wrappingViewProvider"/> every time a new view has been loaded.
		/// </summary>
		internal virtual void NewViewLoaded(object sender, ViewLoadedEventArgs args)
		{
			DialogManagerViewModelHelper.SetupViewModel(this, args.ViewModel, args.View);
		}

		#endregion

		#region Show
		
		/// <inheritdoc />
		public DialogTask ShowMessage
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
				messageDialogModel.ContentView = _wrappingViewProvider.GetViewInstance(messageDialogModel.ContentViewModel);
			}

			return this.Show(viewModel, _dialogAssemblyViewProvider, buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);
		}

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
			return this.Show(viewModel, _wrappingViewProvider, buttonConfigurations, displayLocation, displayBehavior, dialogOptions, cancellationToken);
		}

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
		
		/// <summary> Returns a string representation of the object. </summary>
		public override string ToString() => $"[<{this.GetType().Name}> :: Initialized: {this.IsInitialized} | DialogHandlers: {_dialogHandlers.Count}]";

		#endregion
	}
}