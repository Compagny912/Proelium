using UnityEngine;
using System.Collections;

public class SyncValues : Photon.MonoBehaviour {

	void Start () {
	    
	}
	
	void Update () {
	
	}

    void OnGUI()
    {

    }
    void OnJoinedRoom()
    {
        if (PhotonNetwork.room.playerCount <= 2)
        {
            showMessage.inputMessage("Waiting Other players ...");
        }
    }
}
