// @sonhg: class: TopupResponse
using System;
using OnePF;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public class TopupResponse : BaseResponse
{
	public void PaserInapp(ISFSArray dataSFSArray)
	{
		UnityEngine.Debug.Log("//////////////////////////////////" + dataSFSArray.GetDump());
		if (ResourcesManager.inappList.Count > 0)
		{
			return;
		}
		ResourcesManager.inappList.Clear();
		foreach (object obj in dataSFSArray)
		{
			SFSObject sfsobject = (SFSObject)obj;
			ResourcesManager.inappList.Add(new InappItem(sfsobject.GetUtfString("id"), sfsobject.GetDouble("pay").ToString(), sfsobject.GetInt("chip"), sfsobject.GetInt("gold")));
		}
	}

	public void PaserSmsCard(ISFSArray dataSFSArray)
	{
		ResourcesManager.smsList.Clear();
		ResourcesManager.cardList.Clear();
		foreach (object obj in dataSFSArray)
		{
			SFSObject sfsobject = (SFSObject)obj;
			int @int = sfsobject.GetInt("id");
			if (@int == -1)
			{
				ResourcesManager.smsList.Add(new SMSItem(sfsobject.GetUtfString("cp"), sfsobject.GetUtfString("cardName"), sfsobject.GetInt("active"), sfsobject.GetUtfString("header")));
			}
			else
			{
				ResourcesManager.cardList.Add(new CardItem(@int, sfsobject.GetUtfString("header"), sfsobject.GetUtfString("cp"), sfsobject.GetUtfString("cardName"), (float)sfsobject.GetDouble("coefficient"), sfsobject.GetInt("zorder"), sfsobject.GetInt("validationType"), sfsobject.GetUtfString("description"), sfsobject.GetUtfString("validationRequire"), sfsobject.GetInt("id")));
			}
		}
	}

	public override void UpdateBusiness()
	{
		SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
		this._task = sfsobject.GetInt("task");
		UnityEngine.Debug.Log(sfsobject.GetDump());
		switch (this._task)
		{
		case 1:
			this.PaserSmsCard(sfsobject.GetSFSArray("data"));
			break;
		case 2:
			this.PaserInapp(sfsobject.GetSFSArray("data"));
			break;
		case 3:
			Context.Messenger.AddOkMessage(Language.TOPUP_PURCHASED_SUCCESSFUL, null, null, string.Empty, string.Empty);
			break;
		case 4:
			Context.Messenger.AddOkMessage(Language.TOPUP_PURCHASED_SUCCESSFUL, null, null, string.Empty, string.Empty);
			UnityEngine.Debug.Log("ADD_INAPP" + sfsobject.GetDump());
			break;
		case 5:
			UnityEngine.Debug.Log("add sms" + sfsobject.GetDump());
			Context.Messenger.AddOkMessage(Language.TOPUP_PURCHASED_SUCCESSFUL, null, null, string.Empty, string.Empty);
			break;
		}
	}

	public override void UpdateGUI()
	{
		TopupBox component = StaticGameObject.GetGameObject("Prefabs/Joker2x/Boxs/TopupBoxUI").GetComponent<TopupBox>();
		component.transform.localPosition = Vector3.zero;
		component.transform.localScale = Vector3.one;
		component.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		component.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		int task = this._task;
		if (task != 1)
		{
			if (task == 2)
			{
				foreach (InappItem inappItem in ResourcesManager.inappList)
				{
					component.AddInapp(inappItem.Chip.ToString(), inappItem.Gold.ToString(), inappItem.Pay, inappItem.Id);
					if (!TopupResponse.isRegistered)
					{
						OpenIAB.mapSku(inappItem.Id, Context.GameInfo.StoreName, inappItem.Id);
					}
				}
				TopupResponse.isRegistered = true;
			}
		}
		else
		{
			foreach (SMSItem sms in ResourcesManager.smsList)
			{
				component.AddSMS(sms);
			}
			foreach (CardItem card in ResourcesManager.cardList)
			{
				component.AddCard(card);
			}
		}
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new TopupResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}

	public static bool isRegistered;

	private int _task;
}
