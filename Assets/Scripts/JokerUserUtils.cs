// @sonhg: class: JokerUserUtils
using System;
using Sfs2X.Entities;

public class JokerUserUtils
{
	public static long GetUserId(User user)
	{
		if (user.ContainsVariable("userId"))
		{
			return Convert.ToInt64(user.GetVariable("userId").GetDoubleValue());
		}
		return -1L;
	}

	private static string GetDisplayName(User user)
	{
		if (user.ContainsVariable("displayname"))
		{
			return user.GetVariable("displayname").GetStringValue();
		}
		return string.Empty;
	}

	public static string GetGameName(User user)
	{
		if (user.ContainsVariable("gameName"))
		{
			return user.GetVariable("gameName").GetStringValue().ToUpper();
		}
		return string.Empty;
	}

	public static int GetSex(User user)
	{
		if (user.ContainsVariable("sex"))
		{
			return user.GetVariable("sex").GetIntValue();
		}
		return -1;
	}

	public static int GetAccountType(User user)
	{
		if (user.ContainsVariable("accountType"))
		{
			return user.GetVariable("accountType").GetIntValue();
		}
		return -1;
	}

	public static int GetLevel(User user)
	{
		if (user.ContainsVariable("level"))
		{
			return user.GetVariable("level").GetIntValue();
		}
		return -1;
	}

	public static int GetExp(User user)
	{
		if (user.ContainsVariable("exp"))
		{
			return user.GetVariable("exp").GetIntValue();
		}
		return -1;
	}

	public static int GetExpNextLevel(User user)
	{
		if (user.ContainsVariable("expToNextLevel"))
		{
			return user.GetVariable("expToNextLevel").GetIntValue();
		}
		return -1;
	}

	public static int GetGold(User user)
	{
		if (user.ContainsVariable("gold"))
		{
			return user.GetVariable("gold").GetIntValue();
		}
		return -1;
	}

	public static int GetChip(User user)
	{
		if (user.ContainsVariable("chip"))
		{
			return user.GetVariable("chip").GetIntValue();
		}
		return -1;
	}

	public static int GetWin(User user)
	{
		if (user.ContainsVariable("win"))
		{
			return user.GetVariable("win").GetIntValue();
		}
		return -1;
	}

	public static int GetLose(User user)
	{
		if (user.ContainsVariable("lose"))
		{
			return user.GetVariable("lose").GetIntValue();
		}
		return -1;
	}

	public static string GetAvatar(User user)
	{
		if (user.ContainsVariable("avatar"))
		{
			return user.GetVariable("avatar").GetStringValue();
		}
		return string.Empty;
	}

	public static int GetBiggestWin(User user)
	{
		if (user.ContainsVariable("biggestWin"))
		{
			return user.GetVariable("biggestWin").GetIntValue();
		}
		return -1;
	}

	public static string GetCountry(User user)
	{
		if (user.ContainsVariable("country"))
		{
			return user.GetVariable("country").GetStringValue();
		}
		return "vn";
	}

	public static string GetFormatChip(User user)
	{
		return Joker2XUtils.FormatChip(JokerUserUtils.GetChip(user));
	}

	public static string GetFormatGold(User user)
	{
		return Joker2XUtils.FormatChip(JokerUserUtils.GetGold(user));
	}

	public static string GetFormatDisplayName(User user, int maxleng = 0)
	{
		if (maxleng < 1)
		{
			maxleng = 7;
		}
		string result = string.Empty;
		string displayName = JokerUserUtils.GetDisplayName(user);
		if (displayName.Length < maxleng)
		{
			result = displayName;
		}
		else
		{
			result = displayName.Substring(0, maxleng);
		}
		return result;
	}

	public const int DISPLAY_NAME_MAX_LEN = 7;

	public const int INVALID_VERSION = -1;

	public const long INVALID_USERID = -1L;

	public const string INVALID_DISPLAY_NAME = "";

	public const int INVALID_SEX = -1;

	public const int INVALID_ACCOUNT_TYPE = -1;

	public const int INVALID_LEVEL = -1;

	public const int INVALID_EXP = -1;

	public const int INVALID_GOLD = -1;

	public const int INVALID_CHIP = -1;

	public const int INVALID_WIN = -1;

	public const int INVALID_LOSE = -1;

	public const string INVALID_AVATAR = "";

	public const int INVALID_BIGGEST_WIN = -1;

	public const string INVALID_COUNTRY = "vn";

	public const string INVALID_FIRST_ACCESS = "";

	public const string INVALID_LAST_ACCESS = "";

	public const int INVALID_ONLINE_TIME = -1;

	public const string INVALID_CP_NAME = "";

	public const string INVALID_DATA = "";

	public const string INVALID_DEVICEID = "";

	public const string INVALID_DEVICE_TOKEN = "";

	public const int INVALID_LANGUAGE = -1;

	public const int INVALID_DEVICE_TYPE = -1;
}
