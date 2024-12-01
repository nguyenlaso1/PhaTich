// @sonhg: class: BombOffline.FireWall
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class FireWall : MonoBehaviour
	{
		private void Awake()
		{
			this.fireWallCollider = base.GetComponent<BoxCollider2D>();
			this.fireWallCollider.enabled = false;
		}

		private void Start()
		{
			base.StartCoroutine(this.Explode());
		}

		private IEnumerator Explode()
		{
			this.SetSize(0.1f);
			this.SetSpeed(0.1f);
			this.SetStartLifeTime(0.5f);
			yield return new WaitForSeconds(1.5f);
			this.fireWallCollider.enabled = true;
			this.SetSize(1f);
			this.SetSpeed(1f);
			this.SetStartLifeTime(1.5f);
			yield return new WaitForSeconds(1f);
			this.SetSize(2f);
			this.SetSpeed(2f);
			yield return new WaitForSeconds(1f);
			this.SetSize(0.5f);
			this.SetSpeed(0.5f);
			yield return new WaitForSeconds(1f);
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		private void SetSize(float size)
		{
			this.fireFrontParticle.startSize = size;
			this.fireBackParticle.startSize = size;
		}

		private void SetSpeed(float speed)
		{
			this.fireFrontParticle.startSpeed = speed;
			this.fireBackParticle.startSpeed = speed;
		}

		private void SetStartLifeTime(float speed)
		{
			this.fireFrontParticle.startSpeed = speed;
			this.fireBackParticle.startSpeed = speed;
		}

		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Player"))
			{
				Offline_BaseCharactersController component = collider.gameObject.GetComponent<Offline_BaseCharactersController>();
				component.GetHit(null);
			}
		}

		[SerializeField]
		private ParticleSystem fireFrontParticle;

		[SerializeField]
		private ParticleSystem fireBackParticle;

		private BoxCollider2D fireWallCollider;
	}
}
