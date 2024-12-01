// @sonhg: class: JokerLoginRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class JokerLoginRequest : global::BaseRequest
{
	private JokerLoginRequest(string key, int accountType, string pass)
	{
		this._key = key;
		this._accountType = accountType;
		this._pass = pass;
	}

	protected override IRequest Request
	{
		get
		{
			ISFSObject isfsobject = new SFSObject();
			isfsobject.PutInt("accountType", this._accountType);
			isfsobject.PutUtfString("cpName", Context.GameInfo.CP);
			isfsobject.PutInt("language", Context.GameInfo.DeviceLanguageId);
			isfsobject.PutInt("version", Context.GameInfo.VersionCode);
			isfsobject.PutUtfString("deviceId", Context.GameInfo.DeviceId);
			isfsobject.PutInt("deviceType", Context.GameInfo.DeviceType);
			isfsobject.PutUtfString("deviceToken", Context.GameInfo.DeviceToken);
			isfsobject.PutUtfString("packageName", Context.GameInfo.PackageName);
			isfsobject.PutUtfString("displayname", Context.GameInfo.DisplayName);
			isfsobject.PutBool("isvpconfigloaded", Joker2XConfigUtils.IsLoaded());
			return new LoginRequest(this._key, this._pass, Config.zoneName, isfsobject);
		}
	}

	public static void SendMessage(string key, int accountType, string pass)
	{
		new JokerLoginRequest(key, accountType, pass).Send();
	}

	private string _key;

	private int _accountType;

	private string _pass;
}
