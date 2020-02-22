#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System.Collections.Generic;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels
{
	/// <summary>
	/// View model for warnings.
	/// </summary>
	public class WarningDialogViewModel : MessageDialogViewModel
	{
		/// <inheritdoc />
		public WarningDialogViewModel(ICollection<MessageDialogModel> messageModels)
			: base(messageModels) { }
	}
}