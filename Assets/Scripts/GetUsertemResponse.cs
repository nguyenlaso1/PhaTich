// @sonhg: class: GetUsertemResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

public class GetUsertemResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
		SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
		if (sfsobject.ContainsKey("p-data"))
		{
			ISFSArray sfsarray = sfsobject.GetSFSArray("p-data");
			Context.arrUserItemList = sfsarray;
		}
	}

	public override void UpdateGUI()
	{
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new GetUsertemResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}

	private SFSArray _arrObject;
}
