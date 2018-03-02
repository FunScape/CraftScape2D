using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour {

	public NetworkManager manager;

	// Use this for initialization
	void Start () {
		manager = GetComponent<NetworkManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
