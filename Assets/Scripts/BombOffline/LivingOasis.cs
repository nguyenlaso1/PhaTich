// @sonhg: class: BombOffline.LivingOasis
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BombOffline
{
	public class LivingOasis : Offline_BaseMonster, IAlert, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			this.spawnCooldown = this.spawnTime;
			this.stampCooldown = this.stampTime;
			this.healCooldown = this.healTime;
			this.originalHealth = this.health;
			this.board.ShowBossName(this.monsterName);
		}

		protected override void CreateBrain()
		{
			SequenceNode sequenceNode = new SequenceNode();
			sequenceNode.AddRoutine(new Routine[]
			{
				new CheckPlayerInRange(this),
				new SpecialAction(this)
			});
			Wander routine = new Wander(this);
			Repeat repeat = new Repeat(new TimeLimit(routine, 3f), -1);
			Repeat repeat2 = new Repeat(sequenceNode, -1);
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

		protected virtual void SpawnMonster()
		{
			if (this.CanSpawn())
			{
				this.spawnCooldown = this.spawnTime;
				this.monsterList.RemoveAll((Offline_BaseMonster monster) => monster == null);
				List<Vector3> randomTilePosition = this.board.GetRandomTilePosition(this.maxMonster);
				for (int i = 0; i < this.maxMonster; i++)
				{
					Offline_BaseMonster offline_BaseMonster = this.board.PutMonsterAt(base.currentX, base.currentY, this.spawnMonster);
					this.monsterList.Add(offline_BaseMonster);
					offline_BaseMonster.transform.position = randomTilePosition[i];
				}
			}
		}

		public bool CanSpawn()
		{
			return this.spawnCooldown <= 0f;
		}

		protected virtual void Stamp()
		{
			if (this.CanStamp())
			{
				this.stampCooldown = this.stampTime;
				base.StartCoroutine(this.BeginStamp());
			}
		}

		private IEnumerator BeginStamp()
		{
			this.canAct = false;
			MusicManager.instance.PlaySingle(this.deathSound, 1f);
			base.animator.SetInteger("State", 4);
			this.chargeParticle.Play();
			yield return new WaitForSeconds(0.1f);
			base.animator.SetInteger("State", 1);
			yield return new WaitForSeconds(1.5f);
			base.animator.SetTrigger("SpecialTrigger1");
			yield break;
		}

		public override void OnSpecial1AnimationExit()
		{
			this.health++;
			this.board.TriggerSpecialBomb(new Vector2((float)base.currentX, (float)base.currentY), 3, null);
			Camera.main.transform.DOShakePosition(0.7f, new Vector3(0.6f, 0.1f, 0.5f), 30, 90f, false);
			this.canAct = true;
		}

		public bool CanStamp()
		{
			return this.canAct && this.stampCooldown <= 0f;
		}

		public bool CanHeal()
		{
			return this.canAct && this.healCooldown <= 0f;
		}

		protected virtual void Heal()
		{
			if (this.CanHeal())
			{
				this.healCooldown = this.healTime;
				if (this.health != Mathf.Min(this.health + 2, this.originalHealth))
				{
					this.health = Mathf.Min(this.health + 2, this.originalHealth);
					this.board.Scene.ParticlesController.PlayLoseHeartParticle(base.transform);
				}
				base.RenderHealthBar();
			}
		}

		public bool IsPlayerInRange()
		{
			return this.board.IsInRanceWithPlayer(base.currentX, base.currentY, (float)this.AlertRange);
		}

		public int AlertRange
		{
			get
			{
				return this.alertRange;
			}
		}

		public virtual bool DoSpecialAction()
		{
			this.SpawnMonster();
			this.Stamp();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
			if (this.stampCooldown > 0f)
			{
				this.stampCooldown -= Time.deltaTime;
			}
			if (this.healCooldown > 0f)
			{
				this.healCooldown -= Time.deltaTime;
			}
			else
			{
				this.Heal();
			}
		}

		public override void DestroyMonster()
		{
			base.DestroyMonster();
			DataManager.AchievementCountPlus("KILL_LIVING_OASIS", 1);
		}

		[Header("LivingOasis")]
		[SerializeField]
		protected Offline_BaseMonster spawnMonster;

		[SerializeField]
		protected ParticleSystem chargeParticle;

		[SerializeField]
		protected float spawnTime = 15f;

		[SerializeField]
		protected int maxMonster = 2;

		[SerializeField]
		protected float stampTime = 5f;

		[SerializeField]
		protected float healTime = 20f;

		[SerializeField]
		private ParticleSystem hitParticle;

		protected float spawnCooldown;

		protected float stampCooldown;

		protected float healCooldown;

		public int alertRange = 2;

		private List<Offline_BaseMonster> monsterList = new List<Offline_BaseMonster>();

		private int originalHealth;
	}
}
