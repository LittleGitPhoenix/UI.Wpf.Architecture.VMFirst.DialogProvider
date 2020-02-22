using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Showcase.Views;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Showcase.ViewModels
{
	/// <summary>
	/// This view model provides its own buttons instead of them being provided via the surrounding dialog.
	/// </summary>
	class OwnButtonProvidingViewModel /*DialogContentViewModel,*/ /*ICloseableDialogContentViewModel*/
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties
		
		/// <inheritdoc />
		//public Action<DialogResult> RequestClose { get; set; }
		public Action<bool> RequestClose { get; set; }

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public OwnButtonProvidingViewModel() { }

		#endregion

		#region Methods
		
		public void Close()
		{
			//base.RequestClose.Invoke(DialogResult.Yes);
			//this.RequestClose.Invoke(DialogResult.Yes);
			this.RequestClose.Invoke(true);
		}

		#endregion
	}
}