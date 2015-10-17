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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isOpen = false;
        }
	}
    void OnGUI()
    {
        if (isOpen)
        {

            GUILayout.BeginArea(new Rect(Screen.width/2 - 200, Screen.height/2 - 200, 190, 400));
            foreach (PhotonPlayer pl in PhotonNetwork.playerList)
            {
                if(pl.customProperties["team"].ToString() == "red")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Name: " + pl.name);
                    GUILayout.Label("Kills: " + pl.customProperties["kills"].ToString());
                    GUILayout.Label("Deaths: " + pl.customProperties["death"].ToString());
                    GUILayout.Label("Score: " + pl.GetScore());
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, Screen.height / 2 - 200, 190, 400));
            foreach (PhotonPlayer pl in PhotonNetwork.playerList)
            {
                if (pl.customProperties["team"].ToString() == "blue")
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Name: " + pl.name);
                    GUILayout.Label("Kills: " + pl.customProperties["kills"].ToString());
                    GUILayout.Label("Deaths: " + pl.customProperties["death"].ToString());
                    GUILayout.Label("Score: " + pl.GetScore());
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndArea();
        }
    }
}
