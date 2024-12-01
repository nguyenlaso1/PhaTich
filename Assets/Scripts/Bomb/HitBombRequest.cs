// @sonhg: class: Bomb.HitBombRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class HitBombRequest : global::BaseRequest
	{
		public HitBombRequest(int old_x, int old_y, int new_x, int new_y)
		{
			this.old_x = old_x;
			this.old_y = old_y;
			this.new_x = new_x;
			this.new_y = new_y;
			this.isShowWaiting = false;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "hit-bomb");
				SFSObject sfsobject2 = new SFSObject();
				sfsobject2.PutInt("x", this.old_x);
				sfsobject2.PutInt("y", this.old_y);
				SFSObject sfsobject3 = new SFSObject();
				sfsobject3.PutInt("x", this.new_x);
				sfsobject3.PutInt("y", this.new_y);
				sfsobject.PutSFSObject("old", sfsobject3);
				sfsobject.PutSFSObject("new", sfsobject2);
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage(int old_x, int old_y, int new_x, int new_y)
		{
			new KickBombRequest(old_x, old_y, new_x, new_y).Send();
		}

		private int old_x;

		private int old_y;

		private int new_x;

		private int new_y;
	}
}
