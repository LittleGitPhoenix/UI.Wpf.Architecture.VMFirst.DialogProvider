using System;
using System.Threading;
using System.Windows;
using NUnit.Framework;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;

namespace DialogProvider.Test
{
	[TestFixture]
	public class ICloseableDialogContentViewModelTest
	{
		#region Data

		class ViewModelWithAutoProperty : ICloseableDialogContentViewModel
		{
			public Action<DialogResult> RequestClose { get; }
		}

		class ViewModelWithoutInterface
		{
			public Action<DialogResult> RequestClose { get; set; }
		}

		class ViewModelWithAlternativeCallback
		{
			public Action<bool> RequestClose { get; }
		}

		private static readonly Action<DialogResult> Callback = result => { };

		#endregion

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CloseableDialogContentViewModelHelper_Add_Callback_Succeeds()
		{
			var view = new Window();
			var viewModel = new ViewModelWithAutoProperty();
			view.DataContext = viewModel;

			CloseableDialogContentViewModelHelper.TryAddCloseCallback(view, ICloseableDialogContentViewModelTest.Callback);

			Assert.That(viewModel.RequestClose, Is.EqualTo(ICloseableDialogContentViewModelTest.Callback));
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CloseableDialogContentViewModelHelper_Add_Callback_Without_Interface_Succeeds()
		{
			var view = new Window();
			var viewModel = new ViewModelWithoutInterface();
			view.DataContext = viewModel;

			CloseableDialogContentViewModelHelper.TryAddCloseCallback(view, ICloseableDialogContentViewModelTest.Callback);

			Assert.That(viewModel.RequestClose, Is.EqualTo(ICloseableDialogContentViewModelTest.Callback));
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CloseableDialogContentViewModelHelper_Add_Callback_With_Alternative_Callback_Succeeds()
		{
			var view = new Window();
			var viewModel = new ViewModelWithAlternativeCallback();
			view.DataContext = viewModel;

			CloseableDialogContentViewModelHelper.TryAddCloseCallback(view, ICloseableDialogContentViewModelTest.Callback);

			Assert.That(viewModel.RequestClose, Is.Not.Null);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CloseableDialogContentViewModelHelper_Remove_Callback_Succeeds()
		{
			var view = new Window();
			var viewModel = new ViewModelWithoutInterface
			{
				RequestClose = ICloseableDialogContentViewModelTest.Callback
			};
			view.DataContext = viewModel;

			CloseableDialogContentViewModelHelper.TryRemoveCloseCallback(view);

			Assert.That(viewModel.RequestClose, Is.Null);
		}
	}
}