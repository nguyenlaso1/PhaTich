// @plugin: class: OnePF.OpenIAB
using System;
using UnityEngine;

namespace OnePF
{
	public class OpenIAB
	{
		static OpenIAB()
		{
			UnityEngine.Debug.Log("********** Android OpenIAB plugin initialized **********");
		}

		public static GameObject EventManager
		{
			get
			{
				return GameObject.Find(typeof(OpenIABEventManager).ToString());
			}
		}

		public static void mapSku(string sku, string storeName, string storeSku)
		{
			OpenIAB._billing.mapSku(sku, storeName, storeSku);
		}

		public static void init(Options options)
		{
			OpenIAB._billing.init(options);
		}

		public static void unbindService()
		{
			OpenIAB._billing.unbindService();
		}

		public static bool areSubscriptionsSupported()
		{
			return OpenIAB._billing.areSubscriptionsSupported();
		}

		public static void queryInventory()
		{
			OpenIAB._billing.queryInventory();
		}

		public static void queryInventory(string[] skus)
		{
			OpenIAB._billing.queryInventory(skus);
		}

		public static void purchaseProduct(string sku, string developerPayload = "")
		{
			OpenIAB._billing.purchaseProduct(sku, developerPayload);
		}

		public static void purchaseSubscription(string sku, string developerPayload = "")
		{
			OpenIAB._billing.purchaseSubscription(sku, developerPayload);
		}

		public static void consumeProduct(Purchase purchase)
		{
			OpenIAB._billing.consumeProduct(purchase);
		}

		public static void restoreTransactions()
		{
			OpenIAB._billing.restoreTransactions();
		}

		public static bool isDebugLog()
		{
			return OpenIAB._billing.isDebugLog();
		}

		public static void enableDebugLogging(bool enabled)
		{
			OpenIAB._billing.enableDebugLogging(enabled);
		}

		public static void enableDebugLogging(bool enabled, string tag)
		{
			OpenIAB._billing.enableDebugLogging(enabled, tag);
		}

		private static IOpenIAB _billing = new OpenIAB_Android();
	}
}
