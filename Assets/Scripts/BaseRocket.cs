// @sonhg: class: BaseRocket
using System;
using BombOffline;
using UnityEngine;

public class BaseRocket : MonoBehaviour
{
	private void Start()
	{
		this.InitBullet();
	}

	private void FixedUpdate()
	{
		if (!this.isDestroy)
		{
			this.currentLifeTime += Time.deltaTime;
			if (this.lifeTime < this.currentLifeTime)
			{
				this.DestroyBullet();
			}
			if (this._timeCount < 5f && this.target != null)
			{
				this._timeCount += Time.fixedDeltaTime;
				iTween.Stop(base.gameObject);
				iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
				{
					"position",
					this.target.position,
					"time",
					this.rocketSpeed,
					"easetype",
					iTween.EaseType.linear
				}));
			}
			if (this._timeCount >= 5f)
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
			this.DestroyBullet();
		}
	}

	protected virtual void InitBullet()
	{
	}

	protected virtual void DestroyBullet()
	{
		ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.explosionParticle);
		particleSystem.transform.position = base.transform.position;
		particleSystem.Play();
		UnityEngine.Object.Destroy(particleSystem.gameObject, 1f);
		this.isDestroy = true;
		UnityEngine.Object.Destroy(base.GetComponent<BoxCollider2D>());
		UnityEngine.Object.Destroy(base.gameObject);
		if (this.OnDestroyRocket != null)
		{
			this.OnDestroyRocket(this);
		}
	}

	[Header("BaseRocket")]
	public float lifeTime = 5f;

	public float rocketSpeed = 0.2f;

	[HideInInspector]
	public Transform shooter;

	[HideInInspector]
	public Transform target;

	[HideInInspector]
	public Vector3 targetPosition;

	[SerializeField]
	private ParticleSystem explosionParticle;

	protected float currentLifeTime;

	protected bool isDestroy;

	protected float _timeCount;

	public BaseRocket.onDestroyDelegate OnDestroyRocket;

	public delegate void onDestroyDelegate(BaseRocket bullet);
}
