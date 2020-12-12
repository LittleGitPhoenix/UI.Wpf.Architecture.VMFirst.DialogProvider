using System.Diagnostics;
using System.Threading;
using System.Windows;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;
using Phoenix.UI.Wpf.Architecture.VMFirst.ViewProvider;

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
			// Arrange
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithSetableProperty();
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			_ = new DialogManager(dialogAssemblyViewProvider, viewProvider);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.NotNull(viewModel.DialogManager);
			Assert.That(viewModel.DialogManager.IsInitialized, Is.False);
			view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
			Assert.That(viewModel.DialogManager.IsInitialized, Is.True);
		}

		[Test]
		public void DialogManagerViewModelHelper_Direct_Property_Injection_Without_View_Does_Nothing()
		{
			var view = (FrameworkElement) null;
			var viewModel = new DialogManagerViewModelWithSetableProperty();
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			_ = new DialogManager(dialogAssemblyViewProvider, viewProvider);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.Null(viewModel.DialogManager);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Indirect_Property_Injection_Succeeds()
		{
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithAutoProperty();
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			_ = new DialogManager(dialogAssemblyViewProvider, viewProvider);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.NotNull(viewModel.DialogManager);
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
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			_ = new DialogManager(dialogAssemblyViewProvider, viewProvider);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Check that the direct 'DialogManager' property is still null.
			Assert.Null(viewModel.DialogManager);
			
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
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			var dialogManager = new DialogManager(dialogAssemblyViewProvider, viewProvider);
			viewModel.DialogManager = dialogManager; //! Pre-Initialize the property.

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.NotNull(viewModel.DialogManager);
			Assert.That(viewModel.DialogManager, Is.EqualTo(dialogManager)); // The DialogManager property must still be the same instance, since the helper should only initialize it.
			Assert.That(viewModel.DialogManager.IsInitialized, Is.False);
			view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent)); // Make this view already loaded.
			Assert.That(viewModel.DialogManager.IsInitialized, Is.True);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Only_Has_Property()
		{
			var view = new Window();
			var viewModel = new DialogManagerViewModelWithoutInterface();
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			_ = new DialogManager(dialogAssemblyViewProvider, viewProvider);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.Null(viewModel.DialogManager);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DialogManagerViewModelHelper_Property_Injection_Fails()
		{
			var view = new Window();
			var viewModel = new Mock<IDialogManagerViewModel>().Object; //! Mocked object does not have a setter or a backing field and this is why the test fails.
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			_ = new DialogManager(dialogAssemblyViewProvider, viewProvider);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.That(viewModel.DialogManager, Is.Null);
		}
	}
}