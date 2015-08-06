using UnityEngine;
using System.Collections;

public class ProeliumCursor : Photon.MonoBehaviour {

    public Texture2D texture;
    public Vector2 hotSpot = Vector2.zero;

	void Start () {
	}
	
	void Update () {
        if (Connexion.menu != "" || PauseMenuGUI.pausemenu != "")
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
            Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
        }
        /*else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }*/
	}
}
