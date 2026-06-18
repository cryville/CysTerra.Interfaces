using System;
using System.Text.Json.Serialization;

namespace Cryville.Packages {
	[JsonPolymorphic(TypeDiscriminatorPropertyName = "type", IgnoreUnrecognizedTypeDiscriminators = true)]
	[JsonDerivedType(typeof(FullResourceInfo), "full")]
	[JsonDerivedType(typeof(ExternalResourceInfo), "external")]
	public record ResourceInfo(
		Uri Url
	);
	public record FullResourceInfo(
		Uri Url,
		long Size
	) : ResourceInfo(Url);
	public record ExternalResourceInfo(
		Uri Url
	) : ResourceInfo(Url);
}
