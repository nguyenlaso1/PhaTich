// @sonhg: class: BombOffline.QueenBee
using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace BombOffline
{
	public class QueenBee : Offline_BaseMonster, IAlert, ISpecialAction
	{
		protected override void InitMonsterProperty()
		{
			this.spawnCooldown = this.spawnTime;
			this.dashCooldown = this.dashTime;
			MusicManager.instance.PlaySingle(this.incomingSound, 1f);
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
			Repeat repeat = new Repeat(new TimeLimit(routine, 1f), -1);
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
				MusicManager.instance.PlaySingle(this.summonSound, 1f);
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

		protected virtual void Dash()
		{
			if (this.CanDash())
			{
				this.dashCooldown = this.dashTime;
				MusicManager.instance.PlaySingle(this.dashSound, 1f);
				Vector3 dircetionVector = base.CurrentDirection.GetDircetionVector();
				int num = Mathf.RoundToInt(dircetionVector.x);
				int num2 = Mathf.RoundToInt(dircetionVector.y);
				int num3 = 1;
				while (this.board.ValidateArrayPosition(base.currentX + num * num3, base.currentY + num2 * num3))
				{
					num3++;
				}
				num3--;
				Vector3[] path = new Vector3[]
				{
					base.transform.position,
					new Vector3((float)base.currentX + (float)num3 * dircetionVector.x, (float)base.currentY + (float)num3 * dircetionVector.y, 0f)
				};
				this.canAct = false;
				base.transform.DOPath(path, 0.2f * (float)num3, PathType.CatmullRom, PathMode.TopDown2D, 10, null).OnComplete(delegate
				{
					this.SnapGrid();
					this.canAct = true;
				});
			}
		}

		public bool CanDash()
		{
			return this.canAct && this.dashCooldown <= 0f && base.CurrentDirection != MoveDirection.STAND;
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
			this.Dash();
			return true;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.spawnCooldown > 0f)
			{
				this.spawnCooldown -= Time.deltaTime;
			}
			if (this.dashCooldown > 0f)
			{
				this.dashCooldown -= Time.deltaTime;
			}
		}

		public override void DestroyMonster()
		{
			base.DestroyMonster();
			DataManager.AchievementCountPlus("KILL_QUEEN_BEE", 1);
		}

		[Header("QueenBee")]
		[SerializeField]
		protected Offline_BaseMonster spawnMonster;

		[SerializeField]
		protected float spawnTime = 15f;

		[SerializeField]
		protected int maxMonster = 2;

		[SerializeField]
		protected float dashTime = 5f;

		[SerializeField]
		private ParticleSystem hitParticle;

		protected float spawnCooldown;

		protected float dashCooldown;

		public int alertRange = 7;

		private List<Offline_BaseMonster> monsterList = new List<Offline_BaseMonster>();

		[Header("Sound")]
		[SerializeField]
		private AudioClip dashSound;

		[SerializeField]
		private AudioClip incomingSound;

		[SerializeField]
		private AudioClip summonSound;
	}
}
