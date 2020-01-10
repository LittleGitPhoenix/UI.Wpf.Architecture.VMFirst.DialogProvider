namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// The behavior when showing new dialogs. 
	/// </summary>
	public enum DialogDisplayBehavior
	{
		/// <summary> The dialog will be shown on top of other dialogs and can be removed if needed. </summary>
		Show,
		/// <summary> The dialog will replace the currently shown dialog. </summary>
		Override,
	}
}