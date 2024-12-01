// @sonhg: class: BombOffline.BigDespider
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class BigDespider : Offline_BaseMonster, IAlert, ISpecialAction
	{
		protected override void CreateBrain()
		{
			SequenceNode sequenceNode = new SequenceNode();
			sequenceNode.AddRoutine(new Routine[]
			{
				new CheckPlayerInRange(this),
				new SpecialAction(this)
			});
			Repeat repeat = new Repeat(new Wander(this), -1);
			Repeat repeat2 = new Repeat(sequenceNode, -1);
			this.brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
		}

		protected override void InitMonsterProperty()
		{
			this.shootDirection = new List<Vector3>();
			this.shootDirection.Add(Vector3.up);
			this.shootDirection.Add(Vector3.down);
			this.shootDirection.Add(Vector3.left);
			this.shootDirection.Add(Vector3.right);
			this.shootDirection.Add(Vector3.up + Vector3.left);
			this.shootDirection.Add(Vector3.up + Vector3.right);
			this.shootDirection.Add(Vector3.down + Vector3.left);
			this.shootDirection.Add(Vector3.down + Vector3.right);
			this.shootCooldown = this.cooldown;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
		}

		public override IEnumerable<MapNode> GetMapRoute()
		{
			int toX = 0;
			int toY = 0;
			this.board.GetRandomPositionAt(ref toX, ref toY);
			return this.board.SearchFromTo(base.currentX, base.currentY, toX, toY, this);
		}

		protected virtual void ShootBullet()
		{
			if (this.CanShoot() && this.IsPlayerInRange())
			{
				this.shootCooldown = this.cooldown;
				foreach (Vector3 vector in this.shootDirection)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab);
					gameObject.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					BaseBullet component = gameObject.GetComponent<BaseBullet>();
					component.shootDirection = vector;
					component.shooter = base.transform;
				}
			}
		}

		public bool CanShoot()
		{
			return this.shootCooldown <= 0f;
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
			this.ShootBullet();
			return true;
		}

		[Header("Big Despider")]
		public int alertRange = 7;

		[SerializeField]
		protected float cooldown = 3f;

		[SerializeField]
		protected GameObject bulletPrefab;

		protected float shootCooldown;

		protected List<Vector3> shootDirection;
	}
}
