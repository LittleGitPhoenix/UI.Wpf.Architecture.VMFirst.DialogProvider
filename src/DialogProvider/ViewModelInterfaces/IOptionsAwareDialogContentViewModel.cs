#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces
{
	/// <summary>
	/// Interface for dialog content view models that need to be aware of the <see cref="DialogOptions"/> provided by the <see cref="Dialog"/>.
	/// </summary>
	public interface IOptionsAwareDialogContentViewModel
	{
		/// <summary>
		/// The <see cref="Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes.DialogOptions"/> of the dialog. Can be used by the UI.
		/// </summary>
		DialogOptions DialogOptions { get; }
	}

	/// <summary>
	/// Helper to setup an <see cref="IOptionsAwareDialogContentViewModel"/> from a <see cref="Dialog"/>.
	/// </summary>
	internal static class OptionsAwareDialogContentViewModelHelper
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		private static readonly string InterfacePropertyName = nameof(IOptionsAwareDialogContentViewModel.DialogOptions);

		private static readonly Type InterfaceType = typeof(IOptionsAwareDialogContentViewModel);

		private static readonly PropertyInfo InterfacePropertyInfo = OptionsAwareDialogContentViewModelHelper.InterfaceType.GetProperty(OptionsAwareDialogContentViewModelHelper.InterfacePropertyName);

		private static readonly String InterfaceBackingFieldName = ReflectionHelper.GetInterfaceBackingFieldName(OptionsAwareDialogContentViewModelHelper.InterfacePropertyInfo);

		private static readonly String InstanceBackingFieldName = ReflectionHelper.GetInstanceBackingFieldName(OptionsAwareDialogContentViewModelHelper.InterfacePropertyInfo);

		#endregion

		#region Properties
		#endregion

		#region (De)Constructors
		#endregion

		#region Methods

		/// <summary>
		/// Tries to set the <paramref name="dialogOptions"/> to the <see cref="IOptionsAwareDialogContentViewModel.DialogOptions"/> property of the <paramref name="frameworkElement"/>s view model.
		/// </summary>
		internal static void TryAddDialogOptions(FrameworkElement frameworkElement, DialogOptions dialogOptions)
		{
			if (frameworkElement?.DataContext is null) return;

			frameworkElement.Dispatcher?.Invoke(() =>
			{
				try
				{
					var viewModel = frameworkElement.DataContext;
					var success = ReflectionHelper.TrySetProperty
					(
						OptionsAwareDialogContentViewModelHelper.InterfacePropertyName,
						viewModel,
						dialogOptions,
						OptionsAwareDialogContentViewModelHelper.InterfacePropertyInfo,
						OptionsAwareDialogContentViewModelHelper.InterfaceBackingFieldName,
						OptionsAwareDialogContentViewModelHelper.InstanceBackingFieldName
					);
					
					if (!success)
					{
						Trace.WriteLine($"No close callback found for view model '{viewModel}'.");
					}
				}
				catch { /* ignore */ }
			});
		}

		/// <summary>
		/// Tries to remove the <see cref="IOptionsAwareDialogContentViewModel.DialogOptions"/> callback from the <paramref name="frameworkElement"/>s view model.
		/// </summary>
		internal static void TryRemoveDialogOptions(FrameworkElement frameworkElement)
		{
			try
			{
				frameworkElement?.Dispatcher?.Invoke(() =>
				{
					var viewModel = frameworkElement.DataContext;
					ReflectionHelper.TrySetProperty
					(
						OptionsAwareDialogContentViewModelHelper.InterfacePropertyName,
						viewModel,
						DialogOptions.None,
						OptionsAwareDialogContentViewModelHelper.InterfacePropertyInfo,
						OptionsAwareDialogContentViewModelHelper.InterfaceBackingFieldName,
						OptionsAwareDialogContentViewModelHelper.InstanceBackingFieldName
					);
				});
			}
			catch { /* ignore */ }
		}

		#endregion
	}

}