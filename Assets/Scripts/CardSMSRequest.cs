// @sonhg: class: CardSMSRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class CardSMSRequest : global::BaseRequest
{
	private CardSMSRequest(SFSObject req)
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
				this._req.PutInt("task", 1);
			}
			return new ExtensionRequest("b-topup", this._req);
		}
	}

	public static void SendMessage(SFSObject req = null)
	{
		new CardSMSRequest(req).Send();
	}

	private SFSObject _req;
}
