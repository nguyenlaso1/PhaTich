// @sonhg: class: FlashBullet
using System;
using BombOffline;
using UnityEngine;

public class FlashBullet : BaseBullet
{
	protected override void OnTriggerEnter2D(Collider2D collider)
	{
		base.OnTriggerEnter2D(collider);
		if (collider.gameObject.CompareTag("Bomb") && this.OnHitBomb != null)
		{
			this.OnHitBomb(this, collider.transform);
		}
	}

	[HideInInspector]
	public Offline_GameController board;
}
