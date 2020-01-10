namespace Phoenix.UI.Wpf.DialogProvider.Classes
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