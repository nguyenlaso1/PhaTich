// @sonhg: class: BombOffline.Offline_BaseBuffSystem
using System;
using UnityEngine;

namespace BombOffline
{
	public abstract class Offline_BaseBuffSystem : MonoBehaviour
	{
		public bool IsPoisioned
		{
			get
			{
				return false;
			}
		}

		protected abstract void ApplyBuff();

		protected abstract void ApplySlow();

		protected abstract void ApplySnare();

		protected abstract void ApplyReverse();

		protected abstract void ApplyShield();

		protected abstract void ApplyHaste();

		protected abstract void ApplyAutoFire();

		public abstract bool EatItem(ItemType type, int value, int x, int y);

		public abstract bool ApplyItem(ItemType type, int value = 1);

		private void FixedUpdate()
		{
			this.isSlow = false;
			this.isSnare = false;
			this.isReverse = false;
			this.isHaste = false;
			this.isAutoFire = false;
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
			if (this.hasteTimer > 0f)
			{
				this.ApplyHaste();
			}
			if (this.autoFireTimer > 0f)
			{
				this.ApplyAutoFire();
			}
			this.ApplyBuff();
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.tag == "Item" && this.canEatItem)
			{
				Offline_ItemController component = collider.gameObject.GetComponent<Offline_ItemController>();
				int x = Mathf.RoundToInt(collider.transform.position.x);
				int y = Mathf.RoundToInt(collider.transform.position.y);
				if (this.EatItem(component.type, component.value, x, y))
				{
					UnityEngine.Object.Destroy(collider.gameObject);
				}
			}
			if (collider.gameObject.tag == "Monster")
			{
				this.ApplyItem(ItemType.DEATH, 1);
			}
		}

		private void OnTriggerStay2D(Collider2D collider)
		{
			if (collider.gameObject.tag == "Death")
			{
				this.ApplyItem(ItemType.DEATH, 1);
			}
		}

		[Header("Base")]
		public bool canEatItem = true;

		[SerializeField]
		protected float slowTimer;

		[SerializeField]
		protected float snareTimer;

		[SerializeField]
		protected float reverseTimer;

		[SerializeField]
		protected float hasteTimer;

		[SerializeField]
		protected float autoFireTimer;

		[SerializeField]
		protected float shieldTimer;

		protected bool isSlow;

		protected bool isSnare;

		protected bool isReverse;

		protected bool isHaste;

		protected bool isAutoFire;

		protected bool hasShield;

		protected GameObject slowParticle;

		protected GameObject snareParticle;

		protected GameObject reverseParticle;

		protected GameObject shieldParticle;
	}
}
