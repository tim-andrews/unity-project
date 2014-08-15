using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

	public GameObject pistolBulletFXPrefab;

	[RPC]
	void PistolBulletFX(Vector3 startPos, Vector3 endPos){

		if (pistolBulletFXPrefab != null) {
			GameObject pistolFX = (GameObject)Instantiate(pistolBulletFXPrefab, startPos, Quaternion.LookRotation(endPos - startPos));
		}

	}

}
