// @sonhg: class: ToxicBomb
using System;
using BombOffline;
using UnityEngine;

public class ToxicBomb : BombScript
{
	private void Start()
	{
		int num = Mathf.RoundToInt(base.transform.position.y);
		this.spriteRender.sortingOrder = 100 - 2 * num;
	}

	public override void DestroyBomb()
	{
		this.Pool(base.transform.position);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected virtual void Pool(Vector3 causePosition)
	{
		Offline_Tiled[,] mapTiled = this.board.Scene.MapController.mapTiled;
		int num = Mathf.RoundToInt(causePosition.x);
		int num2 = Mathf.RoundToInt(causePosition.y);
		this.board.PlaceObstacle(this.poisonTrap, new Vector2((float)num, (float)num2), null);
		this.board.PlaceObstacle(this.poisonTrap, new Vector2((float)(num - 1), (float)num2), null);
		this.board.PlaceObstacle(this.poisonTrap, new Vector2((float)(num + 1), (float)num2), null);
		this.board.PlaceObstacle(this.poisonTrap, new Vector2((float)num, (float)(num2 - 1)), null);
		this.board.PlaceObstacle(this.poisonTrap, new Vector2((float)num, (float)(num2 + 1)), null);
	}

	[SerializeField]
	protected PoisonTrap poisonTrap;
}
