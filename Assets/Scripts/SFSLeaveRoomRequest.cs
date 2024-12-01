// @sonhg: class: SFSLeaveRoomRequest
using System;
using Sfs2X.Requests;

public class SFSLeaveRoomRequest : global::BaseRequest
{
	private SFSLeaveRoomRequest()
	{
	}

	protected override IRequest Request
	{
		get
		{
			return new LeaveRoomRequest();
		}
	}

	public static void SendMessage()
	{
		new SFSLeaveRoomRequest().Send();
	}
}
