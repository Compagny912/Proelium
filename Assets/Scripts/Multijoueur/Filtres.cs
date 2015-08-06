using UnityEngine;
using System.Collections;

public class Filtres : MonoBehaviour {

	public GUISkin style;
    public static Language defaultLanguage = Language.French;

	void Start () {
        if (PlayerPrefs.GetString("Langage") == "French")
        {
            defaultLanguage = Language.French;
        }
        if (PlayerPrefs.GetString("Langage") == "English")
        {
            defaultLanguage = Language.English;
        }
        LanguageManager.LoadLanguageFile(defaultLanguage);
	}

	void Update () {
	
	}

	void OnGUI(){
		if (Connexion.menu == "filtres" && PhotonNetwork.insideLobby == true) {

            GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 250, 810, 530), "", style.window);
			GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 + 250, 330, 30), "", style.customStyles[0]); 



		}
	}
}
