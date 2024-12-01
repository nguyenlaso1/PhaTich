// @sonhg: class: Bomb.DieResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

namespace Bomb
{
	public class DieResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			GameController gameController = ((BombGameScene)this.baseScene).GameController;
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			int @int = sfsobject.GetInt("id");
			User userByID = MMOUserUtils.GetUserByID(@int);
			int userPosition = MMOUserUtils.GetUserPosition(userByID);
			if (userByID.IsItMe)
			{
				((BombGameScene)this.baseScene).DisableQuickBuy();
			}
			gameController.SetPlayerDead(@int);
			gameController.waitingPanel.CharacterAvatar[userPosition].GraySubAvatar();
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new DieResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}
	}
}
