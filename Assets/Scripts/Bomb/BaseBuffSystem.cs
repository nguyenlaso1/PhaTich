// @sonhg: class: Bomb.BaseBuffSystem
using System;
using UnityEngine;

namespace Bomb
{
	public abstract class BaseBuffSystem : MonoBehaviour
	{
		public bool IsPoisioned
		{
			get
			{
				return this.isSlow || this.isSnare || this.isReverse;
			}
		}

		protected abstract void ApplyBuff();

		protected abstract void ApplySlow();

		protected abstract void ApplySnare();

		protected abstract void ApplyReverse();

		protected abstract void ApplyShield();

		public abstract bool EatItem(ItemType type, int x, int y);

		public abstract bool ApplyItem(ItemType type);

		private void FixedUpdate()
		{
			this.isSlow = false;
			this.isSnare = false;
			this.isReverse = false;
			this.hasShield = false;
			if (this.slowTimer > 0f)
			{
				this.ApplySlow();
			}
			if (this.snareTimer > 0f)
			{
				this.ApplySnare();
			}
			if (this.reverseTimer > 0f)
			{
				this.ApplyReverse();
			}
			if (this.shieldTimer > 0f)
			{
				this.ApplyShield();
			}
			this.ApplyBuff();
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.tag == "Item" && this.canEatItem)
			{
				ItemController component = collider.gameObject.GetComponent<ItemController>();
				int x = Mathf.RoundToInt(collider.transform.position.x);
				int y = Mathf.RoundToInt(collider.transform.position.y);
				if (this.EatItem(component.type, x, y))
				{
					UnityEngine.Object.Destroy(collider.gameObject);
				}
			}
			if (collider.gameObject.tag == "Death")
			{
				this.ApplyItem(ItemType.DEATH);
			}
		}

		[Header("Base")]
		public bool canEatItem = true;

		[HideInInspector]
		public int amount;

		[SerializeField]
		protected float slowTimer;

		[SerializeField]
		protected float snareTimer;

		[SerializeField]
		protected float reverseTimer;

		[SerializeField]
		protected float shieldTimer;

		protected bool isSlow;

		protected bool isSnare;

		protected bool isReverse;

		protected bool hasShield;

		protected GameObject slowParticle;

		protected GameObject snareParticle;

		protected GameObject reverseParticle;

		protected GameObject shieldParticle;
	}
}
