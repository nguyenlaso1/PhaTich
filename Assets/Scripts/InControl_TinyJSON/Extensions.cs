// @sonhg: class: InControl.TinyJSON.Extensions
using System;
using System.Collections.Generic;

namespace InControl.TinyJSON
{
	public static class Extensions
	{
		public static bool AnyOfType<TSource>(this IEnumerable<TSource> source, Type expectedType)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (expectedType == null)
			{
				throw new ArgumentNullException("expectedType");
			}
			foreach (TSource tsource in source)
			{
				if (expectedType.IsAssignableFrom(tsource.GetType()))
				{
					return true;
				}
			}
			return false;
		}
	}
}
