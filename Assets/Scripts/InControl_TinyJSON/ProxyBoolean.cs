// @sonhg: class: InControl.TinyJSON.ProxyBoolean
using System;

namespace InControl.TinyJSON
{
	public sealed class ProxyBoolean : Variant
	{
		public ProxyBoolean(bool value)
		{
			this.value = value;
		}

		public override bool ToBoolean(IFormatProvider provider)
		{
			return this.value;
		}

		private bool value;
	}
}
