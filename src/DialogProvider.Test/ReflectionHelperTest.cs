using System;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Phoenix.UI.Wpf.Architecture.VMFirst.DialogProvider.ViewModelInterfaces;

namespace DialogProvider.Test
{
	[TestFixture]
	public class ReflectionHelperTest
	{
		#region Data

		public interface IMessage
		{
			string Message { get; }
		}

		class MessageWithAutoProperty : IMessage
		{
			public string Message { get; }
		}

		class MessageWithWithSetableProperty : IMessage
		{
			public string Message { get; set; }
		}

		class MessageWithExplicitProperty : IMessage
		{
			string IMessage.Message { get; }

			public string Message { get; }
		}

		class MessageWithoutInterface
		{
			public string Message { get; }
		}

		private static readonly string InterfacePropertyName = nameof(IMessage.Message);

		private static readonly Type InterfaceType = typeof(IMessage);

		private static readonly PropertyInfo InterfacePropertyInfo = ReflectionHelperTest.InterfaceType.GetProperty(ReflectionHelperTest.InterfacePropertyName);
		
		#endregion

		[Test]
		public void ReflectionHelper_Direct_Property_Injection_Succeeds()
		{
			var value = Guid.NewGuid().ToString();
			var instance = new MessageWithAutoProperty();

			var success = ReflectionHelper.TrySetProperty
			(
				ReflectionHelperTest.InterfacePropertyName,
				instance,
				value,
				ReflectionHelperTest.InterfacePropertyInfo
			);

			Assert.That(success, Is.True);
			Assert.That(instance.Message, Is.EqualTo(value));
		}

		[Test]
		public void ReflectionHelper_Direct_Property_Injection_With_Setter_Succeeds()
		{
			var value = Guid.NewGuid().ToString();
			var instance = new MessageWithWithSetableProperty();

			var success = ReflectionHelper.TrySetProperty
			(
				ReflectionHelperTest.InterfacePropertyName,
				instance,
				value,
				ReflectionHelperTest.InterfacePropertyInfo
			);

			Assert.That(success, Is.True);
			Assert.That(instance.Message, Is.EqualTo(value));
		}

		[Test]
		public void ReflectionHelper_Explicit_Property_Injection_Succeeds()
		{
			var value = Guid.NewGuid().ToString();
			var instance = new MessageWithExplicitProperty();

			var success = ReflectionHelper.TrySetProperty
			(
				ReflectionHelperTest.InterfacePropertyName,
				instance,
				value,
				ReflectionHelperTest.InterfacePropertyInfo
			);

			Assert.That(success, Is.True);
			Assert.That(instance.Message, Is.Null);
			Assert.That(((IMessage)instance).Message, Is.EqualTo(value));
		}

		[Test]
		public void ReflectionHelper_Explicit_Property_Injection_Without_Interface_Info_Fails()
		{
			var value = Guid.NewGuid().ToString();
			var instance = new MessageWithExplicitProperty();

			var success = ReflectionHelper.TrySetProperty
			(
				ReflectionHelperTest.InterfacePropertyName,
				instance,
				value
			);

			Assert.That(success, Is.False);
			Assert.That(instance.Message, Is.Null);
		}

		[Test]
		public void ReflectionHelper_Property_Injection_Without_Interface_Succeeds()
		{
			var value = Guid.NewGuid().ToString();
			var instance = new MessageWithoutInterface();

			var success = ReflectionHelper.TrySetProperty
			(
				ReflectionHelperTest.InterfacePropertyName,
				instance,
				value,
				ReflectionHelperTest.InterfacePropertyInfo
			);

			Assert.That(success, Is.True);
			Assert.That(instance.Message, Is.EqualTo(value));
		}

		[Test]
		public void ReflectionHelper_Property_Injection_Fails()
		{
			var value = Guid.NewGuid().ToString();
			var instance = new Mock<IMessage>().Object; //! Mocked object does not have a setter or a backing field and this is why the test fails.

			var success = ReflectionHelper.TrySetProperty
			(
				ReflectionHelperTest.InterfacePropertyName,
				instance,
				value
			);

			Assert.That(success, Is.False);
			Assert.That(instance.Message, Is.Null);
		}
	}
}