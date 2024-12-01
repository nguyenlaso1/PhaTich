// @sonhg: class: ExceptionResponse
using System;
using Sfs2X.Core;

public class ExceptionResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
	}

	public override void UpdateGUI()
	{
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new ExceptionResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}
}
