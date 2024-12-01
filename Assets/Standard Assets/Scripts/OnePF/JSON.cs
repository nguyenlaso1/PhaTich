// @plugin: class: OnePF.JSON
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace OnePF
{
	public class JSON
	{
		public JSON()
		{
		}

		public JSON(string jsonString)
		{
			this.serialized = jsonString;
		}

		public object this[string fieldName]
		{
			get
			{
				if (this.fields.ContainsKey(fieldName))
				{
					return this.fields[fieldName];
				}
				return null;
			}
			set
			{
				if (this.fields.ContainsKey(fieldName))
				{
					this.fields[fieldName] = value;
				}
				else
				{
					this.fields.Add(fieldName, value);
				}
			}
		}

		public string ToString(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return Convert.ToString(this.fields[fieldName]);
			}
			return string.Empty;
		}

		public int ToInt(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return Convert.ToInt32(this.fields[fieldName]);
			}
			return 0;
		}

		public long ToLong(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return Convert.ToInt64(this.fields[fieldName]);
			}
			return 0L;
		}

		public float ToFloat(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName))
			{
				return Convert.ToSingle(this.fields[fieldName]);
			}
			return 0f;
		}

		public bool ToBoolean(string fieldName)
		{
			return this.fields.ContainsKey(fieldName) && Convert.ToBoolean(this.fields[fieldName]);
		}

		public string serialized
		{
			get
			{
				return JSON._JSON.Serialize(this);
			}
			set
			{
				JSON json = JSON._JSON.Deserialize(value);
				if (json != null)
				{
					this.fields = json.fields;
				}
			}
		}

		public JSON ToJSON(string fieldName)
		{
			if (!this.fields.ContainsKey(fieldName))
			{
				this.fields.Add(fieldName, new JSON());
			}
			return (JSON)this[fieldName];
		}

		public T[] ToArray<T>(string fieldName)
		{
			if (this.fields.ContainsKey(fieldName) && this.fields[fieldName] is IEnumerable)
			{
				List<T> list = new List<T>();
				foreach (object obj in (this.fields[fieldName] as IEnumerable))
				{
					if (list is List<string>)
					{
						(list as List<string>).Add(Convert.ToString(obj));
					}
					else if (list is List<int>)
					{
						(list as List<int>).Add(Convert.ToInt32(obj));
					}
					else if (list is List<float>)
					{
						(list as List<float>).Add(Convert.ToSingle(obj));
					}
					else if (list is List<bool>)
					{
						(list as List<bool>).Add(Convert.ToBoolean(obj));
					}
					else if (list is List<Vector2>)
					{
						(list as List<Vector2>).Add((JSON)obj);
					}
					else if (list is List<Vector3>)
					{
						(list as List<Vector3>).Add((JSON)obj);
					}
					else if (list is List<Rect>)
					{
						(list as List<Rect>).Add((JSON)obj);
					}
					else if (list is List<Color>)
					{
						(list as List<Color>).Add((JSON)obj);
					}
					else if (list is List<Color32>)
					{
						(list as List<Color32>).Add((JSON)obj);
					}
					else if (list is List<Quaternion>)
					{
						(list as List<Quaternion>).Add((JSON)obj);
					}
					else if (list is List<JSON>)
					{
						(list as List<JSON>).Add((JSON)obj);
					}
				}
				return list.ToArray();
			}
			return new T[0];
		}

		public static implicit operator Vector2(JSON value)
		{
			return new Vector3(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]));
		}

		public static explicit operator JSON(Vector2 value)
		{
			JSON json = new JSON();
			json["x"] = value.x;
			json["y"] = value.y;
			return json;
		}

		public static implicit operator Vector3(JSON value)
		{
			return new Vector3(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]), Convert.ToSingle(value["z"]));
		}

		public static explicit operator JSON(Vector3 value)
		{
			JSON json = new JSON();
			json["x"] = value.x;
			json["y"] = value.y;
			json["z"] = value.z;
			return json;
		}

		public static implicit operator Quaternion(JSON value)
		{
			return new Quaternion(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]), Convert.ToSingle(value["z"]), Convert.ToSingle(value["w"]));
		}

		public static explicit operator JSON(Quaternion value)
		{
			JSON json = new JSON();
			json["x"] = value.x;
			json["y"] = value.y;
			json["z"] = value.z;
			json["w"] = value.w;
			return json;
		}

		public static implicit operator Color(JSON value)
		{
			return new Color(Convert.ToSingle(value["r"]), Convert.ToSingle(value["g"]), Convert.ToSingle(value["b"]), Convert.ToSingle(value["a"]));
		}

		public static explicit operator JSON(Color value)
		{
			JSON json = new JSON();
			json["r"] = value.r;
			json["g"] = value.g;
			json["b"] = value.b;
			json["a"] = value.a;
			return json;
		}

		public static implicit operator Color32(JSON value)
		{
			return new Color32(Convert.ToByte(value["r"]), Convert.ToByte(value["g"]), Convert.ToByte(value["b"]), Convert.ToByte(value["a"]));
		}

		public static explicit operator JSON(Color32 value)
		{
			JSON json = new JSON();
			json["r"] = value.r;
			json["g"] = value.g;
			json["b"] = value.b;
			json["a"] = value.a;
			return json;
		}

		public static implicit operator Rect(JSON value)
		{
			return new Rect((float)Convert.ToByte(value["left"]), (float)Convert.ToByte(value["top"]), (float)Convert.ToByte(value["width"]), (float)Convert.ToByte(value["height"]));
		}

		public static explicit operator JSON(Rect value)
		{
			JSON json = new JSON();
			json["left"] = value.xMin;
			json["top"] = value.yMax;
			json["width"] = value.width;
			json["height"] = value.height;
			return json;
		}

		public Dictionary<string, object> fields = new Dictionary<string, object>();

		private sealed class _JSON
		{
			public static JSON Deserialize(string json)
			{
				if (json == null)
				{
					return null;
				}
				return JSON._JSON.Parser.Parse(json);
			}

			public static string Serialize(JSON obj)
			{
				return JSON._JSON.Serializer.Serialize(obj);
			}

			private sealed class Parser : IDisposable
			{
				private Parser(string jsonString)
				{
					this.json = new StringReader(jsonString);
				}

				public static JSON Parse(string jsonString)
				{
					JSON result;
					using (JSON._JSON.Parser parser = new JSON._JSON.Parser(jsonString))
					{
						result = (parser.ParseValue() as JSON);
					}
					return result;
				}

				public void Dispose()
				{
					this.json.Dispose();
					this.json = null;
				}

				private JSON ParseObject()
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					JSON json = new JSON();
					json.fields = dictionary;
					this.json.Read();
					for (;;)
					{
						JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
						switch (nextToken)
						{
						case JSON._JSON.Parser.TOKEN.NONE:
							goto IL_44;
						default:
							if (nextToken != JSON._JSON.Parser.TOKEN.COMMA)
							{
								string text = this.ParseString();
								if (text == null)
								{
									goto Block_2;
								}
								if (this.NextToken != JSON._JSON.Parser.TOKEN.COLON)
								{
									goto Block_3;
								}
								this.json.Read();
								dictionary[text] = this.ParseValue();
							}
							break;
						case JSON._JSON.Parser.TOKEN.CURLY_CLOSE:
							return json;
						}
					}
					IL_44:
					return null;
					Block_2:
					return null;
					Block_3:
					return null;
				}

				private List<object> ParseArray()
				{
					List<object> list = new List<object>();
					this.json.Read();
					bool flag = true;
					while (flag)
					{
						JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
						JSON._JSON.Parser.TOKEN token = nextToken;
						switch (token)
						{
						case JSON._JSON.Parser.TOKEN.SQUARED_CLOSE:
							flag = false;
							break;
						default:
						{
							if (token == JSON._JSON.Parser.TOKEN.NONE)
							{
								return null;
							}
							object item = this.ParseByToken(nextToken);
							list.Add(item);
							break;
						}
						case JSON._JSON.Parser.TOKEN.COMMA:
							break;
						}
					}
					return list;
				}

				private object ParseValue()
				{
					JSON._JSON.Parser.TOKEN nextToken = this.NextToken;
					return this.ParseByToken(nextToken);
				}

				private object ParseByToken(JSON._JSON.Parser.TOKEN token)
				{
					switch (token)
					{
					case JSON._JSON.Parser.TOKEN.CURLY_OPEN:
						return this.ParseObject();
					case JSON._JSON.Parser.TOKEN.SQUARED_OPEN:
						return this.ParseArray();
					case JSON._JSON.Parser.TOKEN.STRING:
						return this.ParseString();
					case JSON._JSON.Parser.TOKEN.NUMBER:
						return this.ParseNumber();
					case JSON._JSON.Parser.TOKEN.TRUE:
						return true;
					case JSON._JSON.Parser.TOKEN.FALSE:
						return false;
					case JSON._JSON.Parser.TOKEN.NULL:
						return null;
					}
					return null;
				}

				private string ParseString()
				{
					StringBuilder stringBuilder = new StringBuilder();
					this.json.Read();
					bool flag = true;
					while (flag)
					{
						if (this.json.Peek() == -1)
						{
							break;
						}
						char nextChar = this.NextChar;
						char c = nextChar;
						if (c != '"')
						{
							if (c != '\\')
							{
								stringBuilder.Append(nextChar);
							}
							else if (this.json.Peek() == -1)
							{
								flag = false;
							}
							else
							{
								nextChar = this.NextChar;
								char c2 = nextChar;
								switch (c2)
								{
								case 'n':
									stringBuilder.Append('\n');
									break;
								default:
									if (c2 != '"' && c2 != '/' && c2 != '\\')
									{
										if (c2 != 'b')
										{
											if (c2 == 'f')
											{
												stringBuilder.Append('\f');
											}
										}
										else
										{
											stringBuilder.Append('\b');
										}
									}
									else
									{
										stringBuilder.Append(nextChar);
									}
									break;
								case 'r':
									stringBuilder.Append('\r');
									break;
								case 't':
									stringBuilder.Append('\t');
									break;
								case 'u':
								{
									StringBuilder stringBuilder2 = new StringBuilder();
									for (int i = 0; i < 4; i++)
									{
										stringBuilder2.Append(this.NextChar);
									}
									stringBuilder.Append((char)Convert.ToInt32(stringBuilder2.ToString(), 16));
									break;
								}
								}
							}
						}
						else
						{
							flag = false;
						}
					}
					return stringBuilder.ToString();
				}

				private object ParseNumber()
				{
					string nextWord = this.NextWord;
					if (nextWord.IndexOf('.') == -1)
					{
						long num;
						long.TryParse(nextWord, out num);
						return num;
					}
					double num2;
					double.TryParse(nextWord, out num2);
					return num2;
				}

				private void EatWhitespace()
				{
					while (" \t\n\r".IndexOf(this.PeekChar) != -1)
					{
						this.json.Read();
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
				}

				private char PeekChar
				{
					get
					{
						return Convert.ToChar(this.json.Peek());
					}
				}

				private char NextChar
				{
					get
					{
						return Convert.ToChar(this.json.Read());
					}
				}

				private string NextWord
				{
					get
					{
						StringBuilder stringBuilder = new StringBuilder();
						while (" \t\n\r{}[],:\"".IndexOf(this.PeekChar) == -1)
						{
							stringBuilder.Append(this.NextChar);
							if (this.json.Peek() == -1)
							{
								break;
							}
						}
						return stringBuilder.ToString();
					}
				}

				private JSON._JSON.Parser.TOKEN NextToken
				{
					get
					{
						this.EatWhitespace();
						if (this.json.Peek() == -1)
						{
							return JSON._JSON.Parser.TOKEN.NONE;
						}
						char peekChar = this.PeekChar;
						char c = peekChar;
						switch (c)
						{
						case '"':
							return JSON._JSON.Parser.TOKEN.STRING;
						default:
							switch (c)
							{
							case '[':
								return JSON._JSON.Parser.TOKEN.SQUARED_OPEN;
							default:
							{
								switch (c)
								{
								case '{':
									return JSON._JSON.Parser.TOKEN.CURLY_OPEN;
								case '}':
									this.json.Read();
									return JSON._JSON.Parser.TOKEN.CURLY_CLOSE;
								}
								string nextWord = this.NextWord;
								string text = nextWord;
								switch (text)
								{
								case "false":
									return JSON._JSON.Parser.TOKEN.FALSE;
								case "true":
									return JSON._JSON.Parser.TOKEN.TRUE;
								case "null":
									return JSON._JSON.Parser.TOKEN.NULL;
								}
								return JSON._JSON.Parser.TOKEN.NONE;
							}
							case ']':
								this.json.Read();
								return JSON._JSON.Parser.TOKEN.SQUARED_CLOSE;
							}
							break;
						case ',':
							this.json.Read();
							return JSON._JSON.Parser.TOKEN.COMMA;
						case '-':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							return JSON._JSON.Parser.TOKEN.NUMBER;
						case ':':
							return JSON._JSON.Parser.TOKEN.COLON;
						}
					}
				}

				private const string WHITE_SPACE = " \t\n\r";

				private const string WORD_BREAK = " \t\n\r{}[],:\"";

				private StringReader json;

				private enum TOKEN
				{
					NONE,
					CURLY_OPEN,
					CURLY_CLOSE,
					SQUARED_OPEN,
					SQUARED_CLOSE,
					COLON,
					COMMA,
					STRING,
					NUMBER,
					TRUE,
					FALSE,
					NULL
				}
			}

			private sealed class Serializer
			{
				private Serializer()
				{
					this.builder = new StringBuilder();
				}

				public static string Serialize(JSON obj)
				{
					JSON._JSON.Serializer serializer = new JSON._JSON.Serializer();
					serializer.SerializeValue(obj);
					return serializer.builder.ToString();
				}

				private void SerializeValue(object value)
				{
					if (value == null)
					{
						this.builder.Append("null");
					}
					else if (value is string)
					{
						this.SerializeString(value as string);
					}
					else if (value is bool)
					{
						this.builder.Append(value.ToString().ToLower());
					}
					else if (value is JSON)
					{
						this.SerializeObject(value as JSON);
					}
					else if (value is IDictionary)
					{
						this.SerializeDictionary(value as IDictionary);
					}
					else if (value is IList)
					{
						this.SerializeArray(value as IList);
					}
					else if (value is char)
					{
						this.SerializeString(value.ToString());
					}
					else
					{
						this.SerializeOther(value);
					}
				}

				private void SerializeObject(JSON obj)
				{
					this.SerializeDictionary(obj.fields);
				}

				private void SerializeDictionary(IDictionary obj)
				{
					bool flag = true;
					this.builder.Append('{');
					foreach (object obj2 in obj.Keys)
					{
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeString(obj2.ToString());
						this.builder.Append(':');
						this.SerializeValue(obj[obj2]);
						flag = false;
					}
					this.builder.Append('}');
				}

				private void SerializeArray(IList anArray)
				{
					this.builder.Append('[');
					bool flag = true;
					foreach (object value in anArray)
					{
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeValue(value);
						flag = false;
					}
					this.builder.Append(']');
				}

				private void SerializeString(string str)
				{
					this.builder.Append('"');
					char[] array = str.ToCharArray();
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

				private void SerializeOther(object value)
				{
					if (value is float || value is int || value is uint || value is long || value is double || value is sbyte || value is byte || value is short || value is ushort || value is ulong || value is decimal)
					{
						this.builder.Append(value.ToString());
					}
					else
					{
						this.SerializeString(value.ToString());
					}
				}

				private StringBuilder builder;
			}
		}
	}
}
