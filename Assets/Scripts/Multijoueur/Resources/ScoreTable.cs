using System.IO;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class ScoreTable : Photon.MonoBehaviour {

    private bool isOpen;
    public GUISkin skin;
    ObscuredString gamemode = "mme";

	// Use this for initialization
	void Start () {
    }

    public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("gamemode") && PhotonNetwork.inRoom)
        {
            gamemode = (string)propertiesThatChanged[gamemode];
            showMessage.inputMessage(gamemode);
        }
    }

    // Update is called once per frame
    void Update () {

        if (PhotonNetwork.inRoom)
        {

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isOpen = true;
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                isOpen = false;
            }
        }
	}
    void OnGUI()
    {
        if (isOpen)
        {
            object gamemod;
            PhotonNetwork.room.customProperties.TryGetValue("gm", out gamemod);

            if ((string)gamemod != "mcc")
            {
                GUI.skin = this.skin;

                GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                GUILayout.BeginArea(new Rect(Screen.width / 2 - 455, Screen.height / 2 - 250, 450, 500), skin.box);
                GUI.color = Color.white;

                GUI.Label(new Rect(0, 7, 450, 30), "", skin.window);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("<b><color=red>Rouges</color></b>");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal((skin.label));
                GUILayout.Label("Name");
                GUILayout.Space(150);
                GUILayout.FlexibleSpace();
                GUILayout.Label("Kills");
                GUILayout.FlexibleSpace();
                GUILayout.Label("Deaths");
                GUILayout.FlexibleSpace();
                GUILayout.Label("Score");
                GUILayout.EndHorizontal();

                foreach (PhotonPlayer pl in PunTeams.PlayersPerTeam[PunTeams.Team.red])
                {
                    GUILayout.BeginHorizontal(skin.label);
                    string n = pl.name;
                    while (n.Length < 16)
                    {
                        n += " ";
                    }
                    GUILayout.Label(n);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(pl.customProperties["kills"].ToString());
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(pl.customProperties["death"].ToString());
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(pl.GetScore() + "");
                    GUILayout.EndHorizontal();
                }
                foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (pl.GetComponent<AI>() && pl.GetComponent<AI>().getTeam() == "blue")
                    {
                        GUILayout.BeginHorizontal(skin.label);
                        string n = pl.name;
                        while (n.Length < 16)
                        {
                            n += " ";
                        }
                        GUILayout.Label(n);
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(pl.GetComponent<AI>().getKills().ToString());
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(pl.GetComponent<AI>().getDeaths().ToString());
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(pl.GetComponent<AI>().getScore().ToString());
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndArea();

                GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                GUILayout.BeginArea(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 250, 450, 500), skin.box);
                GUI.color = Color.white;

                GUI.Label(new Rect(0, 7, 450, 30), "", skin.window);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("<b><color=teal>Bleus</color></b>");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(skin.label);
                GUILayout.Label("Name");
                GUILayout.Space(150);
                GUILayout.FlexibleSpace();
                GUILayout.Label("Kills");
                GUILayout.FlexibleSpace();
                GUILayout.Label("Deaths");
                GUILayout.FlexibleSpace();
                GUILayout.Label("Score");
                GUILayout.EndHorizontal();

                foreach (PhotonPlayer pl in PunTeams.PlayersPerTeam[PunTeams.Team.blue])
                {
                    GUILayout.BeginHorizontal(skin.label);
                    string n = pl.name;
                    while (n.Length < 16)
                    {
                        n += " ";
                    }
                    GUILayout.Label(pl.name);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(pl.customProperties["kills"].ToString());
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(pl.customProperties["death"].ToString());
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(pl.GetScore() + "");
                    GUILayout.EndHorizontal();
                }

                foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (pl.GetComponent<AI>() && pl.GetComponent<AI>().getTeam() == "red")
                    {
                        GUILayout.BeginHorizontal(skin.label);
                        string n = pl.name;
                        while (n.Length < 16)
                        {
                            n += " ";
                        }
                        GUILayout.Label(n);
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(pl.GetComponent<AI>().getKills().ToString());
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(pl.GetComponent<AI>().getDeaths().ToString());
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(pl.GetComponent<AI>().getScore().ToString());
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndArea();
            }

            if ((string)gamemod == "mcc")
            {

            }
        }
    }
}
