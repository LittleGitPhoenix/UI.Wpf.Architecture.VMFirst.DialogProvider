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
	/// Interface for dialog content view models. The provided callback <see cref="RequestClose"/> will be set by the <see cref="Dialog"/> and can be used to close the dialog directly from within the view model.
	/// </summary>
	/// <remarks>
	/// <para> Implementing this interface is the preferred way, but directly defining one of the following property callbacks is fine too. </para>
	/// <para> - <c>Action{</c><see cref="DialogResult"/><c>} RequestClose { get; set; }</c> </para>
	/// <para> - <c>Action{</c><see cref="Boolean"/><c>} RequestClose { get; set; }</c> </para>
	/// </remarks>
	public interface ICloseableDialogContentViewModel
	{
		/// <summary>
		/// Callback method that will be set by the <see cref="Dialog"/> and can be used to close the dialog directly from within the view model.
		/// </summary>
		Action<DialogResult> RequestClose { get; }
	}

	/// <summary>
	/// Helper to setup an <see cref="ICloseableDialogContentViewModel"/> from a <see cref="Dialog"/>.
	/// </summary>
	internal static class CloseableDialogContentViewModelHelper
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		private static readonly string InterfacePropertyName = nameof(ICloseableDialogContentViewModel.RequestClose);

		private static readonly Type InterfaceType = typeof(ICloseableDialogContentViewModel);

		private static readonly PropertyInfo InterfacePropertyInfo = CloseableDialogContentViewModelHelper.InterfaceType.GetProperty(CloseableDialogContentViewModelHelper.InterfacePropertyName);

		private static readonly String InterfaceBackingFieldName = ReflectionHelper.GetInterfaceBackingFieldName(CloseableDialogContentViewModelHelper.InterfacePropertyInfo);

		private static readonly String InstanceBackingFieldName = ReflectionHelper.GetInstanceBackingFieldName(CloseableDialogContentViewModelHelper.InterfacePropertyInfo);

		#endregion

		#region Properties
		#endregion

		#region (De)Constructors
		#endregion

		#region Methods

		/// <summary>
		/// Tries to set the <paramref name="closeCallback"/> to the <see cref="ICloseableDialogContentViewModel.RequestClose"/> property of the <paramref name="frameworkElement"/>s view model.
		/// </summary>
		internal static void TryAddCloseCallback(FrameworkElement frameworkElement, Action<DialogResult> closeCallback)
		{
			if (frameworkElement?.DataContext is null) return;

			frameworkElement.Dispatcher?.Invoke(() =>
			{
				try
				{
					var viewModel = frameworkElement.DataContext;
					var success = ReflectionHelper.TrySetProperty
					(
						CloseableDialogContentViewModelHelper.InterfacePropertyName,
						viewModel,
						closeCallback,
						CloseableDialogContentViewModelHelper.InterfacePropertyInfo,
						CloseableDialogContentViewModelHelper.InterfaceBackingFieldName,
						CloseableDialogContentViewModelHelper.InstanceBackingFieldName
					);

					if (!success)
					{
						Action<bool> alternativeCloseCallback = result => closeCallback.Invoke(result ? DialogResult.Yes : DialogResult.No);
						success = ReflectionHelper.TrySetProperty
						(
							CloseableDialogContentViewModelHelper.InterfacePropertyName,
							viewModel,
							alternativeCloseCallback,
							CloseableDialogContentViewModelHelper.InterfacePropertyInfo,
							CloseableDialogContentViewModelHelper.InterfaceBackingFieldName,
							CloseableDialogContentViewModelHelper.InstanceBackingFieldName
						);
					}

					if (!success)
					{
						Trace.WriteLine($"No close callback found for view model '{viewModel}'.");
					}
				}
				catch { /* ignore */ }
			});
		}

		/// <summary>
		/// Tries to remove the <see cref="ICloseableDialogContentViewModel.RequestClose"/> callback from the <paramref name="frameworkElement"/>s view model.
		/// </summary>
		internal static void TryRemoveCloseCallback(FrameworkElement frameworkElement)
		{
			try
			{
				frameworkElement?.Dispatcher?.Invoke(() =>
				{
					var viewModel = frameworkElement.DataContext;
					ReflectionHelper.TrySetProperty
					(
						CloseableDialogContentViewModelHelper.InterfacePropertyName,
						viewModel,
						null,
						CloseableDialogContentViewModelHelper.InterfacePropertyInfo,
						CloseableDialogContentViewModelHelper.InterfaceBackingFieldName,
						CloseableDialogContentViewModelHelper.InstanceBackingFieldName
					);
				});
			}
			catch { /* ignore */ }
		}

		#endregion
	}
}