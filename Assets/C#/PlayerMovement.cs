using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


	//this componet is only enabled for myplayer

	float speed = 5f;
	float JumpSeed = 5f;
	Vector3 direction = Vector3.zero;
	float verticalVelocity = 0;

	CharacterController cc;
	Animator anim;
	//CapsuleCollider capsCollider;
	float realCapsuleCollider = 2f;
	float jumpCapsuleCollider = 1.2f;


	// Use this for initialization
	void Start () {

		cc = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		//capsCollider = (CapsuleCollider)collider;
	}
	
	// Update is called once per frame
	void Update () {

		direction = transform.rotation * new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
	
		if (direction.magnitude > 1f){
			direction = direction.normalized;
		}

		anim.SetFloat ("Speed", direction.magnitude);

		if(cc.isGrounded) {
			anim.SetBool ("Jumping", false);
			//capsCollider.height = realCapsuleCollider;


			if(cc.isGrounded && Input.GetButtonDown ("Jump")) {
				verticalVelocity = JumpSeed;
			}
			else {	
				verticalVelocity = 0;
			}
		}
		else {
				anim.SetBool("Jumping", true);
			//capsCollider.height = jumpCapsuleCollider;
		}
	}
	// is called once per physics loop10f
	void FixedUpdate () {

		Vector3 distance = direction * speed * Time.deltaTime;

		if (cc.isGrounded && verticalVelocity < 0){

			anim.SetBool("Jumping", false);
			verticalVelocity = Physics.gravity.y * Time.deltaTime;

		}
		else {
			if(Mathf.Abs(verticalVelocity) > JumpSeed * 0.75f) {
				anim.SetBool ("Jumping", true);
			}

			verticalVelocity += Physics.gravity.y * Time.deltaTime;
		}



		distance.y = verticalVelocity * Time.deltaTime;

		cc.Move (distance);

	}
}
