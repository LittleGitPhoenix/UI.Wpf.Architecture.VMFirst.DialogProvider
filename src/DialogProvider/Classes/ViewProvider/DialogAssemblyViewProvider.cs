#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion

using System;
using System.Reflection;
using System.Windows;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels;
using Phoenix.UI.Wpf.Architecture.VMFirst.ViewProvider;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Base class for <see cref="IViewProvider"/>s inside assemblies that contain views for default dialogs view models like <see cref="IDialogContainerViewModel"/> or <see cref="MessageDialogViewModel"/>.
	/// </summary>
	/// <remarks> Create a derived class within the assembly that contains views for default dialog view models. </remarks>
	public abstract class DialogAssemblyViewProvider : DefaultViewProvider
	{
		/// <inheritdoc />
		/// <remarks> This always uses the assembly of the implementing class for resolving. </remarks>
		public override FrameworkElement GetViewInstance<TClass>(TClass viewModel, Assembly viewAssembly)
			=> base.GetViewInstance(viewModel, this.GetType().Assembly);

		/// <inheritdoc />
		protected abstract override string GetViewFullName(Type viewModelType, string viewModelNamespaceSuffix, string viewNamespaceSuffix, string viewModelNameSuffix, string viewNameSuffix);
	}
}