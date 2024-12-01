// @sonhg: class: MergeFBRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class MergeFBRequest : global::BaseRequest
{
	private MergeFBRequest()
	{
	}

	protected override IRequest Request
	{
		get
		{
			ISFSObject isfsobject = new SFSObject();
			isfsobject.PutUtfString("accessToken", Context.fbKey);
			isfsobject.PutInt("accountType", 2);
			return new ExtensionRequest("b-um", isfsobject);
		}
	}

	public static void SendMessage()
	{
		new MergeFBRequest().Send();
	}
}
