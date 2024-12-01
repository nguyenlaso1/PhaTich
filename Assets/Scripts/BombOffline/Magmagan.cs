// @sonhg: class: BombOffline.Magmagan
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class Magmagan : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			AIMove routine = new AIMove(this);
			SpecialAction routine2 = new SpecialAction(this);
			Repeat repeat = new Repeat(routine, -1);
			Repeat repeat2 = new Repeat(routine2, -1);
			Parallel brain = new Parallel(3, 3, new Routine[]
			{
				repeat,
				repeat2
			});
			this.brain = brain;
		}

		protected override void InitMonsterProperty()
		{
			if (this.initalDirection == MoveDirection.STAND)
			{
				base.ChangeDirection();
			}
			else
			{
				base.CurrentDirection = this.initalDirection;
			}
			this.shootCooldown = this.shootTime;
			this.poisonTrap.time = 1f;
			this.parameter.Add("isDestroyTile", false);
			this.parameter.Add("isHitMonster", false);
			this.parameter.Add("isDestroyBomb", false);
			this.parameter.Add("isDestroyItem", false);
			this.OnDestroy = delegate(BaseBullet bullet, Transform hitObject)
			{
				List<Vector3> list = new List<Vector3>();
				float num = Mathf.Round(bullet.transform.position.x);
				float num2 = Mathf.Round(bullet.transform.position.y);
				list.Add(new Vector2(num, num2));
				list.Add(new Vector2(num + 1f, num2));
				list.Add(new Vector2(num - 1f, num2));
				list.Add(new Vector2(num, num2 + 1f));
				list.Add(new Vector2(num, num2 - 1f));
				this.Pool(list);
			};
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
		}

		protected void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.shootTime;
				MusicManager.instance.PlayOneShot(this.shootSound, 1f);
				BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
				baseBullet.bulletSpeed = 3f;
				baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
				baseBullet.shootDirection = this.target.transform.position - base.transform.position;
				baseBullet.targetPosition = this.target.transform.position;
				baseBullet.OnDestroyBullet = this.OnDestroy;
			}
		}

		protected virtual void Pool(List<Vector3> positionList)
		{
			Offline_Tiled[,] mapTiled = this.board.Scene.MapController.mapTiled;
			foreach (Vector3 position in positionList)
			{
				int num = Mathf.RoundToInt(position.x);
				int num2 = Mathf.RoundToInt(position.y);
				if (this.board.ValidateArrayPosition(num, num2))
				{
					if (mapTiled[num, num2].status != 1 && !mapTiled[num, num2].IsBrick())
					{
						this.board.PlaceObstacle(this.poisonTrap, position, null);
					}
				}
			}
		}

		public bool CanShoot()
		{
			return this.shootCooldown <= 0f;
		}

		public bool DoSpecialAction()
		{
			this.Shoot();
			return true;
		}

		[Header("Magmagan")]
		public MoveDirection initalDirection = MoveDirection.STAND;

		[SerializeField]
		protected float shootTime = 3f;

		[SerializeField]
		protected BaseBullet bulletPrefab;

		[SerializeField]
		protected PoisonTrap poisonTrap;

		[SerializeField]
		protected AudioClip shootSound;

		protected float shootCooldown;

		protected BaseBullet.BulletCallbackDelegate OnDestroy;

		private Dictionary<string, bool> parameter = new Dictionary<string, bool>();
	}
}
