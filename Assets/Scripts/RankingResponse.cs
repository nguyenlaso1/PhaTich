// @sonhg: class: RankingResponse
using System;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public class RankingResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
		SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
		UnityEngine.Debug.Log("RankingResponse = " + sfsobject.GetDump());
		this._task = sfsobject.GetInt("task");
		this._arrObject = (SFSArray)sfsobject.GetSFSArray("p-data");
	}

	public override void UpdateGUI()
	{
		UnityEngine.Object.Destroy(Context.Waiting.gameObject);
		StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/RankingBox").GetComponent<RankingBox>().ShowRanking(this._task, this._arrObject);
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new RankingResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}

	private SFSArray _arrObject;

	private int _task;
}
