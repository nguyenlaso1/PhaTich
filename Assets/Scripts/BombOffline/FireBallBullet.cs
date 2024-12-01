// @sonhg: class: BombOffline.FireBallBullet
using System;
using UnityEngine;

namespace BombOffline
{
	public class FireBallBullet : BaseBullet
	{
		protected override void MoveBullet()
		{
			base.MoveBullet();
			this.board.PlaceObstacle(this.poisonTrap, new Vector2((float)Mathf.RoundToInt(base.transform.position.x), (float)Mathf.RoundToInt(base.transform.position.y)), null);
		}

		protected override void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Player"))
			{
				Offline_BaseCharactersController component = collider.gameObject.GetComponent<Offline_BaseCharactersController>();
				component.GetHit(null);
			}
		}

		protected override void DestroyBullet()
		{
			this.isDestroy = true;
			UnityEngine.Object.Destroy(base.GetComponent<BoxCollider2D>());
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public Offline_GameController board;

		[SerializeField]
		protected PoisonTrap poisonTrap;
	}
}
