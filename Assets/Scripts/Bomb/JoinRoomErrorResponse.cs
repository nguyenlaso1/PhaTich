// @sonhg: class: Bomb.JoinRoomErrorResponse
using System;
using Sfs2X.Core;
using UnityEngine;

namespace Bomb
{
	public class JoinRoomErrorResponse : BaseResponse
	{
		public override void UpdateBusiness()
		{
			foreach (object obj in this.evt.Params.Keys)
			{
				string text = (string)obj;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					" key: ",
					text,
					" |value: ",
					this.evt.Params[text]
				}));
			}
			string message = (string)this.evt.Params["errorMessage"];
			short num = (short)this.evt.Params["errorCode"];
			short num2 = num;
			if (num2 != 28)
			{
				if (num2 != 41)
				{
					Context.Messenger.AddOkMessage(message, null, null, string.Empty, string.Empty);
					((BomberMainMenu)this.baseScene).ShowLoading(false, null);
				}
				else if (((BomberMainMenu)this.baseScene).count < 5)
				{
					((BomberMainMenu)this.baseScene).ReJoin();
				}
				else
				{
					((BomberMainMenu)this.baseScene).ShowPopupConfirmJoinRoom();
					((BomberMainMenu)this.baseScene).ShowLoading(false, null);
				}
			}
			else
			{
				Context.Messenger.AddOkMessage(message, null, null, string.Empty, string.Empty);
				((BomberMainMenu)this.baseScene).ShowLoading(false, null);
			}
		}

		public override void UpdateGUI()
		{
		}

		public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
		{
			BaseResponse baseResponse = new JoinRoomErrorResponse();
			baseResponse.SetParams(baseEvent, gameScene);
			baseResponse.Run(true);
		}

		private int count;
	}
}
