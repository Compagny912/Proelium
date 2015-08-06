using UnityEngine;
using System.Collections;

public class SetDefaultResolutionGraphics : Photon.MonoBehaviour {

    public bool fullscreen;
    public string quality;
    public int qualityInt;

	// Use this for initialization
	void Start () {

        fullscreen = (PlayerPrefs.GetInt("Fullscreen") == 0) ? false : true;
        quality = PlayerPrefs.GetString("Quality");

        if (PlayerPrefs.GetFloat("MouseSensibility") == 0)
        {
            PlayerPrefs.SetFloat("MouseSensibility", 5);
        }

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

	// Update is called once per frame
	void Update () {
	
	}
}
