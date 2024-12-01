// @sonhg: class: InControl.TinyJSON.Encoder
using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace InControl.TinyJSON
{
	public sealed class Encoder
	{
		private Encoder(EncodeOptions options)
		{
			this.options = options;
			this.builder = new StringBuilder();
			this.indent = 0;
		}

		public static string Encode(object obj, EncodeOptions options = EncodeOptions.None)
		{
			Encoder encoder = new Encoder(options);
			encoder.EncodeValue(obj, false);
			return encoder.builder.ToString();
		}

		private bool PrettyPrintEnabled
		{
			get
			{
				return (this.options & EncodeOptions.PrettyPrint) == EncodeOptions.PrettyPrint;
			}
		}

		private bool TypeHintsEnabled
		{
			get
			{
				return (this.options & EncodeOptions.NoTypeHints) != EncodeOptions.NoTypeHints;
			}
		}

		private void EncodeValue(object value, bool forceTypeHint)
		{
			string value2;
			Array value3;
			IList value4;
			IDictionary value5;
			if (value == null)
			{
				this.builder.Append("null");
			}
			else if ((value2 = (value as string)) != null)
			{
				this.EncodeString(value2);
			}
			else if (value is bool)
			{
				this.builder.Append(value.ToString().ToLower());
			}
			else if (value is Enum)
			{
				this.EncodeString(value.ToString());
			}
			else if ((value3 = (value as Array)) != null)
			{
				this.EncodeArray(value3, forceTypeHint);
			}
			else if ((value4 = (value as IList)) != null)
			{
				this.EncodeList(value4, forceTypeHint);
			}
			else if ((value5 = (value as IDictionary)) != null)
			{
				this.EncodeDictionary(value5, forceTypeHint);
			}
			else if (value is char)
			{
				this.EncodeString(value.ToString());
			}
			else
			{
				this.EncodeOther(value, forceTypeHint);
			}
		}

		private void EncodeObject(object value, bool forceTypeHint)
		{
			Type type = value.GetType();
			this.AppendOpenBrace();
			forceTypeHint = (forceTypeHint || this.TypeHintsEnabled);
			bool firstItem = !forceTypeHint;
			if (forceTypeHint)
			{
				if (this.PrettyPrintEnabled)
				{
					this.AppendIndent();
				}
				this.EncodeString("@type");
				this.AppendColon();
				this.EncodeString(type.FullName);
				firstItem = false;
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				bool forceTypeHint2 = false;
				bool flag = fieldInfo.IsPublic;
				foreach (object obj in fieldInfo.GetCustomAttributes(true))
				{
					if (Encoder.excludeAttrType.IsAssignableFrom(obj.GetType()))
					{
						flag = false;
					}
					if (Encoder.includeAttrType.IsAssignableFrom(obj.GetType()))
					{
						flag = true;
					}
					if (Encoder.typeHintAttrType.IsAssignableFrom(obj.GetType()))
					{
						forceTypeHint2 = true;
					}
				}
				if (flag)
				{
					this.AppendComma(firstItem);
					this.EncodeString(fieldInfo.Name);
					this.AppendColon();
					this.EncodeValue(fieldInfo.GetValue(value), forceTypeHint2);
					firstItem = false;
				}
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanRead)
				{
					bool forceTypeHint3 = false;
					bool flag2 = false;
					foreach (object obj2 in propertyInfo.GetCustomAttributes(true))
					{
						if (Encoder.includeAttrType.IsAssignableFrom(obj2.GetType()))
						{
							flag2 = true;
						}
						if (Encoder.typeHintAttrType.IsAssignableFrom(obj2.GetType()))
						{
							forceTypeHint3 = true;
						}
					}
					if (flag2)
					{
						this.AppendComma(firstItem);
						this.EncodeString(propertyInfo.Name);
						this.AppendColon();
						this.EncodeValue(propertyInfo.GetValue(value, null), forceTypeHint3);
						firstItem = false;
					}
				}
			}
			this.AppendCloseBrace();
		}

		private void EncodeDictionary(IDictionary value, bool forceTypeHint)
		{
			if (value.Count == 0)
			{
				this.builder.Append("{}");
			}
			else
			{
				this.AppendOpenBrace();
				bool firstItem = true;
				foreach (object obj in value.Keys)
				{
					this.AppendComma(firstItem);
					this.EncodeString(obj.ToString());
					this.AppendColon();
					this.EncodeValue(value[obj], forceTypeHint);
					firstItem = false;
				}
				this.AppendCloseBrace();
			}
		}

		private void EncodeList(IList value, bool forceTypeHint)
		{
			if (value.Count == 0)
			{
				this.builder.Append("[]");
			}
			else
			{
				this.AppendOpenBracket();
				bool firstItem = true;
				foreach (object value2 in value)
				{
					this.AppendComma(firstItem);
					this.EncodeValue(value2, forceTypeHint);
					firstItem = false;
				}
				this.AppendCloseBracket();
			}
		}

		private void EncodeArray(Array value, bool forceTypeHint)
		{
			if (value.Rank == 1)
			{
				this.EncodeList(value, forceTypeHint);
			}
			else
			{
				int[] indices = new int[value.Rank];
				this.EncodeArrayRank(value, 0, indices, forceTypeHint);
			}
		}

		private void EncodeArrayRank(Array value, int rank, int[] indices, bool forceTypeHint)
		{
			this.AppendOpenBracket();
			int lowerBound = value.GetLowerBound(rank);
			int upperBound = value.GetUpperBound(rank);
			if (rank == value.Rank - 1)
			{
				for (int i = lowerBound; i <= upperBound; i++)
				{
					indices[rank] = i;
					this.AppendComma(i == lowerBound);
					this.EncodeValue(value.GetValue(indices), forceTypeHint);
				}
			}
			else
			{
				for (int j = lowerBound; j <= upperBound; j++)
				{
					indices[rank] = j;
					this.AppendComma(j == lowerBound);
					this.EncodeArrayRank(value, rank + 1, indices, forceTypeHint);
				}
			}
			this.AppendCloseBracket();
		}

		private void EncodeString(string value)
		{
			this.builder.Append('"');
			char[] array = value.ToCharArray();
			foreach (char c in array)
			{
				char c2 = c;
				switch (c2)
				{
				case '\b':
					this.builder.Append("\\b");
					break;
				case '\t':
					this.builder.Append("\\t");
					break;
				case '\n':
					this.builder.Append("\\n");
					break;
				default:
					if (c2 != '"')
					{
						if (c2 != '\\')
						{
							int num = Convert.ToInt32(c);
							if (num >= 32 && num <= 126)
							{
								this.builder.Append(c);
							}
							else
							{
								this.builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
							}
						}
						else
						{
							this.builder.Append("\\\\");
						}
					}
					else
					{
						this.builder.Append("\\\"");
					}
					break;
				case '\f':
					this.builder.Append("\\f");
					break;
				case '\r':
					this.builder.Append("\\r");
					break;
				}
			}
			this.builder.Append('"');
		}

		private void EncodeOther(object value, bool forceTypeHint)
		{
			if (value is float || value is double || value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong || value is decimal)
			{
				this.builder.Append(value.ToString());
			}
			else
			{
				this.EncodeObject(value, forceTypeHint);
			}
		}

		private void AppendIndent()
		{
			for (int i = 0; i < this.indent; i++)
			{
				this.builder.Append('\t');
			}
		}

		private void AppendOpenBrace()
		{
			this.builder.Append('{');
			if (this.PrettyPrintEnabled)
			{
				this.builder.Append('\n');
				this.indent++;
			}
		}

		private void AppendCloseBrace()
		{
			if (this.PrettyPrintEnabled)
			{
				this.builder.Append('\n');
				this.indent--;
				this.AppendIndent();
			}
			this.builder.Append('}');
		}

		private void AppendOpenBracket()
		{
			this.builder.Append('[');
			if (this.PrettyPrintEnabled)
			{
				this.builder.Append('\n');
				this.indent++;
			}
		}

		private void AppendCloseBracket()
		{
			if (this.PrettyPrintEnabled)
			{
				this.builder.Append('\n');
				this.indent--;
				this.AppendIndent();
			}
			this.builder.Append(']');
		}

		private void AppendComma(bool firstItem)
		{
			if (!firstItem)
			{
				this.builder.Append(',');
				if (this.PrettyPrintEnabled)
				{
					this.builder.Append('\n');
				}
			}
			if (this.PrettyPrintEnabled)
			{
				this.AppendIndent();
			}
		}

		private void AppendColon()
		{
			this.builder.Append(':');
			if (this.PrettyPrintEnabled)
			{
				this.builder.Append(' ');
			}
		}

		private static readonly Type includeAttrType = typeof(Include);

		private static readonly Type excludeAttrType = typeof(Exclude);

		private static readonly Type typeHintAttrType = typeof(TypeHint);

		private StringBuilder builder;

		private EncodeOptions options;

		private int indent;
	}
}
