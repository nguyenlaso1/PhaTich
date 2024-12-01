// @sonhg: class: Bomb.CreateRoomRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class CreateRoomRequest : global::BaseRequest
	{
		private CreateRoomRequest(int groupIndex)
		{
			this.isShowWaiting = false;
			this._groupIndex = groupIndex;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutInt("r-gindex", this._groupIndex);
				return new ExtensionRequest("c-create-room", sfsobject);
			}
		}

		public static void SendMessage(int groupIndex)
		{
			new CreateRoomRequest(groupIndex).Send();
		}

		private int _groupIndex;
	}
}
