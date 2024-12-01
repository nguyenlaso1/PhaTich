// @sonhg: class: Bomb.GameController
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bomb
{
	public class GameController : MonoBehaviour
	{
		public BombGameScene Scene
		{
			get
			{
				if (this._bombScene == null)
				{
					this._bombScene = base.GetComponent<BombGameScene>();
				}
				return this._bombScene;
			}
		}

		public void SetPlayerPosition(int userId, float x, float y, MoveDirection direction, int elapsed)
		{
			if (this.players.ContainsKey(userId))
			{
				BaseCharactersController baseCharactersController = this.players[userId];
				baseCharactersController.px = x;
				baseCharactersController.py = y;
				baseCharactersController.updateNow = true;
				baseCharactersController.CurrentDirection = direction;
				baseCharactersController.lastRenderTime = (float)(this.getTimer() - elapsed);
				this.RenderPlayer(baseCharactersController);
			}
		}

		public void SetMyPlayerPosition(float x, float y)
		{
			this.myBomber.transform.position = new Vector3(x, y);
		}

		public void SetPlayerSpeed(int userId, float speed)
		{
			if (this.players.ContainsKey(userId))
			{
				BaseCharactersController baseCharactersController = this.players[userId];
				baseCharactersController.baseMoveSpeed = speed;
			}
		}

		private void RenderPlayer(BaseCharactersController bomber)
		{
			float lastRenderTime = (float)this.getTimer();
			bomber.RenderCharacter();
			bomber.lastRenderTime = lastRenderTime;
		}

		public void SetSkinForPlayer(int[] _id, Transform _char, Color _hairColor)
		{
			Texture2D[] textureArr = new Texture2D[]
			{
				TextureCutter.GetSkinTexture(_id[0]),
				TextureCutter.GetSkinTexture(_id[1]),
				TextureCutter.GetSkinTexture(_id[2])
			};
			TextureCutter.CutAll(textureArr, _char, _hairColor);
		}

		public void CreatePlayer(int userId, string userName, bool isMine)
		{
			if (this.players.ContainsKey(userId))
			{
				if (!(this.players[userId].GetComponent<BotStupidController>() != null))
				{
					return;
				}
				UnityEngine.Object.Destroy(this.players[userId].gameObject);
				this.players.Remove(userId);
			}
			BaseCharactersController baseCharactersController;
			if (isMine)
			{
				baseCharactersController = UnityEngine.Object.Instantiate<BaseCharactersController>(this.bomberPrefab);
				baseCharactersController.transform.Find("CharacterName").GetComponent<TextMesh>().text = string.Empty;
				this.inputHandler.actor = baseCharactersController;
				this.myBomber = baseCharactersController;
				this.myBomber.isMine = true;
			}
			else if (MMOUserUtils.IsBot(MMOUserUtils.GetUserByID(userId)))
			{
				baseCharactersController = UnityEngine.Object.Instantiate<BaseCharactersController>(this.botBomberPrefab);
				this.listBotObjects.Add(baseCharactersController.gameObject);
				((BotStupidController)baseCharactersController).userID = userId;
				((BotStupidController)baseCharactersController).isDead = false;
				if (MMOUserUtils.GetUserByID(userId) != null)
				{
					baseCharactersController.transform.Find("CharacterName").GetComponent<TextMesh>().text = JokerUserUtils.GetFormatDisplayName(MMOUserUtils.GetUserByID(userId), 0);
				}
			}
			else
			{
				baseCharactersController = UnityEngine.Object.Instantiate<BaseCharactersController>(this.enemyBomberPrefab);
				if (MMOUserUtils.GetUserByID(userId) != null)
				{
					baseCharactersController.transform.Find("CharacterName").GetComponent<TextMesh>().text = JokerUserUtils.GetFormatDisplayName(MMOUserUtils.GetUserByID(userId), 0);
				}
			}
			if (MMOUserUtils.GetUserByID(userId) != null)
			{
				int[] id = new int[]
				{
					MMOUserUtils.GetHead(MMOUserUtils.GetUserByID(userId)),
					MMOUserUtils.GetBody(MMOUserUtils.GetUserByID(userId)),
					MMOUserUtils.GetHair(MMOUserUtils.GetUserByID(userId))
				};
				this.SetSkinForPlayer(id, baseCharactersController.transform, TextureCutter.Parse(MMOUserUtils.GetHairColor(MMOUserUtils.GetUserByID(userId))));
			}
			else
			{
				UnityEngine.Debug.LogError("Can't get User by ID");
			}
			baseCharactersController.bombScene = this.Scene;
			baseCharactersController.mapController = this.Scene.MapController;
			this.players.Add(userId, baseCharactersController);
			baseCharactersController.transform.position = new Vector3(1f, 1f, 0f);
		}

		public void RemovePlayer(int userId)
		{
			if (!this.players.ContainsKey(userId))
			{
				return;
			}
			BaseCharactersController baseCharactersController = this.players[userId];
			this.players.Remove(userId);
			UnityEngine.Object.Destroy(baseCharactersController.gameObject);
			if (baseCharactersController == this.myBomber)
			{
				this.myBomber = null;
			}
		}

		public void RemoveAll()
		{
			if (this.players != null)
			{
				foreach (KeyValuePair<int, BaseCharactersController> keyValuePair in this.players)
				{
					BaseCharactersController baseCharactersController = this.players[keyValuePair.Key];
					UnityEngine.Object.Destroy(baseCharactersController.gameObject);
					if (baseCharactersController == this.myBomber)
					{
						this.myBomber = null;
					}
				}
			}
			this.players = new Dictionary<int, BaseCharactersController>();
		}

		public void PlaceBomb(int userId, float x, float y)
		{
			if (this.players.ContainsKey(userId))
			{
				BaseCharactersController baseCharactersController = this.players[userId];
				int bombLength = BombUserUtils.GetBombLength(userId);
				if (baseCharactersController is EnemyController)
				{
					int bomb = MMOUserUtils.GetBomb(MMOUserUtils.GetUserByID(userId));
					this.Scene.MapController.PlaceBomb((baseCharactersController as EnemyController).bombPrefab, bombLength, new Vector3(x, y, 0f), false, userId, bomb);
				}
			}
		}

		public void SetPlayerDead(int userId)
		{
			if (this.players.ContainsKey(userId))
			{
				BaseCharactersController baseCharactersController = this.players[userId];
				if (!baseCharactersController.isMine)
				{
					baseCharactersController.GetHit(null);
				}
			}
		}

		public void PickUpItem(int userId, ItemType type, int amount)
		{
			if (this.players.ContainsKey(userId))
			{
				BaseCharactersController baseCharactersController = this.players[userId];
				baseCharactersController.PickUpItem(type, amount);
			}
		}

		public void ResetPlayer()
		{
			if (this.players != null)
			{
				foreach (KeyValuePair<int, BaseCharactersController> keyValuePair in this.players)
				{
					keyValuePair.Value.ResetCharacter();
					if (keyValuePair.Value.GetComponent<BotStupidController>() != null)
					{
						UnityEngine.Debug.LogError("Reset bot");
						keyValuePair.Value.GetComponent<EnemyAStar>().isMoving = false;
					}
				}
			}
		}

		public void ResetPlayer(int userId)
		{
			if (this.players.ContainsKey(userId))
			{
				BaseCharactersController baseCharactersController = this.players[userId];
				baseCharactersController.ResetCharacter();
			}
		}

		public void ResetReady()
		{
			this.waitingPanel.ResetReady();
		}

		private void Start()
		{
			this.players = new Dictionary<int, BaseCharactersController>();
		}

		private void LateUpdate()
		{
			if (this.players != null)
			{
				foreach (KeyValuePair<int, BaseCharactersController> keyValuePair in this.players)
				{
					this.RenderPlayer(keyValuePair.Value);
				}
			}
		}

		public int getTimer()
		{
			return (int)Mathf.Round(Time.time * 1000f);
		}

		private BombGameScene _bombScene;

		[Header("Buttons")]
		public GameObject readyButton;

		public GameObject startButton;

		[Header("Prefabs")]
		public BaseCharactersController bomberPrefab;

		public BaseCharactersController enemyBomberPrefab;

		public BaseCharactersController botBomberPrefab;

		[Header("UI")]
		public WaittingRoom waitingPanel;

		public EndGamePanel endGamePanel;

		[Header("Others")]
		public InputHandler inputHandler;

		[HideInInspector]
		public BaseCharactersController myBomber;

		private Dictionary<int, BaseCharactersController> players;

		public BaseTimerController timmer;

		public BaseTimerController clock;

		public List<GameObject> listBotObjects = new List<GameObject>();
	}
}
