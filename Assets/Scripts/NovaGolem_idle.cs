// @sonhg: class: NovaGolem_idle
using System;
using BombOffline;
using UnityEngine;

public class NovaGolem_idle : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Offline_BaseCharactersController component = animator.transform.GetComponent<Offline_BaseCharactersController>();
		if (component != null)
		{
			component.canAct = true;
		}
	}
}
