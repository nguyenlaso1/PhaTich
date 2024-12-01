// @sonhg: class: Language
using System;

public class Language
{
	public static string GetGameStateDes(JokerEnum.ClientGameState gameState)
	{
		switch (gameState)
		{
		case JokerEnum.ClientGameState.GS_INIT_GAME:
			return Language.GS_INIT_GAME;
		case JokerEnum.ClientGameState.GS_LOGOUT_NORMAL:
			return Language.GS_LOGOUT_NORMAL;
		case JokerEnum.ClientGameState.GS_LOGOUT_MERGE_FB:
			return Language.GS_LOGOUT_MERGE_FB;
		case JokerEnum.ClientGameState.GS_LOGIN_NOT_SUCCESS:
			return Language.GS_LOGIN_NOT_SUCCESS;
		case JokerEnum.ClientGameState.GS_LOGIN_ERROR:
			return Language.GS_LOGIN_ERROR;
		case JokerEnum.ClientGameState.GS_RECONNECT_TIMEOUT:
			return Language.GS_RECONNECT_TIMEOUT;
		case JokerEnum.ClientGameState.GS_REQUEST_TIMEOUT:
			return Language.GS_REQUEST_TIMEOUT;
		case JokerEnum.ClientGameState.GS_CANCEL_FB:
			return Language.GS_CANCEL_FB;
		case JokerEnum.ClientGameState.GS_LOGINFB_ERROR:
			return Language.GS_LOGINFB_ERROR;
		case JokerEnum.ClientGameState.GS_GAME_BAN:
			return Language.GS_GAME_BAN;
		case JokerEnum.ClientGameState.GS_GAME_KICK:
			return Language.GS_GAME_KICK;
		case JokerEnum.ClientGameState.GS_GAME_IDLE:
			return Language.GS_GAME_IDLE;
		case JokerEnum.ClientGameState.GS_LOGIN_ERROR_BAN:
			return Language.GS_LOGIN_ERROR_BAN;
		case JokerEnum.ClientGameState.GS_GAME_UNKNOWN:
			return Language.GS_GAME_UNKNOWN;
		}
		return string.Empty;
	}

	public static string GetGameName(string key)
	{
		return Localization.Get(key);
	}

	public static string CHIP
	{
		get
		{
			return Localization.Get("CHIP");
		}
	}

	public static string GS_INIT_GAME
	{
		get
		{
			return Localization.Get("GS_INIT_GAME");
		}
	}

	public static string GS_LOGOUT_NORMAL
	{
		get
		{
			return Localization.Get("GS_LOGOUT_NORMAL");
		}
	}

	public static string GS_LOGOUT_MERGE_FB
	{
		get
		{
			return Localization.Get("GS_LOGOUT_MERGE_FB");
		}
	}

	public static string GS_LOGIN_NOT_SUCCESS
	{
		get
		{
			return Localization.Get("GS_LOGIN_NOT_SUCCESS");
		}
	}

	public static string GS_LOGIN_ERROR
	{
		get
		{
			return Localization.Get("GS_LOGIN_ERROR");
		}
	}

	public static string MERGE_FB_REQUEST
	{
		get
		{
			return Localization.Get("MERGE_FB_REQUEST");
		}
	}

	public static string MERGE_FB
	{
		get
		{
			return Localization.Get("MERGE_FB");
		}
	}

	public static string GS_GAME_BAN
	{
		get
		{
			return Localization.Get("GS_GAME_BAN");
		}
	}

	public static string GS_RECONNECT_TIMEOUT
	{
		get
		{
			return Localization.Get("GS_RECONNECT_TIMEOUT");
		}
	}

	public static string OFFLINE_GS_RECONNECT_TIMEOUT
	{
		get
		{
			return Localization.Get("OFFLINE_GS_RECONNECT_TIMEOUT");
		}
	}

	public static string GS_REQUEST_TIMEOUT
	{
		get
		{
			return Localization.Get("GS_REQUEST_TIMEOUT");
		}
	}

	public static string GS_CANCEL_FB
	{
		get
		{
			return Localization.Get("GS_CANCEL_FB");
		}
	}

	public static string GS_LOGINFB_ERROR
	{
		get
		{
			return Localization.Get("GS_LOGINFB_ERROR");
		}
	}

	public static string GS_GAME_KICK
	{
		get
		{
			return Localization.Get("GS_GAME_KICK");
		}
	}

	public static string GS_GAME_IDLE
	{
		get
		{
			return Localization.Get("GS_GAME_IDLE");
		}
	}

	public static string GS_GAME_UNKNOWN
	{
		get
		{
			return Localization.Get("GS_GAME_UNKNOWN");
		}
	}

	public static string GS_LOGIN_ERROR_BAN
	{
		get
		{
			return Localization.Get("GS_LOGIN_ERROR_BAN");
		}
	}

	public static string MSG_OK
	{
		get
		{
			return Localization.Get("MSG_OK");
		}
	}

	public static string MSG_ACCEPT
	{
		get
		{
			return Localization.Get("MSG_ACCEPT");
		}
	}

	public static string MSG_CANCEL
	{
		get
		{
			return Localization.Get("MSG_CANCEL");
		}
	}

	public static string MSG_YES
	{
		get
		{
			return Localization.Get("MSG_YES");
		}
	}

	public static string MSG_NO
	{
		get
		{
			return Localization.Get("MSG_NO");
		}
	}

	public static string SYSTEM_MESSAGE_TITLE
	{
		get
		{
			return Localization.Get("SYSTEM_MESSAGE_TITLE");
		}
	}

	public static string EXIT_CONFIRM
	{
		get
		{
			return Localization.Get("EXIT_CONFIRM");
		}
	}

	public static string FEEDBACK_THANKS
	{
		get
		{
			return Localization.Get("FEEDBACK_THANKS");
		}
	}

	public static string DATA_MISTYPED
	{
		get
		{
			return Localization.Get("DATA_MISTYPED");
		}
	}

	public static string BACK_MAINMENU_CONFIRM
	{
		get
		{
			return Localization.Get("BACK_MAINMENU_CONFIRM");
		}
	}

	public static string BACK_MAINMENU_CONFIRM_PLAYING
	{
		get
		{
			return Localization.Get("BACK_MAINMENU_CONFIRM_PLAYING");
		}
	}

	public static string ROOM
	{
		get
		{
			return Localization.Get("ROOM");
		}
	}

	public static string NOTIFY_RECONNECT
	{
		get
		{
			return Localization.Get("NOTIFY_RECONNECT");
		}
	}

	public static string PROFILE_FB_LOGOUT
	{
		get
		{
			return Localization.Get("PROFILE_FB_LOGOUT");
		}
	}

	public static string INVITE_INVITE
	{
		get
		{
			return Localization.Get("INVITE_INVITE");
		}
	}

	public static string PLAY_NOW_NOT_FOUND
	{
		get
		{
			return Localization.Get("PLAY_NOW_NOT_FOUND");
		}
	}

	public static string ALL_ROOM_ARE_BUSY
	{
		get
		{
			return Localization.Get("ALL_ROOM_ARE_BUSY");
		}
	}

	public static string PLAY_OTHER_GAME
	{
		get
		{
			return Localization.Get("PLAY_OTHER_GAME");
		}
	}

	public static string CREATE_NEW_ROOM
	{
		get
		{
			return Localization.Get("CREATE_NEW_ROOM");
		}
	}

	public static string TOPUP_PURCHASED_SUCCESSFUL
	{
		get
		{
			return Localization.Get("TOPUP_PURCHASED_SUCCESSFUL");
		}
	}

	public static string PROFILE_LV
	{
		get
		{
			return Localization.Get("PROFILE_LV");
		}
	}

	public static string PROFILE_WIN_RATING
	{
		get
		{
			return Localization.Get("PROFILE_WIN_RATING");
		}
	}

	public static string NO_READY
	{
		get
		{
			return Localization.Get("NO_READY");
		}
	}

	public static string INVITE_FRIEND_NOT_FOUND
	{
		get
		{
			return Localization.Get("INVITE_FRIEND_NOT_FOUND");
		}
	}

	public static string DAILY_CHIP
	{
		get
		{
			return Localization.Get("DAILY_CHIP");
		}
	}

	public static string TOPUP_IN_PROGRESS
	{
		get
		{
			return Localization.Get("TOPUP_IN_PROGRESS");
		}
	}

	public static string SETTING_UPDATE
	{
		get
		{
			return Localization.Get("SETTING_UPDATE");
		}
	}

	public static string TOPUP_PURCHASED_NOT_SUPPORTED
	{
		get
		{
			return Localization.Get("TOPUP_PURCHASED_NOT_SUPPORTED");
		}
	}

	public static string NOT_ENOUGH_MONEY
	{
		get
		{
			return Localization.Get("NOT_ENOUGH_MONEY");
		}
	}

	public static string EXIT_IM
	{
		get
		{
			return Localization.Get("EXIT_IM");
		}
	}

	public static string WAIT_END_GAME
	{
		get
		{
			return Localization.Get("WAIT_END_GAME");
		}
	}

	public static string HOT_LINE
	{
		get
		{
			return "Hotline: " + Joker2XConfigUtils.HOT_LINE;
		}
	}

	public static string LEAVE_AT_GAME
	{
		get
		{
			return Localization.Get("LEAVE_AT_GAME");
		}
	}

	public static string KICK_CONFIRM
	{
		get
		{
			return Localization.Get("KICK_CONFIRM");
		}
	}

	public static string KICK_YES
	{
		get
		{
			return Localization.Get("KICK_YES");
		}
	}

	public static string NOT_ENOUGH_CHIP_YES
	{
		get
		{
			return Localization.Get("NOT_ENOUGH_CHIP_YES");
		}
	}

	public static string NOT_ENOUGH_CHIP_NO
	{
		get
		{
			return Localization.Get("NOT_ENOUGH_CHIP_NO");
		}
	}

	public static string DAILY_REWARD
	{
		get
		{
			return Localization.Get("DAILY_REWARD");
		}
	}

	public static string GetDailyChipDayText(int day)
	{
		return Localization.Get("DAILY_CHIP_DAY" + day);
	}

	public static string NOTIFY_UPDATE_LINK
	{
		get
		{
			return Localization.Get("NOTIFY_UPDATE_LINK");
		}
	}

	public static string JOIN_ROOM_NOT_ENOUGH_CHIP
	{
		get
		{
			return Localization.Get("JOIN_ROOM_NOT_ENOUGH_CHIP");
		}
	}

	public static string INVITE_FB_FRIENDS_ERROR
	{
		get
		{
			return Localization.Get("INVITE_FB_FRIENDS_ERROR");
		}
	}

	public static string INVITE_FB_FRIENDS_SUCCESS
	{
		get
		{
			return Localization.Get("INVITE_FB_FRIENDS_SUCCESS");
		}
	}

	public static string INVITE_FB_FRIENDS_NOTPERMITTED
	{
		get
		{
			return Localization.Get("INVITE_FB_FRIENDS_NOTPERMITTED");
		}
	}

	public static string KICKED_TIMEOUT
	{
		get
		{
			return Localization.Get("KICKED_TIMEOUT");
		}
	}

	public static string NETWORK_UNSTABLE
	{
		get
		{
			return Localization.Get("NETWORK_UNSTABLE");
		}
	}
}
