#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Phoenix.UI.Wpf.DialogProvider.Classes
{
	/// <summary>
	/// Default <see cref="IDialogManager"/> using the current applications <see cref="Application.MainWindow"/> for showing dialogs.
	/// </summary>
	public class DefaultDialogManager : DialogManager, IDefaultDialogManager
	{
		#region Delegates / Events

		/// <summary> Is raised once the <see cref="MainWindow"/> is available. </summary>
		private static event EventHandler MainWindowAvailable;

		#endregion

		#region Constants
		#endregion

		#region Fields

		private static readonly object Lock;

		#endregion

		#region Properties

		/// <summary> The current applications <see cref="Application.MainWindow"/>. </summary>
		private static Window MainWindow
		{
			get
			{
				lock (DefaultDialogManager.Lock)
				{
					return _mainWindow;
				}
			}
			set
			{
				lock (DefaultDialogManager.Lock)
				{
					// Don't set null values.
					if (value is null) return;

					// Allow setting only once.
					if (_mainWindow != null) return;

					// Remove listener for application activation.
					Application.Current.Activated -= DefaultDialogManager.HandleApplicationActivated;

					// Save the value.
					_mainWindow = value;

					// Raise the window available event.
					DefaultDialogManager.MainWindowAvailable?.Invoke(null, EventArgs.Empty);
				}
			}
		}
		private static Window _mainWindow;

		#endregion

		#region (De)Constructors

		/// <summary>
		/// Static constructor
		/// </summary>
		static DefaultDialogManager()
		{
			DefaultDialogManager.Lock = new object();

			// Attach to the applications activated event, so that an reference to its main window can be obtained once it is available.
			Application.Current.Activated += DefaultDialogManager.HandleApplicationActivated;
			
			// Try to get a reference to the applications main window regardless of the activation event (it could have been fired already).
			DefaultDialogManager.MainWindow = DefaultDialogManager.GetMainWindow();
		}

		/// <inheritdoc />
		public DefaultDialogManager(DialogAssemblyViewProvider dialogAssemblyViewProvider)
			: this(dialogAssemblyViewProvider, new IViewProvider[] { new AssemblyViewProvider(), new DefaultViewProvider() }) { }

		/// <inheritdoc />
		public DefaultDialogManager(DialogAssemblyViewProvider dialogAssemblyViewProvider, ICollection<IViewProvider> viewProviders)
			: base(dialogAssemblyViewProvider, viewProviders)
		{
			lock (DefaultDialogManager.Lock)
			{
				if (DefaultDialogManager.MainWindow is null)
				{
					void HandleMainWindowAvailable(object sender, EventArgs args)
					{
						DefaultDialogManager.MainWindowAvailable -= HandleMainWindowAvailable;
						base.Initialize(DefaultDialogManager.MainWindow);
					}

					DefaultDialogManager.MainWindowAvailable += HandleMainWindowAvailable;
				}
				else
				{
					base.Initialize(DefaultDialogManager.MainWindow);
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Handler for the current applications <see cref="Application.Activated"/> event.
		/// </summary>
		private static void HandleApplicationActivated(object sender, EventArgs args)
		{
			// Try to get a reference to the applications main window.
			DefaultDialogManager.MainWindow = DefaultDialogManager.GetMainWindow();
		}

		/// <summary>
		/// Get the current applications <see cref="Application.MainWindow"/>.
		/// </summary>
		/// <returns> A reference to the current applications <see cref="Application.MainWindow"/>. </returns>
		private static Window GetMainWindow()
		{
			Window GetWindow()
			{
				return Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive) ?? Application.Current.MainWindow;
			}

			return Application.Current.Dispatcher.CheckAccess() ? GetWindow() : Application.Current.Dispatcher.Invoke(GetWindow);
		}

		#endregion
	}
}