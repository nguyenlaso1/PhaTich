// @sonhg: class: GetUserItemRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class GetUserItemRequest : global::BaseRequest
{
	private GetUserItemRequest(int category)
	{
		this.isShowWaiting = false;
		this._category = category;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("task", 0);
			sfsobject.PutInt("category", this._category);
			return new ExtensionRequest("b-item", sfsobject);
		}
	}

	public static void SendMessage(int category)
	{
		new GetUserItemRequest(category).Send();
	}

	private int _category;
}
