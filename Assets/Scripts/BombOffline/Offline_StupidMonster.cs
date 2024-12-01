// @sonhg: class: BombOffline.Offline_StupidMonster
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombOffline
{
	public class Offline_StupidMonster : Offline_BaseCharactersController
	{
		protected virtual void Start()
		{
			base.transform.ChangeColorRecursive(this.monsterColor);
			base.CurrentDirection = this.defaultDirect;
			base.StartCoroutine(this.RandomChangeDirection());
		}

		protected virtual void FixedUpdate()
		{
			if (this.canMove)
			{
				this.Move(base.CurrentDirection, false);
			}
		}

		protected IEnumerator RandomChangeDirection()
		{
			for (;;)
			{
				float randomTime = (float)UnityEngine.Random.Range(5, 15);
				yield return new WaitForSeconds(randomTime);
				this.ChangeDirection();
			}
			yield break;
		}

		protected void ChangeDirection()
		{
			List<MoveDirection> list = new List<MoveDirection>();
			List<MoveDirection> list2 = new List<MoveDirection>();
			foreach (Offline_DrawRayCast offline_DrawRayCast in this.rayCasts)
			{
				if (!offline_DrawRayCast.hit && !list.Contains(offline_DrawRayCast.direction))
				{
					list.Add(offline_DrawRayCast.direction);
					if (offline_DrawRayCast.direction != base.CurrentDirection && offline_DrawRayCast.direction != base.CurrentDirection.Reverse())
					{
						list2.Add(offline_DrawRayCast.direction);
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			int index2 = UnityEngine.Random.Range(0, list2.Count);
			if (list2.Count > 0)
			{
				base.CurrentDirection = list2[index2];
			}
			else
			{
				base.CurrentDirection = list[index];
			}
		}

		protected override void Notify(GameObject collision)
		{
			this.ChangeDirection();
		}

		public bool IsAtPosition(int x, int y)
		{
			float num = 0.5f;
			float num2 = (float)x - num;
			float num3 = (float)x + num;
			float num4 = (float)y - num;
			float num5 = (float)y + num;
			return num2 <= base.transform.position.x && base.transform.position.x <= num3 && num4 <= base.transform.position.y && base.transform.position.y <= num5;
		}

		[Header("StupidBot")]
		public Offline_BaseCharactersController target;

		public Offline_MapController gameController;

		[SerializeField]
		protected bool canMove = true;

		protected MoveDirection defaultDirect = MoveDirection.RIGHT;

		[SerializeField]
		public Color monsterColor;
	}
}
