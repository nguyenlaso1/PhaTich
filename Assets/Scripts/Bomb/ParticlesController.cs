// @sonhg: class: Bomb.ParticlesController
using System;
using UnityEngine;

namespace Bomb
{
	public class ParticlesController : MonoBehaviour
	{
		public BombGameScene Scene
		{
			get
			{
				if (this._bombScene == null)
				{
					this._bombScene = base.GetComponent<BombGameScene>();
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

		public GameObject PlayGoldDropParticle(Transform actor, int amount)
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(this._goldDropParticlePrefab, actor.position, Quaternion.identity) as ParticleSystem;
			int count = Mathf.Max(1, amount / 10);
			particleSystem.Emit(count);
			UnityEngine.Object.Destroy(particleSystem.gameObject, 2f);
			return particleSystem.gameObject;
		}

		private BombGameScene _bombScene;

		[SerializeField]
		private ParticleSystem _bombParticlePrefab;

		[SerializeField]
		private ParticleSystem _shieldParticlePrefab;

		[SerializeField]
		private ParticleSystem _pickupParticlePrefab;

		[SerializeField]
		private ParticleSystem _debuffParticlePrefab;

		[SerializeField]
		private ParticleSystem _goldDropParticlePrefab;
	}
}
