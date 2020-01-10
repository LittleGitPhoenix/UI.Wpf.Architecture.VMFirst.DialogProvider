#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Linq;
using System.Reflection;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// <para> View provider that creates views for a given view model instance with the help of the <see cref="ViewAssemblyProvider"/> for finding the proper view assembly under the following requirements: </para>
	/// <para> The views are located in a namespace named 'Views'. </para>
	/// <para> The view models are located in a namespace named 'ViewModels' and have a name equaling the view plus a 'Model' suffix. </para>
	/// </summary>
	/// <example>
	/// <para> ViewAssembly\...\Views\MainWindow -> ViewModelAssembly\...\ViewModels\MainWindowModel </para>
	/// <para> ViewAssembly\...\Views\DetailsView -> ViewModelAssembly\...\ViewModels\DetailsViewModel </para>
	/// </example>
	public class AssemblyViewProvider : ViewProviderBase
	{
		/// <inheritdoc />
		public AssemblyViewProvider()
			: base
			(
				viewNamespace: "Views",
				viewNameSuffix: "",
				viewModelNamespace: "ViewModels",
				viewModelNameSuffix: "Model"
			) { }

		/// <inheritdoc />
		/// <remarks> Delegates its responsibility to <see cref="ViewAssemblyProvider.GetViewAssemblies"/>. </remarks>
		protected override Assembly[] GetViewAssemblies(Type viewModelType, Assembly viewAssembly = null)
		{
			return viewAssembly is null ? ViewAssemblyProvider.GetViewAssemblies(viewModelType) : new[] {viewAssembly};
		}

		/// <inheritdoc />
		protected override Type[] GetViewTypes(Assembly[] viewAssemblies, Type viewModelType, out string viewName)
		{
			// Fullname matching probably won't work, due to the namespaces not matching between different assemblies. So just use the expected view name.
			var name = base.GetViewName(viewModelType);

			var viewTypes = viewAssemblies
				.SelectMany(assembly => assembly.GetExportedTypes())
				.Where(type => type.Name == name)
				.ToArray()
				;

			viewName = name;
			return viewTypes;
		}
	}
}