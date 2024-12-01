// @sonhg: class: Bomb.EnemyController
using System;
using UnityEngine;

namespace Bomb
{
	[RequireComponent(typeof(EnemyBuffSystem))]
	public class EnemyController : BaseCharactersController
	{
		protected override void Awake()
		{
			base.Awake();
			EnemyBuffSystem component = base.GetComponent<EnemyBuffSystem>();
			component.enemy = this;
			this._buffSystem = component;
		}

		public override void RenderCharacter()
		{
			float num = (float)this.getTimer();
			float num2 = num - this.lastRenderTime;
			num2 = Mathf.Min(100f, num2);
			if (this.updateNow)
			{
				Vector3 dircetionVector = base.GetDircetionVector(base.CurrentDirection);
				Vector3 a = Vector3.zero;
				int num3 = 0;
				while ((float)num3 < num2)
				{
					a += dircetionVector / 1000f * base.CurrentMoveSpeed;
					num3++;
				}
				int layerMask = 1 << LayerMask.NameToLayer("Wall");
				RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, dircetionVector, a.magnitude, layerMask);
				if (raycastHit2D.collider != null)
				{
					float num4 = Vector3.Distance(raycastHit2D.collider.transform.position, base.transform.position);
					if (num4 < a.magnitude + 1f)
					{
						int num5 = Mathf.RoundToInt(num4) - 1;
						a = dircetionVector * (float)num5;
					}
				}
				base.transform.position = new Vector2(this.px + a.x, this.py + a.y);
				this.updateNow = false;
			}
			new MoveCommand(base.CurrentDirection).Execute(this);
			if (!this.isDead)
			{
				if (this._buffSystem.IsPoisioned && !this.isDead)
				{
					base.animator.SetInteger("Status", 1);
				}
				else
				{
					base.animator.SetInteger("Status", 0);
				}
			}
		}

		public int getTimer()
		{
			return (int)Mathf.Round(Time.time * 1000f);
		}

		public override void GetHit(BombModel bombModel)
		{
			if (!this.isDead)
			{
				this.isDead = true;
				this._buffSystem.canEatItem = false;
				this.CanMoveThrough = true;
				base.animator.SetInteger("Status", 2);
				this.tombObject = (UnityEngine.Object.Instantiate(this.tombPrefab, base.transform.position, Quaternion.identity) as GameObject);
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

		public override void PickUpItem(ItemType type, int amount)
		{
			this._buffSystem.amount = amount;
			this._buffSystem.ApplyItem(type);
		}

		[Header("Enemy")]
		public GameObject bombPrefab;

		public GameObject tombPrefab;

		public bool hasShield;

		[SerializeField]
		private bool isDead;

		private GameObject tombObject;

		private EnemyBuffSystem _buffSystem;
	}
}
