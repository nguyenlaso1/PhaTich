// @sonhg: class: PublicMessageRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class PublicMessageRequest : global::BaseRequest
{
	private PublicMessageRequest(SFSObject req, string content)
	{
		this.isShowWaiting = false;
		this._req = req;
		this._content = content;
	}

	protected override IRequest Request
	{
		get
		{
			return new Sfs2X.Requests.PublicMessageRequest(this._content, this._req);
		}
	}

	public static void SendMessage(SFSObject req, string content)
	{
		new global::PublicMessageRequest(req, content).Send();
	}

	public static void SendMessage(string content)
	{
		new global::PublicMessageRequest(null, content).Send();
	}

	private SFSObject _req;

	private string _content;
}
