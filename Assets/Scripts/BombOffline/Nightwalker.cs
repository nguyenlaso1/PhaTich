// @sonhg: class: BombOffline.Nightwalker
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace BombOffline
{
	public class Nightwalker : QueenBee
	{
		protected override void SpawnMonster()
		{
		}

		protected override void Dash()
		{
			if (base.CanDash())
			{
				this.dashCooldown = this.dashTime;
				Vector3 dircetionVector = base.CurrentDirection.GetDircetionVector();
				int num = Mathf.RoundToInt(dircetionVector.x);
				int num2 = Mathf.RoundToInt(dircetionVector.y);
				int num3 = 1;
				while (this.board.ValidateArrayPosition(base.currentX + num * num3, base.currentY + num2 * num3) && this.board.Scene.MapController.mapTiled[base.currentX + num * num3, base.currentY + num2 * num3].IsEmptyTiled())
				{
					num3++;
				}
				num3--;
				Vector3[] path = new Vector3[]
				{
					base.transform.position,
					new Vector3((float)base.currentX + (float)num3 * dircetionVector.x, (float)base.currentY + (float)num3 * dircetionVector.y, 0f)
				};
				this.canAct = false;
				base.transform.DOPath(path, 0.2f * (float)num3, PathType.CatmullRom, PathMode.TopDown2D, 10, null).OnComplete(delegate
				{
					this.SnapGrid();
					this.canAct = true;
				});
			}
		}
	}
}
