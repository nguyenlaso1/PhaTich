// @sonhg: class: JokerLeaveRoomResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public class JokerLeaveRoomResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
	}

	public override void UpdateGUI()
	{
		SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
		UnityEngine.Debug.Log(sfsobject.GetDump());
		int @int = sfsobject.GetInt("endGameCards");
		string utfString = sfsobject.GetUtfString("data");
		UnityEngine.Debug.Log("joker leave room: " + @int);
		switch (@int)
		{
		case 0:
			((GameScene)this.baseScene).ForceShowMainmenu(utfString);
			break;
		case 1:
			Context.ShowMainmenuScreen();
			break;
		case 2:
			((GameScene)this.baseScene).ForceShowMainmenu(utfString);
			break;
		case 3:
			((GameScene)this.baseScene).ForceShowMainmenuAndTopup(utfString);
			break;
		case 4:
			((GameScene)this.baseScene).ForceShowMainmenu(utfString);
			break;
		default:
			((GameScene)this.baseScene).ForceShowMainmenu(utfString);
			break;
		}
	}

	public static void RunMessage(BaseEvent baseEvent, BaseScene gameScene)
	{
		BaseResponse baseResponse = new JokerLeaveRoomResponse();
		baseResponse.SetParams(baseEvent, gameScene);
		baseResponse.Run(true);
	}
}
