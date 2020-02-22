using System.Threading;
using System.Windows;
using NUnit.Framework;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;

namespace DialogProvider.Test
{
	[TestFixture]
	public class IOptionsAwareDialogContentViewModelTest
	{
		#region Data

		class ViewModelWithAutoProperty : IOptionsAwareDialogContentViewModel
		{
			public DialogOptions DialogOptions { get; }
		}

		class ViewModelWithoutInterface
		{
			public DialogOptions DialogOptions { get; set; }
		}

		private static DialogOptions Options = DialogOptions.HideTransparencyToggle;

		#endregion

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CloseableDialogContentViewModelHelper_Add_Callback_Succeeds()
		{
			var view = new Window();
			var viewModel = new ViewModelWithAutoProperty();
			view.DataContext = viewModel;

			OptionsAwareDialogContentViewModelHelper.TryAddDialogOptions(view, IOptionsAwareDialogContentViewModelTest.Options);

			Assert.That(viewModel.DialogOptions, Is.EqualTo(IOptionsAwareDialogContentViewModelTest.Options));
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CloseableDialogContentViewModelHelper_Add_Callback_Without_Interface_Succeeds()
		{
			var view = new Window();
			var viewModel = new ViewModelWithoutInterface();
			view.DataContext = viewModel;

			OptionsAwareDialogContentViewModelHelper.TryAddDialogOptions(view, IOptionsAwareDialogContentViewModelTest.Options);

			Assert.That(viewModel.DialogOptions, Is.EqualTo(IOptionsAwareDialogContentViewModelTest.Options));
		}
		
		[Test]
		[Apartment(ApartmentState.STA)]
		public void CloseableDialogContentViewModelHelper_Remove_Callback_Succeeds()
		{
			var view = new Window();
			var viewModel = new ViewModelWithoutInterface
			{
				DialogOptions = IOptionsAwareDialogContentViewModelTest.Options
			};
			view.DataContext = viewModel;

			OptionsAwareDialogContentViewModelHelper.TryRemoveDialogOptions(view);

			Assert.That(viewModel.DialogOptions, Is.EqualTo(DialogOptions.None));
		}
	}
}