using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour {


	private int terrainMask;

	private NavMeshAgent agent;
	private Animator anim;

	// Use this for initialization
	void Start () {
		this.agent = GetComponent<NavMeshAgent> ();
		this.anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,100)) {
				this.agent.destination = hit.point;
			}
		}
		anim.SetBool ("move", this.agent.hasPath);
	}
}