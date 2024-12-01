// @sonhg: class: WaveBullet
using System;
using System.Collections;
using System.Collections.Generic;
using BombOffline;
using UnityEngine;

public class WaveBullet : BaseBullet
{
	protected override void InitBullet()
	{
		this.cacheX = Mathf.RoundToInt(base.transform.position.x);
		this.cacheY = Mathf.RoundToInt(base.transform.position.y);
		this.directionX = Mathf.RoundToInt(this.shootDirection.normalized.x);
		this.directionY = Mathf.RoundToInt(this.shootDirection.normalized.y);
		this.positionList = new List<Vector3>();
		base.StartCoroutine(this.MoveWave());
	}

	private IEnumerator MoveWave()
	{
		int index = 0;
		Dictionary<string, bool> parameter = new Dictionary<string, bool>();
		Offline_Tiled[,] map = this.board.Scene.MapController.mapTiled;
		parameter.Add("isHitMonster", false);
		while (this.board.ValidateArrayPosition(this.cacheX + index * this.directionX, this.cacheY + index * this.directionY))
		{
			if (map[this.cacheX + index * this.directionX, this.cacheY + index * this.directionY].status == 1)
			{
				break;
			}
			this.GenPositionList(this.cacheX + index * this.directionX, this.cacheY + index * this.directionY);
			this.board.TriggerCustomBomb(this.positionList, parameter, null);
			yield return new WaitForSeconds(0.25f);
			index++;
		}
		this.DestroyBullet();
		yield break;
	}

	protected void GenPositionList(int x, int y)
	{
		this.positionList.Clear();
		this.positionList.Add(new Vector3((float)x, (float)y, 0f));
		if (this.directionX != 0)
		{
			if (this.board.ValidateArrayPosition(x, y - 1))
			{
				this.positionList.Add(new Vector3((float)x, (float)(y - 1), 0f));
			}
			if (this.board.ValidateArrayPosition(x, y + 1))
			{
				this.positionList.Add(new Vector3((float)x, (float)(y + 1), 0f));
			}
		}
		if (this.directionY != 0)
		{
			if (this.board.ValidateArrayPosition(x - 1, y))
			{
				this.positionList.Add(new Vector3((float)(x - 1), (float)y, 0f));
			}
			if (this.board.ValidateArrayPosition(x + 1, y))
			{
				this.positionList.Add(new Vector3((float)(x + 1), (float)y, 0f));
			}
		}
	}

	protected override void MoveBullet()
	{
	}

	[HideInInspector]
	public Offline_GameController board;

	private int cacheX;

	private int cacheY;

	private int directionX;

	private int directionY;

	private List<Vector3> positionList;
}
