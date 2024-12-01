// @sonhg: class: ThornBullet
using System;
using UnityEngine;

public class ThornBullet : BaseBullet
{
	protected override void InitBullet()
	{
		base.InitBullet();
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			this._localRotate = new Vector3(0f, 0f, 1f);
		}
		else
		{
			this._localRotate = new Vector3(0f, 0f, -1f);
		}
	}

	protected override void MoveBullet()
	{
		base.MoveBullet();
		this.bulletSprite.transform.Rotate(this.rotationSpeed * this._localRotate * Time.deltaTime);
	}

	public float rotationSpeed = 100f;

	private Vector3 _localRotate;
}
