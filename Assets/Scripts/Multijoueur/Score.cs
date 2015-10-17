#pragma warning disable 618
using CodeStage.AntiCheat.ObscuredTypes;


using UnityEngine;
using System.Collections;

public class Score : Photon.MonoBehaviour {

	public Transform personnage;
	public string equipe;
	public static bool estMort = true;
	public string pseudo;
	public GameObject chat;
    public Camera cam;
	public GameObject[] spawn_rouge;
	public GameObject[] spawn_bleu;
	public GUISkin skin;
    public static enumGameState gameState;

    void Start()
    {
        cam = null;
    }

    void Update()
    {
        if (GameObject.Find("CameraMainMenuBeforeSpawn") && cam == null)
        {
            cam = GameObject.Find("CameraMainMenuBeforeSpawn").GetComponent<Camera>();
        }
    }

    void OnGUI()
    {

        if (PhotonNetwork.room != null && equipe == "")
        {
            CursorGestion.setInMenu();

            GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            GUI.Label(new Rect(0, 0, Screen.width, 100), "", skin.customStyles[0]);
            GUI.color = Color.white;

            Wait.isStart = false;

            GUI.Box(new Rect(20, 25, 300, 50), LanguageManager.GetText("chooseATeam"), skin.customStyles[0]); //skin.window
            if (GUI.Button(new Rect(Screen.width / 2 - 40, 25, 100, 50), LanguageManager.GetText("equipeRouge"), skin.button))
            {
                equipe = "rouge";
                PhotonNetwork.player.customProperties.Add("team", "red");
                PhotonNetwork.player.customProperties.Add("kills", 0);
                PhotonNetwork.player.customProperties.Add("death", 0);
                PhotonNetwork.player.SetScore(0);
                chat.SendMessage("Equipe", equipe);
            }
            if (GUI.Button(new Rect(Screen.width / 2 + 65, 25, 100, 50), LanguageManager.GetText("equipeBleu"), skin.button))
            {
                equipe = "bleu";
                PhotonNetwork.player.customProperties.Add("team", "blue");
                PhotonNetwork.player.customProperties.Add("kills", 0);
                PhotonNetwork.player.customProperties.Add("death", 0);
                PhotonNetwork.player.SetScore(0);
                chat.SendMessage("Equipe", equipe);
            }
        }

        if (PhotonNetwork.room != null && equipe != "" && estMort == true)
        {
            cam.enabled = true;
            cam.GetComponent<AudioListener>().enabled = true;


            GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            GUI.Label(new Rect(0, 0, Screen.width, 100), "", skin.customStyles[0]);
            GUI.color = Color.white;
            if (RoundTime.IsItTimeYet) {
                if (GUI.Button(new Rect(Screen.width / 2 - 100, 25, 200, 50), LanguageManager.GetText("spawn"), skin.button))
                {
                    var aleatoire = Random.Range(0, 2);

                    Cursor.visible = false;

                    if (equipe == "rouge")
                    {

                        GameObject player = PhotonNetwork.Instantiate(personnage.name, spawn_rouge[aleatoire].transform.position, spawn_rouge[aleatoire].transform.rotation, 0);

                        cam.enabled = false;
                        cam.GetComponent<AudioListener>().enabled = false;

                        CursorGestion.setInGame();

                        GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, pseudo);

                        player.GetComponent<Animator>().SetBool("IsDead", false);
                    }
                    if (equipe == "bleu")
                    {

                        GameObject player = PhotonNetwork.Instantiate(personnage.name, spawn_bleu[aleatoire].transform.position, spawn_bleu[aleatoire].transform.rotation, 0);

                        cam.enabled = false;
                        cam.GetComponent<AudioListener>().enabled = false;

                        CursorGestion.setInGame();

                        GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, pseudo);

                        player.GetComponent<Animator>().SetBool("IsDead", false);
                    }

                    estMort = false;

                }
            }
        }
    }

	void GetName(string nom){
		pseudo = nom;
	}

    [PunRPC]
    void RefreshName(string[] donnee){

        GameObject go;
        
        go = GameObject.Find(donnee[0]);
        go.name = donnee[1];
    }
}
