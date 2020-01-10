﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.DialogProvider.Classes;
using Phoenix.UI.Wpf.DialogProvider.Models;

namespace Phoenix.UI.Wpf.DialogProvider.ViewModels
{
	/// <summary>
	/// View model for <see cref="Exception"/>s.
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class ExceptionDialogViewModel : DialogContentViewModel
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties

		/// <summary> Title of the exception dialog. </summary>
		[PropertyChanged.DoNotNotify]
		public string Title { get; }

		/// <summary> Exception message. </summary>
		[PropertyChanged.DoNotNotify]
		public string Message { get; }

		/// <summary> All <see cref="ExceptionDialogModel"/>s that should be displayed. </summary>
		[PropertyChanged.DoNotNotify]
		public ICollection<ExceptionDialogModel> Exceptions { get; }

		/// <summary> The currently selected <see cref="ExceptionDialogModel"/>. </summary>
		public ExceptionDialogModel SelectedExceptionModel { get; set; }
		private void OnSelectedExceptionModelChanged()
		{
			this.SelectedInnerExceptionModel = this.SelectedExceptionModel.FirstOrDefault();
		}

		/// <summary> The currently selected <see cref="InnerExceptionDialogModel"/>. </summary>
		public InnerExceptionDialogModel SelectedInnerExceptionModel { get; set; }

		#endregion

		#region Enumerations
		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="title"> The title of the dialog. </param>
		/// <param name="message"> The message of the dialog. </param>
		/// <param name="exceptions"> The <see cref="Exceptions"/> that should be visualized. </param>
		/// <param name="dialogOptions"> Contains different options for the dialog. </param>
		public ExceptionDialogViewModel(string title, string message, ICollection<Exception> exceptions, DialogOptions dialogOptions)
			: base(dialogOptions)
		{
			// Save parameters.
			this.Title = title;
			this.Message = message;
			this.Exceptions = new List<ExceptionDialogModel>(exceptions.Select(exception => new ExceptionDialogModel(exception)));

			// Initialize fields.
			this.SelectedExceptionModel = this.Exceptions.FirstOrDefault();
		}

		#endregion

		#region Methods
		#endregion
	}
}
