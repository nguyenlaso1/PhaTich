// @sonhg: class: UpdateUserInfoRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class UpdateUserInfoRequest : global::BaseRequest
{
	private UpdateUserInfoRequest(SFSObject req)
	{
		this.isShowWaiting = false;
		this._req = req;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("b-up", this._req);
		}
	}

	public static void SendMessage(SFSObject req)
	{
		new UpdateUserInfoRequest(req).Send();
	}

	private SFSObject _req;
}
