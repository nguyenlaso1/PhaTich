// @sonhg: class: Bomb.MoveRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class MoveRequest : global::BaseRequest
	{
		public MoveRequest(float x, float y, MoveDirection direction)
		{
			this._x = x;
			this._y = y;
			this._direction = (int)direction;
			this.isShowWaiting = false;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "move");
				sfsobject.PutFloat("x", this._x);
				sfsobject.PutFloat("y", this._y);
				sfsobject.PutInt("direction", this._direction);
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage(float x, float y, MoveDirection direction)
		{
			new MoveRequest(x, y, direction).Send();
		}

		private float _x;

		private float _y;

		private int _direction;
	}
}
