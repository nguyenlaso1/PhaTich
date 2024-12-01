// @sonhg: class: WhaleBullet
using System;
using UnityEngine;

public class WhaleBullet : BaseBullet
{
	protected override void InitBullet()
	{
		this.bulletSprite.transform.localRotation = Quaternion.FromToRotation(Vector3.down, this.shootDirection);
	}
}
