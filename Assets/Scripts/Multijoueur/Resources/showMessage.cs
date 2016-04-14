using UnityEngine;
using System.Collections;

public class showMessage : Photon.MonoBehaviour {

    public GUISkin style;
    private static float timeShowMessage = 3f;
    private static float elapsedtime;
    private static bool alreadyShow = false;
    private static int w;
    private static string message;

    //public string testline;
    void Start()
    {

    }
	void Update () {
        if (alreadyShow && elapsedtime > 0)
        {
            elapsedtime -= Time.deltaTime;
        }
        else if (elapsedtime <= 0)
        {
            alreadyShow = false;
            w = 0;
            message = "";
            elapsedtime = timeShowMessage;
        }
	}

    void OnGUI()
    {
        /*testline = GUI.TextField(new Rect(0, 50, 200, 50), testline);
        if(GUI.Button(new Rect(50, 0, 100, 50), "TEST") && testline.Length != 0)
        {
            inputMessage(testline);
            testline = "";
        }*/

        if (alreadyShow)
        {
            GUI.color = elapsedtime > 0.6 ? new Color(1.0f, 1.0f, 1.0f, 0.6f) : new Color(1.0f, 1.0f, 1.0f, elapsedtime);
            GUI.Label(new Rect(Screen.width / 2 - w / 2, Screen.height / 2 - 20, w, 40), message, style.button);
        }
    }

    public static void inputMessage(string msg)
    {
        elapsedtime = timeShowMessage;
        msg = msg.Replace("%PLAYER%", PhotonNetwork.player.name);
        if (alreadyShow)
        {
            message = msg;
            w = 15 * msg.Length;
        }

        else
        {
            message = msg;
            w = (15 * msg.Length);
            alreadyShow = true;
        }
    }
}
