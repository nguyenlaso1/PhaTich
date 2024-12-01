// @sonhg: class: JokerEnum
using System;

public class JokerEnum
{
	public enum ClientGameState
	{
		GS_INIT_GAME,
		GS_LOGOUT_NORMAL,
		GS_LOGOUT_MERGE_FB,
		GS_LOGIN_NOT_SUCCESS,
		GS_LOGIN_ERROR,
		GS_RECONNECT_TIMEOUT,
		GS_REQUEST_TIMEOUT,
		GS_CANCEL_FB,
		GS_LOGINFB_ERROR,
		GS_GAME_BAN,
		GS_GAME_KICK,
		GS_GAME_IDLE,
		GS_LOGIN_ERROR_BAN,
		GS_ON_MAINMENU_NORMAL,
		GS_IN_GAME_WAITING,
		GS_IN_GAME_PLAYING,
		GS_ON_MAINMENU_TOPUP,
		GS_BACK_FROM_OFFLINE,
		GS_GAME_UNKNOWN
	}

	public enum LanguageId
	{
		EN,
		VN,
		CN,
		IN
	}

	public enum SystemMessageGroup
	{
		Popup,
		Tooltip
	}

	public enum LeaveType
	{
		PULL_ON_START,
		NOT_READY,
		OUT
	}

	public enum GroupId
	{
		r1,
		r2,
		r3,
		r4
	}

	public enum CardDistributor
	{
		Mobifone,
		Vinaphone,
		Viettel,
		Gate
	}

	public enum ServerGameState
	{
		GS_IN_GAME_WAITING = 1,
		GS_IN_GAME_PLAYING
	}
}
