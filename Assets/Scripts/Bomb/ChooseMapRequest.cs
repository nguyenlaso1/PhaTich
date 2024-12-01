// @sonhg: class: Bomb.ChooseMapRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class ChooseMapRequest : global::BaseRequest
	{
		public ChooseMapRequest(int id)
		{
			this._id = id;
			this.isShowWaiting = false;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "choose-map");
				sfsobject.PutInt("id", this._id);
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage(int id)
		{
			new ChooseMapRequest(id).Send();
		}

		private int _id;
	}
}
