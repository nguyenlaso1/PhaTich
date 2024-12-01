// @sonhg: class: PlayNowRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class PlayNowRequest : global::BaseRequest
{
	private PlayNowRequest()
	{
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject parameters = new SFSObject();
			return new ExtensionRequest("bp-playnow", parameters);
		}
	}

	public static void SendMessage()
	{
		new PlayNowRequest().Send();
	}
}
