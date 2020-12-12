using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Classes;
using Phoenix.UI.Wpf.Architecture.VMFirst.ViewProvider;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces
{
	/// <summary>
	/// Interface of view models utilizing an <see cref="IDialogManager"/>.
	/// </summary>
	public interface IDialogManagerViewModel
	{
		/// <summary> <see cref="IDialogManager"/> used for displaying dialogs. </summary>
		IDialogManager DialogManager { get; }
	}

	/// <summary>
	/// Helper to setup an <see cref="IDialogManagerViewModel"/> via the <c>SetupViewModel</c> method of the <c>Phoenix.UI.Wpf.Architecture.VMFirst.ViewProvider.DefaultViewProvider</c>.
	/// </summary>
	public static class DialogManagerViewModelHelper
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		private static readonly string InterfacePropertyName = nameof(IDialogManagerViewModel.DialogManager);

		private static readonly Type InterfaceType = typeof(IDialogManagerViewModel);

		private static readonly PropertyInfo InterfacePropertyInfo = DialogManagerViewModelHelper.InterfaceType.GetProperty(DialogManagerViewModelHelper.InterfacePropertyName);

		private static readonly String InterfaceBackingFieldName = ReflectionHelper.GetInterfaceBackingFieldName(DialogManagerViewModelHelper.InterfacePropertyInfo);

		private static readonly String InstanceBackingFieldName = ReflectionHelper.GetInstanceBackingFieldName(DialogManagerViewModelHelper.InterfacePropertyInfo);

		#endregion

		#region Properties
		#endregion

		#region (De)Constructors
		#endregion

		#region Methods

		/// <summary>
		/// Creates a custom setup method for view models of type <see cref="IDialogManagerViewModel"/>.
		/// </summary>
		/// <param name="dialogAssemblyViewProvider"> An instance of a <see cref="DialogAssemblyViewProvider"/> used to resolve views for the direct dialog view models. </param>
		/// <param name="viewProviders"> Optional <see cref="IViewProvider"/>s used to resolve views for content view models. Default is only a <see cref="DefaultViewProvider"/>. </param>
		[Obsolete("Instead of view model setup callbacks, the 'IViewProvider' now uses an event based mechanism via 'IViewProvider.ViewLoaded'. The 'DialogManager' automatically attaches a handler to this event and calls 'SetupViewModel' by itself. Therefore using this method is not needed anymore and its implementation does nothing.", error: true)]
		public static Action<object, FrameworkElement> CreateViewModelSetupCallback(DialogAssemblyViewProvider dialogAssemblyViewProvider, params IViewProvider[] viewProviders)
		{
			//return (viewModel, view) => SetupViewModel(viewModel, view, dialogAssemblyViewProvider, viewProviders);
			return (viewModel, view) => {};
		}

		//private static void SetupViewModel(object viewModel, FrameworkElement view, DialogAssemblyViewProvider dialogAssemblyViewProvider, params IViewProvider[] viewProviders)
		//{
		//	if (!(viewModel is IDialogManagerViewModel dialogManagerViewModel)) return;
		//	if (view is null) return;
			
		//	var type = dialogManagerViewModel.GetType();
			
		//	// Get the 'DialogManager' property directly from the interface and not the type. This is needed in case of explicit interface implementation.
		//	var propertyInfo = DialogManagerViewModelHelper.InterfacePropertyInfo;
		//	if (propertyInfo is null) return; // This can never be, as the IDialogManagerViewModel interface forces that this property is implemented. This check is just for keeping the compiler from annoying me.

		//	// Check if its value is null and must be filled with a new instance.
		//	var dialogManager = (IDialogManager) propertyInfo.GetValue(viewModel);
		//	if (dialogManager is null)
		//	{
		//		// YES: Create an initialized dialog manager.
		//		dialogManager = new DialogManager(dialogAssemblyViewProvider, viewProviders).Initialize(view);
		//		var success = ReflectionHelper.TrySetProperty
		//		(
		//			DialogManagerViewModelHelper.InterfacePropertyName,
		//			viewModel,
		//			dialogManager,
		//			DialogManagerViewModelHelper.InterfacePropertyInfo,
		//			DialogManagerViewModelHelper.InterfaceBackingFieldName,
		//			DialogManagerViewModelHelper.InstanceBackingFieldName
		//		);
				
		//		if (success) return;
		//		Trace.WriteLine($"ERROR: Could not inject a '{nameof(IDialogManager)}' into the view model '{type.Name}' as its '{nameof(IDialogManagerViewModel.DialogManager)}' property either has no setter or its backing field could not be found.");
		//	}
		//	else
		//	{
		//		// NO: Just initialize it.
		//		dialogManager.Initialize(view);
		//	}
		//}

		internal static void SetupViewModel(DialogManager callingDialogManager, object viewModel, FrameworkElement view)
		{
			if (!(viewModel is IDialogManagerViewModel dialogManagerViewModel)) return;
			if (view is null) return;
			
			var type = dialogManagerViewModel.GetType();
			
			// Get the 'DialogManager' property directly from the interface and not the type. This is needed in case of explicit interface implementation.
			var propertyInfo = DialogManagerViewModelHelper.InterfacePropertyInfo;
			if (propertyInfo is null) return; // This can never be, as the IDialogManagerViewModel interface forces that this property is implemented. This check is just for keeping the compiler from annoying me.

			// Check if its value is null and must be filled with a new instance.
			var dialogManager = (IDialogManager) propertyInfo.GetValue(viewModel);
			if (dialogManager is null)
			{
				// YES: Create an initialized dialog manager.
				dialogManager = new DialogManager(callingDialogManager).Initialize(view);
				var success = ReflectionHelper.TrySetProperty
				(
					DialogManagerViewModelHelper.InterfacePropertyName,
					viewModel,
					dialogManager,
					DialogManagerViewModelHelper.InterfacePropertyInfo,
					DialogManagerViewModelHelper.InterfaceBackingFieldName,
					DialogManagerViewModelHelper.InstanceBackingFieldName
				);
				
				if (success) return;
				Trace.WriteLine($"ERROR: Could not inject a '{nameof(IDialogManager)}' into the view model '{type.Name}' as its '{nameof(IDialogManagerViewModel.DialogManager)}' property either has no setter or its backing field could not be found.");
			}
			else
			{
				// NO: Just initialize it.
				dialogManager.Initialize(view);
			}
		}

		#endregion
	}
}