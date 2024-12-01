// @sonhg: class: Offline_TopupController
using System;
using System.Collections.Generic;
using BombOffline;
using OnePF;
using UnityEngine;

public class Offline_TopupController : MonoBehaviour
{
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

	private void OnPurchaseIab(string sku, Context.OnDeletegateNone OnSuccess = null)
	{
		Offline_Context.Waitting.ShowWaiting();
		UnityEngine.Debug.Log("TopupController.OnPurchaseIab" + sku);
		if (OnSuccess != null)
		{
			this.OnSuccess = OnSuccess;
		}
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
				UnityEngine.Debug.LogError("-----OnQueryInventorySucceeded +++++++++++++++++ REQUEST" + allPurchases[i].Sku);
				this.OnPurchaseSucceded(allPurchases[i]);
			}
		}
	}

	private void OnQueryInventoryFailed(string error)
	{
		UnityEngine.Debug.Log("Query inventory failed: " + error);
	}

	private void OnPurchaseSucceded(Purchase purchase)
	{
		Offline_Context.Waitting.HideWaiting();
		try
		{
			if (purchase.Sku.Equals("removeads"))
			{
				UnityEngine.Debug.LogError("-----OnPurchaseSucceded +++++++++++++++++ REQUEST");
			}
			else
			{
				InappItem inappItem = ResourcesManager.inappList.Find((InappItem x) => x.Id.Equals(purchase.Sku));
				Offline_ShopController.SetCharacterCoint(Offline_ShopController.GetCharacterCoin() + inappItem.Chip);
				if (base.transform.GetComponent<Offline_ShopController>() != null)
				{
					base.transform.GetComponent<Offline_ShopController>().UpdateCoin();
				}
				Offline_Context.Confirm.AddMessage("Buy Success !", string.Empty, string.Empty);
				Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INAPP, Analystics.L_SUCCESS, (long)inappItem.Chip);
				OpenIAB.consumeProduct(purchase);
			}
			if (this.OnSuccess != null)
			{
				this.OnSuccess();
			}
		}
		catch (Exception ex)
		{
		}
	}

	private void OnPurchaseFailed(int errorCode, string error)
	{
		Offline_Context.Waitting.HideWaiting();
		Offline_Context.Confirm.AddMessage("Purchase Item Error", string.Empty, string.Empty);
		UnityEngine.Debug.Log("Purchase failed: " + error);
		Context.googleAnalytics.LogEvent(Analystics.C_MAIN_MENU, Analystics.A_INAPP, Analystics.L_FAILD + " - " + error, 0L);
	}

	private void OnConsumePurchaseSucceeded(Purchase purchase)
	{
		UnityEngine.Debug.Log("Consume purchase succeded: " + purchase.ToString());
	}

	private void OnConsumePurchaseFailed(string error)
	{
		UnityEngine.Debug.Log("Consume purchase failed: " + error);
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
	}

	public Offline_TopupBox TopupBox;

	public Context.OnDeletegateNone OnSuccess;
}
