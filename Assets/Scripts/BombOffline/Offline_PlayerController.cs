// @sonhg: class: BombOffline.Offline_PlayerController
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace BombOffline
{
	[RequireComponent(typeof(Offline_PlayerBuffSystem))]
	public class Offline_PlayerController : Offline_BaseCharactersController
	{
		public int TotalHeart
		{
			get
			{
				return this.totalHeart;
			}
			set
			{
				this.totalHeart = value;
				this.RenderHeart();
			}
		}

		public int TotalBomb
		{
			get
			{
				return this.totalBomb;
			}
			set
			{
				this.totalBomb = value;
				if (this.totalBomb >= 8)
				{
					this.totalBomb = 8;
					this.totalBombNumber.text = "Max";
				}
				else
				{
					this.RenderTotalBomb();
				}
			}
		}

		public int CurrentBombLength
		{
			get
			{
				return this.bombLenghth;
			}
			set
			{
				this.bombLenghth = value;
				if (this.bombLenghth >= 8)
				{
					this.bombLenghth = 8;
					this.bombLengthNumber.text = "Max";
				}
				else
				{
					this.RenderBombLength();
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this.RenderHeart();
			this.RenderTotalBomb();
			this.RenderBombLength();
			this.RenderShoes();
			Offline_PlayerBuffSystem component = base.GetComponent<Offline_PlayerBuffSystem>();
			component.player = this;
			this._buffSystem = component;
			this.secondaryCommand = new Offline_JumpCommand();
		}

		private void Start()
		{
			this.mapController = this.bombScene.MapController;
			this.goldCollect.text = Joker2XUtils.FormatChip(DataManager.GetGold());
		}

		private void FixedUpdate()
		{
			this.RenderCharacter();
		}

		public void PlaceBomb()
		{
			if (this.canThrowBomb && this.canAct && base.CurrentDirection == MoveDirection.STAND && this.CheckStandOnBomb())
			{
				this.StartThrowCount();
			}
			else if (this.remoteBomb.Count > 0)
			{
				foreach (BombModel bombModel in this.remoteBomb)
				{
					this.bombScene.GameController.TriggerBomb(bombModel.position);
				}
				this.remoteBomb.Clear();
				this.canTrigger = false;
				base.StartCoroutine(this.DelayPlaceBomb());
			}
			else if (this.bombPrefab != null && this.canTrigger && !this.isDead && this.canAct && this.MyBombCount < this.TotalBomb)
			{
				this.ResetThrowCount();
				BombModel bombModel2 = this.mapController.PlaceBomb(this.bombPrefab, this.CurrentBombLength, base.transform.position, true, 0, new int?(PlayerPrefs.GetInt("PlayerBomb", 30)), true);
				if (bombModel2 != null)
				{
					this.MyBombCount++;
					MusicManager.instance.PlayOneShot(this.placeBombSound, 0.5f);
				}
			}
		}

		private IEnumerator DelayPlaceBomb()
		{
			yield return new WaitForSeconds(0.5f);
			this.canTrigger = true;
			yield break;
		}

		public override bool Move(MoveDirection direction, bool isCheck = false)
		{
			MoveDirection currentDirection = this._currentDirection;
			bool result = base.Move((!this.reverseDirection) ? direction : direction.Reverse(), isCheck);
			if (currentDirection == this._currentDirection && this.CheckBombCollision() && this.canKickBomb)
			{
				this.StartKickCount();
			}
			else
			{
				this.ResetKickCount();
			}
			if (currentDirection != this._currentDirection)
			{
				Vector3 position = this.ValidateEndPosition(base.transform.position);
				base.transform.position = position;
			}
			return result;
		}

		protected override void Notify(GameObject collision)
		{
			this.collisionObject = collision;
		}

		public virtual bool CheckBombCollision()
		{
			return this.collisionObject != null && this.collisionObject.CompareTag("Bomb");
		}

		public virtual bool CheckWallCollision()
		{
			return this.collisionObject != null && this.collisionObject.CompareTag("Untagged");
		}

		public virtual bool CheckStandOnBomb()
		{
			return this.bombScene.GameController.GetBombModelAt(base.currentX, base.currentY) != null;
		}

		public override void GetHit(BombModel bombModel)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.getHitTimer <= 2f)
			{
				return;
			}
			this.getHitTimer = 0f;
			if (!this.isDead)
			{
				if (this.hasShield)
				{
					this.hasShield = false;
				}
				else
				{
					this.bombScene.ParticlesController.PlayLoseHeart2Particle(base.transform);
					MusicManager.instance.PlayOneShot(this.getHitSound, 1f);
					this.TotalHeart--;
					if (this.TotalHeart <= 0)
					{
						this.ShowExtraLife();
					}
					else
					{
						base.animator.SetTrigger("GetHit");
					}
				}
			}
		}

		private void ShowExtraLife()
		{
			if (this.extraHearCount < this.priceList.Length)
			{
				int num = this.priceList[this.extraHearCount];
			}
			Time.timeScale = 0f;
			this.bombScene.GameController.EnableController(true);
			this.bombScene.GameController.EnableController(false);
			Time.timeScale = 1f;
			this.isDead = true;
			this._buffSystem.canEatItem = false;
			this.CanMoveThrough = true;
			base.animator.SetInteger("Status", 2);
			DataManager.AchievementCountPlus("PLAYER_DIE", 1);
			this.tombObject = (UnityEngine.Object.Instantiate(this.tombPrefab, base.transform.position, Quaternion.identity) as GameObject);
			base.StartCoroutine(this.ShowEndgame());
		}

		public override void RenderCharacter()
		{
			this.getHitTimer += Time.deltaTime;
			if (!this.isDead)
			{
				if (this._buffSystem.IsPoisioned)
				{
					base.animator.SetInteger("Status", 1);
				}
				else
				{
					base.animator.SetInteger("Status", 0);
				}
			}
		}

		public override void ResetCharacter()
		{
			base.ResetCharacter();
			this.isDead = false;
			this._buffSystem.canEatItem = true;
			this.CanMoveThrough = false;
			base.animator.SetInteger("Status", 0);
			if (this.tombObject != null)
			{
				UnityEngine.Object.Destroy(this.tombObject);
				this.tombObject = null;
			}
		}

		public override void PickUpItem(ItemType type)
		{
			UnityEngine.Debug.Log("<color=red>Player PickUpItem:</color>" + type.ToString());
			this._buffSystem.ApplyItem(type, 1);
		}

		public void JumpCommand()
		{
			if (this.canAct)
			{
				Vector3[] jumpPath = this.bombScene.GameController.GetJumpPath(this.faceDirection);
				if (jumpPath != null)
				{
					this.canAct = false;
					BoxCollider2D collider = base.GetComponent<BoxCollider2D>();
					collider.enabled = false;
					base.transform.DOPath(jumpPath, 0.450000018f, PathType.Linear, PathMode.TopDown2D, 10, null).OnComplete(delegate
					{
						this.canAct = true;
						collider.enabled = true;
					});
				}
			}
		}

		private bool CanJumpCommand()
		{
			if (this.canAct)
			{
				Vector3[] jumpPath = this.bombScene.GameController.GetJumpPath(this.faceDirection);
				if (jumpPath != null)
				{
					return true;
				}
			}
			return false;
		}

		public void DegreeBomb()
		{
			if (this.MyBombCount <= 0)
			{
				this.MyBombCount = 0;
			}
			else
			{
				this.MyBombCount--;
			}
		}

		public void IncreaseTotalBomb()
		{
			this.TotalBomb++;
		}

		public void IncreaseBaseMoveSpeed()
		{
			this.baseMoveSpeed += 0.2f;
			this.RenderShoes();
		}

		public void IncreaseBombLegth()
		{
			this.CurrentBombLength++;
		}

		public ItemType GetRandomItem()
		{
			return this.mapController.GetRandomItem();
		}

		public void ApplyItem(ItemType item)
		{
			this._buffSystem.ApplyItem(item, 1);
		}

		private void StartKickCount()
		{
			this.kickTimer += Time.deltaTime;
			if (this.kickTimer > 0.3f)
			{
				BombModel bomb = this.collisionObject.GetComponent<Offline_Bomb>().bomb;
				this.ResetKickCount();
				this.bombScene.GameController.PushBomb(bomb, base.CurrentDirection, false);
			}
		}

		private void ResetKickCount()
		{
			this.kickTimer = 0f;
			this.collisionObject = null;
		}

		private void ResetThrowCount()
		{
			this.throwTimer = 0f;
		}

		private void StartThrowCount()
		{
			this.throwTimer += Time.deltaTime;
			if (this.throwTimer > 0.5f)
			{
				BombModel bombModelAt = this.bombScene.GameController.GetBombModelAt(base.currentX, base.currentY);
				this.ResetThrowCount();
				this.bombScene.GameController.ThrowBomb(bombModelAt, this.faceDirection);
			}
		}

		private void RenderHeart()
		{
			this.heartNumber.text = this.TotalHeart.ToString();
		}

		private void RenderTotalBomb()
		{
			this.totalBombNumber.text = this.TotalBomb.ToString();
		}

		private void RenderBombLength()
		{
			this.bombLengthNumber.text = (this.CurrentBombLength - 1).ToString();
		}

		private void RenderShoes()
		{
			if (this.baseMoveSpeed >= 3.4f)
			{
				this.baseMoveSpeed = 3.4f;
				this.Shoes.text = "Max";
			}
			else
			{
				this.Shoes.text = Mathf.RoundToInt((this.baseMoveSpeed - this.baseSpeedShoes) / 0.2f).ToString();
			}
		}

		private Vector3 ValidateEndPosition(Vector3 position)
		{
			Offline_Tiled[,] mapTiled = this.bombScene.MapController.mapTiled;
			if (mapTiled == null)
			{
				return position;
			}
			int num = Mathf.RoundToInt(position.x);
			int num2 = Mathf.RoundToInt(position.y);
			bool flag = false;
			if (num < 0)
			{
				num = 0;
				flag = true;
			}
			if (num > mapTiled.GetLength(0) - 1)
			{
				num = mapTiled.GetLength(0) - 1;
				flag = true;
			}
			if (num2 < 0)
			{
				num2 = 0;
				flag = true;
			}
			if (num2 > mapTiled.GetLength(1) - 1)
			{
				num2 = mapTiled.GetLength(1) - 1;
				flag = true;
			}
			if (flag)
			{
				return new Vector2((float)num, (float)num2);
			}
			return position;
		}

		public void UpdateGold(int gold)
		{
			this.goldCollect.text = Joker2XUtils.FormatChip(DataManager.PlusGold(gold));
			this.goldText.gameObject.SetActive(true);
			this.goldText.GetComponent<Text>().text = "+" + gold;
			this.goldText.transform.position = new Vector3(base.transform.position.x + 1f, base.transform.position.y);
			this.goldText.transform.DOMove(new Vector3(this.goldText.transform.position.x, this.goldText.transform.position.y + 2f), 0.5f, false).OnComplete(delegate
			{
				this.goldText.gameObject.SetActive(false);
			});
		}

		public void SetMoveSpeed(float speed)
		{
			this.baseMoveSpeed = speed;
			this.RenderShoes();
		}

		public void UpdateGold()
		{
			this.goldCollect.text = Joker2XUtils.FormatChip(DataManager.GetGold());
		}

		public void Item1Trigger()
		{
			if (!string.IsNullOrEmpty(this.helper1ID))
			{
				this.ItemUsed(this.helper1ID);
				this.UpdateButtonItem();
			}
		}

		public void Item2Trigger()
		{
			if (!string.IsNullOrEmpty(this.helper2ID))
			{
				this.ItemUsed(this.helper2ID);
				this.UpdateButtonItem();
			}
		}

		private void ItemUsed(string id)
		{
			if (this.canTrigger)
			{
				this.canTrigger = false;
				base.StartCoroutine(this.DelayPlaceBomb());
				if (this.bombPrefab != null && !this.isDead && this.canAct && id.Equals("7") && this.mapController.CanPlaceBomb(base.transform.position) && DataManager.MinusMyItemHelper("7"))
				{
					BombModel bombModel = this.mapController.PlaceBomb(this.bombPrefab, this.CurrentBombLength, base.transform.position, true, 0, new int?(7), false);
					if (bombModel != null)
					{
						this.remoteBomb.Add(bombModel);
						DataManager.AchievementCountPlus("USE_REMOTE_BOMB", 1);
					}
				}
				else if (id.Equals("13") && DataManager.MinusMyItemHelper("13"))
				{
					this.PickUpItem(ItemType.SHIELD);
					DataManager.AchievementCountPlus("USE_SHIELD", 1);
				}
				else if (id.Equals("11") && DataManager.MinusMyItemHelper("11"))
				{
					this.mapController.ItemVisible();
					DataManager.AchievementCountPlus("USE_RADAR", 1);
				}
				else if (this.bombScene.GameController.CanDestroyBrick(this.faceDirection) && id.Equals("9") && DataManager.MinusMyItemHelper("9"))
				{
					this.bombScene.GameController.DestroyBrick(this.faceDirection);
					DataManager.AchievementCountPlus("USE_SHOVEL", 1);
				}
				else if (this.CanJumpCommand() && id.Equals("15") && DataManager.MinusMyItemHelper("15"))
				{
					this.JumpCommand();
					DataManager.AchievementCountPlus("USE_JUMP", 1);
				}
			}
		}

		public void InitItemHelper()
		{
			if (!string.IsNullOrEmpty(DataManager.GetItemHelper1()))
			{
				this.helper1ID = DataManager.GetItemHelper1();
				string path = ResourcesManager.ItemsDict[this.helper1ID].Path;
				this.itemButton[0].image.sprite = ResourcesManager.SpriteList[path];
			}
			else
			{
				this.itemButton[0].interactable = false;
			}
			if (!string.IsNullOrEmpty(DataManager.GetItemHelper2()))
			{
				this.helper2ID = DataManager.GetItemHelper2();
				string path2 = ResourcesManager.ItemsDict[this.helper2ID].Path;
				this.itemButton[1].image.sprite = ResourcesManager.SpriteList[path2];
			}
			else
			{
				this.itemButton[1].interactable = false;
			}
			this.UpdateButtonItem();
		}

		private void UpdateButtonItem()
		{
			if (this.itemButton[0].interactable)
			{
				int myItemHelper = DataManager.GetMyItemHelper(this.helper1ID);
				if (myItemHelper <= 0)
				{
					this.itemButton[0].interactable = false;
				}
				this.itemText[0].text = myItemHelper + string.Empty;
			}
			if (this.itemButton[1].interactable)
			{
				int myItemHelper2 = DataManager.GetMyItemHelper(this.helper2ID);
				if (myItemHelper2 <= 0)
				{
					this.itemButton[1].interactable = false;
				}
				this.itemText[1].text = myItemHelper2 + string.Empty;
			}
		}

		private IEnumerator ShowEndgame()
		{
			yield return new WaitForSeconds(0.75f);
			this.bombScene.GameController.EndGame(false);
			yield break;
		}

		[Header("Player")]
		public const float GET_HIT_COOLDOWN = 2f;

		public const int MAX_BOMB_LENGTH = 8;

		public const int MAX_BOMB_TOTAL = 8;

		public const float MAX_SPEED = 3.4f;

		public const float SHOE_SPEED = 0.2f;

		public GameObject bombPrefab;

		public GameObject tombPrefab;

		public Offline_BombScene bombScene;

		public bool reverseDirection;

		public bool hasShield;

		public Offline_BaseCommand secondaryCommand;

		private bool canKickBomb;

		private bool canThrowBomb;

		private bool isDead;

		[SerializeField]
		private AudioClip placeBombSound;

		[SerializeField]
		private AudioClip getHitSound;

		[SerializeField]
		private Text heartNumber;

		[SerializeField]
		private Text totalBombNumber;

		[SerializeField]
		private Text bombLengthNumber;

		[SerializeField]
		private Text Shoes;

		[SerializeField]
		private Text goldCollect;

		[SerializeField]
		private Text goldText;

		private GameObject collisionObject;

		private GameObject tombObject;

		private float kickTimer;

		private float throwTimer;

		[Header("Player Start")]
		protected int totalHeart = 3;

		protected int totalBomb = 1;

		protected int bombLenghth = 2;

		protected float getHitTimer;

		[HideInInspector]
		public Sprite bombSprite;

		private List<BombModel> remoteBomb = new List<BombModel>();

		private bool canTrigger = true;

		public Button[] itemButton;

		public Text[] itemText;

		private int MyBombCount;

		private Offline_PlayerBuffSystem _buffSystem;

		private float baseSpeedShoes = 2f;

		private int[] priceList = new int[]
		{
			500,
			1000
		};

		private int extraHearCount;

		private string helper1ID;

		private string helper2ID;
	}
}
