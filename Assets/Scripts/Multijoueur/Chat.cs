using UnityEngine;
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

            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
            {
                if (!string.IsNullOrEmpty(this.texteChat) || GUI.GetNameOfFocusedControl() == "texte")
                {
                    GUI.FocusControl(null); 
                    texteModifie = pseudo + ": " + texteChat;
                    GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, texteModifie);
                    texteChat = "";
                    return;
                }
                else
                {
                    GUI.FocusControl("texte");
                }
			}
		}
	}
	
	void Connecte(string nom)
    {
		pseudo = nom;
		nom = "Le joueur " + nom + " s'est connecté";
		GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, nom);
	}
	void Deconnecte(string monpseudo)
    {
		pseudo = monpseudo;
		pseudo = "Le joueur " + monpseudo + " s'est déconnecté";
		GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, pseudo);
	}
    void Message(string message)
    {
        GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, message);
    }
	
	void Equipe(string couleur){
		var texteEquipe = pseudo + " rejoins l'équipe: " + couleur;
		GetComponent<PhotonView>().RPC("RafraichirChat", PhotonTargets.All, texteEquipe);
	}
	
	[RPC]
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