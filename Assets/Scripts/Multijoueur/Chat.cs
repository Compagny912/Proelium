using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;

public class Chat : Photon.MonoBehaviour {

	string[] ligneChat;
	public string texteChat;
	public string pseudo;
	public string texteModifie;
	public GUISkin style;

	void Start(){

		ligneChat = new string[7];

	}

	void OnGUI () {

        showMessage.inputMessage(GUI.GetNameOfFocusedControl());

		if(PhotonNetwork.room != null){
			
			//CHAT--------------------------------
			GUI.Label(new Rect(0, Screen.height-180, 400, 20), ligneChat[6], style.customStyles[1]);
			GUI.Label(new Rect(0, Screen.height-160, 400, 20), ligneChat[5], style.customStyles[1]);
			GUI.Label(new Rect(0, Screen.height-140, 400, 20), ligneChat[4], style.customStyles[1]);
			GUI.Label(new Rect(0, Screen.height-120, 400, 20), ligneChat[3], style.customStyles[1]);
			GUI.Label(new Rect(0, Screen.height-100, 400, 20), ligneChat[2], style.customStyles[1]);
			GUI.Label(new Rect(0, Screen.height-80, 400, 20), ligneChat[1], style.customStyles[1]);
			GUI.Label(new Rect(0, Screen.height-60, 400, 20), ligneChat[0], style.customStyles[1]);
			//FIN DE CHAT--------------------------------

			GUI.SetNextControlName ("texte");
			texteChat = GUI.TextField(new Rect(5, Screen.height-40, 295, 40), texteChat, 50);

            /*

            if(Input.GetKey(KeyCode.T) && GUI.GetNameOfFocusedControl() != "texte")
            {
                print("test0");
                GUI.FocusControl("texte");
                return;
            }
            if(Input.GetKeyDown(KeyCode.Escape) && GUI.GetNameOfFocusedControl() == "texte")
            {
                print("test1");
                GUI.FocusControl(null);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Return) && GUI.GetNameOfFocusedControl() == "texte")
            {
                print("test3");
                GUI.FocusControl(null);
                if (texteChat != "")
                {
                    print("test4");
                    texteModifie = pseudo + ": " + texteChat;
                    GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, texteModifie);
                    texteChat = "";
                    return;
                }
                return;
               
            } */ //DOESNT WORK DOESNT WORK DOESNT WORK DOESNT WORK
		}
	}
	
	void OnPhotonPlayerConnected(PhotonPlayer p)
    {
		GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, "Le joueur " + PhotonNetwork.playerName + " s'est connecté");
	}

    void OnPhotonPlayerDisconnected(PhotonPlayer p)
    {
        GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, "Le joueur " + PhotonNetwork.playerName + " s'est déconnecté");
    }
    void Message(string message)
    {
        GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, message);
    }
	
	void Equipe(string couleur){
		var texteEquipe = PhotonNetwork.playerName + " rejoins l'équipe: " + couleur;
		GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, texteEquipe);
	}
	
	[PunRPC]
	void RafraichirChat (string texte)
    {
		ligneChat[6] = ligneChat[5];
		ligneChat[5] = ligneChat[4];
		ligneChat[4] = ligneChat[3];
		ligneChat[3] = ligneChat[2];
		ligneChat[2] = ligneChat[1];
		ligneChat[1] = ligneChat[0];
		ligneChat[0] = texte;
	}
}