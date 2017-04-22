using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyController : MonoBehaviour {

	public bool touchedElvis = false;
	public bool paused = false;

	float aggroRange = 5f;
	float attractionForce = 0.01f;
	float maxVelocity = 2f;
	bool falling = false;


	int terrainLayer;
	GameObject elvis;
	Rigidbody rb;
	Animator anim;


	// Use this for initialization
	void Start () {
		aggroRange = Mathf.Pow (aggroRange, 2);
		terrainLayer = LayerMask.GetMask ("Terrain");
		elvis = GameObject.FindGameObjectWithTag ("Elvis");	
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (paused)
			return;
		
		Vector3 toElvis = (elvis.transform.position - transform.position);
		toElvis.y = 0;
		if (falling) {
			rb.velocity = rb.velocity + (new Vector3 (0, -2, 0) * Time.deltaTime);
			transform.Rotate (180 * Time.fixedDeltaTime,0,0);
		} else if (toElvis.sqrMagnitude < aggroRange) {
			// Walk towards elvis if in range.
			anim.SetBool ("walking", true);
			Vector3 newVel = Vector3.ClampMagnitude(rb.velocity + (toElvis * attractionForce), maxVelocity);
			newVel.y = 0;
			rb.velocity = newVel; 
			transform.LookAt (transform.position + rb.velocity);
		
			// Check if we're over the edge.
			if (! Physics.Raycast(transform.position + new Vector3(0,1,0), -Vector3.up,100,terrainLayer)){
				falling = true;
				anim.SetBool ("falling", true);
				anim.SetBool ("walking", false);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Elvis") {
			touchedElvis = true;
		}
	}
		
}
