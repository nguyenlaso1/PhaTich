// @sonhg: class: InviteRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class InviteRequest : global::BaseRequest
{
	private InviteRequest(SFSObject req)
	{
		this.isShowWaiting = false;
		this._req = req;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("b-inv", this._req);
		}
	}

	public static void SendMessage(SFSObject req)
	{
		new InviteRequest(req).Send();
	}

	private SFSObject _req;
}
