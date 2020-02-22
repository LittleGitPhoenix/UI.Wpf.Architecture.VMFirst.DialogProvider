#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
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