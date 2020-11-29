using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Showcase.ViewModels;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Showcase.Views
{
	/// <summary>
	/// Interaction logic for InnerView.xaml
	/// </summary>
	public partial class InnerView : UserControl
	{
		public InnerView()
		{
			InitializeComponent();

			this.DataContext = new InnerViewModel();
		}

		private void InnerViewLoaded(object sender, RoutedEventArgs e)
		{
			if (this.DataContext is InnerViewModel innerViewModel)
			{
				innerViewModel.Loaded(this);
			}
		}
	}
}
