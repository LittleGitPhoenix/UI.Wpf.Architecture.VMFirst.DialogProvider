using System;
using System.Threading;
using System.Windows;
using Moq;
using NUnit.Framework;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.ViewProvider;

namespace DialogProvider.Test
{
	[TestFixture]
	class DialogManagerTest
	{
		#region Data

		class DialogManagerMock : DialogManager
		{
			private readonly Action _callback;

			/// <inheritdoc />
			internal override void NewViewLoaded(object sender, ViewLoadedEventArgs args)
			{
				_callback.Invoke();
			}

			/// <inheritdoc />
			public DialogManagerMock(Action callback, DialogAssemblyViewProvider dialogAssemblyViewProvider, params IViewProvider[] viewProviders) : base(dialogAssemblyViewProvider, viewProviders)
			{
				_callback = callback;
			}

			/// <inheritdoc />
			internal DialogManagerMock(Action callback, DialogManager other) : base(other)
			{
				_callback = callback;
			}
		}

		#endregion

		[Test]
		[Apartment(ApartmentState.STA)]
		public void Check_Independent_DialogManagers_Attache_To_ViewLoaded_Event()
		{
			// Arrange
			var view = new Window();
			var viewModel = new object();
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			var counter = 0;
			Action callBack = () => counter++;

			//! Create two independent instances. Each should handle the ViewLoaded event.
			_ = new DialogManagerMock(callBack, dialogAssemblyViewProvider, viewProvider);
			_ = new DialogManagerMock(callBack, dialogAssemblyViewProvider, viewProvider);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.That(counter, Is.EqualTo(2));
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void Check_Dependent_DialogManager_Does_Not_Attache_To_ViewLoaded_Event()
		{
			// Arrange
			var view = new Window();
			var viewModel = new object();
			var dialogAssemblyViewProvider = Mock.Of<DialogAssemblyViewProvider>();
			var viewProvider = Mock.Of<IViewProvider>();
			var counter = 0;
			Action callBack = () => counter++;

			//! Create a dependent instance, which should not handle ViewLoaded itself.
			var dialogManager = new DialogManagerMock(callBack, dialogAssemblyViewProvider, viewProvider);
			_ = new DialogManagerMock(callBack, dialogManager);

			// Act
			Mock.Get(viewProvider).Raise(provider => provider.ViewLoaded += null, new ViewLoadedEventArgs(viewModel, view));

			// Assert
			Assert.That(counter, Is.EqualTo(1));
		}
	}
}