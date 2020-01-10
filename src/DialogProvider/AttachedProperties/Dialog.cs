#region LICENSE NOTICE
//! This file is subject to the terms and conditions defined in file 'LICENSE.md', which is part of this source code package.
#endregion


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Media;

//namespace PhoenixDialog.AttachedProperties
//{
//	/// <summary>
//	/// XAML registration helper for MVVM dialogs.
//	/// </summary>
//	/// <remarks> Inspired by : http://putridparrot.com/blog/wpf-adorners/ </remarks>
//	public static class Dialog
//	{
//		#region Delegates / Events
//		#endregion

//		#region Constants
//		#endregion

//		#region Fields

//		private static readonly object Lock;

//		/// <summary> Collection of all dialog registrations. </summary>
//		private static readonly Dictionary<object, FrameworkElement> Registrations;

//		#endregion

//		#region Properties

//		#region Attached Properties

//		#region Register
		
//		public static readonly DependencyProperty RegisterProperty = DependencyProperty.RegisterAttached
//		(
//			name: "Register",
//			propertyType: typeof(object),
//			ownerType: typeof(Dialog),
//			defaultMetadata: new PropertyMetadata
//			(
//				defaultValue: default(object),
//				propertyChangedCallback: Dialog.RegisterCallback
//			)
//		);

//		public static object GetRegister(DependencyObject element)
//		{
//			return element.GetValue(Dialog.RegisterProperty);
//		}

//		/// <summary>
//		/// Use this to register a dialog directly for this element.
//		/// </summary>
//		public static void SetRegister(DependencyObject element, object value)
//		{
//			element.SetValue(Dialog.RegisterProperty, value);
//		}

//		#endregion

//		#region RegisterForWindow

//		public static readonly DependencyProperty RegisterForWindowProperty = DependencyProperty.RegisterAttached
//		(
//			name: "RegisterForWindow",
//			propertyType: typeof(object),
//			ownerType: typeof(Dialog),
//			defaultMetadata: new PropertyMetadata
//				(
//					defaultValue: default(object),
//					propertyChangedCallback: Dialog.RegisterWindowCallback
//				)
//		);

//		public static object GetRegisterForWindow(DependencyObject element)
//		{
//			return element.GetValue(RegisterForWindowProperty);
//		}

//		/// <summary>
//		/// Use this to register a dialog for window containing this element.
//		/// </summary>
//		public static void SetRegisterForWindow(DependencyObject element, object value)
//		{
//			element.SetValue(RegisterForWindowProperty, value);
//		}

//		#endregion

//		#region RegisterForParent

//		public static readonly DependencyProperty RegisterForParentProperty = DependencyProperty.RegisterAttached
//		(
//			name: "RegisterForParent",
//			propertyType: typeof(object),
//			ownerType: typeof(Dialog),
//			defaultMetadata: new PropertyMetadata
//				(
//					defaultValue: default(object),
//					propertyChangedCallback: Dialog.RegisterParentCallback
//				)
//		);

//		public static object GetRegisterForParent(DependencyObject element)
//		{
//			return element.GetValue(RegisterForWindowProperty);
//		}

//		/// <summary>
//		/// Use this to register a dialog for the parent element of this element.
//		/// </summary>
//		public static void SetRegisterForParent(DependencyObject element, object value)
//		{
//			element.SetValue(RegisterForWindowProperty, value);
//		}

//		#endregion

//		#endregion

//		#endregion

//		#region (De)Constructors

//		static Dialog()
//		{
//			Dialog.Lock = new object();
//			Dialog.Registrations = new Dictionary<object, FrameworkElement>();
//		}

//		#endregion

//		#region Methods

//		private static void RegisterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
//		{
//			if (dependencyObject is FrameworkElement frameworkElement)
//			{
//				Dialog.Register(frameworkElement, args.OldValue, args.NewValue);
//			}
//		}

//		private static void RegisterWindowCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
//		{
//			var window = Window.GetWindow(dependencyObject);
//			if (window != null)
//			{
//				Dialog.Register(window, args.OldValue, args.NewValue);
//			}
//		}

//		private static void RegisterParentCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
//		{
//			if (dependencyObject is FrameworkElement frameworkElement && frameworkElement.Parent is FrameworkElement parent)
//			{
//				Dialog.Register(parent, args.OldValue, args.NewValue);
//			}
//		}

//		private static void Register(FrameworkElement frameworkElement, object oldContext, object newContext)
//		{
//			Dialog.RemoveOldDialog(oldContext);
//			Dialog.RegisterNewDialog(newContext, frameworkElement);
//		}

//		private static void RemoveOldDialog(object context)
//		{
//			lock (Lock)
//			{
//				// Always remove the old value from the registrations.
//				if (context != null)
//				{
//					Dialog.Registrations.Remove(context);
//				}
//			}
//		}

//		private static void RegisterNewDialog(object context, FrameworkElement frameworkElement)
//		{
//			lock (Lock)
//			{
//				// Add a new registration if possible.
//				if (context != null && frameworkElement != null && !Dialog.Registrations.ContainsKey(context))
//				{
//					// Check if the visual is already loaded.
//					if (frameworkElement.IsLoaded)
//					{
//						// YES: Directly add a new registration.
//						Dialog.Registrations.Add(context, frameworkElement);
//					}
//					else
//					{
//						// NO: Add the new registration after the element has been loaded.
//						Dialog.HandleFrameworkElementLoaded(context, frameworkElement);
//					}
//				}
//			}
//		}

//		private static void HandleFrameworkElementLoaded(object context, FrameworkElement frameworkElement)
//		{
//			void LocalHandler(object sender, RoutedEventArgs args)
//			{
//				// Remove the handler itself.
//				frameworkElement.Loaded -= LocalHandler;

//				lock (Lock)
//				{
//					Dialog.Registrations.Add(context, frameworkElement);
//				}
//			}
			
//			// NO: Hook up to the loaded event of the visual.
//			frameworkElement.Loaded += LocalHandler;
//		}

//		//private static void FrameworkElementLoaded(object sender, RoutedEventArgs args)
//		//{
//		//	if (sender is FrameworkElement frameworkElement)
//		//	{
//		//		// Remove this event handler.
//		//		frameworkElement.Loaded -= Dialog.FrameworkElementLoaded;
				
//		//		// Update the registration.
//		//		Dialog.UpdateRegistration(frameworkElement);
//		//	}
//		//}
		
//		//private static void UpdateRegistration(FrameworkElement frameworkElement)
//		//{
//		//	// Find the first matching framework element.
//		//	var matches = Dialog.TemporaryRegistrations
//		//		.Where(pair => Object.ReferenceEquals(pair.Value, frameworkElement))
//		//		.Select(pair => pair.Key)
//		//		.ToArray()
//		//		;
//		//	if (matches.Length == 1)
//		//	{
//		//		var context = matches.First();
//		//		Dialog.TemporaryRegistrations.TryRemove(context, out var unused);
//		//		Dialog.Registrations.TryAdd(context, frameworkElement);
//		//	}
//		//}

//		//private static void UpdateRegistration(DialogRegistration registration)
//		//{
//		//	// Try to get the adorner layer of the framework element. If this fails, check if the element is already loaded and if not, hook up to its loaded event to later try again.
//		//	var adornerLayer = Dialog.GetAdornerLayer(registration.FrameworkElement);
//		//	if (adornerLayer != null)
//		//	{
//		//		registration.AdornerLayer = adornerLayer;
//		//	}
//		//	else if (!registration.FrameworkElement.IsLoaded)
//		//	{
//		//		registration.FrameworkElement.Loaded += Dialog.FrameworkElementLoaded;
//		//	}
//		//	else
//		//	{
//		//		// TODO: Error message...
//		//	}
//		//}

//		//internal static bool TryGetDialogRegistration(object context, out DialogRegistration registration)
//		//	=> Dialog.Registrations1.TryGet(context, out registration);

//		internal static bool TryGetFrameworkElement(object context, out FrameworkElement frameworkElement)
//			=> Dialog.Registrations.TryGetValue(context, out frameworkElement);

//		#endregion
//	}
//}