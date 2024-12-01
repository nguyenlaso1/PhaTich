// @sonhg: class: EnumUtils
using System;

public static class EnumUtils
{
	public static T ToEnum<T>(this string value)
	{
		return (T)((object)Enum.Parse(typeof(T), value, true));
	}
}
