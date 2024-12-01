// @sonhg: class: BattleTeam.BattleTeamScene
using System;
using System.Collections;
using System.Collections.Generic;
using Bomb;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace BattleTeam
{
	[RequireComponent(typeof(ParticlesController))]
	[RequireComponent(typeof(MapController))]
	[RequireComponent(typeof(GameController))]
	public class BattleTeamScene : BombGameScene
	{
    static Dictionary<string, int> _003C_003Ef__switch_0024map5;
		public override void SceneAwake()
		{
			this.goldText.text = JokerUserUtils.GetFormatChip(SmartFoxConnection.Connection.MySelf);
			this.diamondText.text = JokerUserUtils.GetFormatGold(SmartFoxConnection.Connection.MySelf);
			Context.googleAnalytics.LogScreen(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}

		public override void InitScene()
		{
			base.PlayMusic();
			List<User> userList = RoomUtils.GetUserList();
			foreach (User user in userList)
			{
				if (user == this.smartFox.MySelf)
				{
					if (RoomUtils.GetRoomOwnerId() == user.Id)
					{
						base.GameController.readyButton.SetActive(false);
						base.GameController.startButton.SetActive(true);
					}
					else if (RoomUtils.GetRoomOwnerId() != user.Id)
					{
						base.GameController.readyButton.SetActive(true);
						base.GameController.startButton.SetActive(false);
					}
				}
				base.GameController.waitingPanel.CharacterAvatar[MMOUserUtils.GetUserPosition(user)].SetUser(user);
				base.GameController.waitingPanel.CharacterAvatar[MMOUserUtils.GetUserPosition(user)].SetTeam(user);
			}
			this.ChangeMapThumb();
		}

		protected override void OnEndGame(BaseEvent evt)
		{
			EndGameResponse.RunMessage(evt, this);
			this.joystick.SetActive(false);
		}

		protected override void OnPublicMessage(BaseEvent evt)
		{
			base.OnPublicMessage(evt);
			User user = (User)evt.Params["sender"];
			string content = (string)evt.Params["message"];
			base.GameController.waitingPanel.CharacterAvatar[MMOUserUtils.GetUserPosition(user)].ShowChatMessage(content);
		}

		protected override void OnSFSUserLeaveRoom(BaseEvent evt)
		{
			User user = (User)evt.Params["user"];
			if (user != null && MMOUserUtils.GetUserPosition(user) >= 0 && MMOUserUtils.GetUserPosition(user) < 4)
			{
				AvatarController avatarController = base.GameController.waitingPanel.CharacterAvatar[MMOUserUtils.GetUserPosition(user)];
				avatarController.ResetInfo();
				avatarController.SetKickButton(false);
				base.GameController.ResetPlayer(user.Id);
				base.GameController.RemovePlayer(user.Id);
			}
		}

		protected override void OnStartGame(BaseEvent evt)
		{
			OnlineAdmob.HideBannerAds();
			this.InviteUI.SetActive(false);
			List<User> userList = RoomUtils.GetUserList();
			base.GameController.waitingPanel.HideAllLevelUp();
			foreach (User user in userList)
			{
				if (user.IsItMe)
				{
					base.GameController.CreatePlayer(user.Id, user.Name, true);
				}
				else
				{
					base.GameController.CreatePlayer(user.Id, user.Name, false);
				}
				float x = (float)BombUserUtils.GetX(user);
				float y = (float)BombUserUtils.GetY(user);
				int userPosition = MMOUserUtils.GetUserPosition(user);
				MoveDirection direction = (MoveDirection)BombUserUtils.GetDirection(user);
				base.GameController.SetPlayerPosition(user.Id, x, y, direction, this.clientServerLag + 10);
				base.GameController.timmer.StopRaising("OnStartGame");
				base.GameController.waitingPanel.CharacterAvatar[userPosition].ResetSubAvatar();
			}
			SFSObject sfsobject = (SFSObject)evt.Params["params"];
			int @int = sfsobject.GetInt("x");
			int int2 = sfsobject.GetInt("y");
			base.GameController.SetMyPlayerPosition((float)@int, (float)int2);
			ISFSArray sfsarray = sfsobject.GetSFSArray("data");
			List<int> list = new List<int>();
			foreach (object obj in sfsarray)
			{
				int item = (int)obj;
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			this.GenerateQuickBuy(list);
			float maxTime = (float)sfsobject.GetInt("time") - (float)this.clientServerLag / 1000f;
			base.GameController.clock.StartRaise(0f, maxTime, null, null);
			base.GameController.waitingPanel.gameObject.SetActive(false);
			this.joystick.SetActive(true);
			this.PlayMusicRandomize();
		}

		public override void OnUserVariablesUpdate(BaseEvent evt)
		{
			base.OnUserVariablesUpdate(evt);
			User user = evt.Params["user"] as User;
			ArrayList arrayList = (ArrayList)evt.Params["changedVars"];
			if (arrayList.Contains("team"))
			{
				base.GameController.waitingPanel.CharacterAvatar[MMOUserUtils.GetUserPosition(user)].SetTeam(user);
			}
		}

		public override void OnExtensionResponse(BaseEvent evt)
		{
			SFSObject sfsobject = (SFSObject)evt.Params["params"];
			string text = (string)evt.Params["cmd"];
			string text2 = text;
			if (text2 != null)
			{
				if (BattleTeamScene._003C_003Ef__switch_0024map5 == null)
				{
					BattleTeamScene._003C_003Ef__switch_0024map5 = new Dictionary<string, int>(1)
					{
						{
							"s-map",
							0
						}
					};
				}
				int num;
				if (BattleTeamScene._003C_003Ef__switch_0024map5.TryGetValue(text2, out num))
				{
					if (num == 0)
					{
						InitGameMapResponse.RunMessage(evt, this);
						return;
					}
				}
			}
			base.OnExtensionResponse(evt);
		}

		public void OnClickStartGame()
		{
			StartGameRequest.SendMessage();
			Context.Waiting.ShowWaiting();
		}

		public void OnClickReady()
		{
			BombUserReadyRequest.SendMessage();
			this.readyButton.interactable = false;
		}

		public void OnClickExit()
		{
			OnlineAdmob.HideBannerAds();
			this.ConfirmLeaveRoom();
		}

		protected override void ConfirmLeaveRoom()
		{
			ConfirmBox confirm = Context.Confirm;
			confirm.GetComponent<ConfirmBox>().AddMessageYesNo(Language.EXIT_CONFIRM, new Context.OnDeletegateObject(this.Exit), null, null, string.Empty, string.Empty, string.Empty, false);
		}

		private void Exit(object obj)
		{
			int gold = JokerUserUtils.GetGold(SmartFoxConnection.Connection.MySelf);
			if (gold < Joker2XConfigUtils.ONLINE_ADMOB_GOLD)
			{
				if (Joker2XConfigUtils.ONLINE_ADMOB_TIME > 0)
				{
					base.AdsController.ShowInterAds(true);
				}
				else
				{
					base.AdsController.ShowInterAds(false);
				}
			}
			base.ConfirmLeaveRoom();
		}

		public void OnClickSettingButton()
		{
			MainMenuSettingBox setting = Context.Setting;
		}

		private void BuyItemInGame(int idItem)
		{
			BuyItemInGameRequest.SendMessage(idItem);
		}

		private void PlayMusicRandomize()
		{
			MusicManager.instance.RandomizeMusic(new AudioClip[]
			{
				this.ingameMusic1,
				this.ingameMusic3
			});
		}

		private void GenerateQuickBuy(List<int> itemList)
		{
			foreach (int informationById in itemList)
			{
				QuickBuyButton quickBuyButton = UnityEngine.Object.Instantiate<QuickBuyButton>(this.QuickBuyPrefab);
				quickBuyButton.SetInformationById(informationById);
				quickBuyButton.transform.SetParent(this.QuickBuyContainer.transform, false);
			}
		}

		protected override void onSpectatorToPlayer(BaseEvent evt)
		{
		}

		protected override void OnUserEnterRoom(BaseEvent evt)
		{
		}

		protected override void onPlayerToSpectator(BaseEvent evt)
		{
		}

		protected override void OnUserAction(BaseEvent evt)
		{
		}

		public override void OnJoinRoomError(BaseEvent evt)
		{
			Context.Messenger.AddOkMessage(Language.KICKED_TIMEOUT, delegate(object o)
			{
				Context.ShowMainmenuScreen();
			}, null, string.Empty, string.Empty);
		}

		[Header("Prefab")]
		[SerializeField]
		private QuickBuyButton QuickBuyPrefab;

		public GameObject joystick;
	}
}
