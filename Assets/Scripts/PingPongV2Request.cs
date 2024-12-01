// @sonhg: class: PingPongV2Request
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class PingPongV2Request : global::BaseRequest
{
	private PingPongV2Request()
	{
		this.isShowWaiting = false;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("b-pingv2", new SFSObject());
		}
	}

	public static void SendMessage()
	{
		new PingPongV2Request().Send();
	}
}
