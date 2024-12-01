// @sonhg: class: TopupController
using System;
using System.Collections.Generic;
using OnePF;
using Sfs2X.Entities.Data;
using UnityEngine;

public class TopupController : MonoBehaviour
{
	public TopupBox TopupBox
	{
		get
		{
			if (this._topupBox == null)
			{
				this._topupBox = base.GetComponent<TopupBox>();
			}
			return this._topupBox;
		}
	}

	private void Awake()
	{
		OpenIABEventManager.billingSupportedEvent += this.OnBillingSupported;
		OpenIABEventManager.billingNotSupportedEvent += this.OnBillingNotSupported;
		OpenIABEventManager.queryInventorySucceededEvent += this.OnQueryInventorySucceeded;
		OpenIABEventManager.queryInventoryFailedEvent += this.OnQueryInventoryFailed;
		OpenIABEventManager.purchaseSucceededEvent += this.OnPurchaseSucceded;
		OpenIABEventManager.purchaseFailedEvent += this.OnPurchaseFailed;
		OpenIABEventManager.consumePurchaseSucceededEvent += this.OnConsumePurchaseSucceeded;
		OpenIABEventManager.consumePurchaseFailedEvent += this.OnConsumePurchaseFailed;
		OpenIABEventManager.transactionRestoredEvent += this.OnTransactionRestored;
		OpenIABEventManager.restoreSucceededEvent += this.OnRestoreSucceeded;
		OpenIABEventManager.restoreFailedEvent += this.OnRestoreFailed;
		this.TopupBox.OnPurchaseIab += this.OnPurchaseIab;
	}

	private void OnPurchaseIab(string sku)
	{
		UnityEngine.Debug.Log("TopupController.OnPurchaseIab" + sku);
		OpenIAB.purchaseProduct(sku, string.Empty);
	}

	private void Start()
	{
		OpenIAB.init(new Options
		{
			checkInventory = false,
			verifyMode = OptionsVerifyMode.VERIFY_EVERYTHING,
			storeKeys = 
			{
				{
					Context.GameInfo.StoreName,
					Config.inappAndroidKeyHash
				}
			}
		});
	}

	private void OnDestroy()
	{
		OpenIABEventManager.billingSupportedEvent -= this.OnBillingSupported;
		OpenIABEventManager.billingNotSupportedEvent -= this.OnBillingNotSupported;
		OpenIABEventManager.queryInventorySucceededEvent -= this.OnQueryInventorySucceeded;
		OpenIABEventManager.queryInventoryFailedEvent -= this.OnQueryInventoryFailed;
		OpenIABEventManager.purchaseSucceededEvent -= this.OnPurchaseSucceded;
		OpenIABEventManager.purchaseFailedEvent -= this.OnPurchaseFailed;
		OpenIABEventManager.consumePurchaseSucceededEvent -= this.OnConsumePurchaseSucceeded;
		OpenIABEventManager.consumePurchaseFailedEvent -= this.OnConsumePurchaseFailed;
		OpenIABEventManager.transactionRestoredEvent -= this.OnTransactionRestored;
		OpenIABEventManager.restoreSucceededEvent -= this.OnRestoreSucceeded;
		OpenIABEventManager.restoreFailedEvent -= this.OnRestoreFailed;
	}

	private void OnBillingSupported()
	{
		OpenIAB.queryInventory();
		UnityEngine.Debug.Log("Billing is supported");
	}

	private void OnBillingNotSupported(string error)
	{
		UnityEngine.Debug.Log("Billing not supported: " + error);
		Context.GameInfo.Toast(Language.TOPUP_PURCHASED_NOT_SUPPORTED);
	}

	private void OnQueryInventorySucceeded(Inventory inventory)
	{
		List<Purchase> allPurchases = inventory.GetAllPurchases();
		if (Context.GameInfo.DeviceType == 0)
		{
			for (int i = 0; i < allPurchases.Count; i++)
			{
				this.OnPurchaseSucceded(allPurchases[i]);
			}
		}
	}

	private void OnQueryInventoryFailed(string error)
	{
		Context.Tooltip.AddMessage("Query inventory failed: ", 5, string.Empty, string.Empty);
		UnityEngine.Debug.Log("Query inventory failed: " + error);
	}

	private void OnPurchaseSucceded(Purchase purchase)
	{
		UnityEngine.Debug.Log("OnPurchaseSucceded");
		try
		{
			if (SmartFoxConnection.IsConnected)
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutInt("task", 4);
				sfsobject.PutInt("tu-type", Context.GameInfo.DeviceType);
				sfsobject.PutUtfString("tu-dataJson", purchase.OriginalJson);
				sfsobject.PutUtfString("tu-signature", purchase.Signature);
				sfsobject.PutUtfString("tu-packageName", Config.package_name);
				sfsobject.PutUtfString("tu-orderId", purchase.OrderId);
				sfsobject.PutUtfString("tu-productId", purchase.Sku);
				UnityEngine.Debug.Log("dump---------" + sfsobject.GetDump());
				InappRequest.SendMessage(sfsobject);
				OpenIAB.consumeProduct(purchase);
			}
		}
		catch (Exception ex)
		{
			Context.Tooltip.AddMessage("OnPurchaseSucceded. exception", 5, string.Empty, string.Empty);
		}
	}

	private void OnPurchaseFailed(int errorCode, string error)
	{
		UnityEngine.Debug.Log("Purchase failed: " + error);
		Context.Waiting.HideWaiting();
	}

	private void OnConsumePurchaseSucceeded(Purchase purchase)
	{
		UnityEngine.Debug.Log("Consume purchase succeded: " + purchase.ToString());
	}

	private void OnConsumePurchaseFailed(string error)
	{
		UnityEngine.Debug.Log("Consume purchase failed: " + error);
		Context.Waiting.HideWaiting();
	}

	private void OnTransactionRestored(string sku)
	{
		UnityEngine.Debug.Log("Transaction restored: " + sku);
	}

	private void OnRestoreSucceeded()
	{
		UnityEngine.Debug.Log("Transactions restored successfully");
	}

	private void OnRestoreFailed(string error)
	{
		UnityEngine.Debug.Log("Transaction restore failed: " + error);
		Context.Waiting.HideWaiting();
	}

	private TopupBox _topupBox;
}
