namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Defines different behaviors of buttons.
	/// </summary>
	public enum DialogButtonBehavior
	{
		/// <summary> No special behavior. </summary>
		None = 0,
		/// <summary> Button command can be executed via <see cref="System.Windows.Input.Key.Enter"/>. </summary>
		Enter = 1,
		/// <summary> Button command can be executed via <see cref="System.Windows.Input.Key.Escape"/>. </summary>
		Cancel = 2,
	}
}