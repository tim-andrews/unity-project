﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float hitPoints = 100f;
	float currentHitPoints;

	// Use this for initialization
	void Start () {

		currentHitPoints = hitPoints;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	public void TakeDamage (float amt) {

		currentHitPoints -= amt;

		if (currentHitPoints <= 0) {
			Die ();
		}
	}

	void Die () {
		if (GetComponent<PhotonView>().instantiationId == 0) {

			Destroy (gameObject);
		}
		else {
			if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.Destroy (gameObject);
			}
		}
	}
}
