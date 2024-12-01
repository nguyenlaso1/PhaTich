// @sonhg: class: InviteListRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Match;
using Sfs2X.Requests;

public class InviteListRequest : global::BaseRequest
{
	protected override IRequest Request
	{
		get
		{
			MatchExpression expr = new MatchExpression(UserProperties.IS_IN_ANY_ROOM, BoolMatch.EQUALS, true);
			return new FindUsersRequest(expr);
		}
	}

	public static void SendMessage()
	{
		new InviteListRequest().Send();
	}

	private SFSObject _req;
}
