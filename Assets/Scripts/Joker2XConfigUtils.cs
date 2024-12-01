// @sonhg: class: Joker2XConfigUtils
using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class Joker2XConfigUtils
{
	public static void SetInfo(SFSObject dataObject)
	{
		Joker2XConfigUtils.IS_JOKER2X_CONFIG_LOADED = true;
		UnityEngine.Debug.Log(" Joker2XConfigUtils --------- dataObject: " + dataObject.GetDump());
		if (dataObject.ContainsKey("UPDATE_LINK"))
		{
			Joker2XConfigUtils.UPDATE_LINK = dataObject.GetUtfString("UPDATE_LINK");
		}
		if (dataObject.ContainsKey("PING_IP"))
		{
			Joker2XConfigUtils.PING_IP = dataObject.GetUtfString("PING_IP");
		}
		if (dataObject.ContainsKey("PINGPONG_TIME"))
		{
			Joker2XConfigUtils.PINGPONG_TIME = int.Parse(dataObject.GetUtfString("PINGPONG_TIME").Trim());
		}
		if (dataObject.ContainsKey("PING_REPEAT"))
		{
			Joker2XConfigUtils.PING_REPEAT = int.Parse(dataObject.GetUtfString("PING_REPEAT").Trim());
		}
		if (Config.turnOnInapp)
		{
			Joker2XConfigUtils.TURN_ON_INAPP = bool.Parse(dataObject.GetUtfString("TURN_ON_INAPP").Trim());
		}
		else
		{
			Joker2XConfigUtils.TURN_ON_INAPP = false;
		}
		if (Config.turnOnCard)
		{
			Joker2XConfigUtils.TURN_ON_CARD = bool.Parse(dataObject.GetUtfString("TURN_ON_CARD").Trim());
		}
		else
		{
			Joker2XConfigUtils.TURN_ON_CARD = false;
		}
		if (Config.turnOnSms)
		{
			Joker2XConfigUtils.TURN_ON_SMS = bool.Parse(dataObject.GetUtfString("TURN_ON_SMS").Trim());
		}
		else
		{
			Joker2XConfigUtils.TURN_ON_SMS = false;
		}
		if (dataObject.ContainsKey("HOT_LINE"))
		{
			Joker2XConfigUtils.HOT_LINE = dataObject.GetUtfString("HOT_LINE");
		}
		if (dataObject.ContainsKey("RECONNECT_TIMEOUT"))
		{
			Joker2XConfigUtils.RECONNECT_TIMEOUT = float.Parse(dataObject.GetUtfString("RECONNECT_TIMEOUT").Trim());
		}
		if (dataObject.ContainsKey("TURN_ON_ADMOB"))
		{
			string[] array = dataObject.GetUtfString("TURN_ON_ADMOB").Trim().Split(new char[]
			{
				','
			});
			if (array.Length == 2)
			{
				Joker2XConfigUtils.TURN_ON_ADMOB_OFFLINE = array[0].Equals("1");
				Joker2XConfigUtils.TURN_ON_ADMOB_ONLINE = array[1].Equals("1");
			}
		}
		if (dataObject.ContainsKey("TURN_ON_BANNER"))
		{
			Joker2XConfigUtils.TURN_ON_BANNER = dataObject.GetUtfString("TURN_ON_BANNER").Equals("1");
		}
		if (dataObject.ContainsKey("ONLINE_PLAY_COUNT_TYPE"))
		{
			Joker2XConfigUtils.ONLINE_PLAY_COUNT_TYPE = int.Parse(dataObject.GetUtfString("ONLINE_PLAY_COUNT_TYPE").Trim());
		}
		if (dataObject.ContainsKey("ONLINE_ADMOB_FREQUENCE"))
		{
			Joker2XConfigUtils.ONLINE_ADMOB_FREQUENCE = int.Parse(dataObject.GetUtfString("ONLINE_ADMOB_FREQUENCE").Trim());
		}
		if (dataObject.ContainsKey("ONLINE_ADMOB_TIME"))
		{
			Joker2XConfigUtils.ONLINE_ADMOB_TIME = int.Parse(dataObject.GetUtfString("ONLINE_ADMOB_TIME").Trim());
		}
		if (dataObject.ContainsKey("ONLINE_ADMOB_GOLD"))
		{
			Joker2XConfigUtils.ONLINE_ADMOB_GOLD = int.Parse(dataObject.GetUtfString("ONLINE_ADMOB_GOLD").Trim());
		}
		if (dataObject.ContainsKey("ADMOB_POSITION"))
		{
			Joker2XConfigUtils.ADMOB_POSITION = int.Parse(dataObject.GetUtfString("ADMOB_POSITION").Trim());
		}
		if (dataObject.ContainsKey("ADMOB_SMART_ADS"))
		{
			Joker2XConfigUtils.ADMOB_SMART_ADS = dataObject.GetUtfString("ADMOB_SMART_ADS").Trim().Equals("1");
		}
		if (dataObject.ContainsKey("LIST_GAME_ID"))
		{
			Joker2XConfigUtils.LIST_GAMES_ID = GameNames.RemoveUnused(dataObject.GetUtfString("LIST_GAME_ID").Split(new char[]
			{
				','
			}));
		}
		if (dataObject.ContainsKey("PING_CONSTANT"))
		{
			Joker2XConfigUtils.PING_CONSTANT = int.Parse(dataObject.GetUtfString("PING_CONSTANT"));
		}
		if (dataObject.ContainsKey("FB_APPLINK_URI"))
		{
			Joker2XConfigUtils.FB_APPLINK_URI = dataObject.GetUtfString("FB_APPLINK_URI");
		}
		if (dataObject.ContainsKey("FB_APPLINK_PICTURE_URI"))
		{
			Joker2XConfigUtils.FB_APPLINK_PICTURE_URI = dataObject.GetUtfString("FB_APPLINK_PICTURE_URI");
		}
	}

	public static bool IsLoaded()
	{
		return Joker2XConfigUtils.IS_JOKER2X_CONFIG_LOADED;
	}

	public static bool IS_JOKER2X_CONFIG_LOADED = false;

	public static string URL_PHOTO = "http://pokerair.com";

	public static List<string> PERM_FACEBOOK = new List<string>
	{
		"email",
		"public_profile",
		"user_friends"
	};

	public static string ANALYTIC_ID;

	public static string UPDATE_LINK = "http://bigfox.vn/update";

	public static bool TURN_ON_INAPP = true;

	public static bool TURN_ON_SMS = true;

	public static bool TURN_ON_CARD = true;

	public static bool TURN_ON_ATM = false;

	public static string HOT_LINE = "19002096";

	public static float RECONNECT_TIMEOUT = 8f;

	public static bool TURN_ON_BANNER = false;

	public static bool TURN_ON_ADMOB_OFFLINE = true;

	public static bool TURN_ON_ADMOB_ONLINE = true;

	public static int ONLINE_ADMOB_FREQUENCE = 300;

	public static int ONLINE_ADMOB_TIME = 2;

	public static int ONLINE_ADMOB_GOLD = 0;

	public static int ONLINE_PLAY_COUNT_TYPE = -1;

	public static int ADMOB_POSITION = 1;

	public static bool ADMOB_SMART_ADS = true;

	public static string PING_IP = "8.8.8.8";

	public static int PINGPONG_TIME = 30;

	public static List<string> LIST_GAMES_ID = new List<string>();

	public static int PING_REPEAT = 2;

	public static int PING_DELAY = 2;

	public static int PING_CONSTANT = 200;

	public static int PING_DURATION = 30;

	public static string FB_APPLINK_URI = "https://fb.me/2163741863766522";

	public static string FB_APPLINK_PICTURE_URI = "http://pokerair.com/joker2x_bomb/share_facebook.png";
}
