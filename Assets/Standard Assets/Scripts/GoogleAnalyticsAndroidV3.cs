// @plugin: class: GoogleAnalyticsAndroidV3
using System;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAnalyticsAndroidV3 : IDisposable
{
	internal void InitializeTracker()
	{
		UnityEngine.Debug.Log("Initializing Google Analytics Android Tracker.");
		this.analyticsTrackingFields = new AndroidJavaClass("com.google.analytics.tracking.android.Fields");
		using (AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.google.analytics.tracking.android.GoogleAnalytics"))
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.analytics.tracking.android.GAServiceManager"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					this.currentActivityObject = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
					this.googleAnalyticsSingleton = androidJavaObject.CallStatic<AndroidJavaObject>("getInstance", new object[]
					{
						this.currentActivityObject
					});
					this.gaServiceManagerSingleton = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
					this.gaServiceManagerSingleton.Call("setLocalDispatchPeriod", new object[]
					{
						this.dispatchPeriod
					});
					this.tracker = this.googleAnalyticsSingleton.Call<AndroidJavaObject>("getTracker", new object[]
					{
						this.trackingCode
					});
					this.SetTrackerVal(Fields.SAMPLE_RATE, this.sampleFrequency.ToString());
					this.SetTrackerVal(Fields.APP_NAME, this.appName);
					this.SetTrackerVal(Fields.APP_ID, this.bundleIdentifier);
					this.SetTrackerVal(Fields.APP_VERSION, this.appVersion);
					if (this.anonymizeIP)
					{
						this.SetTrackerVal(Fields.ANONYMIZE_IP, "1");
					}
					this.googleAnalyticsSingleton.Call("setDryRun", new object[]
					{
						this.dryRun
					});
					this.SetLogLevel(this.logLevel);
				}
			}
		}
	}

	internal void SetTrackerVal(Field fieldName, object value)
	{
		object[] args = new object[]
		{
			fieldName.ToString(),
			value
		};
		this.tracker.Call(GoogleAnalyticsV3.SET, args);
	}

	public void SetSampleFrequency(int sampleFrequency)
	{
		this.sampleFrequency = sampleFrequency;
	}

	private void SetLogLevel(GoogleAnalyticsV3.DebugMode logLevel)
	{
		using (this.logger = this.googleAnalyticsSingleton.Call<AndroidJavaObject>("getLogger", new object[0]))
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.analytics.tracking.android.Logger$LogLevel"))
			{
				switch (logLevel)
				{
				case GoogleAnalyticsV3.DebugMode.ERROR:
					using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("ERROR"))
					{
						this.logger.Call("setLogLevel", new object[]
						{
							@static
						});
					}
					goto IL_148;
				case GoogleAnalyticsV3.DebugMode.INFO:
					using (AndroidJavaObject static2 = androidJavaClass.GetStatic<AndroidJavaObject>("INFO"))
					{
						this.logger.Call("setLogLevel", new object[]
						{
							static2
						});
					}
					goto IL_148;
				case GoogleAnalyticsV3.DebugMode.VERBOSE:
					using (AndroidJavaObject static3 = androidJavaClass.GetStatic<AndroidJavaObject>("VERBOSE"))
					{
						this.logger.Call("setLogLevel", new object[]
						{
							static3
						});
					}
					goto IL_148;
				}
				using (AndroidJavaObject static4 = androidJavaClass.GetStatic<AndroidJavaObject>("WARNING"))
				{
					this.logger.Call("setLogLevel", new object[]
					{
						static4
					});
				}
				IL_148:;
			}
		}
	}

	private void SetSessionOnBuilder(AndroidJavaObject hitBuilder)
	{
		if (this.startSessionOnNextHit)
		{
			object[] args = new object[]
			{
				Fields.SESSION_CONTROL.ToString(),
				"start"
			};
			hitBuilder.Call<AndroidJavaObject>("set", args);
			this.startSessionOnNextHit = false;
		}
		else if (this.endSessionOnNextHit)
		{
			object[] args2 = new object[]
			{
				Fields.SESSION_CONTROL.ToString(),
				"end"
			};
			hitBuilder.Call<AndroidJavaObject>("set", args2);
			this.endSessionOnNextHit = false;
		}
	}

	private AndroidJavaObject BuildMap(string methodName)
	{
		AndroidJavaObject result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.analytics.tracking.android.MapBuilder"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>(methodName, new object[0]))
			{
				this.SetSessionOnBuilder(androidJavaObject);
				result = androidJavaObject.Call<AndroidJavaObject>("build", new object[0]);
			}
		}
		return result;
	}

	private AndroidJavaObject BuildMap(string methodName, object[] args)
	{
		AndroidJavaObject result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.analytics.tracking.android.MapBuilder"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>(methodName, args))
			{
				this.SetSessionOnBuilder(androidJavaObject);
				result = androidJavaObject.Call<AndroidJavaObject>("build", new object[0]);
			}
		}
		return result;
	}

	private AndroidJavaObject BuildMap(string methodName, Dictionary<AndroidJavaObject, string> parameters)
	{
		return this.BuildMap(methodName, null, parameters);
	}

	private AndroidJavaObject BuildMap(string methodName, object[] simpleArgs, Dictionary<AndroidJavaObject, string> parameters)
	{
		AndroidJavaObject result;
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]))
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.analytics.tracking.android.MapBuilder"))
			{
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				foreach (KeyValuePair<AndroidJavaObject, string> keyValuePair in parameters)
				{
					using (AndroidJavaObject key = keyValuePair.Key)
					{
						using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
						{
							keyValuePair.Value
						}))
						{
							array[0] = key;
							array[1] = androidJavaObject2;
							AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						}
					}
				}
				if (simpleArgs != null)
				{
					using (AndroidJavaObject androidJavaObject3 = androidJavaClass.CallStatic<AndroidJavaObject>(methodName, simpleArgs))
					{
						androidJavaObject3.Call<AndroidJavaObject>(GoogleAnalyticsV3.SET_ALL, new object[]
						{
							androidJavaObject
						});
						this.SetSessionOnBuilder(androidJavaObject3);
						return androidJavaObject3.Call<AndroidJavaObject>("build", new object[0]);
					}
				}
				using (AndroidJavaObject androidJavaObject4 = androidJavaClass.CallStatic<AndroidJavaObject>(methodName, new object[0]))
				{
					androidJavaObject4.Call<AndroidJavaObject>(GoogleAnalyticsV3.SET_ALL, new object[]
					{
						androidJavaObject
					});
					this.SetSessionOnBuilder(androidJavaObject4);
					result = androidJavaObject4.Call<AndroidJavaObject>("build", new object[0]);
				}
			}
		}
		return result;
	}

	private Dictionary<AndroidJavaObject, string> AddCustomVariablesAndCampaignParameters<T>(HitBuilder<T> builder)
	{
		Dictionary<AndroidJavaObject, string> dictionary = new Dictionary<AndroidJavaObject, string>();
		foreach (KeyValuePair<int, string> keyValuePair in builder.GetCustomDimensions())
		{
			AndroidJavaObject key = this.analyticsTrackingFields.CallStatic<AndroidJavaObject>("customDimension", new object[]
			{
				keyValuePair.Key
			});
			dictionary.Add(key, keyValuePair.Value);
		}
		foreach (KeyValuePair<int, string> keyValuePair2 in builder.GetCustomMetrics())
		{
			AndroidJavaObject key = this.analyticsTrackingFields.CallStatic<AndroidJavaObject>("customMetric", new object[]
			{
				keyValuePair2.Key
			});
			dictionary.Add(key, keyValuePair2.Value);
		}
		if (dictionary.Keys.Count > 0 && GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Added custom variables to hit.");
		}
		if (!string.IsNullOrEmpty(builder.GetCampaignSource()))
		{
			AndroidJavaObject key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("CAMPAIGN_SOURCE");
			dictionary.Add(key, builder.GetCampaignSource());
			key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("CAMPAIGN_MEDIUM");
			dictionary.Add(key, builder.GetCampaignMedium());
			key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("CAMPAIGN_NAME");
			dictionary.Add(key, builder.GetCampaignName());
			key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("CAMPAIGN_CONTENT");
			dictionary.Add(key, builder.GetCampaignContent());
			key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("CAMPAIGN_KEYWORD");
			dictionary.Add(key, builder.GetCampaignKeyword());
			key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("CAMPAIGN_ID");
			dictionary.Add(key, builder.GetCampaignID());
			key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("GCLID");
			dictionary.Add(key, builder.GetGclid());
			key = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("DCLID");
			dictionary.Add(key, builder.GetDclid());
			if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
			{
				UnityEngine.Debug.Log("Added campaign parameters to hit.");
			}
		}
		if (dictionary.Keys.Count > 0)
		{
			return dictionary;
		}
		return null;
	}

	internal void StartSession()
	{
		this.startSessionOnNextHit = true;
	}

	internal void StopSession()
	{
		this.endSessionOnNextHit = true;
	}

	public void SetOptOut(bool optOut)
	{
		this.googleAnalyticsSingleton.Call("setAppOptOut", new object[]
		{
			optOut
		});
	}

	internal void LogScreen(AppViewHitBuilder builder)
	{
		using (AndroidJavaObject @static = this.analyticsTrackingFields.GetStatic<AndroidJavaObject>("SCREEN_NAME"))
		{
			object[] args = new object[]
			{
				@static,
				builder.GetScreenName()
			};
			this.tracker.Call(GoogleAnalyticsV3.SET, args);
		}
		Dictionary<AndroidJavaObject, string> dictionary = this.AddCustomVariablesAndCampaignParameters<AppViewHitBuilder>(builder);
		if (dictionary != null)
		{
			object obj = this.BuildMap(GoogleAnalyticsV3.APP_VIEW, dictionary);
			this.tracker.Call(GoogleAnalyticsV3.SEND, new object[]
			{
				obj
			});
		}
		else
		{
			object[] args2 = new object[]
			{
				this.BuildMap(GoogleAnalyticsV3.APP_VIEW)
			};
			this.tracker.Call(GoogleAnalyticsV3.SEND, args2);
		}
	}

	internal void LogEvent(EventHitBuilder builder)
	{
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.Long", new object[]
		{
			builder.GetEventValue()
		}))
		{
			object[] array = new object[]
			{
				builder.GetEventCategory(),
				builder.GetEventAction(),
				builder.GetEventLabel(),
				androidJavaObject
			};
			Dictionary<AndroidJavaObject, string> dictionary = this.AddCustomVariablesAndCampaignParameters<EventHitBuilder>(builder);
			object obj;
			if (dictionary != null)
			{
				obj = this.BuildMap(GoogleAnalyticsV3.EVENT_HIT, array, dictionary);
			}
			else
			{
				obj = this.BuildMap(GoogleAnalyticsV3.EVENT_HIT, array);
			}
			this.tracker.Call(GoogleAnalyticsV3.SEND, new object[]
			{
				obj
			});
		}
	}

	internal void LogTransaction(TransactionHitBuilder builder)
	{
		AndroidJavaObject[] array = new AndroidJavaObject[]
		{
			new AndroidJavaObject("java.lang.Double", new object[]
			{
				builder.GetRevenue()
			}),
			new AndroidJavaObject("java.lang.Double", new object[]
			{
				builder.GetTax()
			}),
			new AndroidJavaObject("java.lang.Double", new object[]
			{
				builder.GetShipping()
			})
		};
		object[] array2 = new object[6];
		array2[0] = builder.GetTransactionID();
		array2[1] = builder.GetAffiliation();
		array2[2] = array[0];
		array2[3] = array[1];
		array2[4] = array[2];
		if (builder.GetCurrencyCode() == null)
		{
			array2[5] = GoogleAnalyticsV3.currencySymbol;
		}
		else
		{
			array2[5] = builder.GetCurrencyCode();
		}
		Dictionary<AndroidJavaObject, string> dictionary = this.AddCustomVariablesAndCampaignParameters<TransactionHitBuilder>(builder);
		object obj;
		if (dictionary != null)
		{
			obj = this.BuildMap(GoogleAnalyticsV3.TRANSACTION_HIT, array2, dictionary);
		}
		else
		{
			obj = this.BuildMap(GoogleAnalyticsV3.TRANSACTION_HIT, array2);
		}
		this.tracker.Call(GoogleAnalyticsV3.SEND, new object[]
		{
			obj
		});
	}

	internal void LogItem(ItemHitBuilder builder)
	{
		object[] array;
		if (builder.GetCurrencyCode() != null)
		{
			array = new object[]
			{
				null,
				null,
				null,
				null,
				null,
				null,
				builder.GetCurrencyCode()
			};
		}
		else
		{
			array = new object[6];
		}
		array[0] = builder.GetTransactionID();
		array[1] = builder.GetName();
		array[2] = builder.GetSKU();
		array[3] = builder.GetCategory();
		array[4] = new AndroidJavaObject("java.lang.Double", new object[]
		{
			builder.GetPrice()
		});
		array[5] = new AndroidJavaObject("java.lang.Long", new object[]
		{
			builder.GetQuantity()
		});
		Dictionary<AndroidJavaObject, string> dictionary = this.AddCustomVariablesAndCampaignParameters<ItemHitBuilder>(builder);
		object obj;
		if (dictionary != null)
		{
			obj = this.BuildMap(GoogleAnalyticsV3.ITEM_HIT, array, dictionary);
		}
		else
		{
			obj = this.BuildMap(GoogleAnalyticsV3.ITEM_HIT, array);
		}
		this.tracker.Call(GoogleAnalyticsV3.SEND, new object[]
		{
			obj
		});
	}

	public void LogException(ExceptionHitBuilder builder)
	{
		object[] array = new object[]
		{
			builder.GetExceptionDescription(),
			new AndroidJavaObject("java.lang.Boolean", new object[]
			{
				builder.IsFatal()
			})
		};
		Dictionary<AndroidJavaObject, string> dictionary = this.AddCustomVariablesAndCampaignParameters<ExceptionHitBuilder>(builder);
		object obj;
		if (dictionary != null)
		{
			obj = this.BuildMap(GoogleAnalyticsV3.EXCEPTION_HIT, array, dictionary);
		}
		else
		{
			obj = this.BuildMap(GoogleAnalyticsV3.EXCEPTION_HIT, array);
		}
		this.tracker.Call(GoogleAnalyticsV3.SEND, new object[]
		{
			obj
		});
	}

	public void DispatchHits()
	{
		this.gaServiceManagerSingleton.Call("dispatchLocalHits", new object[0]);
	}

	public void LogSocial(SocialHitBuilder builder)
	{
		object[] array = new object[]
		{
			builder.GetSocialNetwork(),
			builder.GetSocialAction(),
			builder.GetSocialTarget()
		};
		Dictionary<AndroidJavaObject, string> dictionary = this.AddCustomVariablesAndCampaignParameters<SocialHitBuilder>(builder);
		object obj;
		if (dictionary != null)
		{
			obj = this.BuildMap(GoogleAnalyticsV3.SOCIAL_HIT, array, dictionary);
		}
		else
		{
			obj = this.BuildMap(GoogleAnalyticsV3.SOCIAL_HIT, array);
		}
		this.tracker.Call(GoogleAnalyticsV3.SEND, new object[]
		{
			obj
		});
	}

	public void LogTiming(TimingHitBuilder builder)
	{
		using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.lang.Long", new object[]
		{
			builder.GetTimingInterval()
		}))
		{
			object[] array = new object[]
			{
				builder.GetTimingCategory(),
				androidJavaObject,
				builder.GetTimingName(),
				builder.GetTimingLabel()
			};
			Dictionary<AndroidJavaObject, string> dictionary = this.AddCustomVariablesAndCampaignParameters<TimingHitBuilder>(builder);
			object obj;
			if (dictionary != null)
			{
				obj = this.BuildMap(GoogleAnalyticsV3.TIMING_HIT, array, dictionary);
			}
			else
			{
				obj = this.BuildMap(GoogleAnalyticsV3.TIMING_HIT, array);
			}
			this.tracker.Call(GoogleAnalyticsV3.SEND, new object[]
			{
				obj
			});
		}
	}

	public void ClearUserIDOverride()
	{
		this.SetTrackerVal(Fields.USER_ID, null);
	}

	public void SetTrackingCode(string trackingCode)
	{
		this.trackingCode = trackingCode;
	}

	public void SetAppName(string appName)
	{
		this.appName = appName;
	}

	public void SetBundleIdentifier(string bundleIdentifier)
	{
		this.bundleIdentifier = bundleIdentifier;
	}

	public void SetAppVersion(string appVersion)
	{
		this.appVersion = appVersion;
	}

	public void SetDispatchPeriod(int dispatchPeriod)
	{
		this.dispatchPeriod = dispatchPeriod;
	}

	public void SetLogLevelValue(GoogleAnalyticsV3.DebugMode logLevel)
	{
		this.logLevel = logLevel;
	}

	public void SetAnonymizeIP(bool anonymizeIP)
	{
		this.anonymizeIP = anonymizeIP;
	}

	public void SetDryRun(bool dryRun)
	{
		this.dryRun = dryRun;
	}

	public void Dispose()
	{
		this.googleAnalyticsSingleton.Dispose();
		this.tracker.Dispose();
		this.analyticsTrackingFields.Dispose();
	}

	private string trackingCode;

	private string appVersion;

	private string appName;

	private string bundleIdentifier;

	private int dispatchPeriod;

	private int sampleFrequency;

	private GoogleAnalyticsV3.DebugMode logLevel;

	private bool anonymizeIP;

	private bool dryRun;

	private int sessionTimeout;

	private AndroidJavaObject tracker;

	private AndroidJavaObject logger;

	private AndroidJavaObject currentActivityObject;

	private AndroidJavaObject googleAnalyticsSingleton;

	private AndroidJavaObject gaServiceManagerSingleton;

	private AndroidJavaClass analyticsTrackingFields;

	private bool startSessionOnNextHit;

	private bool endSessionOnNextHit;
}
