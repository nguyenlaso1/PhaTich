// @sonhg: class: Bomb.LevelUpResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

namespace Bomb
{
	public class LevelUpResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			GameController gameController = ((BombGameScene)this.baseScene).GameController;
			SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
			int @int = sfsobject.GetInt("sfsId");
			User userByID = MMOUserUtils.GetUserByID(@int);
			int userPosition = MMOUserUtils.GetUserPosition(userByID);
			gameController.waitingPanel.CharacterAvatar[userPosition].ShowLevelUp();
			if (userByID.IsItMe)
			{
				gameController.waitingPanel.flagLevelUp = true;
			}
			gameController.waitingPanel.levelUpPosition = userPosition;
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new LevelUpResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}
	}
}
