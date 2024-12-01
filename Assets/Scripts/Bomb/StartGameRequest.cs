// @sonhg: class: Bomb.StartGameRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class StartGameRequest : global::BaseRequest
	{
		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "s-startGame");
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage()
		{
			new StartGameRequest().Send();
		}
	}
}
