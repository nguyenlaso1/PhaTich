// @sonhg: class: BaseBullet
using System;
using BombOffline;
using DG.Tweening;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
	public int currentX
	{
		get
		{
			float num = base.transform.position.x;
			int num2 = Mathf.FloorToInt(num);
			num -= (float)num2;
			if (num > 0.5f)
			{
				return num2 + 1;
			}
			if (num < 0f)
			{
				return num2 - 1;
			}
			return num2;
		}
	}

	public int currentY
	{
		get
		{
			float num = base.transform.position.y;
			int num2 = Mathf.FloorToInt(num);
			num -= (float)num2;
			if (num > 0.5f)
			{
				return num2 + 1;
			}
			if (num < 0f)
			{
				return num2 - 1;
			}
			return num2;
		}
	}

	private void Start()
	{
		this.InitBullet();
	}

	private void FixedUpdate()
	{
		if (!this.isDestroy)
		{
			if (this.isShowDebugLine)
			{
				this.DrawDebugLine();
			}
			this.MoveBullet();
			this.currentLifeTime += Time.deltaTime;
			if (this.lifeTime < this.currentLifeTime)
			{
				this.DestroyBullet();
			}
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			Offline_BaseCharactersController component = collider.gameObject.GetComponent<Offline_BaseCharactersController>();
			component.GetHit(null);
			if (this.OnHitPlayer != null)
			{
				this.OnHitPlayer(this, component.transform);
			}
			this.DestroyBullet();
		}
		if (collider.gameObject.CompareTag("Destroyable"))
		{
			this.DestroyBullet();
		}
		if (collider.gameObject.CompareTag("NonDestroyable"))
		{
			this.DestroyBullet();
		}
		if (collider.gameObject.CompareTag("BorderWall"))
		{
			this.DestroyBullet();
		}
	}

	protected virtual void InitBullet()
	{
	}

	protected virtual void MoveBullet()
	{
		base.transform.Translate(this.shootDirection.normalized * this.bulletSpeed * Time.deltaTime);
	}

	protected bool MoveTo(Vector3 destination)
	{
		Vector3 normalized = (destination - base.transform.position).normalized;
		float magnitude = (destination - base.transform.position).magnitude;
		float num = Mathf.Min(magnitude, this.bulletSpeed * Time.deltaTime);
		base.transform.Translate(num * normalized);
		return num == magnitude;
	}

	protected virtual void DrawDebugLine()
	{
		UnityEngine.Debug.DrawLine(base.transform.position, base.transform.position + this.shootDirection.normalized * 0.2f, Color.red);
	}

	protected virtual void DestroyBullet()
	{
		this.isDestroy = true;
		UnityEngine.Object.Destroy(base.GetComponent<BoxCollider2D>());
		this.bulletSprite.DoAlpha(0f, 0.3f).OnComplete(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
		if (this.OnDestroyBullet != null)
		{
			this.OnDestroyBullet(this, null);
		}
	}

	[Header("BaseBullet")]
	public Vector3 shootDirection;

	public float lifeTime = 10f;

	public float bulletSpeed = 2f;

	public bool isShowDebugLine;

	[HideInInspector]
	public Transform shooter;

	[HideInInspector]
	public Transform target;

	[HideInInspector]
	public Vector3 targetPosition;

	[SerializeField]
	protected SpriteRenderer bulletSprite;

	protected float currentLifeTime;

	protected bool isDestroy;

	public BaseBullet.BulletCallbackDelegate OnDestroyBullet;

	public BaseBullet.BulletCallbackDelegate OnHitPlayer;

	public BaseBullet.BulletCallbackDelegate OnHitBomb;

	public delegate void BulletCallbackDelegate(BaseBullet bullet, Transform hitObject = null);
}
