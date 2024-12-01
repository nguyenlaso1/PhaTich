// @sonhg: class: Bomb.BombGameScene
using System;
using System.Collections;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Bomb
{
	public abstract class BombGameScene : GameScene
	{
		public GameController GameController
		{
			get
			{
				if (this._gameController == null)
				{
					this._gameController = base.GetComponent<GameController>();
				}
				return this._gameController;
			}
		}

		public MapController MapController
		{
			get
			{
				if (this._mapController == null)
				{
					this._mapController = base.GetComponent<MapController>();
				}
				return this._mapController;
			}
		}

		public ParticlesController ParticlesController
		{
			get
			{
				if (this._particlesController == null)
				{
					this._particlesController = base.GetComponent<ParticlesController>();
				}
				return this._particlesController;
			}
		}

		public AdsController AdsController
		{
			get
			{
				if (this._adsController == null)
				{
					this._adsController = base.GetComponent<AdsController>();
				}
				return this._adsController;
			}
		}

		protected override void ShowAdmob()
		{
			int gold = JokerUserUtils.GetGold(SmartFoxConnection.Connection.MySelf);
			if (gold <= Joker2XConfigUtils.ONLINE_ADMOB_GOLD)
			{
				this.AdsController.ShowBannerAds(true);
			}
		}

		public override void AddEventListener()
		{
			base.AddEventListener();
			this.smartFox.AddEventListener(SFSEvent.PING_PONG, new EventListenerDelegate(this.OnPingPong));
		}

		public override void OnUserVariablesUpdate(BaseEvent evt)
		{
			base.OnUserVariablesUpdate(evt);
			User user = evt.Params["user"] as User;
			ArrayList arrayList = (ArrayList)evt.Params["changedVars"];
			if (arrayList.Contains("x") || arrayList.Contains("y"))
			{
				this.SetPlayerPosition(user, arrayList);
			}
			if (arrayList.Contains("mmo-ready"))
			{
				this.SetPlayerReady(user, arrayList);
			}
			if (arrayList.Contains("speed"))
			{
				this.SetPlayerSpeed(user, arrayList);
			}
			if (arrayList.Contains("position"))
			{
				this.SetPlayerRoomPosition(user, arrayList);
			}
			if (arrayList.Contains("chip") && user != null && user.IsItMe)
			{
				this.goldText.text = JokerUserUtils.GetFormatChip(SmartFoxConnection.Connection.MySelf);
			}
			if (arrayList.Contains("gold") && user != null && user.IsItMe)
			{
				this.diamondText.text = JokerUserUtils.GetFormatGold(SmartFoxConnection.Connection.MySelf);
			}
		}

		public override void OnRoomVariablesUpdate(BaseEvent evt)
		{
			base.OnUserVariablesUpdate(evt);
			ArrayList arrayList = (ArrayList)evt.Params["changedVars"];
			if (arrayList.Contains("r-ownerid"))
			{
				this.ChangeRoomOwner();
			}
			if (arrayList.Contains("r-map-id"))
			{
				this.ChangeMapThumb();
			}
			if (arrayList.Contains("r-ewaitingTime"))
			{
				this.StartTimmer();
			}
		}

		public override void OnExtensionResponse(BaseEvent evt)
		{
			SFSObject sfsobject = (SFSObject)evt.Params["params"];
			string text = (string)evt.Params["cmd"];
			string text2 = text;
			switch (text2)
			{
			case "pickup":
				PickItemResponse.RunMessage(evt, this);
				return;
			case "buy-item-ingame":
				BuyItemInGameResponse.RunMessage(evt, this);
				return;
			case "bomb":
				BombResponse.RunMessage(evt, this);
				return;
			case "s-bomb-Explosion":
				BombExplosionResponse.RunMessage(evt, this);
				return;
			case "die":
				DieResponse.RunMessage(evt, this);
				return;
			case "s-levelup":
				LevelUpResponse.RunMessage(evt, this);
				return;
			case "kick-bomb":
				KickBombResponse.RunMessage(evt, this);
				return;
			case "hit-bomb":
				HitBombResponse.RunMessage(evt, this);
				return;
			}
			base.OnExtensionResponse(evt);
		}

		protected void OnPingPong(BaseEvent evt)
		{
			this.clientServerLag = (int)evt.Params["lagValue"] / 2;
		}

		protected virtual void SetPlayerPosition(User user, ArrayList changedVars)
		{
			float x = (float)user.GetVariable("x").GetDoubleValue();
			float y = (float)user.GetVariable("y").GetDoubleValue();
			MoveDirection intValue = (MoveDirection)user.GetVariable("direction").GetIntValue();
			if (user != this.smartFox.MySelf)
			{
				this.GameController.SetPlayerPosition(user.Id, x, y, intValue, this.clientServerLag);
			}
		}

		protected virtual void SetPlayerReady(User user, ArrayList changedVars)
		{
			this.GameController.waitingPanel.CharacterAvatar[MMOUserUtils.GetUserPosition(user)].SetReady(MMOUserUtils.IsReady(user));
		}

		protected virtual void SetPlayerSpeed(User user, ArrayList changedVars)
		{
			float speed = (float)user.GetVariable("speed").GetDoubleValue();
			this.GameController.SetPlayerSpeed(user.Id, speed);
		}

		protected virtual void SetPlayerRoomPosition(User user, ArrayList changedVars)
		{
			for (int i = 0; i < 4; i++)
			{
				this.GameController.waitingPanel.CharacterAvatar[i].ResetInfo();
			}
			foreach (User user2 in RoomUtils.GetUserList())
			{
				this.GameController.waitingPanel.CharacterAvatar[MMOUserUtils.GetUserPosition(user2)].SetUser(user2);
			}
		}

		protected virtual void ChangeMapThumb()
		{
			string text = ResourcesUltis.MapIdToLink(MMORoomUtils.GetMapID().ToString()).Replace(ResourceChecking.BaseIp(), string.Empty);
			this.GameController.waitingPanel.thumbImg.sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
			if (this.GameController.waitingPanel.thumbImg.sprite == null)
			{
				this.GameController.waitingPanel.thumbImg.sprite = ResourcesManager.SpriteList[text];
			}
			if (this.GameController.waitingPanel.thumbImg.sprite == null)
			{
				this.GameController.waitingPanel.iconLoadingMapThumb.SetActive(true);
			}
			else
			{
				this.GameController.waitingPanel.iconLoadingMapThumb.SetActive(false);
			}
		}

		protected virtual void ChangeRoomOwner()
		{
			if (RoomUtils.GetRoomOwnerId() == this.smartFox.MySelf.Id)
			{
				this.GameController.startButton.SetActive(true);
				this.GameController.readyButton.SetActive(false);
			}
			else
			{
				this.GameController.startButton.SetActive(false);
				this.GameController.readyButton.SetActive(true);
			}
			foreach (AvatarController avatarController in this.GameController.waitingPanel.CharacterAvatar)
			{
				if (avatarController.GetUserId() == RoomUtils.GetRoomOwnerId())
				{
					avatarController.readyImage[2].gameObject.SetActive(true);
					avatarController.readyImage[1].gameObject.SetActive(false);
					avatarController.readyImage[0].gameObject.SetActive(false);
					avatarController.SetKickButton(false);
				}
				else if (avatarController.GetUserId() != -1 && RoomUtils.GetRoomOwnerId() == this.smartFox.MySelf.Id)
				{
					avatarController.SetKickButton(true);
				}
				else
				{
					avatarController.SetKickButton(false);
				}
			}
		}

		protected virtual void StartTimmer()
		{
			int num = 5;
			int num2 = num - MMORoomUtils.GetRemainWaitingTime();
			this.GameController.timmer.StartRaise((float)num2, (float)num, delegate(object param)
			{
				this.GameController.waitingPanel.gameObject.SetActive(false);
			}, null);
		}

		public void OnCloseButtonClick()
		{
			this.ClearAtEndGame();
		}

		public void DisableQuickBuy()
		{
			this.QuickBuyContainer.interactable = false;
		}

		private void ClearQuickBuy()
		{
			this.QuickBuyContainer.transform.DestroyChildren();
			this.QuickBuyContainer.interactable = true;
		}

		private void ClearAtEndGame()
		{
			this.GameController.waitingPanel.gameObject.SetActive(true);
			this.GameController.endGamePanel.gameObject.SetActive(false);
			this.GameController.ResetPlayer();
			this.GameController.ResetReady();
			this.ClearQuickBuy();
			this.PlayMusic();
			this.readyButton.interactable = true;
			this.MapController.ResetMap();
			base.StopAllCoroutines();
			this.GameController.clock.StopRaising("BombScene");
		}

		protected void PlayMusic()
		{
			MusicManager.instance.PlayMusic(this.gameMusic);
		}

		protected int clientServerLag;

		public Text goldText;

		public Text diamondText;

		[Header("Container")]
		[SerializeField]
		protected CanvasGroup QuickBuyContainer;

		[Header("Button")]
		public Button readyButton;

		[Header("Music and Sound")]
		public AudioClip gameMusic;

		public AudioClip ingameMusic1;

		public AudioClip ingameMusic3;

		public AudioClip winSound;

		public AudioClip loseSound;

		private GameController _gameController;

		private MapController _mapController;

		private ParticlesController _particlesController;

		private AdsController _adsController;
	}
}
