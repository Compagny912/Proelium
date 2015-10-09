using UnityEngine;
using System.Collections;

public class CursorGestion : Photon.MonoBehaviour{

    private static Texture2D cursorMenu;
    private static Texture2D basic;
    private static Texture2D shoot;

    public void Start()
    {
        cursorMenu = Resources.Load("Curseur", typeof(Texture2D)) as Texture2D;
        basic = Resources.Load("CrossHair", typeof(Texture2D)) as Texture2D;
        shoot = Resources.Load("HitCrossHair", typeof(Texture2D)) as Texture2D;
    
    }

	public static void setInvisible()
    {
        Cursor.visible = false;
    }

    public static void setInMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursorMenu, new Vector2(0, 0), CursorMode.Auto);
    }

    public static void setInGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(basic, new Vector2(basic.height / 2, basic.width / 2), CursorMode.Auto);
    }

    public static IEnumerator hitPlayer()
    {
        Cursor.SetCursor(shoot, new Vector2(shoot.height/2, shoot.width/2), CursorMode.Auto);
        yield return new WaitForSeconds(0.3f);
        Cursor.SetCursor(basic, new Vector2(basic.height/2, basic.width/2), CursorMode.Auto);
    }
}
