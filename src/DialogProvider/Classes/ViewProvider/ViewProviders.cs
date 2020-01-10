using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// A collection of <see cref="IViewProvider"/>s that can be used in sequence to obtain a view for a view model.
	/// </summary>
	class ViewProviders : List<IViewProvider>, IViewProvider
	{
		public ViewProviders(IEnumerable<IViewProvider> viewProviders) : base(viewProviders) { }

		public FrameworkElement GetViewInstance<TClass>(TClass viewModel) where TClass : class
			=> this.GetViewInstance(viewModel, viewAssembly: null);

		public FrameworkElement GetViewInstance<TClass>(TClass viewModel, Assembly viewAssembly) where TClass : class
		{
			if (viewModel is null) return null;

			foreach (var viewProvider in this)
			{
				try
				{
					return viewProvider.GetViewInstance(viewModel, viewAssembly);
				}
				catch (ViewProviderException)
				{
					/* Swallow all exceptions so that all providers are invoked until one finds the view. */
				}
			}

			// If the no view was found, throw an exception.
			if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
			throw new ViewProviderException($"Could not find a matching view for the view model '{viewModel.GetType().FullName}' in any of the available {nameof(IViewProvider)}s.");
		}
	}
}