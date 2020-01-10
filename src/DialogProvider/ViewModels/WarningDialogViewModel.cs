using System.Collections.Generic;
using Phoenix.UI.Wpf.DialogProvider.Classes;
using Phoenix.UI.Wpf.DialogProvider.Models;

namespace Phoenix.UI.Wpf.DialogProvider.ViewModels
{
	/// <summary>
	/// View model for warnings.
	/// </summary>
	public class WarningDialogViewModel : MessageDialogViewModel
	{
		/// <inheritdoc />
		public WarningDialogViewModel(ICollection<MessageDialogModel> messageModels, DialogOptions dialogOptions)
			: base(messageModels, dialogOptions) { }
	}
}