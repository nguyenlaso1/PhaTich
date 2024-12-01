// @sonhg: class: LogToServerRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class LogToServerRequest : global::BaseRequest
{
	private LogToServerRequest(SFSObject req)
	{
		this.isShowWaiting = false;
		this._req = req;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("b-log-server", this._req);
		}
	}

	public static void SendMessage(SFSObject req)
	{
		new LogToServerRequest(req).Send();
	}

	private SFSObject _req;
}
