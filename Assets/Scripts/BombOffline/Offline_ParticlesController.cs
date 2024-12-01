// @sonhg: class: BombOffline.Offline_ParticlesController
using System;
using System.Collections;
using UnityEngine;

namespace BombOffline
{
	public class Offline_ParticlesController : MonoBehaviour
	{
		public Offline_BombScene Scene
		{
			get
			{
				if (this._bombScene == null)
				{
					this._bombScene = base.GetComponent<Offline_BombScene>();
				}
				return this._bombScene;
			}
		}

		public void PlayBombParticle(int x, int y)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._bombParticlePrefab, new Vector3((float)x, (float)y, 0f), Quaternion.identity) as ParticleSystem;
			UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
		}

		public GameObject PlayShiledParticle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._shieldParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			return particleSystem.gameObject;
		}

		public GameObject PlayPickupParticle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._pickupParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
			return particleSystem.gameObject;
		}

		public GameObject PlayDebuffParticle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._debuffParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
			return particleSystem.gameObject;
		}

		public GameObject PlayLoseHeartParticle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._loseHeartParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
			return particleSystem.gameObject;
		}

		public GameObject PlayLoseHeart2Particle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._loseHeart2ParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
			return particleSystem.gameObject;
		}

		public GameObject PlayHighLightParticle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._highLightParticlePrefab, actor.position + new Vector3(0f, 0f, 0f), Quaternion.identity) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			return particleSystem.gameObject;
		}

		public GameObject PlayScanParticle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._scanParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			return particleSystem.gameObject;
		}

		public GameObject PlayDeathParticle(Transform actor, float delayTime = 0f)
		{
			base.StartCoroutine(this.SpawnDeathParticle(delayTime, actor.position));
			return null;
		}

		private IEnumerator SpawnDeathParticle(float time, Vector3 position)
		{
			yield return new WaitForSeconds(time);
			ParticleSystem particle = UnityEngine.Object.Instantiate(this._deathParticlePrefab, position, Quaternion.identity) as ParticleSystem;
			yield break;
		}

		public GameObject PlaySpecialSkillParticle(Transform actor, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._specialSkillParticlePrefab, actor.position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
			}
			return particleSystem.gameObject;
		}

		public GameObject PlayBoostSpeedParticle(Transform actor)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._boostSpeedParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
			return particleSystem.gameObject;
		}

		public GameObject PlayRootParticle(Transform actor, float? time = null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this._rootObjectParticlePrebab, actor.position, Quaternion.identity) as GameObject;
			if (time != null)
			{
				UnityEngine.Object.Destroy(gameObject.gameObject, time.Value);
			}
			return gameObject;
		}

		public GameObject PlayAutoFireParticle(Transform actor, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._autoFireParticlePrefab, actor.position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
			}
			return particleSystem.gameObject;
		}

		public GameObject PlaySlowParticle(Transform actor, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._slowParticlePrefab, actor.position + new Vector3(0f, 1.5f, 0f), Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
			}
			return particleSystem.gameObject;
		}

		public GameObject PlayReverseParticle(Transform actor, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._reverseParticlePrefab, actor.position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
			}
			return particleSystem.gameObject;
		}

		public GameObject PlayBubbleShield1(Transform actor, float? time = null)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._bubbleShield1, actor.position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
			particleSystem.transform.SetParent(actor);
			if (time != null)
			{
				UnityEngine.Object.Destroy(particleSystem.gameObject, time.Value);
			}
			return particleSystem.gameObject;
		}

		private Offline_BombScene _bombScene;

		[SerializeField]
		private ParticleSystem _bombParticlePrefab;

		[SerializeField]
		private ParticleSystem _shieldParticlePrefab;

		[SerializeField]
		private ParticleSystem _pickupParticlePrefab;

		[SerializeField]
		private ParticleSystem _debuffParticlePrefab;

		[SerializeField]
		private ParticleSystem _loseHeartParticlePrefab;

		[SerializeField]
		private ParticleSystem _loseHeart2ParticlePrefab;

		[SerializeField]
		private ParticleSystem _highLightParticlePrefab;

		[SerializeField]
		private ParticleSystem _scanParticlePrefab;

		[SerializeField]
		private ParticleSystem _deathParticlePrefab;

		[SerializeField]
		private ParticleSystem _specialSkillParticlePrefab;

		[SerializeField]
		private ParticleSystem _boostSpeedParticlePrefab;

		[SerializeField]
		private GameObject _rootObjectParticlePrebab;

		[SerializeField]
		private ParticleSystem _autoFireParticlePrefab;

		[SerializeField]
		private ParticleSystem _slowParticlePrefab;

		[SerializeField]
		private ParticleSystem _reverseParticlePrefab;

		[SerializeField]
		private ParticleSystem _bubbleShield1;
	}
}
