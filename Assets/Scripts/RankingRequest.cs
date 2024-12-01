// @sonhg: class: RankingRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class RankingRequest : global::BaseRequest
{
	private RankingRequest(SFSObject req)
	{
		this.isShowWaiting = true;
		this._req = req;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("ranking", this._req);
		}
	}

	public static void SendMessage(SFSObject req)
	{
		new RankingRequest(req).Send();
	}

	private SFSObject _req;
}
