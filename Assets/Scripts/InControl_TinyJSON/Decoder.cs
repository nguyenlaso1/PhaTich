// @sonhg: class: InControl.TinyJSON.Decoder
using System;
using System.IO;
using System.Text;

namespace InControl.TinyJSON
{
	public sealed class Decoder : IDisposable
	{
		private Decoder(string jsonString)
		{
			this.json = new StringReader(jsonString);
		}

		public static Variant Decode(string jsonString)
		{
			Variant result;
			using (Decoder decoder = new Decoder(jsonString))
			{
				result = decoder.DecodeValue();
			}
			return result;
		}

		public void Dispose()
		{
			this.json.Dispose();
			this.json = null;
		}

		private ProxyObject DecodeObject()
		{
			ProxyObject proxyObject = new ProxyObject();
			this.json.Read();
			for (;;)
			{
				Decoder.Token nextToken = this.NextToken;
				switch (nextToken)
				{
				case Decoder.Token.None:
					goto IL_37;
				default:
					if (nextToken != Decoder.Token.Comma)
					{
						string text = this.DecodeString();
						if (text == null)
						{
							goto Block_2;
						}
						if (this.NextToken != Decoder.Token.Colon)
						{
							goto Block_3;
						}
						this.json.Read();
						proxyObject.Add(text, this.DecodeValue());
					}
					break;
				case Decoder.Token.CloseBrace:
					return proxyObject;
				}
			}
			IL_37:
			return null;
			Block_2:
			return null;
			Block_3:
			return null;
		}

		private ProxyArray DecodeArray()
		{
			ProxyArray proxyArray = new ProxyArray();
			this.json.Read();
			bool flag = true;
			while (flag)
			{
				Decoder.Token nextToken = this.NextToken;
				Decoder.Token token = nextToken;
				switch (token)
				{
				case Decoder.Token.CloseBracket:
					flag = false;
					break;
				default:
					if (token == Decoder.Token.None)
					{
						return null;
					}
					proxyArray.Add(this.DecodeByToken(nextToken));
					break;
				case Decoder.Token.Comma:
					break;
				}
			}
			return proxyArray;
		}

		private Variant DecodeValue()
		{
			Decoder.Token nextToken = this.NextToken;
			return this.DecodeByToken(nextToken);
		}

		private Variant DecodeByToken(Decoder.Token token)
		{
			switch (token)
			{
			case Decoder.Token.OpenBrace:
				return this.DecodeObject();
			case Decoder.Token.OpenBracket:
				return this.DecodeArray();
			case Decoder.Token.String:
				return this.DecodeString();
			case Decoder.Token.Number:
				return this.DecodeNumber();
			case Decoder.Token.True:
				return new ProxyBoolean(true);
			case Decoder.Token.False:
				return new ProxyBoolean(false);
			case Decoder.Token.Null:
				return null;
			}
			return null;
		}

		private Variant DecodeString()
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
			return new ProxyString(stringBuilder.ToString());
		}

		private Variant DecodeNumber()
		{
			return new ProxyNumber(this.NextWord);
		}

		private void ConsumeWhiteSpace()
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
				int num = this.json.Peek();
				return (num != -1) ? Convert.ToChar(num) : '\0';
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

		private Decoder.Token NextToken
		{
			get
			{
				this.ConsumeWhiteSpace();
				if (this.json.Peek() == -1)
				{
					return Decoder.Token.None;
				}
				char peekChar = this.PeekChar;
				switch (peekChar)
				{
				case '"':
					return Decoder.Token.String;
				default:
					switch (peekChar)
					{
					case '[':
						return Decoder.Token.OpenBracket;
					default:
					{
						switch (peekChar)
						{
						case '{':
							return Decoder.Token.OpenBrace;
						case '}':
							this.json.Read();
							return Decoder.Token.CloseBrace;
						}
						string nextWord = this.NextWord;
						switch (nextWord)
						{
						case "false":
							return Decoder.Token.False;
						case "true":
							return Decoder.Token.True;
						case "null":
							return Decoder.Token.Null;
						}
						return Decoder.Token.None;
					}
					case ']':
						this.json.Read();
						return Decoder.Token.CloseBracket;
					}
					break;
				case ',':
					this.json.Read();
					return Decoder.Token.Comma;
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
					return Decoder.Token.Number;
				case ':':
					return Decoder.Token.Colon;
				}
			}
		}

		private const string WhiteSpace = " \t\n\r";

		private const string WordBreak = " \t\n\r{}[],:\"";

		private StringReader json;

		private enum Token
		{
			None,
			OpenBrace,
			CloseBrace,
			OpenBracket,
			CloseBracket,
			Colon,
			Comma,
			String,
			Number,
			True,
			False,
			Null
		}
	}
}
