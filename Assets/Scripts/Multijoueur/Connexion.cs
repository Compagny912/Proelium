using UnityEngine;
using System.Collections;

public class Connexion : Photon.MonoBehaviour {

    public static string menu = "listeServeurs";
    public static bool isLoadingScene = false;
	public string pseudoJoueur;
	public GameObject chat;
	public GameObject score;
	public GUISkin style;
    public static Language defaultLanguage = Language.French;
	
	void Awake(){
		DontDestroyOnLoad(this);
		DontDestroyOnLoad(chat);
		DontDestroyOnLoad(score);
	}

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
        PhotonNetwork.ConnectUsingSettings("Version 1.0.1");
        PhotonNetwork.automaticallySyncScene = true;
    }

    void OnGUI()
    {
        if (PhotonNetwork.insideLobby == true && isLoadingScene == false && menu != "")
        {
            GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 250, 810, 30), LanguageManager.GetText("menuMultijoueur"), style.customStyles[0]);
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 250, 480, 30), "", style.customStyles[0]);

            //PSEUDO
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 195, 400, 10), LanguageManager.GetText("pseudo"), style.box);
            pseudoJoueur = GUI.TextArea(new Rect(Screen.width / 2, Screen.height / 2 - 180, 400, 30), pseudoJoueur, 12, style.textArea);

            if (menu == "listeServeurs")
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 150, 400, 30), LanguageManager.GetText("creerServeur"), style.button))
                {
                    menu = "creationServeur";
                }
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 200, 400, 30), LanguageManager.GetText("filtres"), style.button))
                {
                    menu = "filtres";
                }
            }

            if (menu == "filtres")
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 150, 400, 30), LanguageManager.GetText("creerServeur"), style.button))
                {
                    menu = "creationServeur";
                }
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 200, 400, 30), LanguageManager.GetText("appliquerFiltres"), style.button))
                {
                    menu = "listeServeurs";
                    //APPLIQUER LES FILTRES ICI
                }
            }

            if (menu == "creationServeur")
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 150, 400, 30), LanguageManager.GetText("liste"), style.button))
                {
                    menu = "listeServeurs";
                }
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 200, 400, 30), LanguageManager.GetText("filtres"), style.button))
                {
                    menu = "filtres";
                }
            }
        }
        if (PhotonNetwork.insideLobby == true && isLoadingScene == true)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 30, 300, 60), LanguageManager.GetText("levelLoading") + GetConnectingDots(), style.label);
        }
	}
	
	void OnCreatedRoom(){
		chat.SendMessage("Connecte", pseudoJoueur);
		score.SendMessage("GetName", pseudoJoueur);
	}

	void OnJoinedRoom(){
		chat.SendMessage("Connecte", pseudoJoueur);
		score.SendMessage("GetName", pseudoJoueur);
        Debug.Log("Connected to Room");
        isLoadingScene = false;
	}

    string GetConnectingDots()
    {
        string str = "";
        int numberOfDots = Mathf.FloorToInt(Time.timeSinceLevelLoad * 3f % 4);

        for (int i = 0; i < numberOfDots; ++i)
        {
            str += " .";
        }

        return str;
    }

}