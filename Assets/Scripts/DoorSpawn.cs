// @sonhg: class: DoorSpawn
using System;
using BombOffline;
using DG.Tweening;
using UnityEngine;

public class DoorSpawn : MonoBehaviour
{
	private void Start()
	{
		this.DropDoor();
	}

	public void DropDoor()
	{
		this.container.DOMove(this.toPosition, this.moveTime, false).SetEase(Ease.InExpo).OnComplete(delegate
		{
			MusicManager.instance.PlayOneShot(this.doorFell, 1f);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._particleExplode);
			UnityEngine.Object.Destroy(gameObject, 1f);
			gameObject.transform.localPosition = this.toPosition;
			Camera.main.transform.DOShakePosition(this.shakeTime, new Vector3(0.6f, 0.1f, 0.5f), this.vibrato, 90f, false);
			this._boxCollider.enabled = true;
			int num = Mathf.RoundToInt(this.toPosition.x);
			int num2 = Mathf.RoundToInt(this.toPosition.y);
			for (int i = num - 1; i <= num + 1; i++)
			{
				this._gameController.Scene.MapController.DestroyBricks(i, num2);
				this._gameController.Scene.MapController.DestroyItems(i, num2);
				this._gameController.TriggerBomb(new Vector2((float)i, (float)num2));
			}
		});
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			this._gameController.EndGame(true);
		}
	}

	public Vector3 toPosition;

	public float moveTime = 0.7f;

	public float shakeTime = 0.7f;

	public int vibrato = 30;

	[SerializeField]
	private GameObject _particleExplode;

	[SerializeField]
	private AudioClip doorFell;

	[SerializeField]
	private BoxCollider2D _boxCollider;

	[SerializeField]
	private Offline_GameController _gameController;

	[SerializeField]
	private Transform container;
}
