// @sonhg: class: Bomb.BombExplosionResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

namespace Bomb
{
	internal class BombExplosionResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			ISFSArray sfsarray = sfsobject.GetSFSArray("bombs");
			foreach (object obj in sfsarray)
			{
				SFSObject sfsobject2 = (SFSObject)obj;
				int @int = sfsobject2.GetInt("x");
				int int2 = sfsobject2.GetInt("y");
				((BombGameScene)this.baseScene).MapController.ExplodeBomb(new Vector3((float)@int, (float)int2, 0f));
			}
			ISFSArray sfsarray2 = sfsobject.GetSFSArray("destroys");
			foreach (object obj2 in sfsarray2)
			{
				SFSObject sfsobject3 = (SFSObject)obj2;
				int int3 = sfsobject3.GetInt("x");
				int int4 = sfsobject3.GetInt("y");
				if (!((BombGameScene)this.baseScene).MapController.DestroyBricks(int3, int4))
				{
					((BombGameScene)this.baseScene).MapController.DestroyItems(int3, int4);
				}
			}
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new BombExplosionResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}
	}
}
