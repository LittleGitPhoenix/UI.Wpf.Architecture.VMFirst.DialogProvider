using System;
using System.Linq;
using System.Reflection;

namespace Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces
{
	internal static class ReflectionHelper
	{
		/// <summary>
		/// Get the field name from <paramref name="propertyInfo"/> automatically build for explicitly implemented properties.
		/// </summary>
		internal static string GetInterfaceBackingFieldName(PropertyInfo propertyInfo)
		{
			return $"<{propertyInfo.DeclaringType?.FullName?.Replace('+', '.')}.{propertyInfo.Name}>k__BackingField";
		}

		/// <summary>
		/// Get the field name from <paramref name="propertyInfo"/> automatically build for properties.
		/// </summary>
		internal static string GetInstanceBackingFieldName(PropertyInfo propertyInfo)
		{
			return $"<{propertyInfo.Name}>k__BackingField";
		}
		
		/// <summary>
		/// Applies the <paramref name="value"/> to the property identified by <paramref name="propertyName"/> of the <paramref name="instance"/>.
		/// </summary>
		/// <param name="propertyName"> The name of the property to change. </param>
		/// <param name="instance"> The instance where to apply the new value to. </param>
		/// <param name="value"> The new value. </param>
		/// <param name="interfacePropertyInfo"> Optional <see cref="PropertyInfo"/> of an interface property. Needed in case of explicit interface implementation, where it is possible to have multiple properties with the same name. </param>
		/// <param name="explicitBackingFieldName"> Optional name of the interface backing field used for explicit implemented properties. </param>
		/// <param name="backingFieldName"> Optional name of the backing field. </param>
		/// <returns></returns>
		internal static bool TrySetProperty(string propertyName, object instance, object value, PropertyInfo interfacePropertyInfo = null, string explicitBackingFieldName = null, string backingFieldName = null)
		{
			try
			{
				// If the property info itself is not specified, then do nothing.
				if (String.IsNullOrWhiteSpace(propertyName)) return false;
				if (instance is null) return false;

				var type = instance.GetType();
				var valueType = value?.GetType();
				var propertyInfos = type
					.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(info => info.Name.EndsWith(propertyName))              // Allow only matching names. 'EndsWith' is used to get explicitly implemented properties.
					.Where(info => info.PropertyType.IsAssignableFrom(valueType ?? info.PropertyType)) // Allow only matching types or in case the new value is null, use the type itself.
					.ToArray()
					;

				if (!propertyInfos.Any()) return false;

				// Check if multiple properties where found. This can happen in case of explicit interface implementation.
				PropertyInfo propertyInfo;
				if (propertyInfos.Length == 1)
				{
					// Just use the single matching property.
					propertyInfo = propertyInfos.Single();
				}
				else if (interfacePropertyInfo != null)
				{
					// Use the property matching the optionally specified interface property.
					var interfaceType = interfacePropertyInfo.DeclaringType;
					var explicitPropertyName = $"{interfaceType?.FullName?.Replace('+', '.')}.{interfacePropertyInfo?.Name}";
					explicitBackingFieldName ??= GetInterfaceBackingFieldName(interfacePropertyInfo);
					propertyInfo = propertyInfos.SingleOrDefault(info => info.Name == explicitPropertyName);
					if (propertyInfo is null) return false;
				}
				else
				{
					return false;
				}
				
				// Check if the property info has an accessible setter.
				if (propertyInfo.CanWrite)
				{
					propertyInfo.SetValue(instance, value);
					return true;
				}
				
				// If the property doesn't have a setter, then its backing field must be manipulated through reflection.
				//! Always try to find a backing filed for explicit implementation first.
				FieldInfo fieldInfo = null;
				if (!String.IsNullOrWhiteSpace(explicitBackingFieldName))
				{
					fieldInfo = type.GetField(explicitBackingFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
				}
				if (fieldInfo is null)
				{
					backingFieldName ??= GetInstanceBackingFieldName(propertyInfo);
					fieldInfo = type.GetField(backingFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
				}
				if (fieldInfo != null)
				{
					fieldInfo.SetValue(instance, value);
					return true;
				}
			}
			catch (Exception ex)
			{
				if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
			}

			return false;
		}
	}
}