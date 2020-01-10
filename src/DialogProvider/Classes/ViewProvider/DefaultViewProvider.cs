#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// <para> View provider that creates views for a given view model instance under the following requirements: </para>
	/// <para> Both, the views and the view models are located within the same assembly. </para>
	/// <para> The views are located in a namespace named 'Views'. </para>
	/// <para> The view models are located in a namespace named 'ViewModels' and have a name equaling the view plus a 'Model' suffix. </para>
	/// </summary>
	/// <example>
	/// <para> Views\MainWindow -> ViewModels\MainWindowModel </para>
	/// <para> Views\DetailsView -> ViewModels\DetailsViewModel </para>
	/// </example>
	public class DefaultViewProvider : ViewProviderBase
	{
		/// <inheritdoc />
		public DefaultViewProvider()
			: base
			(
				viewNamespace: "Views",
				viewNameSuffix: "",
				viewModelNamespace: "ViewModels",
				viewModelNameSuffix: "Model"
			) { }
	}
}