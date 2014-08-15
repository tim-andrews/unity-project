using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public float fireRate = 0.5f;
	float coolDown = 0;
	public float damage = 25.0f;
	FXManager fxmanager;

	void Start() {
		fxmanager = GameObject.FindObjectOfType<FXManager>();

		if(fxmanager == null){
			Debug.Log("No FXManager");
		}
	}

	// Update is called once per frame
	void Update () {

		coolDown -= Time.deltaTime;

		if (Input.GetButtonDown ("Fire1")) {

			Fire ();
			Debug.Log("Fired Gun");
		}
	
	}

	void Fire() {

		if (coolDown > 0) {
			return;
		}

		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		Transform hitTransform;
		Vector3 hitPoint;

		hitTransform = FindClosestHitObject (ray, out hitPoint);

		if (hitTransform != null) {
			Debug.Log("We Hit" + hitTransform.name);
		

			Health h = hitTransform.transform.GetComponent<Health>();

			while(h == null && hitTransform.parent) {
				hitTransform = hitTransform.parent;
				h = hitTransform.GetComponent<Health>();
			}

			if(h != null){

				PhotonView pv = h.GetComponent<PhotonView>();

				if(pv == null){
				
				} else{

				h.GetComponent<PhotonView>().RPC ("TakeDamage", PhotonTargets.AllBuffered, damage);
				//h.TakeDamage(damage);
				} 

			}

			if (fxmanager != null) {
				fxmanager.GetComponent<PhotonView>().RPC("PistolBulletFX", PhotonTargets.All, Camera.main.transform.position, hitPoint);
				
			} else {

				if (fxmanager != null) {
					hitPoint = Camera.main.transform.position + (Camera.main.transform.forward * 100f);
					fxmanager.GetComponent<PhotonView>().RPC("PistolBulletFX", PhotonTargets.All, Camera.main.transform.position, hitPoint);
			
				}
			 
			}

		coolDown = fireRate;
	}
	}

	Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint) {

		RaycastHit[] hits = Physics.RaycastAll (ray);

		Transform closestHit = null;
		float distance = 0;
		hitPoint = Vector3.zero;


		foreach (RaycastHit hit in hits) {
			if(hit.transform != this.transform && (closestHit == null || hit.distance < distance)) {

				closestHit = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point; 
			}
		}

		return closestHit;

	}
}
