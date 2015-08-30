using UnityEngine;
using System.Collections;

public class PauseMenuGUI : Photon.MonoBehaviour {

    public GUISkin style;
    public static string pausemenu = "";
    bool isOpenQuality;
    public int sensibility;

    public Texture isChecked;
    public Texture isNoChecked;

    bool isOpenLanguage;
    private Vector2 scrollLanguagePos = new Vector2();

    bool isFullScreen;
    public static Language defaultLanguage = Language.French;

	// Use this for initialization
	void Start () {

        sensibility = (int) PlayerPrefs.GetFloat("MouseSensibility");

        isOpenQuality = false;
        isOpenLanguage = false;
        isFullScreen = (PlayerPrefs.GetInt("FullScreen") == 0) ? false : true;
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
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (pausemenu == "options")
            {
                pausemenu = "pause";
                isOpenQuality = false;
                isOpenLanguage = false;
            }
            else if (pausemenu == "pause")
            {
                pausemenu = "";
                Connexion.menu = "listeServeurs";
            }
            else if (pausemenu == "")
            {
                pausemenu = "pause";
                Connexion.menu = "";
            }
        }
	}
    void OnGUI()
    {
        GUI.skin = this.style;
        if ((PhotonNetwork.connected == true) && pausemenu != "")
        {
            if (pausemenu == "pause")
            {
                GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400), LanguageManager.GetText("menu"), style.window);
                GUI.color = Color.white;

                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 120, 300, 30), LanguageManager.GetText("options")))
                {
                    pausemenu = "options";
                }

                if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height / 2 + 180, 300, 40), LanguageManager.GetText("quitter")))
                {
                    Application.Quit();
                    PlayerPrefs.Save();
                }
            }

            if (pausemenu == "options")
            {
                GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 150, 600, 400), LanguageManager.GetText("options"), style.window);
                GUI.color = Color.white;

                if (GUI.Button(new Rect(Screen.width / 2 - 285, Screen.height / 2 - 115, 280, 30), LanguageManager.GetText("langage") + " - " +
                    (defaultLanguage == Language.French ? LanguageManager.GetText("french") : "") +
                    (defaultLanguage == Language.English ? LanguageManager.GetText("english") : "") +
                    (defaultLanguage == Language.Italian ? LanguageManager.GetText("italian") : "") +
                    (defaultLanguage == Language.Russian ? LanguageManager.GetText("russian") : "") +
                    (defaultLanguage == Language.Chinese ? LanguageManager.GetText("chinese") : "") +
                    (defaultLanguage == Language.Spanish ? LanguageManager.GetText("spanish") : ""),
                    style.button))
                {
                    if (isOpenLanguage == true)
                    {
                        isOpenLanguage = false;
                    }
                    else
                    {
                        isOpenLanguage = true;
                        isOpenQuality = false;
                    }
                }

                if (GUI.Button(new Rect(Screen.width / 2 - 290, Screen.height / 2 + 210, 580, 30), LanguageManager.GetText("retourAuMenu")))
                {
                    pausemenu = "pause";
                }

                if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 115, 280, 30), LanguageManager.GetText("quality") + " - " + 
                    (QualitySettings.GetQualityLevel() == 0 ? LanguageManager.GetText("low") : "") +
                    (QualitySettings.GetQualityLevel() == 1 ? LanguageManager.GetText("good") : "") +
                    (QualitySettings.GetQualityLevel() == 2 ? LanguageManager.GetText("fantastic") : "") +
                    (QualitySettings.GetQualityLevel() == 3 ? LanguageManager.GetText("wonderful") : ""), 
                    style.button))
                {
                    if (isOpenQuality == true)
                    {
                        isOpenQuality = false;
                    } else {
                        isOpenQuality = true;
                        isOpenLanguage = false;
                    }
                }

                if (isOpenQuality == false)
                {
                    if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 75, 280, 30), (PlayerPrefs.GetInt("Fullscreen") == 0 ? LanguageManager.GetText("windowed") : LanguageManager.GetText("fullscreen"))))
                    {
                        if (isFullScreen)
                        {
                            isFullScreen = false;
                            PlayerPrefs.SetInt("Fullscreen", 0);
                            Screen.SetResolution(Screen.width, Screen.height, false);
                        }
                        else
                        {
                            isFullScreen = true;
                            PlayerPrefs.SetInt("Fullscreen", 1);
                            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                        }
                    }
                    if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 35, 280, 30), LanguageManager.GetText("defaultScreen")))
                    {
                        isFullScreen = true;
                        PlayerPrefs.SetInt("Fullscreen", 1);
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                    }
                }

                if (isOpenLanguage == false)
                {

                    if (PlayerPrefs.GetInt("InverseAxeY") == 1)
                    {
                        GUI.DrawTexture(new Rect(Screen.width / 2 - 35, Screen.height / 2 - 35, 30, 30), isChecked);
                    }
                    if (PlayerPrefs.GetInt("InverseAxeY") == 0)
                    {
                        GUI.DrawTexture(new Rect(Screen.width / 2 - 35, Screen.height / 2 - 35, 30, 30), isNoChecked);
                    }

                    if (GUI.Button(new Rect(Screen.width / 2 - 285, Screen.height / 2 - 35, 250, 30), LanguageManager.GetText("invertYAxe")))
                    {
                        if (PlayerPrefs.GetInt("InverseAxeY") == 0)
                        {
                            PlayerPrefs.SetInt("InverseAxeY", 1);
                        } else {
                            PlayerPrefs.SetInt("InverseAxeY", 0);
                        }
                    }

                    if (GUI.Button(new Rect(Screen.width / 2 - 285, Screen.height / 2 - 75, 30, 30), "-")  && sensibility > 1)
                    {
                        sensibility--;
                        PlayerPrefs.SetFloat("MouseSensibility", sensibility);
                    }
                    GUI.Label(new Rect(Screen.width / 2 - 255, Screen.height / 2 - 75, 220, 30), LanguageManager.GetText("sensibility") + ": " + sensibility, style.button);

                    if (GUI.Button(new Rect(Screen.width / 2 - 35, Screen.height / 2 - 75, 30, 30), "+") && sensibility < 10)
                    {
                        sensibility++;
                        PlayerPrefs.SetFloat("MouseSensibility", sensibility);
                    }
                }

                if (isOpenQuality == true)
                {
                    isOpenLanguage = false;
                    if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 85, 280, 30), LanguageManager.GetText("low"), (PlayerPrefs.GetString("Quality") != "low") ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Quality", "low");
                        QualitySettings.SetQualityLevel(0);
                        isOpenQuality = false;
                    }
                    if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 55, 280, 30), LanguageManager.GetText("good"), (PlayerPrefs.GetString("Quality") != "good") ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Quality", "good");
                        QualitySettings.SetQualityLevel(1);
                        isOpenQuality = false;
                    }
                    if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 25, 280, 30), LanguageManager.GetText("fantastic"), (PlayerPrefs.GetString("Quality") != "fantastic") ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Quality", "fantastic");
                        QualitySettings.SetQualityLevel(2);
                        isOpenQuality = false;
                    }
                    if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 + 5, 280, 30), LanguageManager.GetText("wonderful"), (PlayerPrefs.GetString("Quality") != "wonderful") ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Quality", "wonderful");
                        QualitySettings.SetQualityLevel(3);
                        isOpenQuality = false;
                    }
                }
                if (isOpenLanguage == true)
                {
                    isOpenQuality = false;

                    GUILayout.Space(Screen.height / 2 - 85); //OK

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(Screen.width / 2 - 285); //OK
                    scrollLanguagePos = GUILayout.BeginScrollView(scrollLanguagePos, GUILayout.Width(280), GUILayout.Height(290));

                    if (GUILayout.Button(LanguageManager.GetText("french"), (defaultLanguage != Language.French) ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Langage", "French");
                        LanguageManager.LoadLanguageFile(Language.French);
                        defaultLanguage = Language.French;
                        isOpenLanguage = false;
                    }
                    if (GUILayout.Button(LanguageManager.GetText("english"), (defaultLanguage != Language.English) ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Langage", "English");
                        LanguageManager.LoadLanguageFile(Language.English);
                        defaultLanguage = Language.English;
                        isOpenLanguage = false;
                    }
                    if (GUILayout.Button(LanguageManager.GetText("spanish"), (defaultLanguage != Language.Spanish) ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Langage", "Spanish");
                        LanguageManager.LoadLanguageFile(Language.Spanish);
                        defaultLanguage = Language.Spanish;
                        isOpenLanguage = false;
                    }
                    if (GUILayout.Button(LanguageManager.GetText("russian"), (defaultLanguage != Language.Russian) ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Langage", "Russian");
                        LanguageManager.LoadLanguageFile(Language.Russian);
                        defaultLanguage = Language.Russian;
                        isOpenLanguage = false;
                    }
                    if (GUILayout.Button(LanguageManager.GetText("italian"), (defaultLanguage != Language.Italian) ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Langage", "Italian");
                        LanguageManager.LoadLanguageFile(Language.Italian);
                        defaultLanguage = Language.Italian;
                        isOpenLanguage = false;
                    }
                    if (GUILayout.Button(LanguageManager.GetText("chinese"), (defaultLanguage != Language.Chinese) ? style.button : style.customStyles[2]))
                    {
                        PlayerPrefs.SetString("Langage", "English");
                        LanguageManager.LoadLanguageFile(Language.Chinese);
                        defaultLanguage = Language.Chinese;
                        isOpenLanguage = false;
                    }

                    GUILayout.EndScrollView();

                   /* GUILayout.BeginVertical(GUILayout.Width(Screen.width - 345));
                    GUILayout.Space(10);
                    GUILayout.EndVertical();*/

                    GUILayout.EndHorizontal();

                }
            }
        }
    }
}
