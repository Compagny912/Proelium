#pragma warning disable 618

using UnityEngine;
using System.Collections;

public class Score : Photon.MonoBehaviour {

	public Transform personnage;
	public string equipe;
	public static bool estMort = true;
	public string pseudo;
	public GameObject chat;
	public GameObject[] spawn_rouge;
	public GameObject[] spawn_bleu;
	public GUISkin skin;
    public Texture2D viseur;
    public Texture2D texture;
    public bool gameIsStart;
    public float timeBeforeStart = 120f;

    public static Language defaultLanguage = Language.French;

    void Start()
    {
        if (PlayerPrefs.GetString("Langage") == "French")
        {
            defaultLanguage = Language.French;
        }
        if (PlayerPrefs.GetString("Langage") == "English")
        {
            defaultLanguage = Language.English;
        }
        LanguageManager.LoadLanguageFile(defaultLanguage);
    }

    void Update()
    {
        if (PhotonNetwork.room != null && (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers))
        {
            PhotonNetwork.room.visible = false;
        }
    }

	void OnGUI(){
		
		if(PhotonNetwork.room != null && equipe == ""){
			
			if(equipe == ""){
				GUI.Box(new Rect(Screen.width/2-150, Screen.height/2-50, 300, 100), LanguageManager.GetText("choisirUneEquipe"), skin.window);
                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 15, 100, 50), LanguageManager.GetText("equipeRouge"), skin.button))
                {
					equipe = "rouge";
                    //PhotonNetwork.player.SetTeam(PunTeams.Team.red);
					chat.SendMessage("Equipe", equipe);
				}
				if(GUI.Button(new Rect(Screen.width/2+50, Screen.height/2-15, 100, 50), LanguageManager.GetText("equipeBleu"), skin.button)){
					equipe = "bleu";
                    //PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
					chat.SendMessage("Equipe", equipe);
				}
			}
		}

        if (PhotonNetwork.room != null && equipe != "" && estMort == true)
        {

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 200, 60), LanguageManager.GetText("spawn"), skin.button))
            {
				var aleatoire = Random.Range(0, 2);

                Cursor.visible = false;

				if(equipe == "rouge"){

                    GameObject player = (GameObject) PhotonNetwork.Instantiate(personnage.name, spawn_rouge[aleatoire].transform.position, spawn_rouge[aleatoire].transform.rotation, 0);

                    string[] donnee = new string[2];
                    donnee[0] = player.name;
                    donnee[1] = pseudo;
                    GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, donnee);

                    player.GetComponent<Animator>().SetBool("IsDead", false);
                }
				if(equipe == "bleu"){
                    
                    GameObject player = (GameObject) PhotonNetwork.Instantiate(personnage.name, spawn_bleu[aleatoire].transform.position, spawn_bleu[aleatoire].transform.rotation, 0);

                    string[] donnee = new string[2];
                    donnee[0] = player.name;
                    donnee[1] = pseudo;
                    GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, donnee);

                    player.GetComponent<Animator>().SetBool("IsDead", false);
                }

				estMort = false;
                
			}
		}
		
		if(estMort == true){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
		}

        if (estMort == false)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 16, 16), viseur);
        }
	}
	
	void GetName(string nom){
		pseudo = nom;
	}

    void GetState(float time)
    {
        timeBeforeStart = time;
        if (time == 0)
        {
            gameIsStart = true;
        }
        else
        {
            gameIsStart = false;
        }
    }

    [PunRPC]
    void RefreshName(string[] donnee){

        GameObject go;
        
        go = GameObject.Find(donnee[0]);
        go.name = donnee[1];
    }
}
