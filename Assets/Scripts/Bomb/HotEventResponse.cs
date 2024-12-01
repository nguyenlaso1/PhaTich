// @sonhg: class: Bomb.HotEventResponse
using System;
using Sfs2X.Core;

namespace Bomb
{
	public class HotEventResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent evt, BaseScene gameScene)
		{
			BaseResponse baseResponse = new HotEventResponse();
			baseResponse.SetParams(evt, gameScene);
			baseResponse.Run(true);
		}
	}
}
