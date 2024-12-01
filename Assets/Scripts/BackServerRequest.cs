// @sonhg: class: BackServerRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class BackServerRequest : global::BaseRequest
{
	private BackServerRequest(int backType, int sfsId, string data)
	{
		this.isShowWaiting = false;
		this._backType = backType;
		this._sfsId = sfsId;
		this._data = data;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutInt("type", this._backType);
			sfsobject.PutInt("sfsId", this._sfsId);
			sfsobject.PutUtfString("data", this._data);
			return new ExtensionRequest("b-back-server", sfsobject);
		}
	}

	public static void SendMessage(int backType, int sfsId, string data)
	{
		new BackServerRequest(backType, sfsId, data).Send();
	}

	private int _backType;

	private int _sfsId;

	private string _data;
}
