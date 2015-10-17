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
    void OnPhotonPlayerConnected()
    {
        if (PhotonNetwork.room.playerCount < 2)
        {
            showMessage.inputMessage("Waiting Other players ...");
        }
    }
    void OnPhotonPlayerDisconnected()
    {
        if (PhotonNetwork.room.playerCount < 2)
        {
            showMessage.inputMessage("Waiting Other players ..."); 
        }
    }
}
