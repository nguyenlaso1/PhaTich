// @sonhg: class: Bomb.PickItemResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

namespace Bomb
{
	public class PickItemResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			int @int = sfsobject.GetInt("id");
			int int2 = sfsobject.GetInt("x");
			int int3 = sfsobject.GetInt("y");
			SFSObject sfsobject2 = (SFSObject)sfsobject.GetSFSObject("data");
			((BombGameScene)this.baseScene).MapController.DestroyItems(int2, int3);
			foreach (string text in sfsobject2.GetKeys())
			{
				ItemType type = (ItemType)((int)Enum.Parse(typeof(ItemType), text));
				int int4 = sfsobject2.GetInt(text);
				((BombGameScene)this.baseScene).GameController.PickUpItem(@int, type, int4);
			}
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new PickItemResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}
	}
}
