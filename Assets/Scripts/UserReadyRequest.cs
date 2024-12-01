// @sonhg: class: UserReadyRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class UserReadyRequest : global::BaseRequest
{
	private UserReadyRequest()
	{
		this.isShowWaiting = false;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("c-ready", new SFSObject());
		}
	}

	public static void SendMessage()
	{
		new UserReadyRequest().Send();
	}
}
