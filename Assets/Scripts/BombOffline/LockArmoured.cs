// @sonhg: class: BombOffline.LockArmoured
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class LockArmoured : Offline_BaseMonster, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			MusicManager.instance.PlaySingle(this.incomingSound, 1f);
			this.board.ShowBossName(this.monsterName);
			this.spawnCooldown = this.spawnTime;
			this.mineCooldown = this.mineTime;
			this.flareCooldown = this.flareTime;
			this.shakeCooldown = this.shakeTime;
			this.shootDirection = new List<Vector3>();
			this.shootDirection.Add(Vector3.up);
			this.shootDirection.Add(Vector3.down);
			this.shootDirection.Add(Vector3.left);
			this.shootDirection.Add(Vector3.right);
		}

		protected override void CreateBrain()
		{
			Wander routine = new Wander(this);
			Repeat repeat = new Repeat(new TimeLimit(routine, 3f), -1);
			Repeat repeat2 = new Repeat(new SpecialAction(this), -1);
			this.brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
		}

		public override bool GetHit(int x, int y)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return false;
			}
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this.hitParticle, base.transform.position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			UnityEngine.Object.Destroy(particleSystem.gameObject, 5f);
			return base.GetHit(x, y);
		}

		public bool CanFlare()
		{
			return this.canAct && this.flareCooldown <= 0f;
		}

		protected virtual void Flare()
		{
			if (this.CanFlare())
			{
				this.flareCooldown = this.flareTime;
				MusicManager.instance.PlaySingle(this.flareSound, 1f);
				BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.rocketPrefab);
				baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
				baseBullet.target = this.board.player.transform;
				base.FreezeAction(0.5f);
			}
		}

		protected virtual void SpawnMonster()
		{
			if (this.CanSpawn())
			{
				this.spawnCooldown = this.spawnTime;
				this.monsterList.RemoveAll((Offline_BaseMonster monster) => monster == null);
				for (int i = 0; i < this.maxMonster; i++)
				{
					Offline_BaseMonster item = this.board.PutMonsterAt(base.currentX, base.currentY, this.spawnMonster);
					this.monsterList.Add(item);
				}
			}
		}

		public bool CanSpawn()
		{
			return this.spawnCooldown <= 0f;
		}

		public bool CanBlow()
		{
			return this.canAct && this.mineCooldown <= 0f;
		}

		public bool CanShake()
		{
			return this.canAct && this.shakeCooldown <= 0f;
		}

		protected virtual void Blow()
		{
			if (this.CanBlow())
			{
				this.mineCooldown = this.mineTime;
				MusicManager.instance.PlaySingle(this.landMineSound, 1f);
				Camera.main.transform.DOShakePosition(0.7f, new Vector3(0.6f, 0.1f, 0.5f), 30, 90f, false);
				this.board.Scene.MapController.PlaceBomb(this.minePrefab.gameObject, 0, base.transform.position, false, 0, null, false);
			}
		}

		protected virtual void Shake()
		{
			if (this.CanShake())
			{
				this.shakeCooldown = this.shakeTime;
				base.StartCoroutine(this.BeginShake());
			}
		}

		private IEnumerator BeginShake()
		{
			this.canAct = false;
			MusicManager.instance.PlaySingle(this.shakeSound, 1f);
			base.animator.SetInteger("State", 4);
			this.chargeParticle.Play();
			yield return new WaitForSeconds(0.1f);
			base.animator.SetInteger("State", 1);
			yield return new WaitForSeconds(1f);
			Camera.main.transform.DOShakePosition(0.7f, new Vector3(0.6f, 0.1f, 0.5f), 30, 90f, false);
			this.board.player.PickUpItem(ItemType.SLOW2);
			foreach (Vector3 direction in this.shootDirection)
			{
				WaveBullet bulletObject = UnityEngine.Object.Instantiate<WaveBullet>(this.bulletPrefab);
				bulletObject.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
				bulletObject.shootDirection = direction;
				bulletObject.board = this.board;
			}
			this.canAct = true;
			yield break;
		}

		public virtual bool DoSpecialAction()
		{
			this.SpawnMonster();
			this.Blow();
			this.Flare();
			this.Shake();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
			if (this.mineCooldown > 0f)
			{
				this.mineCooldown -= Time.deltaTime;
			}
			if (this.flareCooldown > 0f)
			{
				this.flareCooldown -= Time.deltaTime;
			}
			if (this.shakeCooldown > 0f)
			{
				this.shakeCooldown -= Time.deltaTime;
			}
		}

		public override void DestroyMonster()
		{
			base.DestroyMonster();
			DataManager.AchievementCountPlus("KILL_LOCK_ARMOR", 1);
		}

		[Header("LockArmoured")]
		[SerializeField]
		protected LandMine minePrefab;

		[SerializeField]
		protected float mineTime = 7f;

		protected float mineCooldown;

		[SerializeField]
		protected float flareTime = 5f;

		protected float flareCooldown;

		[SerializeField]
		protected float shakeTime = 5f;

		protected float shakeCooldown;

		[SerializeField]
		protected int mineCount;

		[SerializeField]
		private BaseBullet rocketPrefab;

		[SerializeField]
		protected WaveBullet bulletPrefab;

		[SerializeField]
		[Header("MiniGun")]
		protected Offline_BaseMonster spawnMonster;

		[SerializeField]
		protected ParticleSystem chargeParticle;

		[SerializeField]
		protected float spawnTime = 15f;

		[SerializeField]
		protected int maxMonster = 2;

		[SerializeField]
		private ParticleSystem hitParticle;

		protected float spawnCooldown;

		protected List<Vector3> shootDirection;

		[SerializeField]
		[Header("Sound")]
		private AudioClip shakeSound;

		[SerializeField]
		private AudioClip flareSound;

		[SerializeField]
		private AudioClip incomingSound;

		[SerializeField]
		private AudioClip landMineSound;

		private List<Offline_BaseMonster> monsterList = new List<Offline_BaseMonster>();
	}
}
