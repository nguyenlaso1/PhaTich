// @sonhg: class: Bomb.KickBombResponse
using System;
using Sfs2X.Core;

namespace Bomb
{
	public class KickBombResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			throw new NotImplementedException();
		}

		public override void UpdateGUI()
		{
			throw new NotImplementedException();
		}

		public static void RunMessage(BaseEvent evt, BaseScene gameScene)
		{
			BaseResponse baseResponse = new KickBombResponse();
			baseResponse.SetParams(evt, gameScene);
			baseResponse.Run(true);
		}
	}
}
