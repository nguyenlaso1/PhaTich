// @sonhg: class: BombOffline.FreakyEgg
using System;
using UnityEngine;

namespace BombOffline
{
	public class FreakyEgg : Kabuton
	{
		protected override void ProcessBomb()
		{
			if (this.collisionObject != null && this.collisionObject.CompareTag("Bomb"))
			{
				BombModel bomb = this.collisionObject.GetComponent<Offline_Bomb>().bomb;
				this.board.RemoveBomb(bomb.position);
				UnityEngine.Object.Destroy(bomb.bomb);
				this.board.player.DegreeBomb();
				this.board.Scene.ParticlesController.PlayPickupParticle(base.transform);
			}
		}
	}
}
