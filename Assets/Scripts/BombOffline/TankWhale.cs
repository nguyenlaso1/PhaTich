// @sonhg: class: BombOffline.TankWhale
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class TankWhale : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			SequenceNode sequenceNode = new SequenceNode();
			sequenceNode.AddRoutine(new Routine[]
			{
				new AIMoveTo(this, 1, base.currentY),
				new AIMoveTo(this, 10, base.currentY)
			});
			Repeat repeat = new Repeat(sequenceNode, -1);
			Repeat repeat2 = new Repeat(new SpecialAction(this), -1);
			this.brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
		}

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
			this.diveCooldown = this.diveTime;
			this.shootCooldown = this.shootTime;
			this.blowCooldown = this.blowTime;
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

		public virtual bool DoSpecialAction()
		{
			this.Shoot();
			this.Blow();
			return true;
		}

		public bool CanDive()
		{
			return this.canAct && this.diveCooldown <= 0f;
		}

		protected virtual void Dive()
		{
			if (this.CanDive())
			{
				this.diveCooldown = this.diveTime;
				this.board.StartCoroutine(this.UnDive());
				this.canAct = false;
				base.gameObject.SetActive(false);
			}
		}

		private IEnumerator UnDive()
		{
			yield return new WaitForSeconds(3f);
			base.gameObject.SetActive(true);
			this.canAct = true;
			yield break;
		}

		public bool CanShoot()
		{
			return this.canAct && this.shootCooldown <= 0f;
		}

		protected virtual void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.shootTime;
				MusicManager.instance.PlayOneShot(this.shootSound, 1f);
				foreach (Vector3 vector in this.shootDirection)
				{
					BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
					baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					baseBullet.shootDirection = vector;
				}
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
				Camera.main.transform.DOShakePosition(0.7f, new Vector3(0.6f, 0.1f, 0.5f), 30, 90f, false);
				List<Vector3> randomTilePosition = this.board.GetRandomTilePosition(this.bombCount);
				foreach (Vector3 position in randomTilePosition)
				{
					MonsterBomb monsterBomb = UnityEngine.Object.Instantiate<MonsterBomb>(this.bombPrefab);
					monsterBomb.board = this.board;
					monsterBomb.transform.position = position;
				}
				this.bombCount += 2;
				base.StartCoroutine(this.PlayBlowSoundCoroutine());
			}
		}

		private IEnumerator PlayBlowSoundCoroutine()
		{
			yield return new WaitForSeconds(1.5f);
			MusicManager.instance.PlaySingle(this.blowSound, 1f);
			yield break;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!this.canAct)
			{
				return;
			}
			if (this.diveCooldown > 0f)
			{
				this.diveCooldown -= Time.deltaTime;
			}
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
			if (this.blowCooldown > 0f)
			{
				this.blowCooldown -= Time.deltaTime;
			}
		}

		public override void DestroyMonster()
		{
			base.DestroyMonster();
			DataManager.AchievementCountPlus("KILL_TANK_WHALE", 1);
		}

		[Header("TankWhale")]
		[SerializeField]
		protected BaseBullet bulletPrefab;

		[SerializeField]
		protected MonsterBomb bombPrefab;

		[SerializeField]
		protected int bombCount = 2;

		[SerializeField]
		protected float diveTime = 9f;

		[SerializeField]
		protected float shootTime = 7f;

		[SerializeField]
		protected float blowTime = 12f;

		[SerializeField]
		private ParticleSystem hitParticle;

		[SerializeField]
		protected AudioClip shootSound;

		protected float diveCooldown;

		protected float shootCooldown;

		protected float blowCooldown;

		protected List<Vector3> shootDirection;

		[SerializeField]
		[Header("Sound")]
		private AudioClip incomingSound;

		[SerializeField]
		private AudioClip blowSound;
	}
}
