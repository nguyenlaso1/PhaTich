// @sonhg: class: UserListResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public class UserListResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
		SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
		UnityEngine.Debug.Log("UserListResponse = " + sfsobject.GetDump());
		this._task = sfsobject.GetInt("task");
		this._arrObject = (SFSArray)sfsobject.GetSFSArray("p-data");
	}

	public override void UpdateGUI()
	{
		switch (this._task)
		{
		case 0:
			StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/RankingBox").GetComponent<RankingBox>().ShowRanking(this._task, this._arrObject);
			break;
		case 2:
			StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/RankingBox").GetComponent<RankingBox>().ShowRanking(this._task, this._arrObject);
			break;
		}
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new UserListResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}

	private SFSArray _arrObject;

	private int _task;
}
