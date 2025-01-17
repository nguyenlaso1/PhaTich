// @sonhg: class: JSONObject
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class JSONObject
{
	
	public JSONObject(JSONObject.Type t)
	{
		this.type = t;
		if (t != JSONObject.Type.OBJECT)
		{
			if (t == JSONObject.Type.ARRAY)
			{
				this.list = new List<JSONObject>();
			}
		}
		else
		{
			this.list = new List<JSONObject>();
			this.keys = new List<string>();
		}
	}

	public JSONObject(bool b)
	{
		this.type = JSONObject.Type.BOOL;
		this.b = b;
	}

	public JSONObject(float f)
	{
		this.type = JSONObject.Type.NUMBER;
		this.n = f;
	}

	public JSONObject(int i)
	{
		this.type = JSONObject.Type.NUMBER;
		this.i = (long)i;
		this.useInt = true;
		this.n = (float)i;
	}

	public JSONObject(long l)
	{
		this.type = JSONObject.Type.NUMBER;
		this.i = l;
		this.useInt = true;
		this.n = (float)l;
	}

	public JSONObject(Dictionary<string, string> dic)
	{
		this.type = JSONObject.Type.OBJECT;
		this.keys = new List<string>();
		this.list = new List<JSONObject>();
		foreach (KeyValuePair<string, string> keyValuePair in dic)
		{
			this.keys.Add(keyValuePair.Key);
			this.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
		}
	}

	public JSONObject(Dictionary<string, JSONObject> dic)
	{
		this.type = JSONObject.Type.OBJECT;
		this.keys = new List<string>();
		this.list = new List<JSONObject>();
		foreach (KeyValuePair<string, JSONObject> keyValuePair in dic)
		{
			this.keys.Add(keyValuePair.Key);
			this.list.Add(keyValuePair.Value);
		}
	}

	public JSONObject(JSONObject.AddJSONContents content)
	{
		content(this);
	}

	public JSONObject(JSONObject[] objs)
	{
		this.type = JSONObject.Type.ARRAY;
		this.list = new List<JSONObject>(objs);
	}

	public JSONObject()
	{
	}

	public JSONObject(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
	{
		this.Parse(str, maxDepth, storeExcessLevels, strict);
	}

	public bool isContainer
	{
		get
		{
			return this.type == JSONObject.Type.ARRAY || this.type == JSONObject.Type.OBJECT;
		}
	}

	public int Count
	{
		get
		{
			if (this.list == null)
			{
				return -1;
			}
			return this.list.Count;
		}
	}

	public float f
	{
		get
		{
			return this.n;
		}
	}

	public static JSONObject nullJO
	{
		get
		{
			return JSONObject.Create(JSONObject.Type.NULL);
		}
	}

	public static JSONObject obj
	{
		get
		{
			return JSONObject.Create(JSONObject.Type.OBJECT);
		}
	}

	public static JSONObject arr
	{
		get
		{
			return JSONObject.Create(JSONObject.Type.ARRAY);
		}
	}

	public static JSONObject StringObject(string val)
	{
		return JSONObject.CreateStringObject(val);
	}

	public void Absorb(JSONObject obj)
	{
		this.list.AddRange(obj.list);
		this.keys.AddRange(obj.keys);
		this.str = obj.str;
		this.n = obj.n;
		this.useInt = obj.useInt;
		this.i = obj.i;
		this.b = obj.b;
		this.type = obj.type;
	}

	public static JSONObject Create()
	{
		return new JSONObject();
	}

	public static JSONObject Create(JSONObject.Type t)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = t;
		if (t != JSONObject.Type.OBJECT)
		{
			if (t == JSONObject.Type.ARRAY)
			{
				jsonobject.list = new List<JSONObject>();
			}
		}
		else
		{
			jsonobject.list = new List<JSONObject>();
			jsonobject.keys = new List<string>();
		}
		return jsonobject;
	}

	public static JSONObject Create(bool val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.BOOL;
		jsonobject.b = val;
		return jsonobject;
	}

	public static JSONObject Create(float val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.NUMBER;
		jsonobject.n = val;
		return jsonobject;
	}

	public static JSONObject Create(int val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.NUMBER;
		jsonobject.n = (float)val;
		jsonobject.useInt = true;
		jsonobject.i = (long)val;
		return jsonobject;
	}

	public static JSONObject Create(long val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.NUMBER;
		jsonobject.n = (float)val;
		jsonobject.useInt = true;
		jsonobject.i = val;
		return jsonobject;
	}

	public static JSONObject CreateStringObject(string val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.STRING;
		jsonobject.str = val;
		return jsonobject;
	}

	public static JSONObject CreateBakedObject(string val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.BAKED;
		jsonobject.str = val;
		return jsonobject;
	}

	public static JSONObject Create(string val, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.Parse(val, maxDepth, storeExcessLevels, strict);
		return jsonobject;
	}

	public static JSONObject Create(JSONObject.AddJSONContents content)
	{
		JSONObject jsonobject = JSONObject.Create();
		content(jsonobject);
		return jsonobject;
	}

	public static JSONObject Create(Dictionary<string, string> dic)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.OBJECT;
		jsonobject.keys = new List<string>();
		jsonobject.list = new List<JSONObject>();
		foreach (KeyValuePair<string, string> keyValuePair in dic)
		{
			jsonobject.keys.Add(keyValuePair.Key);
			jsonobject.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
		}
		return jsonobject;
	}

	private void Parse(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
	{
		if (!string.IsNullOrEmpty(str))
		{
			str = str.Trim(JSONObject.WHITESPACE);
			if (strict && str[0] != '[' && str[0] != '{')
			{
				this.type = JSONObject.Type.NULL;
				UnityEngine.Debug.LogWarning("Improper (strict) JSON formatting.  First character must be [ or {");
				return;
			}
			if (str.Length > 0)
			{
				if (string.Compare(str, "true", true) == 0)
				{
					this.type = JSONObject.Type.BOOL;
					this.b = true;
				}
				else if (string.Compare(str, "false", true) == 0)
				{
					this.type = JSONObject.Type.BOOL;
					this.b = false;
				}
				else if (string.Compare(str, "null", true) == 0)
				{
					this.type = JSONObject.Type.NULL;
				}
				else if (str == "\"INFINITY\"")
				{
					this.type = JSONObject.Type.NUMBER;
					this.n = float.PositiveInfinity;
				}
				else if (str == "\"NEGINFINITY\"")
				{
					this.type = JSONObject.Type.NUMBER;
					this.n = float.NegativeInfinity;
				}
				else if (str == "\"NaN\"")
				{
					this.type = JSONObject.Type.NUMBER;
					this.n = float.NaN;
				}
				else if (str[0] == '"')
				{
					this.type = JSONObject.Type.STRING;
					this.str = str.Substring(1, str.Length - 2);
				}
				else
				{
					int num = 1;
					int num2 = 0;
					char c = str[num2];
					if (c != '[')
					{
						if (c != '{')
						{
							try
							{
								this.n = Convert.ToSingle(str);
								if (!str.Contains("."))
								{
									this.i = Convert.ToInt64(str);
									this.useInt = true;
								}
								this.type = JSONObject.Type.NUMBER;
							}
							catch (FormatException)
							{
								this.type = JSONObject.Type.NULL;
								UnityEngine.Debug.LogWarning("improper JSON formatting:" + str);
							}
							return;
						}
						this.type = JSONObject.Type.OBJECT;
						this.keys = new List<string>();
						this.list = new List<JSONObject>();
					}
					else
					{
						this.type = JSONObject.Type.ARRAY;
						this.list = new List<JSONObject>();
					}
					string item = string.Empty;
					bool flag = false;
					bool flag2 = false;
					int num3 = 0;
					while (++num2 < str.Length)
					{
						if (Array.IndexOf<char>(JSONObject.WHITESPACE, str[num2]) <= -1)
						{
							if (str[num2] == '\\')
							{
								num2++;
							}
							else
							{
								if (str[num2] == '"')
								{
									if (flag)
									{
										if (!flag2 && num3 == 0 && this.type == JSONObject.Type.OBJECT)
										{
											item = str.Substring(num + 1, num2 - num - 1);
										}
										flag = false;
									}
									else
									{
										if (num3 == 0 && this.type == JSONObject.Type.OBJECT)
										{
											num = num2;
										}
										flag = true;
									}
								}
								if (!flag)
								{
									if (this.type == JSONObject.Type.OBJECT && num3 == 0 && str[num2] == ':')
									{
										num = num2 + 1;
										flag2 = true;
									}
									if (str[num2] == '[' || str[num2] == '{')
									{
										num3++;
									}
									else if (str[num2] == ']' || str[num2] == '}')
									{
										num3--;
									}
									if ((str[num2] == ',' && num3 == 0) || num3 < 0)
									{
										flag2 = false;
										string text = str.Substring(num, num2 - num).Trim(JSONObject.WHITESPACE);
										if (text.Length > 0)
										{
											if (this.type == JSONObject.Type.OBJECT)
											{
												this.keys.Add(item);
											}
											if (maxDepth != -1)
											{
												this.list.Add(JSONObject.Create(text, (maxDepth >= -1) ? (maxDepth - 1) : -2, false, false));
											}
											else if (storeExcessLevels)
											{
												this.list.Add(JSONObject.CreateBakedObject(text));
											}
										}
										num = num2 + 1;
									}
								}
							}
						}
					}
				}
			}
			else
			{
				this.type = JSONObject.Type.NULL;
			}
		}
		else
		{
			this.type = JSONObject.Type.NULL;
		}
	}

	public bool IsNumber
	{
		get
		{
			return this.type == JSONObject.Type.NUMBER;
		}
	}

	public bool IsNull
	{
		get
		{
			return this.type == JSONObject.Type.NULL;
		}
	}

	public bool IsString
	{
		get
		{
			return this.type == JSONObject.Type.STRING;
		}
	}

	public bool IsBool
	{
		get
		{
			return this.type == JSONObject.Type.BOOL;
		}
	}

	public bool IsArray
	{
		get
		{
			return this.type == JSONObject.Type.ARRAY;
		}
	}

	public bool IsObject
	{
		get
		{
			return this.type == JSONObject.Type.OBJECT || this.type == JSONObject.Type.BAKED;
		}
	}

	public void Add(bool val)
	{
		this.Add(JSONObject.Create(val));
	}

	public void Add(float val)
	{
		this.Add(JSONObject.Create(val));
	}

	public void Add(int val)
	{
		this.Add(JSONObject.Create(val));
	}

	public void Add(string str)
	{
		this.Add(JSONObject.CreateStringObject(str));
	}

	public void Add(JSONObject.AddJSONContents content)
	{
		this.Add(JSONObject.Create(content));
	}

	public void Add(JSONObject obj)
	{
		if (obj)
		{
			if (this.type != JSONObject.Type.ARRAY)
			{
				this.type = JSONObject.Type.ARRAY;
				if (this.list == null)
				{
					this.list = new List<JSONObject>();
				}
			}
			this.list.Add(obj);
		}
	}

	public void AddField(string name, bool val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	public void AddField(string name, float val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	public void AddField(string name, int val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	public void AddField(string name, long val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	public void AddField(string name, JSONObject.AddJSONContents content)
	{
		this.AddField(name, JSONObject.Create(content));
	}

	public void AddField(string name, string val)
	{
		this.AddField(name, JSONObject.CreateStringObject(val));
	}

	public void AddField(string name, JSONObject obj)
	{
		if (obj)
		{
			if (this.type != JSONObject.Type.OBJECT)
			{
				if (this.keys == null)
				{
					this.keys = new List<string>();
				}
				if (this.type == JSONObject.Type.ARRAY)
				{
					for (int i = 0; i < this.list.Count; i++)
					{
						this.keys.Add(i + string.Empty);
					}
				}
				else if (this.list == null)
				{
					this.list = new List<JSONObject>();
				}
				this.type = JSONObject.Type.OBJECT;
			}
			this.keys.Add(name);
			this.list.Add(obj);
		}
	}

	public void SetField(string name, string val)
	{
		this.SetField(name, JSONObject.CreateStringObject(val));
	}

	public void SetField(string name, bool val)
	{
		this.SetField(name, JSONObject.Create(val));
	}

	public void SetField(string name, float val)
	{
		this.SetField(name, JSONObject.Create(val));
	}

	public void SetField(string name, int val)
	{
		this.SetField(name, JSONObject.Create(val));
	}

	public void SetField(string name, JSONObject obj)
	{
		if (this.HasField(name))
		{
			this.list.Remove(this[name]);
			this.keys.Remove(name);
		}
		this.AddField(name, obj);
	}

	public void RemoveField(string name)
	{
		if (this.keys.IndexOf(name) > -1)
		{
			this.list.RemoveAt(this.keys.IndexOf(name));
			this.keys.Remove(name);
		}
	}

	public bool GetField(out bool field, string name, bool fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	public bool GetField(ref bool field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.type == JSONObject.Type.OBJECT)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = this.list[num].b;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	public bool GetField(out float field, string name, float fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	public bool GetField(ref float field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.type == JSONObject.Type.OBJECT)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	public bool GetField(out int field, string name, int fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	public bool GetField(ref int field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = (int)this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	public bool GetField(out long field, string name, long fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	public bool GetField(ref long field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = (long)this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	public bool GetField(out uint field, string name, uint fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	public bool GetField(ref uint field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = (uint)this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	public bool GetField(out string field, string name, string fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	public bool GetField(ref string field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = this.list[num].str;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	public void GetField(string name, JSONObject.GetFieldResponse response, JSONObject.FieldNotFound fail = null)
	{
		if (response != null && this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				response(this.list[num]);
				return;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
	}

	public JSONObject GetField(string name)
	{
		if (this.IsObject)
		{
			for (int i = 0; i < this.keys.Count; i++)
			{
				if (this.keys[i] == name)
				{
					return this.list[i];
				}
			}
		}
		return null;
	}

	public bool HasFields(string[] names)
	{
		if (!this.IsObject)
		{
			return false;
		}
		for (int i = 0; i < names.Length; i++)
		{
			if (!this.keys.Contains(names[i]))
			{
				return false;
			}
		}
		return true;
	}

	public bool HasField(string name)
	{
		if (!this.IsObject)
		{
			return false;
		}
		for (int i = 0; i < this.keys.Count; i++)
		{
			if (this.keys[i] == name)
			{
				return true;
			}
		}
		return false;
	}

	public void Clear()
	{
		this.type = JSONObject.Type.NULL;
		if (this.list != null)
		{
			this.list.Clear();
		}
		if (this.keys != null)
		{
			this.keys.Clear();
		}
		this.str = string.Empty;
		this.n = 0f;
		this.b = false;
	}

	public JSONObject Copy()
	{
		return JSONObject.Create(this.Print(false), -2, false, false);
	}

	public void Merge(JSONObject obj)
	{
		JSONObject.MergeRecur(this, obj);
	}

	private static void MergeRecur(JSONObject left, JSONObject right)
	{
		if (left.type == JSONObject.Type.NULL)
		{
			left.Absorb(right);
		}
		else if (left.type == JSONObject.Type.OBJECT && right.type == JSONObject.Type.OBJECT)
		{
			for (int i = 0; i < right.list.Count; i++)
			{
				string text = right.keys[i];
				if (right[i].isContainer)
				{
					if (left.HasField(text))
					{
						JSONObject.MergeRecur(left[text], right[i]);
					}
					else
					{
						left.AddField(text, right[i]);
					}
				}
				else if (left.HasField(text))
				{
					left.SetField(text, right[i]);
				}
				else
				{
					left.AddField(text, right[i]);
				}
			}
		}
		else if (left.type == JSONObject.Type.ARRAY && right.type == JSONObject.Type.ARRAY)
		{
			if (right.Count > left.Count)
			{
				UnityEngine.Debug.LogError("Cannot merge arrays when right object has more elements");
				return;
			}
			for (int j = 0; j < right.list.Count; j++)
			{
				if (left[j].type == right[j].type)
				{
					if (left[j].isContainer)
					{
						JSONObject.MergeRecur(left[j], right[j]);
					}
					else
					{
						left[j] = right[j];
					}
				}
			}
		}
	}

	public void Bake()
	{
		if (this.type != JSONObject.Type.BAKED)
		{
			this.str = this.Print(false);
			this.type = JSONObject.Type.BAKED;
		}
	}

	public IEnumerable BakeAsync()
	{
		if (this.type != JSONObject.Type.BAKED)
		{
			foreach (string s in this.PrintAsync(false))
			{
				if (s == null)
				{
					yield return s;
				}
				else
				{
					this.str = s;
				}
			}
			this.type = JSONObject.Type.BAKED;
		}
		yield break;
	}

	public string Print(bool pretty = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.Stringify(0, stringBuilder, pretty);
		return stringBuilder.ToString();
	}

	public IEnumerable<string> PrintAsync(bool pretty = false)
	{
		StringBuilder builder = new StringBuilder();
		JSONObject.printWatch.Reset();
		JSONObject.printWatch.Start();
		foreach (object obj in this.StringifyAsync(0, builder, pretty))
		{
			IEnumerable e = (IEnumerable)obj;
			yield return null;
		}
		yield return builder.ToString();
		yield break;
	}

	private IEnumerable StringifyAsync(int depth, StringBuilder builder, bool pretty = false)
	{
		int num;
		depth = (num = depth) + 1;
		if (num > 100)
		{
			UnityEngine.Debug.Log("reached max depth!");
			yield break;
		}
		if (JSONObject.printWatch.Elapsed.TotalSeconds > 0.00800000037997961)
		{
			JSONObject.printWatch.Reset();
			yield return null;
			JSONObject.printWatch.Start();
		}
		switch (this.type)
		{
		case JSONObject.Type.NULL:
			builder.Append("null");
			break;
		case JSONObject.Type.STRING:
			builder.AppendFormat("\"{0}\"", this.str);
			break;
		case JSONObject.Type.NUMBER:
			if (this.useInt)
			{
				builder.Append(this.i.ToString());
			}
			else if (float.IsInfinity(this.n))
			{
				builder.Append("\"INFINITY\"");
			}
			else if (float.IsNegativeInfinity(this.n))
			{
				builder.Append("\"NEGINFINITY\"");
			}
			else if (float.IsNaN(this.n))
			{
				builder.Append("\"NaN\"");
			}
			else
			{
				builder.Append(this.n.ToString());
			}
			break;
		case JSONObject.Type.OBJECT:
			builder.Append("{");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\n");
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					string key = this.keys[i];
					JSONObject obj = this.list[i];
					if (obj)
					{
						if (pretty)
						{
							for (int j = 0; j < depth; j++)
							{
								builder.Append("\t");
							}
						}
						builder.AppendFormat("\"{0}\":", key);
						foreach (object obj2 in obj.StringifyAsync(depth, builder, pretty))
						{
							IEnumerable e = (IEnumerable)obj2;
							yield return e;
						}
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\n");
				for (int k = 0; k < depth - 1; k++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("}");
			break;
		case JSONObject.Type.ARRAY:
			builder.Append("[");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\n");
				}
				for (int l = 0; l < this.list.Count; l++)
				{
					if (this.list[l])
					{
						if (pretty)
						{
							for (int m = 0; m < depth; m++)
							{
								builder.Append("\t");
							}
						}
						foreach (object obj3 in this.list[l].StringifyAsync(depth, builder, pretty))
						{
							IEnumerable e2 = (IEnumerable)obj3;
							yield return e2;
						}
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\n");
				for (int n = 0; n < depth - 1; n++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("]");
			break;
		case JSONObject.Type.BOOL:
			if (this.b)
			{
				builder.Append("true");
			}
			else
			{
				builder.Append("false");
			}
			break;
		case JSONObject.Type.BAKED:
			builder.Append(this.str);
			break;
		}
		yield break;
	}

	private void Stringify(int depth, StringBuilder builder, bool pretty = false)
	{
		if (depth++ > 100)
		{
			UnityEngine.Debug.Log("reached max depth!");
			return;
		}
		switch (this.type)
		{
		case JSONObject.Type.NULL:
			builder.Append("null");
			break;
		case JSONObject.Type.STRING:
			builder.AppendFormat("\"{0}\"", this.str);
			break;
		case JSONObject.Type.NUMBER:
			if (this.useInt)
			{
				builder.Append(this.i.ToString());
			}
			else if (float.IsInfinity(this.n))
			{
				builder.Append("\"INFINITY\"");
			}
			else if (float.IsNegativeInfinity(this.n))
			{
				builder.Append("\"NEGINFINITY\"");
			}
			else if (float.IsNaN(this.n))
			{
				builder.Append("\"NaN\"");
			}
			else
			{
				builder.Append(this.n.ToString());
			}
			break;
		case JSONObject.Type.OBJECT:
			builder.Append("{");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\n");
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					string arg = this.keys[i];
					JSONObject jsonobject = this.list[i];
					if (jsonobject)
					{
						if (pretty)
						{
							for (int j = 0; j < depth; j++)
							{
								builder.Append("\t");
							}
						}
						builder.AppendFormat("\"{0}\":", arg);
						jsonobject.Stringify(depth, builder, pretty);
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\n");
				for (int k = 0; k < depth - 1; k++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("}");
			break;
		case JSONObject.Type.ARRAY:
			builder.Append("[");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\n");
				}
				for (int l = 0; l < this.list.Count; l++)
				{
					if (this.list[l])
					{
						if (pretty)
						{
							for (int m = 0; m < depth; m++)
							{
								builder.Append("\t");
							}
						}
						this.list[l].Stringify(depth, builder, pretty);
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\n");
				for (int n = 0; n < depth - 1; n++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("]");
			break;
		case JSONObject.Type.BOOL:
			if (this.b)
			{
				builder.Append("true");
			}
			else
			{
				builder.Append("false");
			}
			break;
		case JSONObject.Type.BAKED:
			builder.Append(this.str);
			break;
		}
	}

	public JSONObject this[int index]
	{
		get
		{
			if (this.list.Count > index)
			{
				return this.list[index];
			}
			return null;
		}
		set
		{
			if (this.list.Count > index)
			{
				this.list[index] = value;
			}
		}
	}

	public JSONObject this[string index]
	{
		get
		{
			return this.GetField(index);
		}
		set
		{
			this.SetField(index, value);
		}
	}

	public override string ToString()
	{
		return this.Print(false);
	}

	public string ToString(bool pretty)
	{
		return this.Print(pretty);
	}

	public Dictionary<string, string> ToDictionary()
	{
		if (this.type == JSONObject.Type.OBJECT)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			int i = 0;
			while (i < this.list.Count)
			{
				JSONObject jsonobject = this.list[i];
				switch (jsonobject.type)
				{
				case JSONObject.Type.STRING:
					dictionary.Add(this.keys[i], jsonobject.str);
					break;
				case JSONObject.Type.NUMBER:
					dictionary.Add(this.keys[i], jsonobject.n + string.Empty);
					break;
				case JSONObject.Type.OBJECT:
				case JSONObject.Type.ARRAY:
					goto IL_C3;
				case JSONObject.Type.BOOL:
					dictionary.Add(this.keys[i], jsonobject.b + string.Empty);
					break;
				default:
					goto IL_C3;
				}
				IL_E8:
				i++;
				continue;
				IL_C3:
				UnityEngine.Debug.LogWarning("Omitting object: " + this.keys[i] + " in dictionary conversion");
				goto IL_E8;
			}
			return dictionary;
		}
		UnityEngine.Debug.Log("Tried to turn non-Object JSONObject into a dictionary");
		return null;
	}

	public static implicit operator WWWForm(JSONObject obj)
	{
		WWWForm wwwform = new WWWForm();
		for (int i = 0; i < obj.list.Count; i++)
		{
			string fieldName = i + string.Empty;
			if (obj.type == JSONObject.Type.OBJECT)
			{
				fieldName = obj.keys[i];
			}
			string text = obj.list[i].ToString();
			if (obj.list[i].type == JSONObject.Type.STRING)
			{
				text = text.Replace("\"", string.Empty);
			}
			wwwform.AddField(fieldName, text);
		}
		return wwwform;
	}

	public static implicit operator bool(JSONObject o)
	{
		return o != null;
	}

	private const int MAX_DEPTH = 100;

	private const string INFINITY = "\"INFINITY\"";

	private const string NEGINFINITY = "\"NEGINFINITY\"";

	private const string NaN = "\"NaN\"";

	private const float maxFrameTime = 0.008f;

	public static readonly char[] WHITESPACE = new char[]
	{
		' ',
		'\r',
		'\n',
		'\t',
		'﻿',
		'\t'
	};

	public JSONObject.Type type;

	public List<JSONObject> list;

	public List<string> keys;

	public string str;

	public float n;

	public bool useInt;

	public long i;

	public bool b;

	private static readonly Stopwatch printWatch = new Stopwatch();

	public enum Type
	{
		NULL,
		STRING,
		NUMBER,
		OBJECT,
		ARRAY,
		BOOL,
		BAKED
	}

	public delegate void AddJSONContents(JSONObject self);

	public delegate void FieldNotFound(string name);

	public delegate void GetFieldResponse(JSONObject obj);
}
