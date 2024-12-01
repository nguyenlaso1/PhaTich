// @sonhg: class: BombOffline.NovaGolem
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class NovaGolem : Offline_BaseMonster, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			MusicManager.instance.PlaySingle(this.incomingSound, 1f);
			this.board.ShowBossName(this.monsterName);
			this.shootDirection = new List<Vector3>();
			this.shootDirection.Add(Vector3.up);
			this.shootDirection.Add(Vector3.down);
			this.shootDirection.Add(Vector3.left);
			this.shootDirection.Add(Vector3.right);
			this.shootDirection.Add(Vector3.up + Vector3.left);
			this.shootDirection.Add(Vector3.down + Vector3.left);
			this.shootDirection.Add(Vector3.up + Vector3.right);
			this.shootDirection.Add(Vector3.down + Vector3.right);
			this.spawnCooldown = this.spawnTime;
			this.blowCooldown = this.blowTime;
			this.shootCooldown = this.shootTime;
			this.strikeCooldown = this.strikeTime;
			this.OnDestroy = delegate(BaseBullet bullet, Transform hitObject)
			{
				this.board.TriggerSquareBomb(bullet.transform.position, 2, null);
			};
			this.listSkill.Add(new NovaGolem.SkillDelegate(this.Blow));
			this.listSkill.Add(new NovaGolem.SkillDelegate(this.Strike));
			this.listSkill.Add(new NovaGolem.SkillDelegate(this.SpawnMonster));
			this.listSkill.Add(new NovaGolem.SkillDelegate(this.Shoot45));
		}

		protected override void CreateBrain()
		{
			this.brain = new Repeat(new SpecialAction(this), -1);
		}

		protected virtual void SpawnMonster()
		{
			if (this.CanSpawn())
			{
				this.spawnCooldown = this.spawnTime;
				this.monsterList.RemoveAll((Offline_BaseMonster monster) => monster == null);
				List<Vector3> randomTilePosition = this.board.GetRandomTilePosition(this.maxMonster - this.monsterList.Count);
				for (int i = 0; i < randomTilePosition.Count; i++)
				{
					Offline_BaseMonster offline_BaseMonster = this.board.PutMonsterAt(base.currentX, base.currentY, this.spawnMonster);
					this.monsterList.Add(offline_BaseMonster);
					offline_BaseMonster.transform.position = randomTilePosition[i];
				}
			}
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

		public bool CanSpawn()
		{
			return this.spawnCooldown <= 0f;
		}

		public virtual bool DoSpecialAction()
		{
			this.listSkill.GetRandomElement<NovaGolem.SkillDelegate>()();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
			if (this.blowCooldown > 0f)
			{
				this.blowCooldown -= Time.deltaTime;
			}
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
			if (this.strikeCooldown > 0f)
			{
				this.strikeCooldown -= Time.deltaTime;
			}
		}

		public bool CanBlow()
		{
			return this.canAct && this.blowCooldown <= 0f;
		}

		protected virtual void Blow()
		{
			if (this.CanBlow())
			{
				this.blowCooldown = this.blowTime;
				List<Vector3> randomTilePosition = this.board.GetRandomTilePosition(this.bombCount);
				foreach (Vector3 position in randomTilePosition)
				{
					FireWall fireWall = UnityEngine.Object.Instantiate<FireWall>(this.bombPrefab);
					fireWall.transform.position = position;
				}
				this.bombCount += 2;
				base.animator.SetTrigger("TriggerSkill3");
				this.canAct = false;
				base.StartCoroutine(this.shakeCamera());
			}
		}

		private IEnumerator shakeCamera()
		{
			MusicManager.instance.PlaySingle(this.blow2StartSound, 1f);
			yield return new WaitForSeconds(1f);
			Camera.main.transform.DOShakePosition(0.7f, new Vector3(0.6f, 0.1f, 0.5f), 30, 90f, false);
			yield return new WaitForSeconds(0.5f);
			MusicManager.instance.PlaySingle(this.blow2EndSound, 1f);
			yield break;
		}

		public bool CanStrike()
		{
			return this.canAct && this.strikeCooldown <= 0f;
		}

		protected virtual void Strike()
		{
			if (this.CanStrike())
			{
				this.strikeCooldown = this.strikeTime;
				base.animator.SetTrigger("TriggerSkill1");
				MusicManager.instance.PlaySingle(this.sunStrikeSound, 1f);
				this.canAct = false;
			}
		}

		public bool CanShoot45()
		{
			return this.canAct && this.shootCooldown <= 0f;
		}

		protected virtual void Shoot45()
		{
			if (this.CanShoot45())
			{
				this.shootCooldown = this.shootTime;
				base.StartCoroutine(this.ShootRoutine());
			}
		}

		private IEnumerator ShootRoutine()
		{
			Vector3 cachePosition = this.target.transform.position;
			this.PlayAimParticle(cachePosition, new float?(1f));
			MusicManager.instance.PlaySingle(this.shoot45StartSound, 1f);
			yield return new WaitForSeconds(1f);
			MusicManager.instance.PlaySingle(this.shoot45EndSound, 1f);
			BaseBullet bulletObject = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
			bulletObject.bulletSpeed = 2.5f;
			bulletObject.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
			bulletObject.shootDirection = cachePosition - base.transform.position;
			bulletObject.targetPosition = cachePosition;
			bulletObject.OnDestroyBullet = this.OnDestroy;
			base.animator.SetTrigger("TriggerSkill2");
			this.canAct = false;
			yield break;
		}

		public GameObject PlayAimParticle(Vector3 position, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._aimParticlePrefab, position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
				particleSystem.startLifetime = time.Value;
			}
			return particleSystem.gameObject;
		}

		public override void DestroyMonster()
		{
			base.DestroyMonster();
			DataManager.AchievementCountPlus("KILL_NOVA_GOLEM", 1);
		}

		private List<NovaGolem.SkillDelegate> listSkill = new List<NovaGolem.SkillDelegate>();

		[Header("Strike")]
		[SerializeField]
		protected float strikeTime = 7f;

		protected float strikeCooldown;

		[SerializeField]
		[Header("Blow 2")]
		protected float blowTime = 7f;

		[SerializeField]
		protected FireWall bombPrefab;

		[SerializeField]
		protected int bombCount = 3;

		protected float blowCooldown;

		[Header("Summon")]
		[SerializeField]
		protected Offline_BaseMonster spawnMonster;

		[SerializeField]
		protected float spawnTime = 10f;

		[SerializeField]
		protected int maxMonster = 2;

		private List<Offline_BaseMonster> monsterList = new List<Offline_BaseMonster>();

		protected float spawnCooldown;

		[Header("Shoot4.5")]
		[SerializeField]
		protected float shootTime = 3f;

		[SerializeField]
		protected BaseBullet bulletPrefab;

		[SerializeField]
		protected ParticleSystem _aimParticlePrefab;

		[SerializeField]
		private ParticleSystem hitParticle;

		protected float shootCooldown;

		protected List<Vector3> shootDirection;

		protected BaseBullet.BulletCallbackDelegate OnDestroy;

		[Header("Sound")]
		[SerializeField]
		private AudioClip blow2EndSound;

		[SerializeField]
		private AudioClip blow2StartSound;

		[SerializeField]
		private AudioClip shoot45StartSound;

		[SerializeField]
		private AudioClip shoot45EndSound;

		[SerializeField]
		private AudioClip incomingSound;

		[SerializeField]
		private AudioClip sunStrikeSound;

		public delegate void SkillDelegate();
	}
}
