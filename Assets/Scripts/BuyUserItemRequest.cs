// @sonhg: class: BuyUserItemRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class BuyUserItemRequest : global::BaseRequest
{
	private BuyUserItemRequest(int gameItemId)
	{
		this.isShowWaiting = false;
		this._gameItemId = gameItemId;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("task", 1);
			sfsobject.PutInt("i-id", this._gameItemId);
			return new ExtensionRequest("b-item", sfsobject);
		}
	}

	public static void SendMessage(int gameItemId)
	{
		new BuyUserItemRequest(gameItemId).Send();
	}

	private int _gameItemId;
}
