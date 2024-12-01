// @sonhg: class: PingResponse
using System;
using Sfs2X.Core;

public class PingResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
	}

	public override void UpdateGUI()
	{
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new PingResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(false);
	}
}
