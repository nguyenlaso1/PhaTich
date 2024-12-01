// @sonhg: class: ChooseGameRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

public class ChooseGameRequest : global::BaseRequest
{
	private ChooseGameRequest(string gameId)
	{
		UnityEngine.Debug.Log(gameId);
		Context.currentGameId = gameId;
		this.isShowWaiting = false;
		this._gameId = gameId;
	}

	protected override IRequest Request
	{
		get
		{
			SFSObject sfsobject = new SFSObject();
			sfsobject.PutUtfString("type", this._gameId);
			return new ExtensionRequest("b-choose-game", sfsobject);
		}
	}

	public static void SendMessage(string gameId)
	{
		new ChooseGameRequest(gameId).Send();
	}

	private string _gameId;
}
