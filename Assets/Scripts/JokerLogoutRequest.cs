// @sonhg: class: JokerLogoutRequest
using System;
using Sfs2X.Requests;

public class JokerLogoutRequest : global::BaseRequest
{
	private JokerLogoutRequest()
	{
	}

	protected override IRequest Request
	{
		get
		{
			return new LogoutRequest();
		}
	}

	public static void SendMessage()
	{
		new JokerLogoutRequest().Send();
	}
}
