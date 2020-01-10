using System.Reflection;
using System.Windows;
using Phoenix.UI.Wpf.DialogProvider.ViewModels;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Base class for a <see cref="IViewProvider"/> that contains views for default dialogs view models like <see cref="IDialogContainerViewModel"/> or <see cref="MessageDialogViewModel"/>.
	/// </summary>
	/// <remarks> Create a derived class within the assembly that contains views for default dialog view models. </remarks>
	public abstract class DialogAssemblyViewProvider : AssemblyViewProvider
	{
		/// <inheritdoc />
		/// <remarks> This always uses the assembly of the implementing class for resolving. </remarks>
		public override FrameworkElement GetViewInstance<TClass>(TClass viewModel, Assembly viewAssembly)
			=> base.GetViewInstance(viewModel, this.GetType().Assembly);
	}
}