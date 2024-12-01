// @sonhg: class: InControl.TinyJSON.Exclude
using System;

namespace InControl.TinyJSON
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class Exclude : Attribute
	{
	}
}
