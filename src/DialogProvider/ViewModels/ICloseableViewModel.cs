#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using Phoenix.UI.Wpf.DialogProvider.Classes;

namespace Phoenix.UI.Wpf.DialogProvider.ViewModels
{
	/// <summary>
	/// Interface that can be implemented by a view model. The provided callback <see cref="RequestClose"/> will be set by the <see cref="DialogHandler"/> and can be used to close the dialog directly from within the view model.
	/// </summary>
	/// <remarks>
	/// <para> Implementing this interface is the preferred way, but directly defining one of the following property callbacks is fine too. </para>
	/// <para> - <c>Action{</c><see cref="DialogResult"/><c>} RequestClose { get; set; }</c> </para>
	/// <para> - <c>Action{</c><see cref="Boolean"/><c>} RequestClose { get; set; }</c> </para>
	/// </remarks>
	public interface ICloseableViewModel
	{
		/// <summary>
		/// Callback method that will be set by the <see cref="DialogHandler"/> and can be used to close the dialog directly from within the view model.
		/// </summary>
		Action<DialogResult> RequestClose { get; set; }
	}

	/* TODO:
	 Add interfaces for interactive view models.
	  - IInitialActivatingViewModel
	  - IViewAccessViewModel<TView>
	 */
	//public interface IInteractiveViewModel
	//{
	//	void OnInitialActivate()
	//	{

	//	}
	//}
}