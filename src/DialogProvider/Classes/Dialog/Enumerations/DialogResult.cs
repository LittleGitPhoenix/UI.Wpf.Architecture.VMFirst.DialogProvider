﻿#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes
{
	/// <summary>
	/// The result of a dialog.
	/// </summary>
	public enum DialogResult
	{
		/// <summary> The dialog was somehow killed. </summary>
		Killed = -2,
		/// <summary> The dialog answer was <c>No</c>. </summary>
		No = -1,
		/// <summary> This is the initial value of any dialog. </summary>
		/// <remarks> If this is returned by any <see cref="ButtonConfiguration.Callback"/> of a <see cref="Button"/>, then the close callback of the <see cref="DialogHandler"/> won't be called thus preventing the dialog from closing. </remarks>
		None = 0,
		/// <summary> The dialog answer was <c>Yes</c>. </summary>
		Yes = 1,
	}
}