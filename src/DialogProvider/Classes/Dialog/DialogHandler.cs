using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Phoenix.UI.Wpf.DialogProvider.ViewModels;
using Button = Phoenix.UI.Wpf.DialogProvider.Models.Button;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Handler for showing dialogs utilizing <see cref="Adorner"/>s.
	/// </summary>
	class DialogHandler
	{
		#region Delegates / Events

		private event EventHandler Initialized;
		private void OnInitialized()
		{
			this.IsInitialized = true;
			this.Initialized?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Constants
		#endregion

		#region Fields
		
		/// <summary> The original registered <see cref="UIElement"/>. If this is a <see cref="Window"/>, the <see cref="_adornedElement"/> will be its content. </summary>
		private UIElement _originalElement;

		/// <summary> The real <see cref="UIElement"/> that will be adorned. </summary>
		private UIElement _adornedElement;

		/// <summary> The <see cref="_adornerLayer"/> of the <see cref="_adornedElement"/>. </summary>
		private AdornerLayer _adornerLayer;

		/// <summary> The <see cref="_contentAdorner"/> that will be displayed. </summary>
		private ContentAdorner _contentAdorner;

		/// <summary> The <see cref="IDialogContainerViewModel"/> that is the data context of the <see cref="_contentAdorner"/>s <see cref="ContentAdorner.Content"/>. This must be used to set the real content of the dialog. </summary>
		private readonly IDialogContainerViewModel _containerViewModel;

		/// <summary> The view for the <see cref="_containerViewModel"/>. </summary>
		private readonly FrameworkElement _containerView;

		/// <summary> Queue of all content views. </summary>
		private readonly ConcurrentStack<Dialog> _dialogs;

		#endregion

		#region Properties

		/// <summary> Flag if this instance is fully initialized. </summary>
		internal bool IsInitialized { get; private set; }

		/// <summary> The <see cref="DialogDisplayLocation"/> of this <see cref="DialogHandler"/>. </summary>
		public DialogDisplayLocation DisplayLocation { get; }

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="displayLocation"> The <see cref="DialogDisplayLocation"/> of this <see cref="DialogHandler"/>. </param>
		/// <param name="dialogViewProvider"> <see cref="DialogAssemblyViewProvider"/> used to obtain the proper view for an <see cref="IDialogContainerViewModel"/>. </param>
		public DialogHandler
		(
			DialogDisplayLocation displayLocation,
			DialogAssemblyViewProvider dialogViewProvider
		)
		{
			// Save parameters.
			this.DisplayLocation = displayLocation;
			_containerViewModel = new DialogContainerViewModel();

			// Initialize fields.
			_containerView = dialogViewProvider.GetViewInstance(_containerViewModel);
			_dialogs = new ConcurrentStack<Dialog>();
			this.IsInitialized = false;
		}

		#endregion

		#region Methods

		#region Initialization

		/// <summary>
		/// Initializes the current instance with the framework <paramref name="element"/> needed to display a dialog.
		/// </summary>
		/// <param name="element"> The <see cref="Visual"/> that will be used to display a dialog. This may be changed depending on the <see cref="DisplayLocation"/> of this <see cref="DialogHandler"/>. </param>
		internal void Initialize(FrameworkElement element)
		{
			// Save the original element.
			_originalElement = element;

			// Further initialize the instance as soon as the element is loaded.
			void LoadedHandler(object sender, RoutedEventArgs args)
			{
				element.Loaded -= LoadedHandler;

				// Change the element accordion to the display location.
				FrameworkElement changedElement;
				switch (this.DisplayLocation)
				{
					case DialogDisplayLocation.Window:
						changedElement = element is Window ? element : Window.GetWindow(element);
						break;

					default:
					case DialogDisplayLocation.Self:
						changedElement = element;
						break;
				}

				this.Initialize(changedElement, _containerView, _containerViewModel);
			}
			element.Loaded += LoadedHandler;
			if (element.IsLoaded)
			{
				LoadedHandler(null, new RoutedEventArgs());
			}
		}

		/// <summary>
		/// Initializes the current instance once the original framework <paramref name="element"/> has been loaded.
		/// </summary>
		/// <param name="element"> The <see cref="Visual"/> that will be used to display a dialog. </param>
		/// <param name="dialogContainerView"> The view for the <paramref name="dialogContainerViewModel"/>. </param>
		/// <param name="dialogContainerViewModel"> The <see cref="IDialogContainerViewModel"/> view model. </param>
		private void Initialize(Visual element, Visual dialogContainerView, IDialogContainerViewModel dialogContainerViewModel)
		{
			// Get the adorner layer.
			_adornerLayer = DialogHandler.GetAdornerLayer(element, out var adornedVisual);
			
			// Check if the really adorned visual is of a UIElement.
			if (!(adornedVisual is UIElement adornedElement)) throw new DialogException($"The adorned element '{adornedVisual}' is not of type '{nameof(UIElement)}'");

			// Save the adorned element.
			_adornedElement = adornedElement;

			// Create a content adorner.
			_contentAdorner = new ContentAdorner(adornedElement, dialogContainerView);
			
			// Modify the dialog container view.
			if (dialogContainerView is FrameworkElement dialogContainerElement)
			{
				// Make the dialog container view focusable, so that key events will be received.
				dialogContainerElement.Focusable = true;

				// Handle the loaded event of the dialog container view.
				void LoadedHandler(object sender, RoutedEventArgs args)
				{
					dialogContainerElement.Loaded -= LoadedHandler;
					
					// Focus the dialog container view once it is loaded.
					dialogContainerElement.Focus();
					
					// Attach key up events from the view to the handler in the view model.
					dialogContainerElement.PreviewKeyUp -= dialogContainerViewModel.HandleKeyUp;
					dialogContainerElement.PreviewKeyUp += dialogContainerViewModel.HandleKeyUp;
				}
				dialogContainerElement.Loaded += LoadedHandler;
				if (dialogContainerElement.IsLoaded)
				{
					LoadedHandler(null, new RoutedEventArgs());
				}
			}

			this.OnInitialized();
		}

		#endregion

		#region Content Handling

		/// <summary>
		/// Shows the <paramref name="contentView"/>.
		/// </summary>
		/// <param name="contentView"> The <see cref="Visual"/> to show. </param>
		/// <param name="buttonConfigurations"> The configurations for <see cref="Models.Button"/>s to show. </param>
		/// <param name="displayBehavior"> The <see cref="DialogDisplayBehavior"/> for showing the <paramref name="contentView"/>. </param>
		/// <param name="dialogOptions"> <see cref="DialogOptions"/> for the new <see cref="Dialog"/>. </param>
		/// <param name="cancellationToken"> An external <see cref="CancellationToken"/> used to cancel the dialog. </param>
		/// <returns> The <see cref="DialogTask"/> used to interact and await the shown view. </returns>
		internal DialogTask ShowView
		(
			Visual contentView,
			IEnumerable<ButtonConfiguration> buttonConfigurations,
			DialogDisplayBehavior displayBehavior,
			DialogOptions dialogOptions,
			CancellationToken cancellationToken
		)
		{
			if (cancellationToken.IsCancellationRequested) return DialogTask.Killed;

			// If necessary, handle the currently displayed view.
			switch (displayBehavior)
			{
				case DialogDisplayBehavior.Show:
					break;
				case DialogDisplayBehavior.Override:
					this.RemoveTopMostView(DialogResult.Killed);
					break;
			}

			// Create buttons from the configuration.
			var buttons = buttonConfigurations
				.Select(configuration => new Models.Button(configuration, this.Revoke))
				.ToList()
				;

			// Create a new dialog.
			var dialog = new Dialog
			(
				contentView: contentView,
				buttons: buttons,
				closeCallback: this.Revoke,
				options: dialogOptions,
				externalCancellationToken: cancellationToken
			);

			if (cancellationToken.IsCancellationRequested) return DialogTask.Killed;
			
			// Save it.
			_dialogs.Push(dialog);

			// Show it.
			this.ShowView();
			
			// Return it.
			return dialog.DialogTask;
		}

		/// <summary>
		/// Shows the next content view as soon as possible.
		/// </summary>
		private void ShowView()
		{
			void ShowNextView()
			{
				if (_adornerLayer is null)
				{
					Trace.WriteLine($"{this.GetType().Name.ToUpper()}: Cannot show any dialog because the underlying {nameof(AdornerLayer)} is NULL. One possible reason would be if the adorned element is a {nameof(Window)} that has no content. Consider using a simple {nameof(System.Windows.Controls.Grid)} as the windows content.");
					return;
				}

				// Get the next available dialog.
				var dialog = this.GetNextDialog();

				// Stop if no new dialog is available.
				if (dialog is null) return;

				dialog.PrepareBeforeShown();

				//// Add a callback for closing to the view model if possible.
				//this.TryAddCloseCallback(dialog.ContentView);

				//// Link the external cancellation token to the revoke method, so that the dialog gets closed alongside the cancellation token.
				//this.TryAddExternalCancellationCallback(dialog);

				// Change the dialog container according to the new dialog.
				_containerViewModel.ContentView = dialog.ContentView;
				_containerViewModel.Buttons = dialog.Buttons;
				_containerViewModel.ShowTransparencyToggle = !dialog.Options.HasFlag(DialogOptions.HideTransparencyToggle);

				// Add the adorner to the layer (but only once).
				var availableAdorners = _adornerLayer.GetAdorners(_adornedElement);
				if (!availableAdorners?.Contains(_contentAdorner) ?? true) _adornerLayer.Add(_contentAdorner);
			}

			// Show the view as soon as this instance is fully initialized.
			void InitializedHandler(object sender, EventArgs args)
			{
				this.Initialized -= InitializedHandler;
				ShowNextView();
			}
			this.Initialized += InitializedHandler;
			if (this.IsInitialized)
			{
				InitializedHandler(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets the next available <see cref="Dialog"/> from the internal collection.
		/// </summary>
		/// <returns> The next available <see cref="Dialog"/>. </returns>
		private Dialog GetNextDialog()
		{
			Dialog dialog;
			do
			{
				_dialogs.TryPeek(out dialog);
				if (dialog.ExternalCancellationToken.IsCancellationRequested)
				{
					Trace.WriteLine($"{this.GetType().Name.ToUpper()}: The next dialog has an external {nameof(CancellationToken)} that is already canceled. Therefore the dialog won't be shown.");
					this.RemoveTopMostView(DialogResult.Killed);
					dialog = null;
				}

			} while (dialog is null && _dialogs.Any());

			return dialog;
		}

		/// <summary>
		/// Removes all content views essentially closing the dialog.
		/// </summary>
		/// <remarks> The top most will get the specified <paramref name="dialogResult"/> and all others <see cref="DialogResult.Killed"/>. </remarks>
		internal void Close(DialogResult dialogResult)
		{
			bool isFirst = true;
			while (!_dialogs.IsEmpty)
			{
				// 
				this.Revoke(isFirst? dialogResult : DialogResult.Killed);
				isFirst = false;
			}
		}

		///// <summary>
		///// Removes the top most content view and shows the next one. If no more content views are available, then the dialog will be closed.
		///// </summary>
		///// <param name="result"> The result as a <see cref="bool"/> that will be converted to either <see cref="DialogResult.Yes"/> or <see cref="DialogResult.No"/>. </param>
		//internal void Revoke(bool result)
		//	=> this.Revoke(result ? DialogResult.Yes : DialogResult.No);

		/// <summary>
		/// Removes the top most content view and shows the next one. If no more content views are available, then the dialog will be closed.
		/// </summary>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> of the revoked dialog. </param>
		internal void Revoke(DialogResult dialogResult)
		{
			this.RemoveTopMostView(dialogResult);

			if (_dialogs.IsEmpty)
			{
				_adornerLayer.Dispatcher.Invoke(() => _adornerLayer.Remove(_contentAdorner));
			}
			else
			{
				this.ShowView();
			}
		}

		/// <summary>
		/// Removes the top most content view.
		/// </summary>
		/// <param name="dialogResult"> The <see cref="DialogResult"/> for the removed view. </param>
		private void RemoveTopMostView(DialogResult dialogResult)
		{
			if (!_dialogs.TryPop(out var dialog)) return;

			//// Remove the close callback from the view model if possible.
			//DialogHandler.TryRemoveCloseCallback(dialog.ContentView);

			//// Remove the close callback fom the external cancellation token.
			//DialogHandler.TryRemoveExternalCancellationCallback(dialog);

			// Cancel the token linked to the view.
			dialog.Cancel(dialogResult);
		}

		#endregion

		#region Helper

		/// <summary>
		/// Gets the <see cref="AdornerLayer"/> for the <paramref name="visual"/>.
		/// </summary>
		/// <param name="visual"> The <see cref="Visual"/> whose <see cref="AdornerLayer"/> to get. </param>
		/// <param name="adornedVisual"> If <paramref name="visual"/> was changed in order to get a proper <see cref="AdornerLayer"/>, then this will contain the new <see cref="Visual"/>, otherwise this will be the <paramref name="visual"/> itself. </param>
		/// <returns> The <see cref="AdornerLayer"/> or <c>Null</c>. </returns>
		/// <remarks> If the <paramref name="visual"/> is a <see cref="Window"/> then the <see cref="AdornerLayer"/> of its content will be returned. </remarks>
		public static AdornerLayer GetAdornerLayer(Visual visual, out Visual adornedVisual)
		{
			adornedVisual = visual;

			if (visual is AdornerDecorator decorator) return decorator.AdornerLayer;
			if (visual is System.Windows.Controls.ScrollContentPresenter presenter) return presenter.AdornerLayer;

			// Check if the visual is a window.
			if (visual is Window window)
			{
				if (window.Content is Visual content)
				{
					adornedVisual = content;
					return AdornerLayer.GetAdornerLayer(content);
				}
			}

			// Just try the normal method for retrieving an adorner layer.
			return AdornerLayer.GetAdornerLayer(visual);
		}

		///// <summary>
		///// Tries to add the internal <see cref="Revoke(DialogResult)"/> or <see cref="Revoke(bool)"/> method to a <see cref="ICloseableViewModel.RequestClose"/> callback in the <paramref name="view"/>s view model.
		///// </summary>
		///// <param name="view"> The view whose view model  that is examined for a close callback. </param>
		///// <returns> <c>True</c> on success, otherwise <c>False</c>. </returns>
		//private void TryAddCloseCallback(Visual view)
		//{
		//	if (!(view is FrameworkElement frameworkElement))
		//	{
		//		Trace.WriteLine($"{this.GetType().Name.ToUpper()}: No close callback found because the view '{view}' is not a {nameof(FrameworkElement)} and therefore has no {nameof(FrameworkElement.DataContext)}.");
		//		return;
		//	}

		//	frameworkElement.Dispatcher.Invoke(() =>
		//	{
		//		// Get the view model.
		//		var viewModel = frameworkElement.DataContext;
		//		if (viewModel is null)
		//		{
		//			Trace.WriteLine($"{this.GetType().Name.ToUpper()}: No close callback found because the view '{view}'s {nameof(FrameworkElement.DataContext)} is null.");
		//			return;
		//		}

		//		try
		//		{
		//			if (viewModel is ICloseableViewModel closeableViewModel)
		//			{
		//				Trace.WriteLine($"{this.GetType().Name.ToUpper()}: View model '{closeableViewModel}' is {nameof(ICloseableViewModel)}, close callback will be set.");
		//				closeableViewModel.RequestClose = this.Revoke;
		//			}
		//			else
		//			{
		//				var closeCallbackProperty = viewModel.GetType().GetProperty(nameof(ICloseableViewModel.RequestClose), BindingFlags.Public | BindingFlags.Instance);
		//				if (closeCallbackProperty != null)
		//				{

		//					var propertyType = closeCallbackProperty.PropertyType;
		//					if (typeof(Action<DialogResult>).IsAssignableFrom(propertyType))
		//					{
		//						Trace.WriteLine($"{this.GetType().Name.ToUpper()}: View model '{viewModel}' has proper '{nameof(ICloseableViewModel.RequestClose)}' property, close callback will be set.");
		//						closeCallbackProperty.SetValue(viewModel, (Action<DialogResult>) this.Revoke);
		//					}
		//					else if (typeof(Action<bool>).IsAssignableFrom(propertyType))
		//					{
		//						Trace.WriteLine($"{this.GetType().Name.ToUpper()}: View model '{viewModel}' has proper '{nameof(ICloseableViewModel.RequestClose)}' property, close callback will be set.");
		//						closeCallbackProperty?.SetValue(viewModel, (Action<bool>) this.Revoke);
		//					}
		//				}
		//				else
		//				{
		//					Trace.WriteLine($"{this.GetType().Name.ToUpper()}: No close callback found for view model '{viewModel}'.");
		//				}
		//			}
		//		}
		//		catch
		//		{
		//			/* ignore */
		//		}
		//	});
		//}

		///// <summary>
		///// Tries to remove the <see cref="ICloseableViewModel.RequestClose"/> callback from the <paramref name="view"/>s view model
		///// </summary>
		///// <param name="view"> The view whose view model  that is examined for a close callback. </param>
		///// <returns> <c>True</c> on success, otherwise <c>False</c>. </returns>
		//private static void TryRemoveCloseCallback(Visual view)
		//{
		//	if (!(view is FrameworkElement frameworkElement)) return;

		//	try
		//	{
		//		frameworkElement.Dispatcher.Invoke(() =>
		//		{
		//			var viewModel = frameworkElement.DataContext;
		//			if (viewModel is ICloseableViewModel closeableViewModel)
		//			{
		//				closeableViewModel.RequestClose = null;
		//			}
		//			else
		//			{
		//				var closeCallbackProperty = viewModel.GetType().GetProperty(nameof(ICloseableViewModel.RequestClose), BindingFlags.Public | BindingFlags.Instance);
		//				closeCallbackProperty?.SetValue(viewModel, null);
		//			}
		//		});
		//	}
		//	catch { /* ignore */ }
		//}

		///// <summary>
		///// Links the <see cref="Dialog.ExternalCancellationToken"/> of the <paramref name="dialog"/> to the <see cref="Revoke(PhoenixDialog.Classes.DialogResult)"/> method.
		///// </summary>
		///// <param name="dialog"> The <see cref="Dialog"/> that should be linked with the external <see cref="CancellationToken"/>. </param>
		//private void TryAddExternalCancellationCallback(Dialog dialog)
		//{
		//	dialog.TokenRegistration = dialog.ExternalCancellationToken.CanBeCanceled
		//		? dialog.ExternalCancellationToken.Register(() => this.Revoke(DialogResult.Killed))
		//		: (CancellationTokenRegistration?) null;
		//}

		///// <summary>
		///// Removes the callback for the external <see cref="CancellationToken"/> of the <paramref name="dialog"/>.
		///// </summary>
		///// <param name="dialog"> The <see cref="Dialog"/> whoose binding to the external <see cref="CancellationToken"/> should be removed. </param>
		//private static void TryRemoveExternalCancellationCallback(Dialog dialog)
		//{
		//	dialog.TokenRegistration?.Dispose();
		//}

		#endregion

		/// <summary> Returns a string representation of the object. </summary>
		public override string ToString() => $"[<{this.GetType().Name}> :: Initialized: {this.IsInitialized} | AdornedElement: {(_adornedElement is FrameworkElement adorndedElement && !String.IsNullOrEmpty(adorndedElement.Name) ? adorndedElement.Name : _adornedElement.GetType().ToString())}{(Object.ReferenceEquals(_adornedElement, _originalElement) ? String.Empty : $" | OriginalElement: {(_originalElement is FrameworkElement originalElement && !String.IsNullOrEmpty(originalElement.Name) ? originalElement.Name : _originalElement.GetType().ToString())}")}]";

		#endregion
	}
}