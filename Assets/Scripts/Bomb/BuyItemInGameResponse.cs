// @sonhg: class: Bomb.BuyItemInGameResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace Bomb
{
	internal class BuyItemInGameResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			int @int = sfsobject.GetInt("id");
			SFSObject sfsobject2 = (SFSObject)sfsobject.GetSFSObject("data");
			foreach (string value in sfsobject2.GetKeys())
			{
				ItemType type = (ItemType)((int)Enum.Parse(typeof(ItemType), value));
				((BombGameScene)this.baseScene).GameController.PickUpItem(@int, type, 1);
			}
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new BuyItemInGameResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}
	}
}
