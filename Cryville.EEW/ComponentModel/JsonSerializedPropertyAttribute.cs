using System;

namespace Cryville.EEW.ComponentModel {
	/// <summary>
	/// Specifies that the property or all the properties within the scope are serialized with JSON.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class JsonSerializedPropertyAttribute : Attribute { }
}
