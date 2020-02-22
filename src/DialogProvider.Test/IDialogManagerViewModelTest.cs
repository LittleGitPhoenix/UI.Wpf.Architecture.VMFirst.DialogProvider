using System.Threading;
using System.Windows;
using Moq;
using NUnit.Framework;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;

namespace DialogProvider.Test
{
	[TestFixture]
	public class IDialogManagerViewModelTest
	{
		#region Data

		class DialogManagerViewModelWithSetableProperty : IDialogManagerViewModel
		{
			/// <inheritdoc />
			public IDialogManager DialogManager { get; set;}
		}

		class DialogManagerViewModelWithAutoProperty : IDialogManagerViewModel
		{
			/// <inheritdoc />
			public IDialogManager DialogManager { get; }
		}

		class DialogManagerViewModelWithExplicitProperty : IDialogManagerViewModel
		{
			/// <inheritdoc />
			IDialogManager IDialogManagerViewModel.DialogManager { get; }

			/// <see cref="IDialogManagerViewModel.DialogManager"/>
			public IDialogManager DialogManager { get; }
		}

		class DialogManagerViewModelWithoutInterface
		{
			public IDialogManager DialogManager { get; set; }
		}
		
		#endregion

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Direct_Property_Injection_Succeeds()
		{
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithSetableProperty();
			var dialogAssemblyViewProvider = new Mock<DialogAssemblyViewProvider>().Object;

			var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
			setupCallback.Invoke(viewModel, view);

			Assert.That(viewModel.DialogManager, Is.Not.Null);
			Assert.That(viewModel.DialogManager.IsInitialized, Is.False);
			view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
			Assert.That(viewModel.DialogManager.IsInitialized, Is.True);
		}
		
		[Test]
		public void DialogManagerViewModelHelper_Direct_Property_Injection_Without_View_Does_Nothing()
		{
			var view = (FrameworkElement) null;
			var viewModel = new DialogManagerViewModelWithSetableProperty();
			var dialogAssemblyViewProvider = new Mock<DialogAssemblyViewProvider>().Object;

			var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
			setupCallback.Invoke(viewModel, view);

			Assert.That(viewModel.DialogManager, Is.Null);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Indirect_Property_Injection_Succeeds()
		{
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithAutoProperty();
			var dialogAssemblyViewProvider = new Mock<DialogAssemblyViewProvider>().Object;

			var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
			setupCallback.Invoke(viewModel, view);

			Assert.That(viewModel.DialogManager, Is.Not.Null);
			Assert.That(viewModel.DialogManager.IsInitialized, Is.False);
			view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
			Assert.That(viewModel.DialogManager.IsInitialized, Is.True);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Explicit_Property_Injection_Succeeds()
		{
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithExplicitProperty();
			var dialogAssemblyViewProvider = new Mock<DialogAssemblyViewProvider>().Object;

			var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
			setupCallback.Invoke(viewModel, view);

			// Check that the direct 'DialogManager' property is still null.
			Assert.That(viewModel.DialogManager, Is.Null);

			// Check if the explicit implemented 'DialogManager' property has been filled.
			var concreteViewModel = (IDialogManagerViewModel) viewModel;
			Assert.That(concreteViewModel.DialogManager, Is.Not.Null);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Just_Initializes_Existing_DialogManager()
		{
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithSetableProperty();
			var dialogAssemblyViewProvider = new Mock<DialogAssemblyViewProvider>().Object;
			var dialogManager = new DefaultDialogManager(dialogAssemblyViewProvider);

			viewModel.DialogManager = dialogManager;
			var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
			setupCallback.Invoke(viewModel, view);
			
			// The DialogManager property must still be the same instance, since the helper should only initialize it.
			Assert.That(viewModel.DialogManager, Is.EqualTo(dialogManager));
			Assert.That(viewModel.DialogManager.IsInitialized, Is.False);
			view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent)); // Make this view already loaded.
			Assert.That(viewModel.DialogManager.IsInitialized, Is.True);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Only_Uses_Property_From_Interface()
		{
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithoutInterface();
			var dialogAssemblyViewProvider = new Mock<DialogAssemblyViewProvider>().Object;

			var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
			setupCallback.Invoke(viewModel, view);

			Assert.That(viewModel.DialogManager, Is.Null);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Property_Injection_Fails()
		{
			var view = new Window();
			var viewModel = new Mock<IDialogManagerViewModel>().Object; //! Mocked object does not have a setter or a backing field and this is why the test fails.
			var dialogAssemblyViewProvider = new Mock<DialogAssemblyViewProvider>().Object;

			var setupCallback = DialogManagerViewModelHelper.CreateViewModelSetupCallback(dialogAssemblyViewProvider);
			setupCallback.Invoke(viewModel, view);

			Assert.That(viewModel.DialogManager, Is.Null);
		}
	}
}