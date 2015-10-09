using UnityEngine;
using System.Collections;

public class Filtres : Photon.MonoBehaviour {

	public GUISkin style;

	void Start () {

	}

	void Update () {
	
	}

	void OnGUI(){
		if (Connexion.menu == "filtres" && PhotonNetwork.insideLobby == true) {

            GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
            GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 250, 810, 530), "", style.window);
            GUI.color = Color.white;
			GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 + 250, 330, 30), "", style.customStyles[0]); 



		}
	}
}
