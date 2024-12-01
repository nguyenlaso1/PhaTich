// @sonhg: class: Bomb.HotEventRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class HotEventRequest : global::BaseRequest
	{
		private HotEventRequest(SFSObject req)
		{
			this.isShowWaiting = true;
			this._req = req;
		}

		protected override IRequest Request
		{
			get
			{
				return new ExtensionRequest("hot-event", this._req);
			}
		}

		public static void SendMessage(SFSObject req)
		{
			new HotEventRequest(req).Send();
		}

		private SFSObject _req;
	}
}
