// @sonhg: class: BaseScene
using System;
using System.Collections;
//using Facebook.Unity;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Logging;
using Sfs2X.Util;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
	protected void ConnectServer()
	{
		this.smartFox = SmartFoxConnection.ConnectServer(Config.serverName, Config.serverPort, JokerSFSErrorCodes.CODES);
		this.AddEventListener();
	}

	protected void InitServer()
	{
		this.smartFox = SmartFoxConnection.Connection;
		if (this.smartFox == null)
		{
			UnityEngine.Debug.Log("InitServer");
			this.ConnectServer();
			Context.Waiting.ShowWaiting(Joker2XConfigUtils.RECONNECT_TIMEOUT, delegate(Hashtable input)
			{
				this.ReconnectTimeOut();
			});
		}
	}

	public virtual void AddEventListener()
	{
		SmartFoxConnection.UnregisterSFSSceneCallbacks();
		this.smartFox.AddEventListener(SFSEvent.CONNECTION, new EventListenerDelegate(this.OnConnection));
		this.smartFox.AddEventListener(SFSEvent.LOGIN, new EventListenerDelegate(this.OnLogin));
		this.smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, new EventListenerDelegate(this.OnLoginError));
		this.smartFox.AddEventListener(SFSEvent.LOGOUT, new EventListenerDelegate(this.OnLogout));
		this.smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, new EventListenerDelegate(this.OnConnectionLost));
		this.smartFox.AddEventListener(SFSEvent.CONNECTION_RESUME, new EventListenerDelegate(this.OnConnectResume));
		this.smartFox.AddEventListener(SFSEvent.CONNECTION_RETRY, new EventListenerDelegate(this.OnConnectRetry));
		this.smartFox.AddLogListener(LogLevel.DEBUG, new EventListenerDelegate(this.OnDebugMessage));
		this.smartFox.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, new EventListenerDelegate(this.OnUserVariablesUpdate));
		this.smartFox.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, new EventListenerDelegate(this.OnRoomVariablesUpdate));
		this.smartFox.AddEventListener(SFSEvent.ADMIN_MESSAGE, new EventListenerDelegate(this.OnAdminMessageResponse));
		this.smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, new EventListenerDelegate(this.OnExtensionResponse));
		this.smartFox.AddEventListener(SFSEvent.USER_FIND_RESULT, new EventListenerDelegate(this.OnUserFindResponse));
		this.smartFox.AddEventListener(SFSEvent.INVITATION_REPLY_ERROR, new EventListenerDelegate(this.OnInvitationReplyError));
		this.smartFox.AddEventListener(SFSEvent.INVITATION, new EventListenerDelegate(this.OnInvitation));
		this.smartFox.AddEventListener(SFSEvent.PUBLIC_MESSAGE, new EventListenerDelegate(this.OnPublicMessage));
		this.smartFox.AddEventListener(SFSEvent.ROOM_JOIN, new EventListenerDelegate(this.OnJoinRoom));
		this.smartFox.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, new EventListenerDelegate(this.OnJoinRoomError));
	}

	private void FixedUpdate()
	{
		if (this.smartFox != null)
		{
			this.smartFox.ProcessEvents();
		}
	}

	private void Update()
	{
		this.JokerUpdate();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !BaseBox.CloseCurrentBox())
		{
			this.OnPressEscape();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Menu))
		{
			this.OnPressMenu();
		}
		JokerPing.SendPingPong();
		JokerPing.SendIPPing(new Context.OnDeletegateObject(this.PingUpdate));
	}

	private void OnApplicationQuit()
	{
		SmartFoxConnection.Destroy();
		this.smartFox = null;
	}

	private void OnApplicationPause(bool state)
	{
		if (state)
		{
			this.OnPause();
		}
		else
		{
			this.OnResume();
		}
	}

	protected virtual void OnPause()
	{
	}

	protected virtual void OnResume()
	{
	}

	protected virtual void OnPressEscape()
	{
	}

	protected virtual void OnPressMenu()
	{
	}

	protected virtual void JokerUpdate()
	{
	}

	public virtual void PingUpdate(object pingTime)
	{
		int num = (int)pingTime;
		TimeSpan timeSpan = DateTime.Now - BaseScene.lastIPPing;
		if (num > Joker2XConfigUtils.PING_CONSTANT && timeSpan.TotalSeconds > (double)Joker2XConfigUtils.PING_DURATION)
		{
			Context.Tooltip.AddMessageYes(Language.NETWORK_UNSTABLE, 3, null, null, "OK");
			BaseScene.lastIPPing = DateTime.Now;
		}
	}

	public abstract void OnJoinRoomError(BaseEvent evt);

	public void OnConnection(BaseEvent evt)
	{
		UnityEngine.Debug.Log("OnConnection");
		bool flag = (bool)evt.Params["success"];
		string text = (string)evt.Params["error"];
		if (flag)
		{
			switch (Context.GameInfo.LastAccounType)
			{
			case 0:
				JokerLoginRequest.SendMessage(Context.GameInfo.UserName, 0, Context.GameInfo.Password);
				break;
			case 2:
				JokerLoginRequest.SendMessage(Context.fbKey, 2, "matkhau");
				break;
			}
		}
		else
		{
			this.OnConnectionError(evt);
		}
	}

	public void OnConnectionError(BaseEvent evt)
	{
		UnityEngine.Debug.Log("OnConnectionError");
		if (Context.SceneName == "Loading" && !this._isReConnecting)
		{
			this.ConnectBackupServer();
		}
		else
		{
			this.ReConnect();
		}
	}

	public void OnConnectionLost(BaseEvent evt)
	{
		UnityEngine.Debug.Log("OnConnectionLost");
		string text = (string)evt.Params["reason"];
		if (text != null)
		{
			if (text == ClientDisconnectionReason.BAN)
			{
				Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_GAME_BAN);
			}
			else if (text == ClientDisconnectionReason.KICK)
			{
				Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_GAME_KICK);
			}
			else if (text == ClientDisconnectionReason.IDLE)
			{
				Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_GAME_IDLE);
			}
			else if (text == ClientDisconnectionReason.UNKNOWN)
			{
				this.ReConnect();
			}
			else
			{
				this.ReConnect();
			}
		}
		else
		{
			this.ReConnect();
		}
	}

	public void OnConnectRetry(BaseEvent evt)
	{
		UnityEngine.Debug.LogError("OnConnectRetry");
		Context.Waiting.ShowWaiting(Joker2XConfigUtils.RECONNECT_TIMEOUT, delegate(Hashtable input)
		{
			this.ReconnectTimeOut();
		});
	}

	public void OnConnectResume(BaseEvent evt)
	{
		UnityEngine.Debug.Log("OnConnectResume");
		Context.Waiting.HideWaiting();
	}

	private void OnLogin(BaseEvent evt)
	{
		UnityEngine.Debug.Log("OnLogin");
		if (evt.Params.Contains("success") && !(bool)evt.Params["success"])
		{
			this.loginErrorMessage = (string)evt.Params["errorMessage"];
			UnityEngine.Debug.Log(this.loginErrorMessage);
			Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_LOGIN_NOT_SUCCESS);
		}
		else
		{
			this.loginErrorMessage = string.Empty;
			this._isReConnecting = false;
			ISFSObject isfsobject = (ISFSObject)evt.Params["data"];
			UnityEngine.Debug.Log(isfsobject.GetDump());
			User user = (User)evt.Params["user"];
			DateUtils.SetDeltaTime(isfsobject.GetLong("serverTimeCurrent"));
			Context.GameInfo.UserName = user.Name;
			SmartFoxConnection.EnablePingPong();
			this.OnLoginSuccess(user, isfsobject);
		}
	}

	protected virtual void OnLoginSuccess(User user, ISFSObject data)
	{
	}

	public void OnLoginError(BaseEvent evt)
	{
		UnityEngine.Debug.Log("OnLoginError");
		bool flag = evt.Params.Contains("errorCode") && ((short)evt.Params["errorCode"] == 4 || (short)evt.Params["errorCode"] == 11);
		if (flag)
		{
			Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_LOGIN_ERROR_BAN);
		}
		else
		{
			Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_LOGIN_ERROR);
		}
	}

	public void OnLogout(BaseEvent evt)
	{
		this.smartFox.Disconnect();
		UnityEngine.Debug.Log("OnLogout");
		Context.ShowLoginScreen(Context.clientGameState);
	}

	public void OnDebugMessage(BaseEvent evt)
	{
		string str = (string)evt.Params["message"];
		UnityEngine.Debug.Log("OnDebugMessage" + str);
	}

	public virtual void OnUserVariablesUpdate(BaseEvent evt)
	{
	}

	public virtual void OnRoomVariablesUpdate(BaseEvent evt)
	{
	}

	protected virtual void OnPublicMessage(BaseEvent evt)
	{
		string str = (string)evt.Params["message"];
		UnityEngine.Debug.Log("<color=blue>OnPublicMessage</color>" + str);
	}

	public void OnUserFindResponse(BaseEvent evt)
	{
		InviteListResponse.RunMessage(evt, this);
	}

	public void OnAdminMessageResponse(BaseEvent evt)
	{
		string str = (string)evt.Params["message"];
		Context.Confirm.AddMessageYes(str, null, null, string.Empty);
	}

	protected void OnJoinRoom(BaseEvent evt)
	{
		Context.currentRoomId = SmartFoxConnection.Connection.LastJoinedRoom.Id;
		UnityEngine.Debug.LogError("LOAD SCENE" + Context.currentGameId);
		Application.LoadLevelAsync(Context.currentGameId);
	}

	public virtual void OnExtensionResponse(BaseEvent evt)
	{
		SFSObject sfsobject = (SFSObject)evt.Params["params"];
		string text = (string)evt.Params["cmd"];
		string text2 = text;
		switch (text2)
		{
		case "s-exception":
		{
			string text3 = (string)evt.Params["errorMessage"];
			ExceptionResponse.RunMessage(evt, this);
			break;
		}
		case "b-topup":
			MonoBehaviour.print("------------topup: " + sfsobject.GetDump());
			TopupResponse.RunMessage(evt, this);
			break;
		case "b-pingv2":
			PingResponse.RunMessage(evt, this);
			break;
		case "b-um":
			UnityEngine.Debug.Log("USER_MERGE");
			this.Logout(JokerEnum.ClientGameState.GS_LOGOUT_MERGE_FB);
			break;
		case "s-sysmessage":
			UnityEngine.Debug.Log("SYSTEM_MESSAGE");
			MessageResponse.RunMessage(evt, this);
			break;
		case "ranking":
			UnityEngine.Debug.Log("user list");
			RankingResponse.RunMessage(evt, this);
			break;
		case "bp-playnow":
			UnityEngine.Debug.Log("PLAY_NOW");
			PlayNowResponse.RunMessage(evt, this);
			break;
		}
	}

	private void ReConnect()
	{
		UnityEngine.Debug.Log("ReConnect");
		this.ConnectServer();
		if (!this._isReConnecting)
		{
			Context.Waiting.ShowWaiting(Joker2XConfigUtils.RECONNECT_TIMEOUT, delegate(Hashtable input)
			{
				UnityEngine.Debug.Log("Show loading time out");
				this.ReconnectTimeOut();
			});
		}
		this._isReConnecting = true;
	}

	public void ConnectBackupServer()
	{
		UnityEngine.Debug.Log("ConnectBackupServer server ");
		Config.backupServerIndex++;
		if (Config.backupServerIndex < Config.arrServerNames.Length)
		{
			Config.serverName = Config.arrServerNames[Config.backupServerIndex];
			this.ConnectServer();
			Context.Waiting.ShowWaiting(Joker2XConfigUtils.RECONNECT_TIMEOUT, delegate(Hashtable input)
			{
				UnityEngine.Debug.Log("Show loading time out");
				this.ReconnectTimeOut();
			});
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"backup server ",
				Config.serverName,
				" ",
				Config.backupServerIndex
			}));
		}
		else if (BombConfigUtils.OFFLINE_ONLY)
		{
			Context.clientGameState = JokerEnum.ClientGameState.GS_RECONNECT_TIMEOUT;
			Context.currentGameId = Config.defaultGame;
			Context.LoadLevelAsyn("BomberMap", false);
		}
		else
		{
			Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_RECONNECT_TIMEOUT);
		}
	}

	public void OnLoginFB()
	{
		UnityEngine.Debug.Log("OnLoginFB");
		//Context.GameInfo.LastAccounType = 2;
		//Context.fbKey = AccessToken.CurrentAccessToken.TokenString;
		//if (SmartFoxConnection.IsConnected)
		//{
		//	this.smartFox = SmartFoxConnection.Connection;
		//	this.AddEventListener();
		//	JokerLoginRequest.SendMessage(Context.fbKey, 2, "matkhau");
		//}
		//else
		//{
		//	UnityEngine.Debug.Log("OnLoginFB !IsConnected");
		//	this.ConnectServer();
		//	Context.Waiting.ShowWaiting(Joker2XConfigUtils.RECONNECT_TIMEOUT, delegate(Hashtable input)
		//	{
		//		UnityEngine.Debug.Log("Show loading time out");
		//		this.ReconnectTimeOut();
		//	});
		//}
	}

	public void OnLoginGuest()
	{
		UnityEngine.Debug.Log("OnLoginGuest");
		Context.LogoutFB();
		Context.ResetLoginStore();
		if (SmartFoxConnection.IsConnected)
		{
			this.smartFox = SmartFoxConnection.Connection;
			this.AddEventListener();
			JokerLoginRequest.SendMessage(Context.GameInfo.UserName, 0, Context.GameInfo.Password);
		}
		else
		{
			UnityEngine.Debug.Log("ConnectServer OnLoginGuest");
			this.ConnectServer();
			Context.Waiting.ShowWaiting(Joker2XConfigUtils.RECONNECT_TIMEOUT, delegate(Hashtable input)
			{
				UnityEngine.Debug.Log("Show loading time out");
				this.ReconnectTimeOut();
			});
		}
	}

	public void ReconnectTimeOut()
	{
		this.ConnectBackupServer();
	}

	public void RequestTimeOut()
	{
		UnityEngine.Debug.Log("RequestTimeOut");
		Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_REQUEST_TIMEOUT);
	}

	//public void LoginFBCallBack(IResult fbResult)
	//{
	//	string str = string.Empty;
	//	if (fbResult.Error != null)
	//	{
	//		str = "Error Response:\n" + fbResult.Error;
	//		Context.Waiting.HideWaiting();
	//		Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_LOGINFB_ERROR);
	//	}
	//	else if (!FB.IsLoggedIn)
	//	{
	//		str = "Login cancelled by Player";
	//		Context.Waiting.HideWaiting();
	//	}
	//	else
	//	{
	//		str = "Login success";
	//		this.OnLoginFB();
	//	}
	//	UnityEngine.Debug.Log("LoginFBCallBack: " + str);
	//}

	//public void OnMergeFB()
	//{
	//	Context.fbKey = AccessToken.CurrentAccessToken.TokenString;
	//	MergeFBRequest.SendMessage();
	//}

	//public void MergeFBCallBack(IResult fbResult)
	//{
	//	string str = string.Empty;
	//	if (fbResult.Error != null)
	//	{
	//		str = "Error Response:\n" + fbResult.Error;
	//		Context.ShowLoginScreen(JokerEnum.ClientGameState.GS_LOGINFB_ERROR);
	//	}
	//	else if (!FB.IsLoggedIn)
	//	{
	//		str = "Login cancelled by Player";
	//		Context.Waiting.HideWaiting();
	//	}
	//	else
	//	{
	//		str = "Login success";
	//		this.OnMergeFB();
	//	}
	//	UnityEngine.Debug.Log("MergeFBCallBack: " + str);
	//}

	public void Logout(JokerEnum.ClientGameState logOutType)
	{
		UnityEngine.Debug.Log("Logout");
		Context.clientGameState = logOutType;
		if (SmartFoxConnection.Connection != null)
		{
			JokerLogoutRequest.SendMessage();
		}
		else
		{
			UnityEngine.Debug.LogError("Logout NULL");
		}
	}

	protected void ExitGame()
	{
		Context.OnDeletegateObject onYesClick = delegate(object obj)
		{
			UnityEngine.Debug.Log("Exit Game");
			SmartFoxConnection.Destroy();
			Application.Quit();
		};
		Context.Confirm.AddMessageYes(Language.EXIT_CONFIRM, onYesClick, null, string.Empty);
	}

	public abstract void InitScene();

	private void OnInvitationReplyError(BaseEvent evt)
	{
		Context.Tooltip.AddMessage("INVITATION REPLY ERROR ", 5, string.Empty, string.Empty);
	}

	public virtual void OnInvitation(BaseEvent evt)
	{
	}

	public virtual void AddDebug(string debug)
	{
	}

	protected SmartFox smartFox;

	protected bool debug;

	protected string loginErrorMessage = string.Empty;

	private bool _isReConnecting;

	private Ping ping;

	//public FacebookAPI.OnLoginFBSuccess onLoginFBSuccess = delegate()
	//{
	//	Context.currentMono.OnLoginFB();
	//};

	private static DateTime lastIPPing = DateTime.Now;
}
