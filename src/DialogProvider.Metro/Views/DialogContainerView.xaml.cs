#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ControlzEx.Theming;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.Metro.Views
{
	/// <summary>
	/// Interaction logic for DialogContainerView.xaml
	/// </summary>
	public partial class DialogContainerView : UserControl
	{
		#region Delegates / Events
		#endregion

		#region Constants
		#endregion

		#region Fields

		/// <summary> This <see cref="Theme"/> will be used if the theme of the current window couldn't be obtained. </summary>
		private static readonly Theme DefaultTheme;

		private DependencyPropertyDescriptor _propertyDescriptor;
		
		private Theme _currentWindowTheme;

		#endregion

		#region Properties
		#endregion

		#region (De)Constructors

		static DialogContainerView()
		{
			DefaultTheme = ThemeManager.Current.GetTheme("Light.Blue");
		}

		/// <inheritdoc />
		public DialogContainerView()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void DialogContainerView_OnLoaded(object sender, RoutedEventArgs args)
		{
			// Get a property descriptor to monitor changes to the content view, so that the application style can be updated if other dialogs are shown on top of the current one.
			_propertyDescriptor = DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, typeof(ContentControl));
			_propertyDescriptor?.RemoveValueChanged(this.ContentHolder, this.ContentChangedHandler);
			_propertyDescriptor?.AddValueChanged(this.ContentHolder, this.ContentChangedHandler);

			// By default try to apply the same theme that is used by the window containing this dialog. (https://mahapps.com/docs/themes/)
			var window = Window.GetWindow(this);
			_currentWindowTheme = window is null ? null : ThemeManager.Current.DetectTheme(window);
			_currentWindowTheme ??= DialogContainerView.DefaultTheme;

			// Apply the theme.
			this.ChangeTheme(_currentWindowTheme);
		}

		private void DialogContainerView_OnUnloaded(object sender, RoutedEventArgs e)
		{
			_propertyDescriptor?.RemoveValueChanged(this.ContentHolder, this.ContentChangedHandler);
		}

		private void ContentChangedHandler(object sender, EventArgs args)
			=> this.ChangeTheme(_currentWindowTheme);
		
		private void ChangeTheme(Theme newTheme)
		{
			if (this.ContentHolder.Content is WarningDialogView)
			{
				var newAccentColor = newTheme.ColorScheme.ToLower() == "red" ? "Crimson" : "Red";
				newTheme = ThemeManager.Current.GetTheme($"{newTheme.BaseColorScheme}.{newAccentColor}") ?? newTheme;
			}
			else if (this.ContentHolder.Content is ExceptionDialogView)
			{
				var newBaseColor = newTheme.BaseColorScheme.ToLower().Contains("light")   ? "Dark" : newTheme.BaseColorScheme.ToLower().Contains("dark") ? "Light" : "Dark";
				var newAccentColor = newTheme.ColorScheme.ToLower() == "red" ? "Crimson" : "Red";
				newTheme = ThemeManager.Current.GetTheme($"{newBaseColor}.{newAccentColor}") ?? newTheme;
			}

			ThemeManager.Current.ChangeTheme(this, newTheme);
		}

		#endregion
	}
}