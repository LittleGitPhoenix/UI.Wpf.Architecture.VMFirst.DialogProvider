#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Views
{
	/// <summary>
	/// Interaction logic for DialogContainerView.xaml
	/// </summary>
	public partial class DialogContainerView : UserControl
	{
		private DependencyPropertyDescriptor _propertyDescriptor;
		private Accent _defaultAccent;
		private AppTheme _defaultTheme;

		/// <inheritdoc />
		public DialogContainerView()
		{
			InitializeComponent();
		}

		private void DialogContainerView_OnLoaded(object sender, RoutedEventArgs args)
		{
			// Get a property descriptor to monitor changes to the content view, so that the application style can be updated.
			_propertyDescriptor = DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, typeof(ContentControl));
			_propertyDescriptor?.RemoveValueChanged(this.ContentHolder, this.ContentChangedHandler);
			_propertyDescriptor?.AddValueChanged(this.ContentHolder, this.ContentChangedHandler);

			// By default try to apply the same style that is used by the window containing this dialog.
			var window = Window.GetWindow(this);
			var appStyle = window is null ? null : ThemeManager.DetectAppStyle(window);

			// Get the values or use default ones.
			// https://mahapps.com/guides/styles.html#thememanager
			_defaultAccent = appStyle?.Item2 ?? ThemeManager.GetAccent("Steel");
			_defaultTheme = appStyle?.Item1 ?? ThemeManager.GetAppTheme("BaseLight");

			this.ChangeAppStyle(_defaultAccent, _defaultTheme);
		}

		private void DialogContainerView_OnUnloaded(object sender, RoutedEventArgs e)
		{
			_propertyDescriptor?.RemoveValueChanged(this.ContentHolder, this.ContentChangedHandler);
		}

		private void ContentChangedHandler(object sender, EventArgs args)
			=> this.ChangeAppStyle(_defaultAccent, _defaultTheme);

		private void ChangeAppStyle(Accent newAccent, AppTheme newTheme)
		{
			if (this.ContentHolder.Content is WarningDialogView)
			{
				var warningAccent = newAccent.Name == "Red" ? ThemeManager.GetAccent("Crimson") : ThemeManager.GetAccent("Red");
				ThemeManager.ChangeAppStyle(this.DialogResourceDictionary, warningAccent, newTheme);
			}
			else if (this.ContentHolder.Content is ExceptionDialogView)
			{
				var exceptionAccent = newAccent.Name == "Red" ? ThemeManager.GetAccent("Crimson") : ThemeManager.GetAccent("Red");
				ThemeManager.ChangeAppStyle(this.DialogResourceDictionary, exceptionAccent, ThemeManager.GetInverseAppTheme(newTheme));
			}
			else
			{
				ThemeManager.ChangeAppStyle(this.DialogResourceDictionary, newAccent, newTheme);
			}
		}
	}
}