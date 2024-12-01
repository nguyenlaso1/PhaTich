// @sonhg: class: GameScene
using System;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine;

public abstract class GameScene : BaseScene
{
	public override void AddEventListener()
	{
		base.AddEventListener();
		this.smartFox.AddEventListener(SFSEvent.USER_ENTER_ROOM, new EventListenerDelegate(this.OnUserEnterRoom));
		this.smartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, new EventListenerDelegate(this.OnSFSUserLeaveRoom));
		this.smartFox.AddEventListener(SFSEvent.SPECTATOR_TO_PLAYER, new EventListenerDelegate(this.onSpectatorToPlayer));
		this.smartFox.AddEventListener(SFSEvent.PLAYER_TO_SPECTATOR, new EventListenerDelegate(this.onPlayerToSpectator));
	}

	private void Awake()
	{
		if (!SmartFoxConnection.IsConnected)
		{
			Application.LoadLevelAsync("Loading");
			return;
		}
		Context.currentMono = this;
		this.smartFox = SmartFoxConnection.Connection;
		this.AddEventListener();
		this.SceneAwake();
		this.ShowAdmob();
	}

	private void Start()
	{
		this.InitScene();
	}

	public virtual void SceneAwake()
	{
	}

	protected abstract void OnUserEnterRoom(BaseEvent evt);

	protected abstract void onSpectatorToPlayer(BaseEvent evt);

	protected abstract void onPlayerToSpectator(BaseEvent evt);

	protected abstract void OnSFSUserLeaveRoom(BaseEvent evt);

	protected abstract void OnUserAction(BaseEvent evt);

	protected abstract void OnEndGame(BaseEvent evt);

	protected abstract void OnStartGame(BaseEvent evt);

	protected virtual void ShowAdmob()
	{
	}

	protected void OnJokerLeaveRoom(BaseEvent evt)
	{
		JokerLeaveRoomResponse.RunMessage(evt, this);
	}

	protected override void OnLoginSuccess(User user, ISFSObject data)
	{
		UnityEngine.Debug.Log("OnLoginSuccess  :" + Context.currentRoomId);
		Context.currentJoinType = 2;
		ChooseGameRequest.SendMessage(Context.currentGameId);
		JoinRoomRequest.SendMessage(Context.currentGroupIndex, Context.currentRoomId, Context.currentJoinType, -1, false);
	}

	public override void OnExtensionResponse(BaseEvent evt)
	{
		SFSObject sfsobject = (SFSObject)evt.Params["params"];
		string text = (string)evt.Params["cmd"];
		string text2 = text;
		switch (text2)
		{
		case "s-endGame":
			this.OnEndGame(evt);
			return;
		case "s-startGame":
			this.OnStartGame(evt);
			if (Joker2XConfigUtils.ONLINE_PLAY_COUNT_TYPE == -1)
			{
				AdsController.play_count++;
			}
			return;
		case "b-user-action":
			this.OnUserAction(evt);
			return;
		case "s-leave-room":
			this.OnJokerLeaveRoom(evt);
			return;
		}
		base.OnExtensionResponse(evt);
	}

	public virtual bool IsPlaying()
	{
		return SmartFoxConnection.Connection.MySelf.IsPlayer;
	}

	public virtual bool IsWaiting()
	{
		return RoomUtils.GetGameState() == JokerEnum.ClientGameState.GS_IN_GAME_WAITING;
	}

	protected virtual void ConfirmLeaveRoom()
	{
		if (SmartFoxConnection.Connection.LastJoinedRoom != null)
		{
			this.SendJokerLeaveRoomRequest();
		}
		else
		{
			Context.ShowMainmenuScreen();
		}
	}

	private void SendJokerLeaveRoomRequest()
	{
		SFSLeaveRoomRequest.SendMessage();
	}

	public void SendReadyRequestOnEndGame()
	{
		if (GameScene.confirmToLeaveOnEndGame)
		{
			if (SmartFoxConnection.Connection.LastJoinedRoom != null)
			{
				this.SendJokerLeaveRoomRequest();
			}
			else
			{
				UnityEngine.Debug.LogError("Ok roi");
			}
			GameScene.confirmToLeaveOnEndGame = false;
		}
	}

	public void ForceShowMainmenuAndTopup(string message)
	{
		Context.OnDeletegateObject onYesClick = delegate(object obj)
		{
			Context.clientGameState = JokerEnum.ClientGameState.GS_ON_MAINMENU_TOPUP;
			Context.ShowMainmenuScreen();
		};
		Context.OnDeletegateObject onNoClick = delegate(object obj)
		{
			UnityEngine.Debug.Log("ForceShowMainmenu");
			Context.ShowMainmenuScreen();
		};
		Context.Confirm.AddMessageYesNo(message, onYesClick, null, onNoClick, null, Language.NOT_ENOUGH_CHIP_YES, Language.NOT_ENOUGH_CHIP_NO, false);
	}

	public void ForceShowMainmenu(string message)
	{
		Context.OnDeletegateObject onYesClick = delegate(object obj)
		{
			UnityEngine.Debug.Log("ForceShowMainmenu");
			Context.ShowMainmenuScreen();
		};
		ConfirmBox confirm = Context.Confirm;
		confirm.AddMessageYes(message, onYesClick, null, "Ok").EnableBackground();
	}

	public void ForceLeaveRoom(string message)
	{
		Context.OnDeletegateObject onYesClick = delegate(object obj)
		{
			UnityEngine.Debug.Log("ForceLeaveRoom");
			this.SendJokerLeaveRoomRequest();
		};
		Context.Confirm.AddMessageYes(message, onYesClick, null, string.Empty);
	}

	public void ClosePopup()
	{
		BaseBox.CloseCurrentBox();
	}

	[Header("UI")]
	public GameObject InviteUI;

	public static bool confirmToLeaveOnEndGame;
}
