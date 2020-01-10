using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Searches loaded assemblies for instances of <see cref="IViewHolder"/> to later provide those to an <see cref="AssemblyViewProvider"/>.
	/// </summary>
	public static class ViewAssemblyProvider
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		/// <summary>
		/// Mapping of view model assemblies to view assemblies.
		/// </summary>
		//private static readonly List<(Assembly ViewModelAssembly, Assembly ViewAssembly)> AssemblyMappings;
		private static readonly List<AssemblyMapping> AssemblyMappings;

		#endregion

		#region Properties
		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		static ViewAssemblyProvider()
		{
			// Initialize fields.
			ViewAssemblyProvider.AssemblyMappings = new List<AssemblyMapping>();

			// Start watching loaded assemblies for IViewHolders.
			ViewAssemblyProvider.UpdatedLoadedAssemblies();
			AppDomain.CurrentDomain.AssemblyLoad += ViewAssemblyProvider.AssemblyLoaded;
		}

		#endregion

		#region Methods

		private static void UpdatedLoadedAssemblies()
		{
			AppDomain.CurrentDomain
				.GetAssemblies()
				.ToList()
				.ForEach(ViewAssemblyProvider.TryAddNewAssembly)
				;
		}
		private static void AssemblyLoaded(object sender, AssemblyLoadEventArgs args)
		{
			var loadedAssembly = args.LoadedAssembly;
			ViewAssemblyProvider.TryAddNewAssembly(loadedAssembly);
		}

		private static void TryAddNewAssembly(Assembly newAssemby)
		{
			if (newAssemby.IsDynamic) return;
			if (newAssemby.GetName().Name.StartsWith("Microsoft.VisualStudio.")) return; // Hopefully no one names their assemblies this way. Removing this may lead to a 'FileNotFoundException' for 'Microsoft.VisualStudio.Text.Data' in design mode when opening a dialog within the loaded event of a view.

			// Check if the assembly contains a IViewHolder.
			var viewHolders = newAssemby
				.GetExportedTypes()
				.Where(type => !type.IsAbstract && !type.IsValueType && typeof(IViewHolder).IsAssignableFrom(type))
				.Select(viewHolderType => viewHolderType.GetConstructor(Type.EmptyTypes)) // Get the default constructor.
				.Where(viewHolderConstructor => viewHolderConstructor != null)
				.Select(viewHolderConstructor => (IViewHolder) viewHolderConstructor.Invoke(null))
				.ToArray()
				;

			if (!viewHolders.Any()) return;

			foreach (var viewHolder in viewHolders)
			{
				foreach (var viewModelAssembly in viewHolder.ViewModelAssemblies)
				{
					Debug.WriteLine($"{nameof(ViewAssemblyProvider).ToUpper()}: Added new view model to view mapping: {viewModelAssembly.GetName().Name} -> {newAssemby.GetName().Name}");
					ViewAssemblyProvider.AssemblyMappings.Add(new AssemblyMapping(viewModelAssembly, newAssemby));
				}
			}
		}

		/// <summary>
		/// Gets all available <see cref="Assembly"/>s that have mappings for the <paramref name="viewModelType"/>.
		/// </summary>
		/// <param name="viewModelType"> The <see cref="Type"/> of the view model to look for. </param>
		/// <returns> A collection of <see cref="Assembly"/>s that have registered mappings for the <paramref name="viewModelType"/>. </returns>
		public static Assembly[] GetViewAssemblies(Type viewModelType)
		{
			var viewModelAssembly = viewModelType.Assembly;
			var viewAssemblies = ViewAssemblyProvider.AssemblyMappings
				.Where(mapping => mapping.ViewModelAssembly == viewModelAssembly)
				.Select(mapping => mapping.ViewAssembly)
				.ToArray()
				;
			return viewAssemblies;
		}

		#endregion

		#region Nested Types

		private class AssemblyMapping
		{
			internal Assembly ViewModelAssembly { get; }
			internal Assembly ViewAssembly { get; }

			public AssemblyMapping(Assembly viewModelAssembly, Assembly viewAssembly)
			{
				this.ViewModelAssembly = viewModelAssembly;
				this.ViewAssembly = viewAssembly;
			}
		}

		#endregion
	}
}