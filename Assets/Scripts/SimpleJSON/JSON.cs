// @sonhg: class: SimpleJSON.JSON
using System;

namespace SimpleJSON
{
	public static class JSON
	{
		public static JSONNode Parse(string aJSON)
		{
			return JSONNode.Parse(aJSON);
		}
	}
}
