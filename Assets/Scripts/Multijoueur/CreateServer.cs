using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class CreateServer : Photon.MonoBehaviour {
	
	public GUISkin style;
	public RoomOptions roomOptions;
    public Texture[] text;
	private bool isVisible;
    public string nomServeur;
    RoomOptions newRoomOptions;
    private static string nameMap = "Map1";
    private static ObscuredString gamemode = "deathmatch";

	void Start () {
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

            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 195, 350, 10), LanguageManager.GetText("serverName"), style.box);
            nomServeur = GUI.TextArea(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 180, 350, 30), nomServeur, 12, style.textArea);

            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 125, 330, 10), LanguageManager.GetText("numberOfPlayers"), style.box);
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

            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 65, 330, 10), LanguageManager.GetText("chooseAMap"), style.box);

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

            if (GUI.Button(new Rect(Screen.width / 2 - 405, Screen.height / 2 + 250, 330, 30), "<color=orange>" + LanguageManager.GetText("createTheRoom") + "</color>", style.button) && newRoomOptions.maxPlayers != 0 && nomServeur.Length >= 2 && this.GetComponent<Connexion>().pseudoJoueur.GetEncrypted().Length >= 3)
            {
                Wait.isStart = true;
                Connexion.menu = "";
                Connexion.isLoadingScene = true;
                PhotonNetwork.CreateRoom(nomServeur, newRoomOptions, TypedLobby.Default);
                PhotonNetwork.LoadLevel(nameMap);
                CursorGestion.setInvisible();
            }

		}
	}

    void OnPhotonCreateRoomFailed()
    {
        showMessage.inputMessage(LanguageManager.GetText("randomFailed"));
    }
}