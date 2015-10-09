using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;

public class SetDefaultResolutionGraphics : Photon.MonoBehaviour {

    public bool fullscreen;
    public string quality;
    public int qualityInt;
    public static Language defaultLanguage = Language.French;


    // Use this for initialization
    void Start () {

        fullscreen = (PlayerPrefs.GetInt("Fullscreen") == 0) ? false : true;
        quality = PlayerPrefs.GetString("Quality");

        if (ObscuredPrefs.GetInt("MouseSensibility") == 0)
        {
            ObscuredPrefs.SetInt("MouseSensibility", 5);
        }

        CursorGestion.setInvisible();

        //_LANGUAGE_PARAMETER
        switch (PlayerPrefs.GetString("Langage"))
        {
            case "French":
                defaultLanguage = Language.French;
                break;
            case "English":
                defaultLanguage = Language.English;
                break;
            case "Chinese":
                defaultLanguage = Language.Chinese;
                break;
            case "Italian":
                defaultLanguage = Language.Italian;
                break;
            case "Russian":
                defaultLanguage = Language.Russian;
                break;
            case "Spanish":
                defaultLanguage = Language.Spanish;
                break;
            default:
                defaultLanguage = Language.French;
                break;
        }
        LanguageManager.LoadLanguageFile(defaultLanguage);

        if (quality == "low")
        {
            qualityInt = 0;
        }
        if (quality == "good")
        {
            qualityInt = 1;
        }
        if (quality == "fantastic")
        {
            qualityInt = 2;
        }
        if (quality == "wonderful")
        {
            qualityInt = 3;
        }
        QualitySettings.SetQualityLevel(qualityInt);
	}
}
