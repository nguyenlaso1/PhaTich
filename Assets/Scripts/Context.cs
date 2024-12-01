// @sonhg: class: Context
using System;
using System.Collections.Generic;
//using Facebook.Unity;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;

public class Context
{
	public static UserItemList userItemList
	{
		get
		{
			if (Context._userItemList != null)
			{
				return Context._userItemList;
			}
			if (Context.arrUserItemList != null)
			{
				Context._userItemList = new UserItemList(Context.arrUserItemList);
				return Context._userItemList;
			}
			return null;
		}
	}

	public static ISFSArray arrUserItemList
	{
		get
		{
			if (Context._arrUserItemList != null)
			{
				return Context._arrUserItemList;
			}
			if (PlayerPrefs.HasKey(StoreKey.KEY_USER_ITEM))
			{
				string @string = PlayerPrefs.GetString(StoreKey.KEY_USER_ITEM);
				byte[] buf = Convert.FromBase64String(@string);
				Context._arrUserItemList = SFSArray.NewFromBinaryData(new ByteArray(buf));
				return Context._arrUserItemList;
			}
			return null;
		}
		set
		{
			Context._arrUserItemList = value;
			byte[] bytes = Context._arrUserItemList.ToBinary().Bytes;
			PlayerPrefs.SetString(StoreKey.KEY_USER_ITEM, Convert.ToBase64String(bytes));
		}
	}

	public static BaseGameInfo GameInfo
	{
		get
		{
			if (Context._gameInfo == null)
			{
				Context._gameInfo = new GameInfoAndroid();
			}
			return Context._gameInfo;
		}
	}

	public static void Init()
	{
		OneSignal.Init(Config.pushNotifiactionID, Config.googleProjectID, delegate(string message, Dictionary<string, object> additionalData, bool isActive)
		{
			UnityEngine.Debug.Log("Init push notification");
		});
		Context.GameInfo.InitAppsFlyer();
		Context.ActiveFBApp();
		Context.clientGameState = JokerEnum.ClientGameState.GS_INIT_GAME;
		Screen.sleepTimeout = -1;
		Application.runInBackground = true;
		NGUITools.soundVolume = Context.GameInfo.Volume;
		Context.GameInfo.DeviceLanguageId = 0;
		Localization.language = Context.GameInfo.DeviceLanguageName;
	}

	public static AsyncOperation LoadLevelAsyn(string scene, bool isFade = true)
	{
		UnityEngine.Debug.Log("LoadLevelAsyn " + scene);
		SmartFoxConnection.UnregisterSFSSceneCallbacks();
		return Application.LoadLevelAsync(scene.Trim());
	}

	public static bool IsLoadingScece
	{
		get
		{
			return Context.currentMono == null || Context.currentMono.gameObject == null;
		}
	}

	public static string SceneName
	{
		get
		{
			if (Context.IsLoadingScece)
			{
				return string.Empty;
			}
			return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		}
	}

	public static void ActiveFBApp()
	{
		try
		{
			//FB.ActivateApp();
		}
		catch (Exception ex)
		{
		}
	}

	public static void ShowGameStateDes()
	{
		if (Context.clientGameState == JokerEnum.ClientGameState.GS_RECONNECT_TIMEOUT)
		{
			Context.OnDeletegateObject onYesClick = delegate(object obj)
			{
				Application.LoadLevelAsync("BomberMap");
			};
			Context.Confirm.AddMessageYesNo(Language.OFFLINE_GS_RECONNECT_TIMEOUT, onYesClick, null, null, null, string.Empty, string.Empty, false);
		}
		else if (Context.ReasonGameStates.Contains(Context.clientGameState))
		{
			string gameStateDes = Language.GetGameStateDes(Context.clientGameState);
			Context.OnDeletegateObject onDeletegateObject = delegate(object o)
			{
				Context.Messenger.CloseBox();
			};
			Context.Messenger.AddOkMessage(gameStateDes, null, null, string.Empty, string.Empty);
		}
	}

	public static void ShowLoginScreen(JokerEnum.ClientGameState gameState)
	{
		UnityEngine.Debug.LogError("ShowLoginScreen");
		Context.clientGameState = gameState;
		Context.ResetLoginStore();
		Config.backupServerIndex = -1;
		if (Context.SceneName != "Loading")
		{
			Context.LoadLevelAsyn("Loading", true);
		}
		else
		{
			Context.Waiting.HideWaiting();
			SmartFoxConnection.UnregisterSFSSceneCallbacks();
			Context.ShowGameStateDes();
			if (Context.currentMono is LoadingScene)
			{
				((LoadingScene)Context.currentMono).ActiveBottomButton();
			}
		}
		if (Context.DestroySfsGameStates.Contains(Context.clientGameState))
		{
			UnityEngine.Debug.Log("destroySfsStates" + Context.clientGameState.ToString());
			SmartFoxConnection.Destroy();
		}
		if (Context.LogoutFBGameStates.Contains(Context.clientGameState))
		{
			UnityEngine.Debug.Log("LogoutFBGameStates" + Context.clientGameState.ToString());
			Context.LogoutFB();
		}
	}

	public static void ShowMainmenuScreen()
	{
		UnityEngine.Debug.Log("ShowMainmenuScreen");
		Context.Waiting.HideWaiting();
		SmartFoxConnection.UnregisterSFSSceneCallbacks();
		if (Context.SceneName != "BomberMainMenu")
		{
			Context.LoadLevelAsyn("BomberMainMenu", true);
		}
	}

	public static void ResetLoginStore()
	{
		UnityEngine.Debug.Log("DeleteLoginStore");
		Context.GameInfo.DeleteKey(StoreKey.KEY_LAST_ACCOUNT_TYPE);
		Context.GameInfo.DeleteKey(StoreKey.KEY_USERNAME);
		Context.GameInfo.DeleteKey(StoreKey.KEY_PASSWORD);
		Context.fbKey = string.Empty;
	}

	public static void LogoutFB()
	{
		//if (FB.IsLoggedIn)
		//{
		//	FB.LogOut();
		//}
		Context.fbKey = string.Empty;
	}

	public static JokerEnum.ClientGameState MapServerToClientGameState(int serverGameState)
	{
		if (serverGameState == 1)
		{
			return JokerEnum.ClientGameState.GS_IN_GAME_WAITING;
		}
		if (serverGameState != 2)
		{
			return JokerEnum.ClientGameState.GS_IN_GAME_WAITING;
		}
		return JokerEnum.ClientGameState.GS_IN_GAME_PLAYING;
	}

	public static MessageBox Messenger
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/MessageBox").GetComponent<MessageBox>();
		}
	}

	public static TooltipBox Tooltip
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/TooltipBoxDropDown").GetComponent<TooltipBox>();
		}
	}

	public static WaitingBox Waiting
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/WaitingBox").GetComponent<WaitingBox>();
		}
	}

	public static ConfirmBox Confirm
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/ConfirmPopup").GetComponent<ConfirmBox>();
		}
	}

	public static MainMenuSettingBox Setting
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Bomber/Boxs/GameSettingBox").GetComponent<MainMenuSettingBox>();
		}
	}

	public static GameObject Profile
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Bomber/Boxs/ProfileBox");
		}
	}

	public static ChooseModeBox ChooseMode
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Bomber/Boxs/ChooseGameBox").GetComponent<ChooseModeBox>();
		}
	}

	public static RankingBox Ranking
	{
		get
		{
			return StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/RankingBox").GetComponent<RankingBox>();
		}
	}

	public static BaseScene currentMono;

	public static string fbKey = string.Empty;

	public static JokerEnum.ClientGameState clientGameState = JokerEnum.ClientGameState.GS_INIT_GAME;

	public static string currentGameId = string.Empty;

	public static int currentGroupIndex;

	public static int currentRoomId = -1;

	public static int currentJoinType = -1;

	public static int dailyGiftChip = -1;

	public static int daysGiftChip = -1;

	public static int position;

	public static string loginMessage = string.Empty;

	public static GoogleAnalyticsV3 googleAnalytics;

	private static UserItemList _userItemList;

	private static ISFSArray _arrUserItemList;

	private static BaseGameInfo _gameInfo;

	public static List<JokerEnum.ClientGameState> DestroySfsGameStates = new List<JokerEnum.ClientGameState>
	{
		JokerEnum.ClientGameState.GS_LOGIN_NOT_SUCCESS,
		JokerEnum.ClientGameState.GS_LOGIN_ERROR,
		JokerEnum.ClientGameState.GS_RECONNECT_TIMEOUT,
		JokerEnum.ClientGameState.GS_REQUEST_TIMEOUT,
		JokerEnum.ClientGameState.GS_LOGINFB_ERROR,
		JokerEnum.ClientGameState.GS_GAME_BAN,
		JokerEnum.ClientGameState.GS_GAME_KICK,
		JokerEnum.ClientGameState.GS_GAME_IDLE,
		JokerEnum.ClientGameState.GS_GAME_UNKNOWN,
		JokerEnum.ClientGameState.GS_LOGIN_ERROR_BAN
	};

	public static List<JokerEnum.ClientGameState> LogoutFBGameStates = new List<JokerEnum.ClientGameState>
	{
		JokerEnum.ClientGameState.GS_LOGINFB_ERROR,
		JokerEnum.ClientGameState.GS_LOGIN_ERROR_BAN,
		JokerEnum.ClientGameState.GS_LOGOUT_NORMAL,
		JokerEnum.ClientGameState.GS_GAME_BAN,
		JokerEnum.ClientGameState.GS_GAME_KICK,
		JokerEnum.ClientGameState.GS_GAME_IDLE,
		JokerEnum.ClientGameState.GS_GAME_UNKNOWN
	};

	public static List<JokerEnum.ClientGameState> ReasonGameStates = new List<JokerEnum.ClientGameState>
	{
		JokerEnum.ClientGameState.GS_LOGIN_NOT_SUCCESS,
		JokerEnum.ClientGameState.GS_LOGIN_ERROR,
		JokerEnum.ClientGameState.GS_RECONNECT_TIMEOUT,
		JokerEnum.ClientGameState.GS_REQUEST_TIMEOUT,
		JokerEnum.ClientGameState.GS_LOGINFB_ERROR,
		JokerEnum.ClientGameState.GS_GAME_BAN,
		JokerEnum.ClientGameState.GS_GAME_KICK,
		JokerEnum.ClientGameState.GS_GAME_IDLE,
		JokerEnum.ClientGameState.GS_GAME_UNKNOWN,
		JokerEnum.ClientGameState.GS_LOGIN_ERROR_BAN
	};

	public delegate void OnDeletegateObject(object obj = null);

	public delegate void OnDeletegateNone();
}
