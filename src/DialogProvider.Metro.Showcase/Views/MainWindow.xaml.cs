using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Phoenix.UI.Wpf.DialogProvider.Showcase.ViewModels;

namespace Phoenix.UI.Wpf.DialogProvider.Showcase.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary> Current product version of the program (Version for displaying to a customer). </summary>
		public string ProductVersion { get; } = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion;

		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = new MainWindowModel();
		}

		private void MainWindowLoaded(object sender, RoutedEventArgs e)
		{
			if (this.DataContext is MainWindowModel mainWindowModel)
			{
				mainWindowModel.InnerViewModel = this.InnerView.DataContext as InnerViewModel;
			}
		}

		#region Focus Monitor

		/// <summary> Current focused element. </summary>
		public string FocusedElementName
		{
			get => (string) GetValue(FocusedElementNameProperty);
			set => SetValue(FocusedElementNameProperty, value);
		}
		public static readonly DependencyProperty FocusedElementNameProperty = DependencyProperty.Register(nameof(FocusedElementName), typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

		protected void OnFocusedElementNameChanged()
		{
			System.Diagnostics.Debug.WriteLine(this.FocusedElementName);
		}

		private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			this.CheckKeyboardFocus();
		}

		private void CheckKeyboardFocus()
		{
			IInputElement element = Keyboard.FocusedElement;

			if (element == null)
			{
				this.FocusedElementName = "NO FOCUS";
			}
			else
			{
				if (element is FrameworkElement frameworkElement)
				{
					var identifier = !String.IsNullOrEmpty(frameworkElement.Name) ? frameworkElement.Name : frameworkElement.GetType().ToString();
					this.FocusedElementName = $"FrameworkElement [{identifier}]";
				}
				else
				{
					// Maybe a FrameworkContentElement has focus.
					if (element is FrameworkContentElement contentElement)
					{
						string identifier = !string.IsNullOrEmpty(contentElement.Name) ? contentElement.Name : contentElement.GetType().ToString();
						this.FocusedElementName = $"FrameworkContentElement [{identifier}]";
					}
					else
					{
						this.FocusedElementName = $"Element of type [{element.GetType()}] has focus";
					}
				}
			}
		}

		#endregion
	}
}