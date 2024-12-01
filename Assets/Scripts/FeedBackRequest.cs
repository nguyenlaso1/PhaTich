// @sonhg: class: FeedBackRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class FeedBackRequest : global::BaseRequest
{
	private FeedBackRequest(string text)
	{
		this.isShowWaiting = false;
		this._text = text;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutUtfString("data", this._text);
			return new ExtensionRequest("c-feedback", sfsobject);
		}
	}

	public static void SendMessage(string text)
	{
		new FeedBackRequest(text).Send();
	}

	private string _text;
}
