// @sonhg: class: ShareFacebookRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class ShareFacebookRequest : global::BaseRequest
{
	private ShareFacebookRequest(int number)
	{
		this.number = number;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("data", this.number);
			return new ExtensionRequest("share-facebook", sfsobject);
		}
	}

	public static void SendMessage(int number)
	{
		new ShareFacebookRequest(number).Send();
	}

	private int number;
}
