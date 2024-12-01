// @sonhg: class: InappRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class InappRequest : global::BaseRequest
{
	private InappRequest(SFSObject req)
	{
		this._req = req;
	}

	protected override IRequest Request
	{
		get
		{
			if (this._req == null)
			{
				this._req = new SFSObject();
				this._req.PutInt("task", 2);
			}
			return new ExtensionRequest("b-topup", this._req);
		}
	}

	public static void SendMessage(SFSObject req = null)
	{
		new InappRequest(req).Send();
	}

	private SFSObject _req;
}
