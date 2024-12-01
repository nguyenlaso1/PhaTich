// @sonhg: class: Special1
using System;
using BombOffline;
using UnityEngine;

public class Special1 : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Offline_BaseCharactersController component = animator.transform.GetComponent<Offline_BaseCharactersController>();
		if (component != null)
		{
			component.OnSpecial1AnimationExit();
		}
	}
}
