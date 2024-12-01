// @sonhg: class: InControl.TinyJSON.Variant
using System;
using System.Globalization;

namespace InControl.TinyJSON
{
	public abstract class Variant : IConvertible
	{
		public void Make<T>(out T item)
		{
			JSON.MakeInto<T>(this, out item);
		}

		public T Make<T>()
		{
			T result;
			JSON.MakeInto<T>(this, out result);
			return result;
		}

		public virtual TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		public virtual object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new InvalidCastException(string.Concat(new object[]
			{
				"Cannot convert ",
				base.GetType(),
				" to ",
				conversionType.Name
			}));
		}

		public virtual DateTime ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to DateTime");
		}

		public virtual bool ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Boolean");
		}

		public virtual byte ToByte(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Byte");
		}

		public virtual char ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Char");
		}

		public virtual decimal ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Decimal");
		}

		public virtual double ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Double");
		}

		public virtual short ToInt16(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Int16");
		}

		public virtual int ToInt32(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Int32");
		}

		public virtual long ToInt64(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Int64");
		}

		public virtual sbyte ToSByte(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to SByte");
		}

		public virtual float ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to Single");
		}

		public virtual string ToString(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to String");
		}

		public virtual ushort ToUInt16(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to UInt16");
		}

		public virtual uint ToUInt32(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to UInt32");
		}

		public virtual ulong ToUInt64(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot convert " + base.GetType() + " to UInt64");
		}

		public override string ToString()
		{
			return this.ToString(Variant.formatProvider);
		}

		public virtual Variant this[string key]
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public virtual Variant this[int index]
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public static implicit operator bool(Variant variant)
		{
			return variant.ToBoolean(Variant.formatProvider);
		}

		public static implicit operator float(Variant variant)
		{
			return variant.ToSingle(Variant.formatProvider);
		}

		public static implicit operator double(Variant variant)
		{
			return variant.ToDouble(Variant.formatProvider);
		}

		public static implicit operator ushort(Variant variant)
		{
			return variant.ToUInt16(Variant.formatProvider);
		}

		public static implicit operator short(Variant variant)
		{
			return variant.ToInt16(Variant.formatProvider);
		}

		public static implicit operator uint(Variant variant)
		{
			return variant.ToUInt32(Variant.formatProvider);
		}

		public static implicit operator int(Variant variant)
		{
			return variant.ToInt32(Variant.formatProvider);
		}

		public static implicit operator ulong(Variant variant)
		{
			return variant.ToUInt64(Variant.formatProvider);
		}

		public static implicit operator long(Variant variant)
		{
			return variant.ToInt64(Variant.formatProvider);
		}

		public static implicit operator decimal(Variant variant)
		{
			return variant.ToDecimal(Variant.formatProvider);
		}

		public static implicit operator string(Variant variant)
		{
			return variant.ToString(Variant.formatProvider);
		}

		protected static IFormatProvider formatProvider = new NumberFormatInfo();
	}
}
