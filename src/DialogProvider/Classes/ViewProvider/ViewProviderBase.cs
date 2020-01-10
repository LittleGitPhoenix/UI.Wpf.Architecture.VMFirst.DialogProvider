#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// View provider that creates views for a given view model.
	/// </summary>
	public abstract class ViewProviderBase : IViewProvider
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		/// <summary> Mapping of view model types to their view counterparts. </summary>
		private readonly ConcurrentDictionary<Type, Type> _viewModelToViewMappings;

		#endregion

		#region Properties

		/// <summary> The namespace of the views. </summary>
		protected string ViewNamespace { get; }

		/// <summary> The suffix of the views. </summary>
		protected string ViewNameSuffix { get; }

		/// <summary> The namespace of the view models. </summary>
		protected string ViewModelNamespace { get; }

		/// <summary> The suffix of the view models. </summary>
		protected string ViewModelNameSuffix { get; }

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="viewNamespace"> The namespace of the views. </param>
		/// <param name="viewNameSuffix"> The suffix of the views. </param>
		/// <param name="viewModelNamespace"> The namespace of the view models. </param>
		/// <param name="viewModelNameSuffix"> The suffix of the view models. </param>
		protected ViewProviderBase
		(
			string viewNamespace,
			string viewNameSuffix,
			string viewModelNamespace,
			string viewModelNameSuffix
		)
		{
			// Save parameters.
			this.ViewNameSuffix = viewNameSuffix;
			this.ViewNamespace = viewNamespace;
			this.ViewModelNameSuffix = viewModelNameSuffix;
			this.ViewModelNamespace = viewModelNamespace;

			// Initialize fields.
			_viewModelToViewMappings = new ConcurrentDictionary<Type, Type>();
		}
		
		#endregion

		#region Methods

		/// <inheritdoc />
		public FrameworkElement GetViewInstance<TClass>(TClass viewModel)
			where TClass : class
			=> this.GetViewInstance(viewModel, null);

		/// <inheritdoc />
		public virtual FrameworkElement GetViewInstance<TClass>(TClass viewModel, Assembly viewAssembly)
			where TClass : class
		{
			if (viewModel is null) return null;

			// Get the type of the view model.
			var viewModelType = viewModel.GetType();

			try
			{
				// Get the view type.
				var viewType = _viewModelToViewMappings.GetOrAdd(viewModelType, type => this.GetViewType(type, viewAssembly));

				// Create an instance from it.
				var view = this.GetViewInstance(viewType);

				// Always(!) set the data context.
				view.DataContext = viewModel;

				// Bind the view model to the view.
				this.BindView(view, viewModel);

				// Return it.
				return view;
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"{this.GetType().Name.ToUpper()}: {ex.Message}");
				throw;
			}
		}
		
		private Type GetViewType(Type viewModelType, Assembly viewAssembly)
		{
			// Either use the provided view assembly or obtain one via the callback.
			var viewAssemblies = this.GetViewAssemblies(viewModelType, viewAssembly);
			if (!viewAssemblies.Any()) throw new ViewProviderException($"Could not find the assembly containing the views for the view model '{viewModelType.FullName}'.");
			
			// Get the view type.
			var viewTypes = this.GetViewTypes(viewAssemblies, viewModelType, out var viewName);
			
			// Check if only one view type was found.
			if (viewTypes.Length == 0) throw new ViewProviderException($"Could not find a view named '{viewName}' for the view model '{viewModelType.FullName}' in the assemblies '{String.Join(", ", viewAssemblies.Select(assembly => assembly.GetName().Name))}'.");
			if (viewTypes.Length > 1) Trace.WriteLine($"{this.GetType().Name.ToUpper()}: More than one view matches the name '{viewName}' for the view model '{viewModelType.FullName}': '{String.Join(", ", viewTypes.Select(type => type.FullName))}'. The first one will be used.");

			// Return the first and view type.
			return viewTypes.First();
		}

		/// <summary>
		/// Gets assemblies that contain views.
		/// </summary>
		/// <param name="viewModelType"> The <see cref="Type"/> of the view model. </param>
		/// <param name="viewAssembly"> Optional <see cref="Assembly"/> that contains the view. Default is <c>Null</c>. Is set in case the assembly is already known. </param>
		/// <returns> A collection of <see cref="Assembly"/>s that contain views. </returns>
		protected virtual Assembly[] GetViewAssemblies(Type viewModelType, Assembly viewAssembly = null)
		{
			//return this.ProviderConfiguration.ViewAssembliesProviderCallback.Invoke(viewModelType);
			return new[] { viewAssembly ?? viewModelType.Assembly};
		}

		/// <summary>
		/// Gets the view type from the <paramref name="viewAssemblies"/> matching the <paramref name="viewModelType"/>.
		/// </summary>
		/// <param name="viewAssemblies"> The <see cref="Assembly"/>s containing views. </param>
		/// <param name="viewModelType"> The <see cref="Type"/> of the view model. </param>
		/// <param name="viewName"> The expected name of the view that was used for lookup. </param>
		/// <returns> All matching view types. </returns>
		protected virtual Type[] GetViewTypes(Assembly[] viewAssemblies, Type viewModelType, out string viewName)
		{
			// Transform the name of the view model into what should be the name of the view.
			var viewNameFullName = this.GetViewFullName(viewModelType);

			var viewTypes = viewAssemblies
				.Select(assembly => assembly.GetType(viewNameFullName))
				.Where(type => type != null)
				.ToArray()
				;

			viewName = viewNameFullName;
			return viewTypes;
		}

		/// <summary>
		/// Builds a full view name from the <paramref name="viewModelType"/>.
		/// </summary>
		/// <param name="viewModelType"> The <see cref="Type"/> of the view model. </param>
		/// <returns> The full name for the view or <c>Null</c>. </returns>
		private string GetViewFullName(Type viewModelType)
		{
			// Get namespace and name of the view.
			var viewNamespace = this.GetViewNamespace(viewModelType);
			var viewName = this.GetViewName(viewModelType);

			if (String.IsNullOrWhiteSpace(viewNamespace) || String.IsNullOrWhiteSpace(viewName)) throw new ViewProviderException($"Could not build a view name from the view model '{viewModelType.FullName}'."); ;

			// Merge them together and return the result.
			var viewFullName = $"{viewNamespace}.{viewName}";
			Debug.WriteLine($"{this.GetType().Name.ToUpper()}: Transformed view model name '{viewModelType.FullName}' into '{viewFullName}'.");
			return viewFullName;
		}

		/// <summary>
		/// Builds the view name from the <paramref name="viewModelType"/>.
		/// </summary>
		/// <param name="viewModelType"> The <see cref="Type"/> of the view model. </param>
		/// <returns> The name for the view or <c>Null</c>. </returns>
		protected virtual string GetViewName(Type viewModelType)
		{
			// Get the name of the view.
			string viewModelName = viewModelType.Name;
			var lastIndex = viewModelName.LastIndexOf(this.ViewModelNameSuffix, StringComparison.Ordinal);
			var viewName = viewModelName.Remove(lastIndex).Insert(lastIndex, this.ViewNameSuffix);

			return viewName;
		}

		/// <summary>
		/// Builds the namespace of the view from the <paramref name="viewModelType"/>.
		/// </summary>
		/// <param name="viewModelType"> The <see cref="Type"/> of the view model. </param>
		/// <returns> The name space for the view or <c>Null</c>. </returns>
		protected virtual string GetViewNamespace(Type viewModelType)
		{
			var viewModelNamespace = viewModelType.Namespace;
			if (String.IsNullOrWhiteSpace(viewModelNamespace)) return null;
			var lastIndex = viewModelNamespace.LastIndexOf(this.ViewModelNamespace, StringComparison.Ordinal);
			var viewNamespace = viewModelNamespace.Remove(lastIndex).Insert(lastIndex, this.ViewNamespace);

			return viewNamespace;
		}

		/// <summary>
		/// Creates an instance of the <paramref name="viewType"/>.
		/// </summary>
		/// <param name="viewType"> The <see cref="Type"/> of the view of which an instance should be created. </param>
		/// <returns> A new instance of the <paramref name="viewType"/>. </returns>
		protected virtual FrameworkElement GetViewInstance(Type viewType)
		{
			//return this.ProviderConfiguration.ViewCreationCallback.Invoke(viewType);
			if (viewType.IsAbstract) throw new ViewProviderException($"The view '{viewType.FullName}' is abstract and cannot be instantiated.");
			if (viewType.IsValueType) throw new ViewProviderException($"The view '{viewType.FullName}' is a value type which is not supported.");
			if (!typeof(FrameworkElement).IsAssignableFrom(viewType)) throw new ViewProviderException($"The view '{viewType.FullName}' is not a {nameof(FrameworkElement)}.");

			return Activator.CreateInstance(viewType) as FrameworkElement;
		}

		/// <summary>
		/// <para> Binds the <paramref name="view"/> to the <paramref name="viewModel"/>. </para>
		/// <para> If the <paramref name="viewModel"/> provides a custom <c>BindView</c> method with a single parameter of type <see cref="FrameworkElement"/>, then this will be used. </para>
		/// </summary>
		/// <typeparam name="TClass"> The <see cref="Type"/> of the <paramref name="viewModel"/>. </typeparam>
		/// <param name="view"> The view as <see cref="FrameworkElement"/>. </param>
		/// <param name="viewModel"> The view model. </param>
		/// <remarks>
		/// <para> • The <paramref name="view"/>s <see cref="FrameworkElement.DataContext"/> is already set to the <paramref name="viewModel"/> by now. </para>
		/// <para> • Override this to implement custom / advanced binding or use a custom <c>BindView</c> method in the <paramref name="viewModel"/>. </para>
		/// </remarks>
		protected virtual void BindView<TClass>(FrameworkElement view, TClass viewModel) where TClass : class
		{
			// Check if the view model provides a custom 'BindView' method.
			var customBindingMethodInfo = viewModel.GetType().GetMethod(name: nameof(this.BindView), types: new[] {typeof(FrameworkElement)});
			if (customBindingMethodInfo is null)
			{
				Debug.WriteLine($"{this.GetType().Name.ToUpper()}: No custom '{nameof(this.BindView)}' method found in view model '{viewModel}'.");
			}
			else
			{
				Trace.WriteLine($"{this.GetType().Name.ToUpper()}: View model '{viewModel}' has proper '{nameof(this.BindView)}' method, that will now be executed.");
				customBindingMethodInfo.Invoke(viewModel, new object[] {view});
			}
		}

		#endregion
	}
}