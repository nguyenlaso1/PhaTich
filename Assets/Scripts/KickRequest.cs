// @sonhg: class: KickRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

internal class KickRequest : global::BaseRequest
{
	private KickRequest(int sfsId)
	{
		this.isShowWaiting = false;
		this._sfsId = sfsId;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("sfsId", this._sfsId);
			return new ExtensionRequest("b-kick", sfsobject);
		}
	}

	public static void SendMessage(int sfsId)
	{
		new KickRequest(sfsId).Send();
	}

	private int _sfsId;
}
