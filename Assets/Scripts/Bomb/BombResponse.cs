// @sonhg: class: Bomb.BombResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace Bomb
{
	public class BombResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			int @int = sfsobject.GetInt("x");
			int int2 = sfsobject.GetInt("y");
			if (sfsobject.ContainsKey("id"))
			{
				int int3 = sfsobject.GetInt("id");
				((BombGameScene)this.baseScene).GameController.PlaceBomb(int3, (float)@int, (float)int2);
			}
			else
			{
				((BombGameScene)this.baseScene).MapController.RemoveBomb(@int, int2);
			}
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new BombResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}
	}
}
