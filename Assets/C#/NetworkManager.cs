using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public GameObject standbyCamera;
	SpawnScript[] spawnSpots;

	public bool offlineMode;
	bool connecting = false;

	List<string> chatMessages;
	int maxChatMessages = 5;


	// Use this for initialization
	void Start () {

		spawnSpots = GameObject.FindObjectsOfType<SpawnScript> ();
		PhotonNetwork.player.name = PlayerPrefs.GetString ("Username", "SpecialFriend");
		chatMessages = new List<string>();
		//Connect ();

	}

	void OnDestroy () {
		PlayerPrefs.SetString ("Username", PhotonNetwork.player.name);
	}

	public void AddChatMessage (string m) {
		GetComponent<PhotonView>().RPC("AddChatMessage_RPC", PhotonTargets.AllBuffered, m );
	}

	[RPC]
	public void AddChatMessage_RPC (string m) {
		while (chatMessages.Count >= maxChatMessages) {
			chatMessages.RemoveAt (0);
		}
		chatMessages.Add (m);
	}

	void Connect() {

		//if (offlineMode) {
		//	PhotonNetwork.offlineMode = true;
		//	OnJoinedLobby();
		//} else {
			PhotonNetwork.ConnectUsingSettings ("FPS 1.0.0");
		    
		//}
	}

	void OnGUI() {
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());

		if (PhotonNetwork.connected == false && connecting == false) {

			GUILayout.BeginArea(new Rect(0, 0, Screen.width,Screen.height));
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginVertical ();

			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Username: ");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
			GUILayout.EndHorizontal ();


			GUILayout.FlexibleSpace ();

			if (GUILayout.Button("Single Player")) {
				connecting = true;
				PhotonNetwork.offlineMode = true;
				OnJoinedLobby();
			}

			if (GUILayout.Button("Multiplayer")) {
				connecting = true;
				Connect ();
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndVertical ();
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea();
		}

		if (PhotonNetwork.connected == true && connecting == false) {

			GUILayout.BeginArea(new Rect(0, 0, Screen.width,Screen.height));
			GUILayout.BeginVertical ();
			GUILayout.FlexibleSpace ();

			foreach(string msg in chatMessages){
				GUILayout.Label (msg);
			}
		
			GUILayout.EndVertical ();
			GUILayout.EndArea();
		}

	}

	void OnJoinedLobby() {
		Debug.Log ("OnJoindedLobby");
		PhotonNetwork.JoinRandomRoom ();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom (null);
	}

	void OnJoinedRoom() {
		Debug.Log ("OnJoinedRoom");
		connecting = false;
		SpawnMyPlayer ();
	}

	void SpawnMyPlayer() {
		AddChatMessage ("Spawning player:" + PhotonNetwork.player.name);

		if(spawnSpots == null){
			Debug.LogError ("Spawn's Broken");
			return;
		}

		SpawnScript mySpawnSpot = spawnSpots [Random.Range (0, spawnSpots.Length)];

		GameObject myPlayerObj = PhotonNetwork.Instantiate ("FPSController", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		standbyCamera.SetActive(false);
		//((MonoBehaviour)myPlayerObj.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerObj.GetComponent("PlayerShooting")).enabled = true;
		((MonoBehaviour)myPlayerObj.GetComponent("PlayerMovement")).enabled = true;
		((MonoBehaviour)myPlayerObj.GetComponent("MouseLook")).enabled = true;
		myPlayerObj.transform.FindChild ("Main Camera").gameObject.SetActive(true);
	} 
}

