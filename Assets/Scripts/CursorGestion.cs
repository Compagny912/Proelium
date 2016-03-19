using UnityEngine;
using System.Collections;

public class CursorGestion : Photon.MonoBehaviour{

    private Texture2D cursorMenu;
    private Texture2D basic;
    private Texture2D shoot;
    public Texture2D bas;

    public void Start()
    {
        cursorMenu = Resources.Load("Curseur", typeof(Texture2D)) as Texture2D;
        basic = Resources.Load("CrossHair", typeof(Texture2D)) as Texture2D;
        shoot = Resources.Load("HitCrossHair", typeof(Texture2D)) as Texture2D;
    }
	public void setInvisible()
    {
        Cursor.visible = false;
    }

    public void setInMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursorMenu, new Vector2(0, 0), CursorMode.Auto);
    }
    public void setInGame()
    {
        Cursor.SetCursor(bas, new Vector2(bas.height / 2, bas.width / 2), CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public IEnumerator hitPlayer()
    {
        Cursor.SetCursor(shoot, new Vector2(shoot.height/2, shoot.width/2), CursorMode.Auto);
        yield return new WaitForSeconds(0.3f);
        Cursor.SetCursor(basic, new Vector2(basic.height/2, basic.width/2), CursorMode.Auto);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(300, 10, 500, 200), Cursor.visible ? "Visible - " : "Invisible : " + (Cursor.lockState == CursorLockMode.Locked ? "Lock" : "Non Lock"));
    }
}
