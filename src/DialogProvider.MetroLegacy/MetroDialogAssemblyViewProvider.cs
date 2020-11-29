#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Views;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro
{
	/// <summary>
	/// Special <see cref="DialogAssemblyViewProvider"/> for metro styled dialogs.
	/// </summary>
	public sealed class MetroDialogAssemblyViewProvider : DialogAssemblyViewProvider
	{
		/// <summary>
		/// The namespace retrieved from one of the views.
		/// </summary>
		private static readonly string ViewNamespace = typeof(MessageDialogView).Namespace;
		
		/// <inheritdoc />
		protected override string GetViewFullName(Type viewModelType, string viewModelNamespaceSuffix, string viewNamespaceSuffix, string viewModelNameSuffix, string viewNameSuffix)
		{
			// Clean the name of the view model if necessary.
			var viewModelName = viewModelType.Name;
			if (!String.IsNullOrWhiteSpace(viewModelNameSuffix))
			{
				var index = viewModelName.LastIndexOf(viewModelNameSuffix, StringComparison.Ordinal);
				if (index != -1) viewModelName = viewModelName.Remove(index);
			}

			// Build the name of the view.
			var viewName = $"{viewModelName}{viewNameSuffix}";
			
			// Build the complete name of the view.
			var viewFullName = $"{ViewNamespace}.{viewName}";
			return viewFullName;
		}
	}
}