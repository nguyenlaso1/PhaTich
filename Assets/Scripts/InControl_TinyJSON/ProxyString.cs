// @sonhg: class: InControl.TinyJSON.ProxyString
using System;

namespace InControl.TinyJSON
{
	public sealed class ProxyString : Variant
	{
		public ProxyString(string value)
		{
			this.value = value;
		}

		public override string ToString(IFormatProvider provider)
		{
			return this.value;
		}

		private string value;
	}
}
