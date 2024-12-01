// @plugin: class: OnePF.OpenIAB_Android
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OnePF
{
	public class OpenIAB_Android : IOpenIAB
	{
		static OpenIAB_Android()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				OpenIAB_Android.STORE_GOOGLE = "STORE_GOOGLE";
				OpenIAB_Android.STORE_AMAZON = "STORE_AMAZON";
				OpenIAB_Android.STORE_SAMSUNG = "STORE_SAMSUNG";
				OpenIAB_Android.STORE_NOKIA = "STORE_NOKIA";
				OpenIAB_Android.STORE_YANDEX = "STORE_YANDEX";
				return;
			}
			// nguyennq: temp not using billing on android
			return;
			//
			AndroidJNI.AttachCurrentThread();
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("org.onepf.openiab.UnityPlugin"))
			{
				OpenIAB_Android._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
				OpenIAB_Android.STORE_GOOGLE = androidJavaClass.GetStatic<string>("STORE_GOOGLE");
				OpenIAB_Android.STORE_AMAZON = androidJavaClass.GetStatic<string>("STORE_AMAZON");
				OpenIAB_Android.STORE_SAMSUNG = androidJavaClass.GetStatic<string>("STORE_SAMSUNG");
				OpenIAB_Android.STORE_NOKIA = androidJavaClass.GetStatic<string>("STORE_NOKIA");
				OpenIAB_Android.STORE_YANDEX = androidJavaClass.GetStatic<string>("STORE_YANDEX");
			}
		}

		private bool IsDevice()
		{
			// nguyennq: temp not using billing on android
			//return Application.platform == RuntimePlatform.Android;
			return false;
			//
		}

		private AndroidJavaObject CreateJavaHashMap(Dictionary<string, string> storeKeys)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			if (storeKeys != null)
			{
				object[] array = new object[2];
				foreach (KeyValuePair<string, string> keyValuePair in storeKeys)
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
					{
						keyValuePair.Key
					}))
					{
						using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
						{
							keyValuePair.Value
						}))
						{
							array[0] = androidJavaObject2;
							array[1] = androidJavaObject3;
							AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						}
					}
				}
			}
			return androidJavaObject;
		}

		public void init(Options options)
		{
			if (!this.IsDevice())
			{
				OpenIAB.EventManager.SendMessage("OnBillingSupported", string.Empty);
				return;
			}
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("org.onepf.oms.OpenIabHelper$Options$Builder", new object[0]))
			{
				IntPtr rawClass = androidJavaObject.GetRawClass();
				IntPtr rawObject = androidJavaObject.GetRawObject();
				androidJavaObject.Call<AndroidJavaObject>("setDiscoveryTimeout", new object[]
				{
					options.discoveryTimeoutMs
				}).Call<AndroidJavaObject>("setCheckInventory", new object[]
				{
					options.checkInventory
				}).Call<AndroidJavaObject>("setCheckInventoryTimeout", new object[]
				{
					options.checkInventoryTimeoutMs
				}).Call<AndroidJavaObject>("setVerifyMode", new object[]
				{
					(int)options.verifyMode
				});
				foreach (KeyValuePair<string, string> keyValuePair in options.storeKeys)
				{
					androidJavaObject.Call<AndroidJavaObject>("addStoreKey", new object[]
					{
						keyValuePair.Key,
						keyValuePair.Value
					});
				}
				IntPtr methodID = AndroidJNI.GetMethodID(rawClass, "addPreferredStoreName", "([Ljava/lang/String;)Lorg/onepf/oms/OpenIabHelper$Options$Builder;");
				jvalue[] array = new jvalue[1];
				array[0].l = AndroidJNIHelper.ConvertToJNIArray(options.prefferedStoreNames);
				AndroidJNI.CallObjectMethod(rawObject, methodID, array);
				IntPtr methodID2 = AndroidJNI.GetMethodID(rawClass, "build", "()Lorg/onepf/oms/OpenIabHelper$Options;");
				IntPtr l = AndroidJNI.CallObjectMethod(rawObject, methodID2, new jvalue[0]);
				IntPtr methodID3 = AndroidJNI.GetMethodID(OpenIAB_Android._plugin.GetRawClass(), "initWithOptions", "(Lorg/onepf/oms/OpenIabHelper$Options;)V");
				array = new jvalue[1];
				array[0].l = l;
				AndroidJNI.CallVoidMethod(OpenIAB_Android._plugin.GetRawObject(), methodID3, array);
			}
		}

		public void init(Dictionary<string, string> storeKeys = null)
		{
			if (!this.IsDevice())
			{
				return;
			}
			if (storeKeys != null)
			{
				AndroidJavaObject androidJavaObject = this.CreateJavaHashMap(storeKeys);
				OpenIAB_Android._plugin.Call("init", new object[]
				{
					androidJavaObject
				});
				androidJavaObject.Dispose();
			}
		}

		public void mapSku(string sku, string storeName, string storeSku)
		{
			if (!this.IsDevice())
			{
				return;
			}
			OpenIAB_Android._plugin.Call("mapSku", new object[]
			{
				sku,
				storeName,
				storeSku
			});
		}

		public void unbindService()
		{
			if (this.IsDevice())
			{
				OpenIAB_Android._plugin.Call("unbindService", new object[0]);
			}
		}

		public bool areSubscriptionsSupported()
		{
			return !this.IsDevice() || OpenIAB_Android._plugin.Call<bool>("areSubscriptionsSupported", new object[0]);
		}

		public void queryInventory()
		{
			if (!this.IsDevice())
			{
				return;
			}
			IntPtr methodID = AndroidJNI.GetMethodID(OpenIAB_Android._plugin.GetRawClass(), "queryInventory", "()V");
			AndroidJNI.CallVoidMethod(OpenIAB_Android._plugin.GetRawObject(), methodID, new jvalue[0]);
		}

		public void queryInventory(string[] skus)
		{
			this.queryInventory(skus, skus);
		}

		private void queryInventory(string[] inAppSkus, string[] subsSkus)
		{
			if (!this.IsDevice())
			{
				return;
			}
			jvalue[] args = AndroidJNIHelper.CreateJNIArgArray(new object[]
			{
				inAppSkus,
				subsSkus
			});
			IntPtr methodID = AndroidJNI.GetMethodID(OpenIAB_Android._plugin.GetRawClass(), "queryInventory", "([Ljava/lang/String;[Ljava/lang/String;)V");
			AndroidJNI.CallVoidMethod(OpenIAB_Android._plugin.GetRawObject(), methodID, args);
		}

		public void purchaseProduct(string sku, string developerPayload = "")
		{
			if (!this.IsDevice())
			{
				OpenIAB.EventManager.SendMessage("OnPurchaseSucceeded", Purchase.CreateFromSku(sku, developerPayload).Serialize());
				return;
			}
			OpenIAB_Android._plugin.Call("purchaseProduct", new object[]
			{
				sku,
				developerPayload
			});
		}

		public void purchaseSubscription(string sku, string developerPayload = "")
		{
			if (!this.IsDevice())
			{
				OpenIAB.EventManager.SendMessage("OnPurchaseSucceeded", Purchase.CreateFromSku(sku, developerPayload).Serialize());
				return;
			}
			OpenIAB_Android._plugin.Call("purchaseSubscription", new object[]
			{
				sku,
				developerPayload
			});
		}

		public void consumeProduct(Purchase purchase)
		{
			if (!this.IsDevice())
			{
				OpenIAB.EventManager.SendMessage("OnConsumePurchaseSucceeded", purchase.Serialize());
				return;
			}
			OpenIAB_Android._plugin.Call("consumeProduct", new object[]
			{
				purchase.Serialize()
			});
		}

		public void restoreTransactions()
		{
		}

		public bool isDebugLog()
		{
			return OpenIAB_Android._plugin.Call<bool>("isDebugLog", new object[0]);
		}

		public void enableDebugLogging(bool enabled)
		{
			OpenIAB_Android._plugin.Call("enableDebugLogging", new object[]
			{
				enabled
			});
		}

		public void enableDebugLogging(bool enabled, string tag)
		{
			OpenIAB_Android._plugin.Call("enableDebugLogging", new object[]
			{
				enabled,
				tag
			});
		}

		public static readonly string STORE_GOOGLE;

		public static readonly string STORE_AMAZON;

		public static readonly string STORE_SAMSUNG;

		public static readonly string STORE_NOKIA;

		public static readonly string STORE_YANDEX;

		private static AndroidJavaObject _plugin;
	}
}
