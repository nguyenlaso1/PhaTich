// @sonhg: class: LanguageRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class LanguageRequest : global::BaseRequest
{
	private LanguageRequest(SFSObject req)
	{
		this.isShowWaiting = false;
		this._req = req;
	}

	protected override IRequest Request
	{
		get
		{
			return new ExtensionRequest("b-language", this._req);
		}
	}

	public static void SendMessage(int languageId)
	{
		SFSObject sfsobject = new SFSObject();
		sfsobject.PutInt("language", languageId);
		new LanguageRequest(sfsobject).Send();
	}

	private SFSObject _req;
}
