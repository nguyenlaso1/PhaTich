// @sonhg: class: BombOffline.MadFlower
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class MadFlower : Kabuton
	{
		protected override void ProcessBomb()
		{
			if (this.canMove)
			{
				base.StartCoroutine(this.EatBomb());
			}
			base.CurrentDirection = MoveDirection.STAND;
		}

		private IEnumerator EatBomb()
		{
			this.canAct = false;
			BombModel bomb = null;
			Vector3 EatPosition = Vector3.zero;
			if (this.collisionObject != null && this.collisionObject.CompareTag("Bomb"))
			{
				EatPosition = this.collisionObject.transform.position;
				bomb = this.board.GetBombModelAt(EatPosition);
				UnityEngine.Object.Destroy(bomb.bomb.GetComponent<Offline_Bomb>());
				this.collisionObject = null;
				bomb.bomb.transform.SetParent(base.transform);
			}
			yield return new WaitForSeconds(1f);
			if (bomb != null && EatPosition != Vector3.zero)
			{
				UnityEngine.Object.Destroy(bomb.bomb);
				this.board.RemoveBomb(EatPosition);
			}
			this.canAct = true;
			yield break;
		}
	}
}
