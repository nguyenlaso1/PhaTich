// @sonhg: class: ParticleController
using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour
{
	public void Awake()
	{
		this.pSystem = base.gameObject.GetComponent<ParticleSystem>();
	}

	public void Play()
	{
		this.pSystem.Simulate(Time.unscaledDeltaTime, true, true);
	}

	public void Update()
	{
		this.pSystem.Simulate(Time.unscaledDeltaTime, true, false);
	}

	private ParticleSystem pSystem;
}
