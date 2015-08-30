using UnityEngine;
using System.Collections;

public class CreateServer : Photon.MonoBehaviour {
	
	public GUISkin style;
	public RoomOptions roomOptions;
    public Texture[] text;
	private bool isVisible;
    public string nomServeur;
    RoomOptions newRoomOptions;
    public static Language defaultLanguage = Language.French;
    public static string nameMap = "Map1";

	void Start () {
        if (PlayerPrefs.GetString("Langage") == "French")
        {
            defaultLanguage = Language.French;
        }
        if (PlayerPrefs.GetString("Langage") == "English")
        {
            defaultLanguage = Language.English;
        }
        LanguageManager.LoadLanguageFile(defaultLanguage);
        newRoomOptions = new RoomOptions() { isVisible = true, isOpen = true, cleanupCacheOnLeave = true};
        newRoomOptions.maxPlayers = 6;
	}

	void OnGUI()
	{
		if (Connexion.menu == "creationServeur" && PhotonNetwork.insideLobby == true)
		{
            GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
            GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 250, 810, 530), "", style.window);
            GUI.color = Color.white;

            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 195, 350, 10), LanguageManager.GetText("nomServeur"), style.box);
            nomServeur = GUI.TextArea(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 180, 350, 30), nomServeur, 12, style.textArea);

            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 125, 330, 10), LanguageManager.GetText("nombreJoueurs"), style.box);
            if (GUI.Button(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 110, 110, 30), "6", (newRoomOptions.maxPlayers != 6) ? style.button : style.customStyles[2]))
            {
                newRoomOptions.maxPlayers = 6;
			}
            if (GUI.Button(new Rect(Screen.width / 2 - 290, Screen.height / 2 - 110, 110, 30), "12", (newRoomOptions.maxPlayers != 12) ? style.button : style.customStyles[2]))
            {
                newRoomOptions.maxPlayers = 12;
			}
            if (GUI.Button(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 110, 110, 30), "18", (newRoomOptions.maxPlayers != 18) ? style.button : style.customStyles[2]))
            {
                newRoomOptions.maxPlayers = 18;
			}

            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 65, 330, 10), LanguageManager.GetText("choisirMap"), style.box);

            if (GUI.Button(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 50, 110, 125), text[0], (nameMap != "Map1") ? style.button : style.customStyles[2]))
            {
                nameMap = "Map1";
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 290, Screen.height / 2 - 50, 110, 125), text[1], (nameMap != "Map2") ? style.button : style.customStyles[2]))
            {
                nameMap = "Map1";
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 50, 110, 125), text[2], (nameMap != "Map3") ? style.button : style.customStyles[2]))
            {
                nameMap = "Map1";
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 405, Screen.height / 2 + 250, 330, 30), "<color=orange>" + LanguageManager.GetText("creerServeur") + "</color>", style.button) && newRoomOptions.maxPlayers != 0 && nomServeur.Length >= 2 && this.GetComponent<Connexion>().pseudoJoueur.Length >= 3)
            {
				PhotonNetwork.CreateRoom(nomServeur, newRoomOptions, TypedLobby.Default);
				PhotonNetwork.LoadLevel(nameMap);
                Connexion.menu = "";
                Connexion.isLoadingScene = true;
			}

		}
	}

}