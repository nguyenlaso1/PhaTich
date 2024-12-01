// @sonhg: class: BombOffline.Lolitop
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class Lolitop : Offline_BaseMonster, ISpecialAction
	{
		protected override void CreateBrain()
		{
			SpecialAction routine = new SpecialAction(this);
			this.brain = new Repeat(routine, -1);
		}

		protected override void InitMonsterProperty()
		{
			this.shootDirection = new List<Vector3>();
			this.shootDirection.Add(Vector3.up);
			this.shootDirection.Add(Vector3.down);
			this.shootDirection.Add(Vector3.left);
			this.shootDirection.Add(Vector3.right);
			this.shootCooldown = this.cooldown;
			this.swapCooldown = this.cooldownSwap;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (this.shootCooldown > 0f)
			{
				this.shootCooldown -= Time.deltaTime;
			}
			if (this.swapCooldown > 0f)
			{
				this.swapCooldown -= Time.deltaTime;
			}
		}

		protected virtual void Shoot()
		{
			if (this.CanShoot())
			{
				this.shootCooldown = this.cooldown;
				MusicManager.instance.PlayOneShot(this.shootSound, 1f);
				foreach (Vector3 vector in this.shootDirection)
				{
					BaseBullet baseBullet = UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab);
					baseBullet.transform.position = new Vector3((float)base.currentX, (float)base.currentY, 0f);
					baseBullet.shootDirection = vector;
				}
			}
		}

		public virtual bool CanShoot()
		{
			return this.shootCooldown < 0f;
		}

		protected virtual void Swap()
		{
			if (this.CanSwap())
			{
				this.swapCooldown = this.cooldownSwap;
				List<Vector3> randomTilePosition = this.board.GetRandomTilePosition(1);
				base.StartCoroutine(this.hide());
				base.StartCoroutine(this.Unhide(randomTilePosition[0]));
			}
		}

		private IEnumerator hide()
		{
			this.PlaySwap1(base.transform, new float?(1.5f));
			this.shootCooldown = 3f;
			yield return new WaitForSeconds(1f);
			foreach (SpriteRenderer render in this.listSprite)
			{
				render.DoAlpha(0f, 0.5f);
			}
			yield break;
		}

		private IEnumerator Unhide(Vector3 position)
		{
			yield return new WaitForSeconds(2f);
			base.transform.position = position;
			this.PlaySwap2(base.transform, new float?(1.5f));
			foreach (SpriteRenderer render in this.listSprite)
			{
				render.DoAlpha(1f, 0.5f);
			}
			yield break;
		}

		protected GameObject PlaySwap1(Transform actor, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._swap1, actor.position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
			}
			return particleSystem.gameObject;
		}

		protected GameObject PlaySwap2(Transform actor, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._swap2, actor.position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
			}
			return particleSystem.gameObject;
		}

		public virtual bool CanSwap()
		{
			return this.swapCooldown < 0f;
		}

		public bool DoSpecialAction()
		{
			this.Shoot();
			this.Swap();
			return true;
		}

		public override bool GetHit(int x, int y)
		{
			if (this.hasShield)
			{
				return false;
			}
			if (!base.gameObject.activeInHierarchy)
			{
				return false;
			}
			this.health--;
			base.RenderHealthBar();
			return this.health == 0;
		}

		[Header("Lolitop")]
		[SerializeField]
		protected float cooldown = 5f;

		[SerializeField]
		protected float cooldownSwap = 7f;

		[SerializeField]
		protected BaseBullet bulletPrefab;

		protected float shootCooldown;

		protected float swapCooldown;

		protected List<Vector3> shootDirection;

		[SerializeField]
		protected List<SpriteRenderer> listSprite;

		[SerializeField]
		private ParticleSystem _swap1;

		[SerializeField]
		private ParticleSystem _swap2;

		[SerializeField]
		protected AudioClip shootSound;

		private bool hasShield;
	}
}
