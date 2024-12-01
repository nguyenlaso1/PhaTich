// @sonhg: class: InControl.TinyJSON.ProxyNumber
using System;
using System.Globalization;

namespace InControl.TinyJSON
{
	public sealed class ProxyNumber : Variant
	{
		public ProxyNumber(IConvertible value)
		{
			if (value is string)
			{
				this.value = this.Parse(value as string);
			}
			else
			{
				this.value = value;
			}
		}

		private IConvertible Parse(string value)
		{
			if (value.IndexOfAny(ProxyNumber.floatingPointCharacters) == -1)
			{
				ulong num2;
				if (value[0] == '-')
				{
					long num;
					if (long.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num))
					{
						return num;
					}
				}
				else if (ulong.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num2))
				{
					return num2;
				}
			}
			decimal num3;
			if (decimal.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num3))
			{
				double num4;
				if (num3 == 0m && double.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num4) && num4 != 0.0)
				{
					return num4;
				}
				return num3;
			}
			else
			{
				double num5;
				if (double.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num5))
				{
					return num5;
				}
				return 0;
			}
		}

		public override bool ToBoolean(IFormatProvider provider)
		{
			return this.value.ToBoolean(provider);
		}

		public override byte ToByte(IFormatProvider provider)
		{
			return this.value.ToByte(provider);
		}

		public override char ToChar(IFormatProvider provider)
		{
			return this.value.ToChar(provider);
		}

		public override decimal ToDecimal(IFormatProvider provider)
		{
			return this.value.ToDecimal(provider);
		}

		public override double ToDouble(IFormatProvider provider)
		{
			return this.value.ToDouble(provider);
		}

		public override short ToInt16(IFormatProvider provider)
		{
			return this.value.ToInt16(provider);
		}

		public override int ToInt32(IFormatProvider provider)
		{
			return this.value.ToInt32(provider);
		}

		public override long ToInt64(IFormatProvider provider)
		{
			return this.value.ToInt64(provider);
		}

		public override sbyte ToSByte(IFormatProvider provider)
		{
			return this.value.ToSByte(provider);
		}

		public override float ToSingle(IFormatProvider provider)
		{
			return this.value.ToSingle(provider);
		}

		public override string ToString(IFormatProvider provider)
		{
			return this.value.ToString(provider);
		}

		public override ushort ToUInt16(IFormatProvider provider)
		{
			return this.value.ToUInt16(provider);
		}

		public override uint ToUInt32(IFormatProvider provider)
		{
			return this.value.ToUInt32(provider);
		}

		public override ulong ToUInt64(IFormatProvider provider)
		{
			return this.value.ToUInt64(provider);
		}

		private static readonly char[] floatingPointCharacters = new char[]
		{
			'.',
			'e'
		};

		private IConvertible value;
	}
}
