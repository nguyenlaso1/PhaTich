// @sonhg: class: ItemRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class ItemRequest : global::BaseRequest
{
	private ItemRequest(SFSObject req)
	{
		this.req = req;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("b-item", this.req);
		}
	}

	public static void SendMessage(SFSObject req)
	{
		new ItemRequest(req).Send();
	}

	private SFSObject req;
}
