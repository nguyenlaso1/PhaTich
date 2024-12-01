// @plugin: class: OpenIABEventManager
using System;
using OnePF;
using UnityEngine;

public class OpenIABEventManager : MonoBehaviour
{
	public static event Action billingSupportedEvent;

	public static event Action<string> billingNotSupportedEvent;

	public static event Action<Inventory> queryInventorySucceededEvent;

	public static event Action<string> queryInventoryFailedEvent;

	public static event Action<Purchase> purchaseSucceededEvent;

	public static event Action<int, string> purchaseFailedEvent;

	public static event Action<Purchase> consumePurchaseSucceededEvent;

	public static event Action<string> consumePurchaseFailedEvent;

	public static event Action<string> transactionRestoredEvent;

	public static event Action<string> restoreFailedEvent;

	public static event Action restoreSucceededEvent;

	public static OpenIABEventManager instance
	{
		get
		{
			if (OpenIABEventManager._instance == null)
			{
				OpenIABEventManager._instance = UnityEngine.Object.FindObjectOfType<OpenIABEventManager>();
				UnityEngine.Object.DontDestroyOnLoad(OpenIABEventManager._instance.gameObject);
			}
			return OpenIABEventManager._instance;
		}
	}

	private void Awake()
	{
		if (OpenIABEventManager._instance == null)
		{
			OpenIABEventManager._instance = this;
			base.gameObject.name = base.GetType().ToString();
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		else if (this != OpenIABEventManager._instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnMapSkuFailed(string exception)
	{
		UnityEngine.Debug.LogError("SKU mapping failed: " + exception);
	}

	private void OnBillingSupported(string empty)
	{
		if (OpenIABEventManager.billingSupportedEvent != null)
		{
			OpenIABEventManager.billingSupportedEvent();
		}
	}

	private void OnBillingNotSupported(string error)
	{
		if (OpenIABEventManager.billingNotSupportedEvent != null)
		{
			OpenIABEventManager.billingNotSupportedEvent(error);
		}
	}

	private void OnQueryInventorySucceeded(string json)
	{
		if (OpenIABEventManager.queryInventorySucceededEvent != null)
		{
			Inventory obj = new Inventory(json);
			OpenIABEventManager.queryInventorySucceededEvent(obj);
		}
	}

	private void OnQueryInventoryFailed(string error)
	{
		if (OpenIABEventManager.queryInventoryFailedEvent != null)
		{
			OpenIABEventManager.queryInventoryFailedEvent(error);
		}
	}

	private void OnPurchaseSucceeded(string json)
	{
		if (OpenIABEventManager.purchaseSucceededEvent != null)
		{
			OpenIABEventManager.purchaseSucceededEvent(new Purchase(json));
		}
	}

	private void OnPurchaseFailed(string message)
	{
		int arg = -1;
		string arg2 = "Unknown error";
		if (!string.IsNullOrEmpty(message))
		{
			string[] array = message.Split(new char[]
			{
				'|'
			});
			if (array.Length >= 2)
			{
				int.TryParse(array[0], out arg);
				arg2 = array[1];
			}
			else
			{
				arg2 = message;
			}
		}
		if (OpenIABEventManager.purchaseFailedEvent != null)
		{
			OpenIABEventManager.purchaseFailedEvent(arg, arg2);
		}
	}

	private void OnConsumePurchaseSucceeded(string json)
	{
		if (OpenIABEventManager.consumePurchaseSucceededEvent != null)
		{
			OpenIABEventManager.consumePurchaseSucceededEvent(new Purchase(json));
		}
	}

	private void OnConsumePurchaseFailed(string error)
	{
		if (OpenIABEventManager.consumePurchaseFailedEvent != null)
		{
			OpenIABEventManager.consumePurchaseFailedEvent(error);
		}
	}

	public void OnTransactionRestored(string sku)
	{
		if (OpenIABEventManager.transactionRestoredEvent != null)
		{
			OpenIABEventManager.transactionRestoredEvent(sku);
		}
	}

	public void OnRestoreTransactionFailed(string error)
	{
		if (OpenIABEventManager.restoreFailedEvent != null)
		{
			OpenIABEventManager.restoreFailedEvent(error);
		}
	}

	public void OnRestoreTransactionSucceeded(string message)
	{
		if (OpenIABEventManager.restoreSucceededEvent != null)
		{
			OpenIABEventManager.restoreSucceededEvent();
		}
	}

	private static OpenIABEventManager _instance;
}
