// @sonhg: class: BombOffline.OfflineEndGamePanel
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace BombOffline
{
	public class OfflineEndGamePanel : MonoBehaviour
	{
		public void SetEndGameInformation(string time, bool isWin)
		{
			string[] array = OfflineMapChooser.CurrentLevel.Replace("lvl", string.Empty).Split(new char[]
			{
				'-'
			});
			int index = int.Parse(array[1]) % 10;
			int num = int.Parse(array[1]) / 10;
			this.leftNumber.sprite = this.numberList[int.Parse(array[0])];
			this.leftNumberLose.sprite = this.numberList[int.Parse(array[0])];
			if (num == 0)
			{
				this.rightNumber1.sprite = this.numberList[index];
				this.rightNumberLose1.sprite = this.numberList[index];
				this.rightNumber.gameObject.SetActive(false);
				this.rightNumberLose.gameObject.SetActive(false);
			}
			else
			{
				this.rightNumber.sprite = this.numberList[index];
				this.rightNumberLose.sprite = this.numberList[index];
				this.rightNumber1.sprite = this.numberList[num];
				this.rightNumberLose1.sprite = this.numberList[num];
			}
			if (isWin)
			{
				this.winPanel.SetActive(true);
				this.losePanel.SetActive(false);
				this.effects[1].SetActive(false);
				this.effects[0].SetActive(true);
			}
			else
			{
				this.winPanel.SetActive(false);
				this.losePanel.SetActive(true);
				this.effects[0].SetActive(false);
				this.effects[1].SetActive(true);
			}
			if (!isWin || OfflineMapChooser.GetNextLevel() == null)
			{
				this.nextButton.SetActive(false);
			}
			if (isWin)
			{
				this.retryButton.SetActive(false);
			}
		}

		private IEnumerator WaitToShowButtonControl(float _time)
		{
			yield return base.StartCoroutine(OfflineEndGamePanel.WaitForRealSeconds(_time));
			if (this._isOpenBox)
			{
				this.OpenBoxRandom();
			}
			this.buttonPanel.SetActive(true);
			this.openAllButton.SetActive(true);
			this.openAllButton.transform.localPosition = Vector3.zero;
			this.shareButton.SetActive(true);
			this.shareButton.transform.localPosition = Vector3.zero;
			yield break;
		}

		private void OpenBoxRandom()
		{
			if (!this._isOpenBox)
			{
				this._isOpenBox = true;
				int num = UnityEngine.Random.Range(0, 3);
				if (num < this.boxObjects.Count && this.boxObjects[num] != null)
				{
					this.OnClickBox(this.boxObjects[num]);
				}
			}
		}

		private IEnumerator ShowGiftOfBox(GameObject _box, Sprite _gift, int number)
		{
			yield return base.StartCoroutine(OfflineEndGamePanel.WaitForRealSeconds(2f));
			_box.GetComponent<Image>().sprite = _gift;
			_box.AddComponent<Outline>();
			_box.transform.DOScale(Vector3.one, 1.5f);
			if (number > 1)
			{
				Text numberLabel = _box.transform.Find("Text").GetComponent<Text>();
				numberLabel.text = number + string.Empty;
				numberLabel.gameObject.SetActive(true);
			}
			this.boxObjects.Remove(_box);
			yield break;
		}

		private IEnumerator WaitToDestroy(GameObject _obj, float _time)
		{
			yield return base.StartCoroutine(OfflineEndGamePanel.WaitForRealSeconds(_time));
			GameObject _eff = UnityEngine.Object.Instantiate<GameObject>(this._itemEffect);
			_eff.transform.SetParent(_obj.transform.parent.parent);
			_eff.transform.SetAsFirstSibling();
			_eff.transform.localPosition = Vector3.zero;
			_eff.transform.localScale = Vector3.one;
			_eff.SetActive(true);
			UnityEngine.Object.Destroy(_obj);
			yield break;
		}

		public void OnClickBox(GameObject _box)
		{
			if (!this._isOpenBox && _box.GetComponent<Image>().sprite == this._boxSprite)
			{
				this._isOpenBox = true;
				MusicManager.instance.PlayOneShot(this.openSound, 2f);
				_box.transform.DOScale(Vector3.zero, 0.5f);
				ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.openBoxEffect);
				particleSystem.transform.parent = _box.transform;
				particleSystem.transform.localPosition = Vector3.zero;
				particleSystem.transform.localScale = Vector3.one;
				particleSystem.gameObject.SetActive(true);
				particleSystem.Play();
				base.StartCoroutine(this.WaitToDestroy(particleSystem.gameObject, 2f));
				OfflineEndGamePanel.DropItem dropItem = OfflineEndGamePanel.DropItem.G20;
				double[] array = new double[]
				{
					50.0,
					30.0,
					15.0,
					5.0
				};
				System.Random random = new System.Random();
				double num = 0.0;
				double num2 = random.NextDouble() * 100.0;
				for (int i = 0; i < array.Length; i++)
				{
					if (num2 > num && num2 < num + array[i])
					{
						dropItem = (OfflineEndGamePanel.DropItem)i;
						break;
					}
					num += array[i];
				}

				int number = 1;
				switch (dropItem)
				{
					case OfflineEndGamePanel.DropItem.G20:
						// DataManager.PlusGold(20);
						// this.playerControl.UpdateGold();
						number = 20;
						break;
					case OfflineEndGamePanel.DropItem.G50:
						// DataManager.PlusGold(50);
						// this.playerControl.UpdateGold();
						number = 50;
						break;
					case OfflineEndGamePanel.DropItem.G100:
						// DataManager.PlusGold(100);
						// this.playerControl.UpdateGold();
						number = 100;
						break;
				}

				if (dropItem == OfflineEndGamePanel.DropItem.Clothes)
				{
					base.StartCoroutine(this.ShowGiftOfBox(_box, this.GenerateCloth(), number));
				}
				else
				{
					base.StartCoroutine(this.ShowGiftOfBox(_box, this.GenerateGift((int)dropItem), number));
					StartCoroutine(SaveGold(number));
				}	

			}
		}

		IEnumerator SaveGold(int number)
		{
			yield return new WaitForSecondsRealtime(2);
			DataManager.PlusGold(number);
			this.playerControl.UpdateGold();
		}

		private Sprite GenerateCloth()
		{
			IEnumerable<Item> source = from x in ResourcesManager.ItemsDict.Values
									   where x.Category >= 2
									   select x;
			int index = UnityEngine.Random.Range(0, source.Count<Item>());
			Item item = source.ElementAt(index);
			if (item.Category == 2)
			{
				DataManager.AddMyItemHelper(item.Id + string.Empty, 1);
			}
			else
			{
				string[] array = DataManager.GetMyItem().Split(new char[]
				{
					'-'
				});
				bool flag = true;
				foreach (string text in array)
				{
					if (text.Equals(item.Id))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					DataManager.SetMyItem(DataManager.GetMyItem() + "-" + item.Id);
				}
			}
			return Resources.Load<Sprite>("Textures/" + item.Icon.Substring(0, item.Icon.Length - 4));
		}

		private Sprite GenerateGift(int index)
		{
			return this.listReward[index];
		}

		public void OpenAllBox()
		{
			this.openAllButton.GetComponent<Button>().interactable = false;
			foreach (GameObject gameObject in this.boxObjects)
			{
				if (gameObject != null)
				{
					this._isOpenBox = false;
					this.OnClickBox(gameObject);
				}
			}
		}

		public static IEnumerator WaitForRealSeconds(float time)
		{
			float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time)
			{
				yield return null;
			}
			yield break;
		}

		public void ShowEndGameBoard(string time, bool isWin, int timePoint, int monsterKill, int monsterKillPoint, int doubleKill, int doubleKillPoint, int tripleKill, int tripleKillPoint, int ultraKill, int ultraKillPoint, int fiveStarPoint, BombSaveGame save)
		{
			this.save = save;
			this.scoreArray[0].text = monsterKillPoint.ToString();
			this.scoreArray[1].text = doubleKillPoint.ToString();
			this.scoreArray[2].text = tripleKillPoint.ToString();
			this.scoreArray[3].text = ultraKillPoint.ToString();
			this.scoreArray[4].text = timePoint.ToString();
			this.totalScore.text = (monsterKillPoint + doubleKillPoint + tripleKillPoint + ultraKillPoint + timePoint).ToString();
			this.goldBonusLabel.text = (float.Parse(this.totalScore.text) * 0.1f).ToString();
			if (isWin)
			{
				DataManager.PlusGold((int)(float.Parse(this.totalScore.text) * 0.1f));
				this.playerControl.UpdateGold();
			}
			this.numberArray[0].text = monsterKill.ToString();
			this.numberArray[1].text = doubleKill.ToString();
			this.numberArray[2].text = tripleKill.ToString();
			this.numberArray[3].text = ultraKill.ToString();
			this.numberArray[4].text = ((int)((float)timePoint * 0.1f)).ToString() + "s";
			this.SetEndGameInformation(time, isWin);
			base.gameObject.SetActive(true);
			if (isWin)
			{
				base.StartCoroutine(this.WaitToShowButtonControl(4f));

                int map;
				long level;
                bool isSucess;
                string[] strList = OfflineMapChooser.CurrentLevel.Replace("lvl", string.Empty).Split('-');
				isSucess = int.TryParse(strList[0], out map);
                isSucess = long.TryParse(strList[1], out level);
				if(isSucess)
					StartCoroutine(API.Instance.UpdateScore(map, level));
			}
			else
			{
				base.StartCoroutine(this.WaitToShowButtonControl(1f));
			}
		}

		public void OnClickExit()
		{
			if (this.closeButton.GetComponent<Button>().interactable)
			{
				this.closeButton.GetComponent<Button>().interactable = false;
				this.nextButton.GetComponent<Button>().interactable = false;
				this.retryButton.GetComponent<Button>().interactable = false;
				Offline_Context.Waitting.ShowWaiting();
				Time.timeScale = 1f;
				LoadingScreenManager.LoadScene("BomberMap", true);
			}
		}

		public void OnClickReload()
		{
			if (this.retryButton.GetComponent<Button>().interactable)
			{
				this.retryButton.GetComponent<Button>().interactable = false;
				this.closeButton.GetComponent<Button>().interactable = false;
				this.nextButton.GetComponent<Button>().interactable = false;
				Offline_Context.Waitting.ShowWaiting();
				Time.timeScale = 1f;
				OfflineMapChooser.CurrentZoneProgress = this.save;
				BombSaveGame.SaveZoneProgress();
				LoadingScreenManager.LoadScene("OfflineMainScene", true);
			}
		}

		public void OnClickNextGame()
		{
			if (this.nextButton.GetComponent<Button>().interactable)
			{
				this.nextButton.GetComponent<Button>().interactable = false;
				this.closeButton.GetComponent<Button>().interactable = false;
				this.retryButton.GetComponent<Button>().interactable = false;
				Offline_Context.Waitting.ShowWaiting();
				Time.timeScale = 1f;
				OfflineMapChooser.CurrentLevel = OfflineMapChooser.GetNextLevel();
				LoadingScreenManager.LoadScene("OfflineMainScene", true);
			}
		}

		public void PlayAnimation(bool isWin)
		{
			if (isWin)
			{
				this.animator.Play("EndGamePanelAnim");
			}
			else
			{
				this.animator.Play("EndGameLoose");
			}
		}

		public Offline_PlayerController playerControl;

		[SerializeField]
		private GameObject winPanel;

		[SerializeField]
		private GameObject _itemEffect;

		[SerializeField]
		private AudioClip openSound;

		[SerializeField]
		private Sprite _boxSprite;

		[SerializeField]
		private GameObject losePanel;

		[SerializeField]
		private GameObject[] effects;

		[SerializeField]
		private List<Sprite> numberList;

		[SerializeField]
		private Image leftNumber;

		[SerializeField]
		private Image rightNumber;

		[SerializeField]
		private Image rightNumber1;

		[SerializeField]
		private Image leftNumberLose;

		[SerializeField]
		private Image rightNumberLose;

		[SerializeField]
		private Image rightNumberLose1;

		[SerializeField]
		private GameObject nextButton;

		[SerializeField]
		private GameObject retryButton;

		[SerializeField]
		private GameObject closeButton;

		[SerializeField]
		private GameObject buttonPanel;

		[SerializeField]
		private GameObject shareButton;

		[SerializeField]
		private List<GameObject> boxObjects;

		[SerializeField]
		private ParticleSystem openBoxEffect;

		[SerializeField]
		private GameObject openAllButton;

		[SerializeField]
		private Animator animator;

		private BombSaveGame save;

		[SerializeField]
		private AudioClip _starSound;

		private bool _isOpenBox;

		public Text[] scoreArray;

		public Text[] numberArray;

		public Text totalScore;

		public Text goldBonusLabel;

		public List<Sprite> listReward;

		public GameObject loadingImage;

		public List<Sprite> openAllList;

		private enum DropItem
		{
			G20,
			G50,
			G100,
			Clothes
		}
	}
}
