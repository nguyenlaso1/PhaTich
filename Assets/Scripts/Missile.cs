// @sonhg: class: Missile
using System;
using BombOffline;
using UnityEngine;

public class Missile : BaseBullet
{
	protected override void InitBullet()
	{
		this.targetPosition = this.target.position;
		this.shootDirection = this.targetPosition - base.transform.position;
		base.transform.localRotation = Quaternion.FromToRotation(Vector3.down, this.shootDirection);
		this._localRotate = new Vector3(0f, 0f, 0f);
	}

	protected override void MoveBullet()
	{
		base.transform.Translate(Vector3.down * this.bulletSpeed * Time.deltaTime);
		this._localDirection = base.transform.TransformDirection(Vector3.down);
		this._angle = this.SignedAngleBetween(this.target.position - base.transform.position, this._localDirection, new Vector3(0f, 0f, 1f));
		if (this._angle > 0f)
		{
			this._localRotate.z = -1f;
		}
		else
		{
			this._localRotate.z = 1f;
		}
		base.transform.Rotate(this.rotationSpeed * this._localRotate * Time.deltaTime);
	}

	protected override void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			Offline_BaseCharactersController component = collider.gameObject.GetComponent<Offline_BaseCharactersController>();
			component.GetHit(null);
			this.DestroyBullet();
		}
	}

	protected override void DestroyBullet()
	{
		ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.explosionParticle);
		particleSystem.transform.position = base.transform.position;
		particleSystem.Play();
		UnityEngine.Object.Destroy(particleSystem.gameObject, 1f);
		this.isDestroy = true;
		UnityEngine.Object.Destroy(base.GetComponent<BoxCollider2D>());
		UnityEngine.Object.Destroy(base.gameObject);
		if (this.OnDestroyBullet != null)
		{
			this.OnDestroyBullet(this, null);
		}
	}

	private float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
	{
		float num = Vector3.Angle(a, b);
		float num2 = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));
		return num * num2;
	}

	[Header("Missile")]
	[SerializeField]
	private ParticleSystem explosionParticle;

	public float rotationSpeed = 10f;

	private Quaternion rotation;

	private Vector3 _localDirection;

	private float _angle;

	private Vector3 _localRotate;
}
