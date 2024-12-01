// @plugin: class: GoogleAnalyticsV3
using System;
using UnityEngine;

public class GoogleAnalyticsV3 : MonoBehaviour
{
	private void Awake()
	{
		this.InitializeTracker();
		if (this.sendLaunchEvent)
		{
			this.LogEvent("Google Analytics", "Auto Instrumentation", "Game Launch", 0L);
		}
		if (this.UncaughtExceptionReporting)
		{
			Application.RegisterLogCallback(new Application.LogCallback(this.HandleException));
			if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
			{
				UnityEngine.Debug.Log("Enabling uncaught exception reporting.");
			}
		}
	}

	private void Update()
	{
		if (this.uncaughtExceptionStackTrace != null)
		{
			this.LogException(this.uncaughtExceptionStackTrace, true);
			this.uncaughtExceptionStackTrace = null;
		}
	}

	private void HandleException(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			this.uncaughtExceptionStackTrace = condition + "\n" + stackTrace + StackTraceUtility.ExtractStackTrace();
		}
	}

	private void InitializeTracker()
	{
		if (!this.initialized)
		{
			GoogleAnalyticsV3.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(GoogleAnalyticsV3.instance);
			UnityEngine.Debug.Log("Initializing Google Analytics 0.1.");
			this.androidTracker.SetTrackingCode(this.androidTrackingCode);
			this.androidTracker.SetAppName(this.productName);
			this.androidTracker.SetBundleIdentifier(this.bundleIdentifier);
			this.androidTracker.SetAppVersion(this.bundleVersion);
			this.androidTracker.SetDispatchPeriod(this.dispatchPeriod);
			this.androidTracker.SetSampleFrequency(this.sampleFrequency);
			this.androidTracker.SetLogLevelValue(this.logLevel);
			this.androidTracker.SetAnonymizeIP(this.anonymizeIP);
			this.androidTracker.SetDryRun(this.dryRun);
			this.androidTracker.InitializeTracker();
			this.initialized = true;
			this.SetOnTracker(Fields.DEVELOPER_ID, "GbOCSs");
		}
	}

	public void SetAppLevelOptOut(bool optOut)
	{
		this.InitializeTracker();
		this.androidTracker.SetOptOut(optOut);
	}

	public void SetUserIDOverride(string userID)
	{
		this.SetOnTracker(Fields.USER_ID, userID);
	}

	public void ClearUserIDOverride()
	{
		this.InitializeTracker();
		this.androidTracker.ClearUserIDOverride();
	}

	public void DispatchHits()
	{
		this.InitializeTracker();
		this.androidTracker.DispatchHits();
	}

	public void StartSession()
	{
		this.InitializeTracker();
		this.androidTracker.StartSession();
	}

	public void StopSession()
	{
		this.InitializeTracker();
		this.androidTracker.StopSession();
	}

	public void SetOnTracker(Field fieldName, object value)
	{
		this.InitializeTracker();
		this.androidTracker.SetTrackerVal(fieldName, value);
	}

	public void LogScreen(string title)
	{
		AppViewHitBuilder builder = new AppViewHitBuilder().SetScreenName(title);
		this.LogScreen(builder);
	}

	public void LogScreen(AppViewHitBuilder builder)
	{
		this.InitializeTracker();
		if (builder.Validate() == null)
		{
			return;
		}
		if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Logging screen.");
		}
		this.androidTracker.LogScreen(builder);
	}

	public void LogEvent(string eventCategory, string eventAction, string eventLabel, long value)
	{
		Debug.Log(eventCategory);
		Debug.Log(eventAction);
		Debug.Log(eventLabel);
		Debug.Log(value);
		EventHitBuilder builder = new EventHitBuilder().SetEventCategory(eventCategory).SetEventAction(eventAction).SetEventLabel(eventLabel).SetEventValue(value);
		this.LogEvent(builder);
	}

	public void LogEvent(EventHitBuilder builder)
	{
		this.InitializeTracker();
		if (builder.Validate() == null)
		{
			return;
		}
		if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Logging event.");
		}
		this.androidTracker.LogEvent(builder);
	}

	public void LogTransaction(string transID, string affiliation, double revenue, double tax, double shipping)
	{
		this.LogTransaction(transID, affiliation, revenue, tax, shipping, string.Empty);
	}

	public void LogTransaction(string transID, string affiliation, double revenue, double tax, double shipping, string currencyCode)
	{
		TransactionHitBuilder builder = new TransactionHitBuilder().SetTransactionID(transID).SetAffiliation(affiliation).SetRevenue(revenue).SetTax(tax).SetShipping(shipping).SetCurrencyCode(currencyCode);
		this.LogTransaction(builder);
	}

	public void LogTransaction(TransactionHitBuilder builder)
	{
		this.InitializeTracker();
		if (builder.Validate() == null)
		{
			return;
		}
		if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Logging transaction.");
		}
		this.androidTracker.LogTransaction(builder);
	}

	public void LogItem(string transID, string name, string sku, string category, double price, long quantity)
	{
		this.LogItem(transID, name, sku, category, price, quantity, null);
	}

	public void LogItem(string transID, string name, string sku, string category, double price, long quantity, string currencyCode)
	{
		ItemHitBuilder builder = new ItemHitBuilder().SetTransactionID(transID).SetName(name).SetSKU(sku).SetCategory(category).SetPrice(price).SetQuantity(quantity).SetCurrencyCode(currencyCode);
		this.LogItem(builder);
	}

	public void LogItem(ItemHitBuilder builder)
	{
		this.InitializeTracker();
		if (builder.Validate() == null)
		{
			return;
		}
		if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Logging item.");
		}
		this.androidTracker.LogItem(builder);
	}

	public void LogException(string exceptionDescription, bool isFatal)
	{
		ExceptionHitBuilder builder = new ExceptionHitBuilder().SetExceptionDescription(exceptionDescription).SetFatal(isFatal);
		this.LogException(builder);
	}

	public void LogException(ExceptionHitBuilder builder)
	{
		this.InitializeTracker();
		if (builder.Validate() == null)
		{
			return;
		}
		if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Logging exception.");
		}
		this.androidTracker.LogException(builder);
	}

	public void LogSocial(string socialNetwork, string socialAction, string socialTarget)
	{
		SocialHitBuilder builder = new SocialHitBuilder().SetSocialNetwork(socialNetwork).SetSocialAction(socialAction).SetSocialTarget(socialTarget);
		this.LogSocial(builder);
	}

	public void LogSocial(SocialHitBuilder builder)
	{
		this.InitializeTracker();
		if (builder.Validate() == null)
		{
			return;
		}
		if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Logging social.");
		}
		this.androidTracker.LogSocial(builder);
	}

	public void LogTiming(string timingCategory, long timingInterval, string timingName, string timingLabel)
	{
		TimingHitBuilder builder = new TimingHitBuilder().SetTimingCategory(timingCategory).SetTimingInterval(timingInterval).SetTimingName(timingName).SetTimingLabel(timingLabel);
		this.LogTiming(builder);
	}

	public void LogTiming(TimingHitBuilder builder)
	{
		this.InitializeTracker();
		if (builder.Validate() == null)
		{
			return;
		}
		if (GoogleAnalyticsV3.belowThreshold(this.logLevel, GoogleAnalyticsV3.DebugMode.VERBOSE))
		{
			UnityEngine.Debug.Log("Logging timing.");
		}
		this.androidTracker.LogTiming(builder);
	}

	public void Dispose()
	{
		this.initialized = false;
		this.androidTracker.Dispose();
	}

	public static bool belowThreshold(GoogleAnalyticsV3.DebugMode userLogLevel, GoogleAnalyticsV3.DebugMode comparelogLevel)
	{
		return comparelogLevel == userLogLevel || (userLogLevel != GoogleAnalyticsV3.DebugMode.ERROR && (userLogLevel == GoogleAnalyticsV3.DebugMode.VERBOSE || ((userLogLevel != GoogleAnalyticsV3.DebugMode.WARNING || (comparelogLevel != GoogleAnalyticsV3.DebugMode.INFO && comparelogLevel != GoogleAnalyticsV3.DebugMode.VERBOSE)) && (userLogLevel != GoogleAnalyticsV3.DebugMode.INFO || comparelogLevel != GoogleAnalyticsV3.DebugMode.VERBOSE))));
	}

	public static GoogleAnalyticsV3 getInstance()
	{
		return GoogleAnalyticsV3.instance;
	}

	private string uncaughtExceptionStackTrace;

	private bool initialized;

	[global::Tooltip("The tracking code to be used for Android. Example value: UA-XXXX-Y.")]
	public string androidTrackingCode;

	[global::Tooltip("The tracking code to be used for iOS. Example value: UA-XXXX-Y.")]
	public string IOSTrackingCode;

	[global::Tooltip("The tracking code to be used for platforms other than Android and iOS. Example value: UA-XXXX-Y.")]
	public string otherTrackingCode;

	[global::Tooltip("The application name. This value should be modified in the Unity Player Settings.")]
	public string productName;

	[global::Tooltip("The application identifier. Example value: com.company.app.")]
	public string bundleIdentifier;

	[global::Tooltip("The application version. Example value: 1.2")]
	public string bundleVersion;

	[RangedTooltip("The dispatch period in seconds. Only required for Android and iOS.", 0f, 3600f)]
	public int dispatchPeriod = 5;

	[RangedTooltip("The sample rate to use. Only required for Android and iOS.", 0f, 100f)]
	public int sampleFrequency = 100;

	[global::Tooltip("The log level. Default is WARNING.")]
	public GoogleAnalyticsV3.DebugMode logLevel = GoogleAnalyticsV3.DebugMode.WARNING;

	[global::Tooltip("If checked, the IP address of the sender will be anonymized.")]
	public bool anonymizeIP;

	[global::Tooltip("Automatically report uncaught exceptions.")]
	public bool UncaughtExceptionReporting;

	[global::Tooltip("Automatically send a launch event when the game starts up.")]
	public bool sendLaunchEvent;

	[global::Tooltip("If checked, hits will not be dispatched. Use for testing.")]
	public bool dryRun;

	[global::Tooltip("The amount of time in seconds your application can stay inthe background before the session is ended. Default is 30 minutes (1800 seconds). A value of -1 will disable session management.")]
	public int sessionTimeout = 1800;

	public static GoogleAnalyticsV3 instance;

	[HideInInspector]
	public static readonly string currencySymbol = "USD";

	public static readonly string EVENT_HIT = "createEvent";

	public static readonly string APP_VIEW = "createAppView";

	public static readonly string SET = "set";

	public static readonly string SET_ALL = "setAll";

	public static readonly string SEND = "send";

	public static readonly string ITEM_HIT = "createItem";

	public static readonly string TRANSACTION_HIT = "createTransaction";

	public static readonly string SOCIAL_HIT = "createSocial";

	public static readonly string TIMING_HIT = "createTiming";

	public static readonly string EXCEPTION_HIT = "createException";

	private GoogleAnalyticsAndroidV3 androidTracker = new GoogleAnalyticsAndroidV3();

	public enum DebugMode
	{
		ERROR,
		WARNING,
		INFO,
		VERBOSE
	}
}
