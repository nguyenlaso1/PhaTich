// @sonhg: class: OfflineForeGround
using System;
using System.Collections;
using BombOffline;
using UnityEngine;
using UnityEngine.UI;

public class OfflineForeGround : MonoBehaviour
{
	private void Awake()
	{
		base.StartCoroutine(this.StartGame());
	}

	private IEnumerator StartGame()
	{
		base.gameObject.SetActive(true);
		this.inControlPanel.SetActive(false);
		this.levelText.text = string.Concat(new string[]
		{
			"<color=yellow>",
			OfflineMapChooser.CurrentZone,
			" ",
			OfflineMapChooser.CurrentLevel,
			"</color>\ndefeat all monster"
		});
		yield return new WaitForSeconds(0.75f);
		this.inControlPanel.SetActive(true);
		yield return new WaitForSeconds(2.75f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private GameObject inControlPanel;

	[SerializeField]
	private Offline_GameController gameControllerPanel;
}
