using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkKnightAnimation : MonoBehaviour {
	const float locomotionAnimationDampTime = .1f;
	NavMeshAgent agent;
	Animator animator;
	//while (damp time) smooths between the animations by 1/10 of a second
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponentInChildren<Animator> ();//searches through child object(in scene) for Animator

	}
	
	// Update is called once per frame
	void Update () {
		float speedPercent = agent.velocity.magnitude / agent.speed;
		animator.SetFloat ("speedPercent", speedPercent, locomotionAnimationDampTime, Time.deltaTime);
		//sets the scale in animator called speed percent to new speedPercent
}
}
