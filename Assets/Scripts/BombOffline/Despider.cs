// @sonhg: class: BombOffline.Despider
using System;
using UnityEngine;

namespace BombOffline
{
	public class Despider : Offline_BaseMonster
	{
		protected override void CreateBrain()
		{
			Wander routine = new Wander(this);
			this.brain = new Repeat(routine, -1);
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.brain.State == Routine.RoutineState.Failure && this.canMove)
			{
				base.CurrentDirection = MoveDirection.STAND;
				this.SnapGrid();
				this.CreateBrain();
			}
			if (this.CanShoot())
			{
				this.shootCooldown = this.cooldown;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab);
				gameObject.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
				BaseBullet component = gameObject.GetComponent<BaseBullet>();
				component.shootDirection = this.shootDirection;
				component.shooter = base.transform;
			}
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
		}

		public bool CanShoot()
		{
			return this.shootCooldown <= 0f && this.CheckForPlayer();
		}

		private bool CheckForPlayer()
		{
			float distance = 300f;
			int layerMask = 1 << LayerMask.NameToLayer("Character") | 1 << LayerMask.NameToLayer("Wall");
			this.shootDirection = base.transform.right;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, this.shootDirection, distance, layerMask);
			if (raycastHit2D.collider == null || !raycastHit2D.collider.CompareTag("Player"))
			{
				this.shootDirection = -base.transform.right;
				raycastHit2D = Physics2D.Raycast(base.transform.position, this.shootDirection, distance, layerMask);
			}
			if (raycastHit2D.collider == null || !raycastHit2D.collider.CompareTag("Player"))
			{
				this.shootDirection = base.transform.up;
				raycastHit2D = Physics2D.Raycast(base.transform.position, this.shootDirection, distance, layerMask);
			}
			if (raycastHit2D.collider == null || !raycastHit2D.collider.CompareTag("Player"))
			{
				this.shootDirection = -base.transform.up;
				raycastHit2D = Physics2D.Raycast(base.transform.position, this.shootDirection, distance, layerMask);
			}
			return raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player");
		}

		[Header("Despider")]
		[SerializeField]
		protected float cooldown = 3f;

		[SerializeField]
		protected GameObject bulletPrefab;

		protected float shootCooldown;

		protected Vector3 shootDirection;
	}
}
