// @sonhg: class: DataManager
using System;
using Sfs2X.Entities.Data;
using UnityEngine;

public class DataManager
{
	public static int InviteCount
	{
		get
		{
			return PlayerPrefs.GetInt("INVITE_COUNT", 0);
		}
		set
		{
			PlayerPrefs.SetInt("INVITE_COUNT", value);
			PlayerPrefs.Save();
		}
	}

	public static DateTime LastInvite
	{
		get
		{
			string @string = PlayerPrefs.GetString("LAST_INVITE", DateTime.Now.ToFileTime() + string.Empty);
			return DateTime.FromFileTime(long.Parse(@string));
		}
		set
		{
			PlayerPrefs.SetString("LAST_INVITE", value.ToFileTime() + string.Empty);
			PlayerPrefs.Save();
		}
	}

	public static int SharedCount
	{
		get
		{
			return PlayerPrefs.GetInt("SHARE_COUNT", 0);
		}
		set
		{
			PlayerPrefs.SetInt("SHARE_COUNT", value);
			PlayerPrefs.Save();
		}
	}

	public static DateTime LastShare
	{
		get
		{
			string @string = PlayerPrefs.GetString("LAST_SHARE", DateTime.Now.ToFileTime() + string.Empty);
			return DateTime.FromFileTime(long.Parse(@string));
		}
		set
		{
			PlayerPrefs.SetString("LAST_SHARE", value.ToFileTime() + string.Empty);
			PlayerPrefs.Save();
		}
	}

	public static bool Rated(string uri)
	{
		return PlayerPrefs.GetInt(uri, 0) != 0;
	}

	public static void Rate(string uri)
	{
		PlayerPrefs.SetInt(uri, 1);
		PlayerPrefs.Save();
	}

	public static void SetGold(int gold)
	{
		PlayerPrefs.SetInt("PlayerCoin", (gold >= 0) ? gold : 0);
		PlayerPrefs.Save();
	}

	public static int GetGold()
	{
		return PlayerPrefs.GetInt("PlayerCoin");
	}

	public static int PlusGold(int gold)
	{
		PlayerPrefs.SetInt("PlayerCoin", PlayerPrefs.GetInt("PlayerCoin") + gold);
		PlayerPrefs.Save();
		return PlayerPrefs.GetInt("PlayerCoin");
	}

	public static void MinusGold(int gold)
	{
		PlayerPrefs.SetInt("PlayerCoin", (PlayerPrefs.GetInt("PlayerCoin") - gold >= 0) ? (PlayerPrefs.GetInt("PlayerCoin") - gold) : 0);
		PlayerPrefs.Save();
	}

	public static string GetMyItem()
	{
		return PlayerPrefs.GetString("AllMyItem", "53-57-49-30");
	}

	public static void SetMyItem(string item)
	{
		PlayerPrefs.SetString("AllMyItem", item);
		PlayerPrefs.Save();
	}

	public static DateTime lastBonus
	{
		get
		{
			string @string = PlayerPrefs.GetString("LAST_DAILY_BONUS", DateTime.Now.AddDays(-1.0).ToFileTime() + string.Empty);
			return DateTime.FromFileTime(long.Parse(@string));
		}
		set
		{
			PlayerPrefs.SetString("LAST_DAILY_BONUS", value.ToFileTime() + string.Empty);
			PlayerPrefs.Save();
		}
	}

	public static ISFSObject GetMyItemHelper()
	{
		return SFSObject.NewFromJsonData(PlayerPrefs.GetString("AllMyItemHelper", "{}"));
	}

	public static void SetMyItemHelper(ISFSObject item)
	{
		PlayerPrefs.SetString("AllMyItemHelper", item.ToJson());
		PlayerPrefs.Save();
	}

	public static void AddMyItemHelper(string id, int num)
	{
		ISFSObject myItemHelper = DataManager.GetMyItemHelper();
		if (myItemHelper.ContainsKey(id))
		{
			myItemHelper.PutInt(id, (myItemHelper.GetInt(id) + num <= 9) ? (myItemHelper.GetInt(id) + num) : 9);
		}
		else
		{
			myItemHelper.PutInt(id, num);
		}
		PlayerPrefs.SetString("AllMyItemHelper", myItemHelper.ToJson());
		PlayerPrefs.Save();
	}

	public static bool MinusMyItemHelper(string id)
	{
		ISFSObject myItemHelper = DataManager.GetMyItemHelper();
		if (myItemHelper.ContainsKey(id) && myItemHelper.GetInt(id) > 0)
		{
			myItemHelper.PutInt(id, myItemHelper.GetInt(id) - 1);
			PlayerPrefs.SetString("AllMyItemHelper", myItemHelper.ToJson());
			PlayerPrefs.Save();
			return true;
		}
		return false;
	}

	public static int GetMyItemHelper(string id)
	{
		ISFSObject myItemHelper = DataManager.GetMyItemHelper();
		if (myItemHelper.ContainsKey(id))
		{
			return myItemHelper.GetInt(id);
		}
		return 0;
	}

	public static void SetItemHelper1(string id)
	{
		PlayerPrefs.SetString("SetItemHelper1", id);
		PlayerPrefs.Save();
	}

	public static string GetItemHelper1()
	{
		return PlayerPrefs.GetString("SetItemHelper1", null);
	}

	public static void SetItemHelper2(string id)
	{
		PlayerPrefs.SetString("SetItemHelper2", id);
		PlayerPrefs.Save();
	}

	public static string GetItemHelper2()
	{
		return PlayerPrefs.GetString("SetItemHelper2", null);
	}

	public static void AchievementCount(string id, int num)
	{
		PlayerPrefs.SetInt(id, num);
		PlayerPrefs.Save();
	}

	public static void AchievementCountPlus(string id, int num)
	{
		PlayerPrefs.SetInt(id, DataManager.AchievementCount(id) + num);
		PlayerPrefs.Save();
	}

	public static int AchievementCount(string id)
	{
		return PlayerPrefs.GetInt(id, 0);
	}

	public static void AchievementProgess(string id, int num)
	{
		PlayerPrefs.SetInt(id + "_PROGRESS", num);
		PlayerPrefs.Save();
	}

	public static int AchievementProgess(string id)
	{
		return PlayerPrefs.GetInt(id + "_PROGRESS", 0);
	}

	private const string LAST_INVITE = "LAST_INVITE";

	private const string INVITE_COUNT = "INVITE_COUNT";

	private const string LAST_SHARE = "LAST_SHARE";

	private const string SHARE_COUNT = "SHARE_COUNT";

	private const string LAST_DAILY_BONUS = "LAST_DAILY_BONUS";

	public const string RADAR = "11";

	public const string REMOTE_BOMB = "7";

	public const string SHOLVE = "9";

	public const string JUMP = "15";

	public const string SHIELD = "13";

	public const string MONSTER_KILL = "MONSTER_KILL";

	public const string DOUBLE_KILL = "DOUBLE_KILL";

	public const string TRIPLE_KILL = "TRIPLE_KILL";

	public const string ULTRA_KILL = "ULTRA_KILL";

	public const string PLAYER_DIE = "PLAYER_DIE";

	public const string USE_RADAR = "USE_RADAR";

	public const string USE_REMOTE_BOMB = "USE_REMOTE_BOMB";

	public const string USE_SHOVEL = "USE_SHOVEL";

	public const string USE_JUMP = "USE_JUMP";

	public const string USE_SHIELD = "USE_SHIELD";

	public const string RESCUE_ZONE = "RESCUE_ZONE";

	public const string LOGIN_FACEBOOK = "LOGIN_FACEBOOK";

	public const string KILL_QUEEN_BEE = "KILL_QUEEN_BEE";

	public const string KILL_TANK_WHALE = "KILL_TANK_WHALE";

	public const string KILL_LIVING_OASIS = "KILL_LIVING_OASIS";

	public const string KILL_CURSED_JELLY = "KILL_CURSED_JELLY";

	public const string KILL_LOCK_ARMOR = "KILL_LOCK_ARMOR";

	public const string KILL_NOVA_GOLEM = "KILL_NOVA_GOLEM";
}
