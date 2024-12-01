// @sonhg: class: Bomb.ChoosePositionRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class ChoosePositionRequest : global::BaseRequest
	{
		public ChoosePositionRequest(int position)
		{
			this.isShowWaiting = false;
			this.position = position;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "choose-position");
				sfsobject.PutInt("position", this.position);
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage(int position)
		{
			new ChoosePositionRequest(position).Send();
		}

		private int position;
	}
}
