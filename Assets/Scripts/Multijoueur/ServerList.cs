using UnityEngine;
using System.Collections;

public class ServerList : Photon.MonoBehaviour {
    public GUISkin Skin;

    private Vector2 scrollPos = new Vector2();
    private RoomInfo[] roomsList;
    private bool randomFailed = false;

    public static Language defaultLanguage = Language.French;

    GUIStyle m_Headline;

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
        m_Headline = new GUIStyle(Skin.label);
        m_Headline.padding = new RectOffset(Screen.width/2-375, Screen.height/2-250, 0, 0);
    }

    void OnGUI()
    {
		if (Connexion.menu == "listeServeurs" && PhotonNetwork.insideLobby == true)
        {

            GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
            GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 250, 810, 530), "", Skin.window);
            GUI.color = Color.white;

            if (GUI.Button(new Rect(Screen.width / 2 - 405, Screen.height / 2 + 250, 330, 30), "<color=orange>" + LanguageManager.GetText("rejoindreUnePartie") + "</color>", Skin.button) && this.GetComponent<Connexion>().pseudoJoueur.Length >= 3)
            {
				PhotonNetwork.JoinRandomRoom();
                randomFailed = false;
                Connexion.menu = "";
                Connexion.isLoadingScene = true;
			}

			GUI.skin = this.Skin;
            GUILayout.Space(Screen.height / 2 - 220); //OK

            GUILayout.BeginHorizontal();
            GUILayout.Space(Screen.width/2-400); //OK
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(330), GUILayout.Height(470));

			//GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 220, 330, 30), "<color=orange>" + LanguageManager.GetText("liste") + "</color>", Skin.customStyles[0]);

            if (PhotonNetwork.GetRoomList().Length != 0)
            {
                foreach (RoomInfo game in roomsList)
                {
                    if (game.playerCount < game.maxPlayers)
                    {
                        GUILayout.Box(game.name + " - " + game.playerCount + "/" + game.maxPlayers + " - Map: Map1");
                        if (GUILayout.Button(LanguageManager.GetText("rejoindre")) && this.GetComponent<Connexion>().pseudoJoueur.Length >= 3)
                        {
                            PhotonNetwork.JoinRoom(game.name);
                            Connexion.menu = "";
                            Connexion.isLoadingScene = true;
                        }
                    }
                }
            }
            if (PhotonNetwork.GetRoomList().Length == 0)
            {
                GUILayout.Label(LanguageManager.GetText("noServeur"), GUILayout.Width(300));
            }

            GUILayout.EndScrollView();

            GUILayout.BeginVertical(GUILayout.Width(Screen.width - 345));
            GUILayout.Space(10);
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

            if (randomFailed)
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 220, 470, 25), LanguageManager.GetText("randomFailed"), Skin.label);
            }
        }
    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }

	void OnPhotonRandomJoinFailed()
    {
        randomFailed = true;
	}
    void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Map1");
    }
    void OnPhotonCreateRoomFailed()
    {
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 220, 470, 25), "Nom de serveur déjà utilisé, prenez-en un autre.", Skin.label);
    }
}