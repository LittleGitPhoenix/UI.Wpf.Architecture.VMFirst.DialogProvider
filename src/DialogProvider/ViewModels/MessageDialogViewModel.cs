﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.DialogProvider.Classes;
using Phoenix.UI.Wpf.DialogProvider.Models;

namespace Phoenix.UI.Wpf.DialogProvider.ViewModels
{
	/// <summary>
	/// View model for simple messages.
	/// </summary>
	public class MessageDialogViewModel : DialogContentViewModel
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> Collection of <see cref="MessageDialogModel"/>s that will be displayed. </summary>
		public ICollection<MessageDialogModel> MessageModels { get; }

		/// <summary> The currently selected <see cref="MessageDialogModel"/>. </summary>
		public MessageDialogModel SelectedMessageModel { get; set; }

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="messageModels"> The <see cref="MessageDialogModel"/>s that will be displayed. </param>
		/// <param name="dialogOptions"> Contains different options for the dialog. </param>
		public MessageDialogViewModel(ICollection<MessageDialogModel> messageModels, DialogOptions dialogOptions)
			: base(dialogOptions)
		{
			// Save parameters.
			this.MessageModels = messageModels;

			// Initialize fields.
			this.SelectedMessageModel = messageModels.FirstOrDefault();

			//// TEST: Self closing
			//Task.Run(async () =>
			//{
			//	await Task.Delay(3000);
			//	//base.RequestClose?.Invoke(DialogResult.Killed);
			//	base.RequestClose?.Invoke(true);
			//});
		}

		#endregion

		#region Methods
		#endregion
	}
}