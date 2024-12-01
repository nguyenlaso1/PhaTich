// @sonhg: class: LobbyMessageRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class LobbyMessageRequest : global::BaseRequest
{
	private LobbyMessageRequest(string content)
	{
		this.isShowWaiting = false;
		this._content = content;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutUtfString("data", this._content);
			return new ExtensionRequest("lobby-message", sfsobject);
		}
	}

	public static void SendMessage(string content)
	{
		new LobbyMessageRequest(content).Send();
	}

	private SFSObject _req;

	private string _content;
}
