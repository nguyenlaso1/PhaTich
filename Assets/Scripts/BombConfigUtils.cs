// @sonhg: class: BombConfigUtils
using System;
using Sfs2X.Entities.Data;

public class BombConfigUtils
{
	public static void SetInfo(SFSObject dataObject)
	{
		if (dataObject.ContainsKey("BASE_RESOURCE_PATH"))
		{
			BombConfigUtils.BASE_RESOURCE_PATH = dataObject.GetUtfString("BASE_RESOURCE_PATH");
		}
		if (dataObject.ContainsKey("OFFLINE_ONLY"))
		{
			BombConfigUtils.OFFLINE_ONLY = dataObject.GetUtfString("OFFLINE_ONLY").Equals("1");
		}
	}

	public static string BASE_RESOURCE_PATH = "http://42.112.17.136/joker2x_bomb/";

	public static bool OFFLINE_ONLY = true;
}
