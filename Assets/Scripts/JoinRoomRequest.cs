// @sonhg: class: JoinRoomRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class JoinRoomRequest : global::BaseRequest
{
	private JoinRoomRequest(int groupIndex, int roomId, int joiType, int position, bool isShowWaiting)
	{
		this._groupIndex = groupIndex;
		this._roomId = roomId;
		this._joinType = joiType;
		this.position = position;
		this.isShowWaiting = isShowWaiting;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			if (this._groupIndex != -1)
			{
				sfsobject.PutInt("r-gindex", this._groupIndex);
			}
			if (this._roomId != -1)
			{
				sfsobject.PutInt("r-id", this._roomId);
			}
			if (this._joinType != -1)
			{
				sfsobject.PutInt("join_type", this._joinType);
			}
			sfsobject.PutInt("position", this.position);
			return new ExtensionRequest("c-join-room", sfsobject);
		}
	}

	public static void SendMessage(int groupIndex, int roomId, int joinType, int position = -1, bool isShowWaiting = false)
	{
		new global::JoinRoomRequest(groupIndex, roomId, joinType, position, isShowWaiting).Send();
	}

	private int _groupIndex;

	private int _roomId;

	private int _joinType;

	private int position;
}
