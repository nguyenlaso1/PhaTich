// @sonhg: class: PlayNowResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public class PlayNowResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
		SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
		bool @bool = sfsobject.GetBool("data");
		UnityEngine.Debug.Log("Play now found? " + @bool);
		if (!@bool)
		{
			Context.Messenger.AddOkMessage(Language.PLAY_NOW_NOT_FOUND, null, null, string.Empty, string.Empty);
		}
		else
		{
			int @int = sfsobject.GetInt("r-id");
			int int2 = sfsobject.GetInt("r-gindex");
			if (Context.SceneName == "BomberMainMenu")
			{
				MainMenuScene mainMenuScene = (MainMenuScene)this.baseScene;
				mainMenuScene.scrollViewController.LoadListGameButton(false);
				mainMenuScene.LoadGame(int2, @int, 4);
			}
		}
	}

	public override void UpdateGUI()
	{
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new PlayNowResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}
}
