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
	/// Interaction logic for OwnButtonProvidingView.xaml
	/// </summary>
	public partial class OwnButtonProvidingView : UserControl
	{
		public OwnButtonProvidingView()
		{
			InitializeComponent();
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs args)
		{
			(this.DataContext as OwnButtonProvidingViewModel)?.Close();
		}
	}
}
