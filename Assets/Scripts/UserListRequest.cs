// @sonhg: class: UserListRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class UserListRequest : global::BaseRequest
{
	private UserListRequest(SFSObject req)
	{
		this.isShowWaiting = true;
		this._req = req;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("b-u-list", this._req);
		}
	}

	public static void SendMessage(SFSObject req)
	{
		new UserListRequest(req).Send();
	}

	private SFSObject _req;
}
