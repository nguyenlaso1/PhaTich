// @sonhg: class: Bomb.HitBombResponse
using System;
using Sfs2X.Core;

namespace Bomb
{
	public class HitBombResponse : BaseResponse
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
			BaseResponse baseResponse = new HitBombResponse();
			baseResponse.SetParams(evt, gameScene);
			baseResponse.Run(true);
		}
	}
}
