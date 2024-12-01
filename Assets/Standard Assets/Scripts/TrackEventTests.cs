// @plugin: class: TrackEventTests
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackEventTests : MonoBehaviour
{
	private void Start()
	{
		MonoBehaviour.print("trackEventClass Start called");
		this.TrackRichEventTest();
	}

	private void Update()
	{
	}

	public void TrackRichEventTest()
	{
		MonoBehaviour.print("trackRichEventTest called");
		AppsFlyer.trackRichEvent("add_to_wish_list", new Dictionary<string, string>
		{
			{
				"currency",
				"USD"
			},
			{
				"productId",
				"123123"
			},
			{
				"price",
				"100"
			}
		});
	}

	public void ValidateReceiptTest()
	{
	}
}
