// @sonhg: class: MagmaBullet
using System;
using UnityEngine;

public class MagmaBullet : BaseBullet
{
	protected override void MoveBullet()
	{
		if (base.MoveTo(this.targetPosition))
		{
			this.DestroyBullet();
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collider)
	{
	}
}
