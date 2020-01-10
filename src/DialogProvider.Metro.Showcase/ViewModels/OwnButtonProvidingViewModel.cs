using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.UI.Wpf.DialogProvider.Classes;
using Phoenix.UI.Wpf.DialogProvider.Showcase.Views;
using Phoenix.UI.Wpf.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.DialogProvider.Showcase.ViewModels
{
	/// <summary>
	/// This view model provides its own buttons instead of them being provided via the surrounding dialog.
	/// </summary>
	class OwnButtonProvidingViewModel : DialogContentViewModel
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields
		#endregion

		#region Properties
		#endregion

		#region (De)Constructors

		/// <inheritdoc />
		public OwnButtonProvidingViewModel()
			: base(DialogOptions.None)
		{

		}

		#endregion

		#region Methods

		/// <summary>
		/// Custom <c>BindView</c> method that will be invoked by the <see cref="IViewProvider"/>.
		/// </summary>
		public void BindView(System.Windows.FrameworkElement frameworkElement)
		{
			// Here the linked view can be accessed.
			//! It is not necessary to set the DataContext of the view to the current instance of this view model. This is already handled at this time by the ViewProviderBase.
			if (frameworkElement is OwnButtonProvidingView view)
			{
				view.Loaded += (sender, args) =>
				{
					Trace.WriteLine("The view has been loaded.");
				};
			}
		}

		public void Close()
		{
			base.RequestClose.Invoke(DialogResult.Yes);
		}

		#endregion
	}
}