using UnityEngine;
using System.Collections;

public class ScoreTable : MonoBehaviour {

    private bool isOpen;
    public GUISkin skin;

	// Use this for initialization
	void Start () {
	
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
                GUILayout.Label(pl.name);
                GUILayout.FlexibleSpace();
                GUILayout.Label(pl.customProperties["kills"].ToString());
                GUILayout.FlexibleSpace();
                GUILayout.Label(pl.customProperties["death"].ToString());
                GUILayout.FlexibleSpace();
                GUILayout.Label(pl.GetScore()+"");
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

            foreach (PhotonPlayer pl in PunTeams.PlayersPerTeam[PunTeams.Team.blue])
            {
                GUILayout.BeginHorizontal(skin.label);
                GUILayout.Label(pl.name);
                GUILayout.FlexibleSpace();
                GUILayout.Label(pl.customProperties["kills"].ToString());
                GUILayout.FlexibleSpace();
                GUILayout.Label(pl.customProperties["death"].ToString());
                GUILayout.FlexibleSpace();
                GUILayout.Label(pl.GetScore() + "");
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
