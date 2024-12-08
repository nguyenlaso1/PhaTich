// @sonhg: class: BombOffline.CursedJelly
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class CursedJelly : Offline_BaseMonster, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			MusicManager.instance.PlaySingle(this.incomingSound, 1f);
			//this.board.ShowBossName(this.monsterName);
			this.spawnCooldown = this.spawnTime;
			this.shootCooldown = this.shootTime;
			this.poolCooldown = this.poolTime;
			this.poisonTrapInstance = UnityEngine.Object.Instantiate<PoisonTrap>(this.poisonTrap);
			this.poisonTrapInstance.transform.position = Vector3.one * 100f;
			this.poisonTrapInstance.time = -1f;
			this.OnHitPlayer = delegate(BaseBullet bullet, Transform hitObject)
			{
				this.board.player.ApplyItem(ItemType.REVERSE);
			};
		}

		protected override void CreateBrain()
		{
			Repeat brain = new Repeat(new SpecialAction(this), -1);
			this.brain = brain;
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

		protected virtual void SpawnMonster()
		{
			if (this.CanSpawn())
			{
				MusicManager.instance.PlaySingle(this.incomingSound, 1f);
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
			return this.canAct && this.spawnCooldown <= 0f;
		}

		protected virtual void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.shootTime;
				BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
				baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
				baseBullet.shootDirection = this.target.transform.position - base.transform.position;
			}
		}

		protected virtual void Pool()
		{
			if (this.CanPool())
			{
				this.poolCooldown = this.poolTime;
				Offline_Tiled[,] mapTiled = this.board.Scene.MapController.mapTiled;
				int length = mapTiled.GetLength(0);
				int length2 = mapTiled.GetLength(1);
				for (int i = this.poolCount; i < length - this.poolCount; i++)
				{
					this.board.PlaceObstacle(this.poisonTrapInstance, new Vector2((float)i, (float)this.poolCount), null);
					this.board.PlaceObstacle(this.poisonTrapInstance, new Vector2((float)i, (float)(length2 - 1 - this.poolCount)), null);
				}
				for (int j = this.poolCount; j < length2 - this.poolCount; j++)
				{
					this.board.PlaceObstacle(this.poisonTrapInstance, new Vector2((float)this.poolCount, (float)j), null);
					this.board.PlaceObstacle(this.poisonTrapInstance, new Vector2((float)(length - 1 - this.poolCount), (float)j), null);
				}
				this.poolCount++;
			}
		}

		public bool CanShoot()
		{
			return this.canAct && this.shootCooldown <= 0f;
		}

		public bool CanPool()
		{
			return this.canAct && this.poolCooldown <= 0f;
		}

		public virtual bool DoSpecialAction()
		{
			this.SpawnMonster();
			this.Shoot();
			this.Pool();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
			if (this.poolCooldown > 0f)
			{
				this.poolCooldown -= Time.deltaTime;
			}
		}

		public override void DestroyMonster()
		{
			base.DestroyMonster();
			DataManager.AchievementCountPlus("KILL_CURSED_JELLY", 1);
		}

		[Header("CursedJelly")]
		[SerializeField]
		protected Offline_BaseMonster spawnMonster;

		[SerializeField]
		protected BaseBullet bulletPrefab;

		[SerializeField]
		protected PoisonTrap poisonTrap;

		[SerializeField]
		protected float spawnTime = 15f;

		[SerializeField]
		protected float poolTime = 15f;

		[SerializeField]
		protected int maxMonster = 2;

		[SerializeField]
		protected float shootTime = 3f;

		[SerializeField]
		private ParticleSystem hitParticle;

		protected float spawnCooldown;

		protected float shootCooldown;

		protected float poolCooldown;

		private List<Offline_BaseMonster> monsterList = new List<Offline_BaseMonster>();

		private int poolCount;

		protected PoisonTrap poisonTrapInstance;

		[SerializeField]
		[Header("Sound")]
		private AudioClip incomingSound;

		protected BaseBullet.BulletCallbackDelegate OnHitPlayer;
	}
}
