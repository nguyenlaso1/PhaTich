// @sonhg: class: Bomb.DieRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class DieRequest : global::BaseRequest
	{
		public DieRequest(int id, int sfsId = -1)
		{
			this.isShowWaiting = false;
			this._id = id;
			this.sfsId = sfsId;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "die");
				sfsobject.PutInt("id", this._id);
				sfsobject.PutInt("sfsId", this.sfsId);
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage(int id, int sfsId = -1)
		{
			new DieRequest(id, sfsId).Send();
		}

		private int _id;

		private int sfsId;
	}
}
