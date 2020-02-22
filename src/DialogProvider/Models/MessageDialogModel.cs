#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Models
{
	/// <summary>
	/// Model for <see cref="MessageDialogViewModel"/> or <see cref="WarningDialogViewModel"/>.
	/// </summary>
	public sealed class MessageDialogModel
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> The identifier of the message dialog. </summary>
		public string Identifier { get; }

		/// <summary> The title of the message. </summary>
		public string Title { get; }

		/// <summary> The message that will be displayed. </summary>
		public string Message { get; }

		internal object ContentViewModel { get; }

		/// <summary> The content view that will additionally be displayed in the dialog. </summary>
		public FrameworkElement ContentView { get; set; }

		#endregion

		#region Enumerations
		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="identifier"> The identifier of the message dialog. </param>
		/// <param name="title"> The title of the message. </param>
		/// <param name="message"> The message that will be displayed. </param>
		/// <param name="contentViewModel"> The view model whose resolved view will be displayed in the dialog.  </param>
		public MessageDialogModel(string identifier, string title = null, string message = null, object contentViewModel = null)
		{
			this.Identifier = identifier;
			this.Title = title;
			this.Message = message;
			this.ContentViewModel = contentViewModel;
		}

		#endregion

		#region Methods
		#endregion
	}
}