// @sonhg: class: JellyBullet
using System;
using BombOffline;
using UnityEngine;

public class JellyBullet : BaseBullet
{
	protected override void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			Offline_BaseCharactersController component = collider.gameObject.GetComponent<Offline_BaseCharactersController>();
			component.GetHit(null);
			this.DestroyBullet();
		}
		if (collider.gameObject.CompareTag("BorderWall"))
		{
			this.DestroyBullet();
		}
	}
}
