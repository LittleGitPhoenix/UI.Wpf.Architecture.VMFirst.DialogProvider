#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// Locations where dialogs can be shown.
	/// </summary>
	public enum DialogDisplayLocation
	{
		/// <summary> Dialogs will be displayed above the registered view. </summary>
		Self,
		/// <summary> Dialogs will be displayed in the window the registered view belongs to. </summary>
		Window,
	}
}