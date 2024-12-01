// @sonhg: class: InControl.TinyJSON.JSON
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace InControl.TinyJSON
{
	public static class JSON
	{
		public static Variant Load(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			return Decoder.Decode(json);
		}

		public static string Dump(object data, EncodeOptions options = EncodeOptions.None)
		{
			return Encoder.Encode(data, options);
		}

		public static void MakeInto<T>(Variant data, out T item)
		{
			item = JSON.DecodeType<T>(data);
		}

		private static Type FindType(string fullName)
		{
			if (fullName == null)
			{
				return null;
			}
			Type type;
			if (JSON.typeCache.TryGetValue(fullName, out type))
			{
				return type;
			}
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(fullName);
				if (type != null)
				{
					JSON.typeCache.Add(fullName, type);
					return type;
				}
			}
			return null;
		}

		private static T DecodeType<T>(Variant data)
		{
			if (data == null)
			{
				return default(T);
			}
			Type type = typeof(T);
			if (type.IsEnum)
			{
				return (T)((object)Enum.Parse(type, data.ToString()));
			}
			if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal))
			{
				return (T)((object)Convert.ChangeType(data, type));
			}
			if (type.IsArray)
			{
				if (type.GetArrayRank() == 1)
				{
					MethodInfo methodInfo = JSON.decodeArrayMethod.MakeGenericMethod(new Type[]
					{
						type.GetElementType()
					});
					return (T)((object)methodInfo.Invoke(null, new object[]
					{
						data
					}));
				}
				ProxyArray proxyArray = data as ProxyArray;
				int arrayRank = type.GetArrayRank();
				int[] array = new int[arrayRank];
				if (proxyArray.CanBeMultiRankArray(array))
				{
					Array array2 = Array.CreateInstance(type.GetElementType(), array);
					MethodInfo methodInfo2 = JSON.decodeMultiRankArrayMethod.MakeGenericMethod(new Type[]
					{
						type.GetElementType()
					});
					try
					{
						methodInfo2.Invoke(null, new object[]
						{
							proxyArray,
							array2,
							1,
							array
						});
					}
					catch (Exception innerException)
					{
						throw new DecodeException("Error decoding multidimensional array. Did you try to decode into an array of incompatible rank or element type?", innerException);
					}
					return (T)((object)Convert.ChangeType(array2, typeof(T)));
				}
				throw new DecodeException("Error decoding multidimensional array; JSON data doesn't seem fit this structure.");
			}
			else
			{
				if (typeof(IList).IsAssignableFrom(type))
				{
					MethodInfo methodInfo3 = JSON.decodeListMethod.MakeGenericMethod(type.GetGenericArguments());
					return (T)((object)methodInfo3.Invoke(null, new object[]
					{
						data
					}));
				}
				if (typeof(IDictionary).IsAssignableFrom(type))
				{
					MethodInfo methodInfo4 = JSON.decodeDictionaryMethod.MakeGenericMethod(type.GetGenericArguments());
					return (T)((object)methodInfo4.Invoke(null, new object[]
					{
						data
					}));
				}
				ProxyObject proxyObject = data as ProxyObject;
				if (proxyObject == null)
				{
					throw new InvalidCastException("ProxyObject expected when decoding into '" + type.FullName + "'.");
				}
				string typeHint = proxyObject.TypeHint;
				T t;
				if (typeHint != null && typeHint != type.FullName)
				{
					Type type2 = JSON.FindType(typeHint);
					if (type2 == null)
					{
						throw new TypeLoadException("Could not load type '" + typeHint + "'.");
					}
					if (!type.IsAssignableFrom(type2))
					{
						throw new InvalidCastException(string.Concat(new string[]
						{
							"Cannot assign type '",
							typeHint,
							"' to type '",
							type.FullName,
							"'."
						}));
					}
					t = (T)((object)Activator.CreateInstance(type2));
					type = type2;
				}
				else
				{
					t = Activator.CreateInstance<T>();
				}
				foreach (KeyValuePair<string, Variant> keyValuePair in ((IEnumerable<KeyValuePair<string, Variant>>)(data as ProxyObject)))
				{
					FieldInfo field = type.GetField(keyValuePair.Key, JSON.instanceBindingFlags);
					if (field != null)
					{
						bool flag = field.IsPublic;
						foreach (object obj in field.GetCustomAttributes(true))
						{
							if (JSON.excludeAttrType.IsAssignableFrom(obj.GetType()))
							{
								flag = false;
							}
							if (JSON.includeAttrType.IsAssignableFrom(obj.GetType()))
							{
								flag = true;
							}
						}
						if (flag)
						{
							MethodInfo methodInfo5 = JSON.decodeTypeMethod.MakeGenericMethod(new Type[]
							{
								field.FieldType
							});
							if (type.IsValueType)
							{
								object obj2 = t;
								field.SetValue(obj2, methodInfo5.Invoke(null, new object[]
								{
									keyValuePair.Value
								}));
								t = (T)((object)obj2);
							}
							else
							{
								field.SetValue(t, methodInfo5.Invoke(null, new object[]
								{
									keyValuePair.Value
								}));
							}
						}
					}
					PropertyInfo property = type.GetProperty(keyValuePair.Key, JSON.instanceBindingFlags);
					if (property != null && property.CanWrite && property.GetCustomAttributes(false).AnyOfType(JSON.includeAttrType))
					{
						MethodInfo methodInfo6 = JSON.decodeTypeMethod.MakeGenericMethod(new Type[]
						{
							property.PropertyType
						});
						if (type.IsValueType)
						{
							object obj3 = t;
							property.SetValue(obj3, methodInfo6.Invoke(null, new object[]
							{
								keyValuePair.Value
							}), null);
							t = (T)((object)obj3);
						}
						else
						{
							property.SetValue(t, methodInfo6.Invoke(null, new object[]
							{
								keyValuePair.Value
							}), null);
						}
					}
				}
				foreach (MethodInfo methodInfo7 in type.GetMethods(JSON.instanceBindingFlags))
				{
					if (methodInfo7.GetCustomAttributes(false).AnyOfType(typeof(AfterDecode)))
					{
						if (methodInfo7.GetParameters().Length == 0)
						{
							methodInfo7.Invoke(t, null);
						}
						else
						{
							methodInfo7.Invoke(t, new object[]
							{
								data
							});
						}
					}
				}
				return t;
			}
		}

		private static List<T> DecodeList<T>(Variant data)
		{
			List<T> list = new List<T>();
			foreach (Variant data2 in ((IEnumerable<Variant>)(data as ProxyArray)))
			{
				list.Add(JSON.DecodeType<T>(data2));
			}
			return list;
		}

		private static Dictionary<K, V> DecodeDictionary<K, V>(Variant data)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			foreach (KeyValuePair<string, Variant> keyValuePair in ((IEnumerable<KeyValuePair<string, Variant>>)(data as ProxyObject)))
			{
				K key = (K)((object)Convert.ChangeType(keyValuePair.Key, typeof(K)));
				V value = JSON.DecodeType<V>(keyValuePair.Value);
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		private static T[] DecodeArray<T>(Variant data)
		{
			ProxyArray proxyArray = data as ProxyArray;
			int count = proxyArray.Count;
			T[] array = new T[count];
			int num = 0;
			foreach (Variant data2 in ((IEnumerable<Variant>)(data as ProxyArray)))
			{
				array[num++] = JSON.DecodeType<T>(data2);
			}
			return array;
		}

		private static void DecodeMultiRankArray<T>(ProxyArray arrayData, Array array, int arrayRank, int[] indices)
		{
			int count = arrayData.Count;
			for (int i = 0; i < count; i++)
			{
				indices[arrayRank - 1] = i;
				if (arrayRank < array.Rank)
				{
					JSON.DecodeMultiRankArray<T>(arrayData[i] as ProxyArray, array, arrayRank + 1, indices);
				}
				else
				{
					array.SetValue(JSON.DecodeType<T>(arrayData[i]), indices);
				}
			}
		}

		private static void SupportTypeForAOT<T>()
		{
			JSON.DecodeType<T>(null);
			JSON.DecodeList<T>(null);
			JSON.DecodeArray<T>(null);
			JSON.DecodeDictionary<short, T>(null);
			JSON.DecodeDictionary<ushort, T>(null);
			JSON.DecodeDictionary<int, T>(null);
			JSON.DecodeDictionary<uint, T>(null);
			JSON.DecodeDictionary<long, T>(null);
			JSON.DecodeDictionary<ulong, T>(null);
			JSON.DecodeDictionary<float, T>(null);
			JSON.DecodeDictionary<double, T>(null);
			JSON.DecodeDictionary<decimal, T>(null);
			JSON.DecodeDictionary<bool, T>(null);
			JSON.DecodeDictionary<string, T>(null);
		}

		private static void SupportValueTypesForAOT()
		{
			JSON.SupportTypeForAOT<short>();
			JSON.SupportTypeForAOT<ushort>();
			JSON.SupportTypeForAOT<int>();
			JSON.SupportTypeForAOT<uint>();
			JSON.SupportTypeForAOT<long>();
			JSON.SupportTypeForAOT<ulong>();
			JSON.SupportTypeForAOT<float>();
			JSON.SupportTypeForAOT<double>();
			JSON.SupportTypeForAOT<decimal>();
			JSON.SupportTypeForAOT<bool>();
			JSON.SupportTypeForAOT<string>();
		}

		private static readonly Type includeAttrType = typeof(Include);

		private static readonly Type excludeAttrType = typeof(Exclude);

		private static Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

		private static BindingFlags instanceBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		private static BindingFlags staticBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		private static MethodInfo decodeTypeMethod = typeof(JSON).GetMethod("DecodeType", JSON.staticBindingFlags);

		private static MethodInfo decodeListMethod = typeof(JSON).GetMethod("DecodeList", JSON.staticBindingFlags);

		private static MethodInfo decodeDictionaryMethod = typeof(JSON).GetMethod("DecodeDictionary", JSON.staticBindingFlags);

		private static MethodInfo decodeArrayMethod = typeof(JSON).GetMethod("DecodeArray", JSON.staticBindingFlags);

		private static MethodInfo decodeMultiRankArrayMethod = typeof(JSON).GetMethod("DecodeMultiRankArray", JSON.staticBindingFlags);
	}
}
