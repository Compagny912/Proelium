using System.IO;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;

public class ScoreTable : Photon.MonoBehaviour {

    private bool isOpen;
    public GUISkin skin;
    ObscuredString gamemode = "mme";

    ArrayList listRED = new ArrayList();
    ArrayList listBLUE = new ArrayList();
    ArrayList listNOTEAM = new ArrayList();


    // Use this for initialization
    void Start () {
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
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
                listRED.Clear();
                listBLUE.Clear();
                foreach (PhotonPlayer pl in PunTeams.PlayersPerTeam[PunTeams.Team.red])
                {
                    listRED.Add(pl.GetScore() + " " + pl.name + " " + pl.GetKills() + " " + pl.GetDeaths());
                }
                foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (pl.layer == 18 && pl.GetComponent<AI>())
                    {
                        listRED.Add(pl.GetComponent<AI>().getScore() + " " + pl.name + " " + pl.GetComponent<AI>().getKills() + " " + pl.GetComponent<AI>().getDeaths());
                    }
                    if (pl.layer == 17 && pl.GetComponent<AI>())
                    {
                        listBLUE.Add(pl.GetComponent<AI>().getScore() + " " + pl.name + " " + pl.GetComponent<AI>().getKills() + " " + pl.GetComponent<AI>().getDeaths());
                    }
                }

                foreach (PhotonPlayer pl in PunTeams.PlayersPerTeam[PunTeams.Team.blue])
                {
                    listBLUE.Add(pl.GetScore() + " " + pl.name + " " + pl.GetKills() + " " + pl.GetDeaths());
                }

                listRED.Sort();
                listBLUE.Sort();
                listRED.Reverse();
                listBLUE.Reverse();
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

                foreach(string s in listRED)
                {
                    string[] str = s.Split(' ');
                    GUILayout.BeginHorizontal(skin.label);
                    string n = str[1];
                    while (n.Length < 16)
                    {
                        n += " ";
                    }
                    GUILayout.Label(n);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(str[2]);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(str[3]);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(str[0]);
                    GUILayout.EndHorizontal();
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

                foreach (string s in listBLUE)
                {
                    string[] str = s.Split(' ');
                    GUILayout.BeginHorizontal(skin.label);
                    string n = str[1];
                    while (n.Length < 16)
                    {
                        n += " ";
                    }
                    GUILayout.Label(n);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(str[2]);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(str[3]);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(str[0]);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndArea();
            }

            if ((string)gamemod == "mcc")
            {

            }
        }
    }
}
