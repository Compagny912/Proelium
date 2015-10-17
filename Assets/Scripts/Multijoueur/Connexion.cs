using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Text.RegularExpressions;

public class Connexion : Photon.MonoBehaviour {

    public bool withLogin = true;
    public static string menu = "login";
    public static ObscuredBool isLoadingScene = false;
    public ObscuredString pseudoJoueur;
    public static ObscuredBool isLogged;
   	public GameObject chat;
	public GameObject score;
	public GUISkin style;
    public static Language defaultLanguage = Language.French;

    private ObscuredString pseudo = "Compagny912";
    private ObscuredString passwd = "";
    private ObscuredString rpasswd = "";
    private int err;

    void Awake(){
		DontDestroyOnLoad(this);
		DontDestroyOnLoad(chat);
		DontDestroyOnLoad(score);
	}

    void Start()
    {
        err = 0;
        isLogged = false;

        CursorGestion.setInMenu();

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
        }
        LanguageManager.LoadLanguageFile(defaultLanguage);

        PhotonNetwork.ConnectUsingSettings("Version 1.1.0");
        PhotonNetwork.automaticallySyncScene = true;
    }

    void OnGUI()
    {
        if(PhotonNetwork.insideLobby == true && isLoadingScene == false)
        {
            if (menu == "login")
            {

                GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                GUI.Label(new Rect(Screen.width / 2 - 410, Screen.height / 2 - 130, 820, 250), "", style.window);
                GUI.color = Color.white;

                GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 125, 400, 10), LanguageManager.GetText("pseudo"), style.box);
                GUI.Label(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 125, 400, 10), LanguageManager.GetText("passwd"), style.box);

                pseudo = GUI.TextArea(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 110, 400, 30), pseudo, 16, style.textArea);
                passwd = GUI.PasswordField(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 110, 400, 30), passwd, "*"[0], 32, style.textArea);

                if (GUI.Button(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 55, 400, 30), LanguageManager.GetText("login"), style.button) && pseudo.GetEncrypted().Length >= 3 && passwd.GetEncrypted().Length != 0)
                {
                    StartCoroutine(logIn(pseudo.ToString(), passwd.ToString()));
                    passwd = "";
                    rpasswd = "";
                }
                if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 55, 400, 30), LanguageManager.GetText("register"), style.button))
                {
                    menu = "register";
                }
            }
            if (menu == "register")
            {
                
                GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                GUI.Label(new Rect(Screen.width / 2 - 410, Screen.height / 2 - 130, 820, 250), "", style.window);
                GUI.color = Color.white;

                GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 125, 400, 10), LanguageManager.GetText("pseudo"), style.box);
                GUI.Label(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 125, 400, 10), LanguageManager.GetText("passwd"), style.box);
                GUI.Label(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 70, 400, 10), LanguageManager.GetText("rpasswd"), style.box);

                pseudo = GUI.TextArea(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 110, 400, 30), pseudo, 16, style.textArea);
                passwd = GUI.PasswordField(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 110, 400, 30), passwd, "*"[0], 32, style.textArea);
                rpasswd = GUI.PasswordField(new Rect(Screen.width / 2 + 5, Screen.height / 2 - 55, 400, 30), rpasswd, "*"[0], 32, style.textArea);

                if(passwd.ToString() != rpasswd.ToString())
                {
                    err = 3;
                }
                if((passwd.ToString() == rpasswd.ToString()) && err != 5)
                {
                    err = 0;
                }
                if (!(pseudo.GetEncrypted().Length >= 3 || pseudo.GetEncrypted().Length <= 16) || !isValidPasswd(passwd.ToString()) || !isValidUserName(pseudo.ToString()))
                {
                    err = 2;
                }
                if (err != 5 && (pseudo.GetEncrypted().Length >= 3 && pseudo.GetEncrypted().Length <= 16) && isValidPasswd(passwd.ToString()) && isValidUserName(pseudo.ToString()) && (passwd.ToString() == rpasswd.ToString()))
                {
                    err = 0;
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 55, 400, 30), LanguageManager.GetText("register"), style.button) && pseudo.GetEncrypted().Length >= 3 && pseudo.GetEncrypted().Length <= 16 && isValidPasswd(passwd.ToString()) && isValidUserName(pseudo.ToString()))
                {
                    StartCoroutine(registerPlayer(pseudo.ToString(), passwd.ToString()));
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 10, 400, 30), LanguageManager.GetText("login"), style.button))
                {
                    menu = "login";
                    err = 0;
                }
            }
        }

        if (PhotonNetwork.insideLobby == true && isLoadingScene == false && menu != "" && menu != "login" && menu != "register")
        {
            GUI.Label(new Rect(Screen.width / 2 - 405, Screen.height / 2 - 250, 810, 30), LanguageManager.GetText("multiplayerMenu"), style.customStyles[0]);
            
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 250, 480, 30), "", style.customStyles[0]);

            //PSEUDO
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 195, 400, 10), LanguageManager.GetText("pseudo"), style.box);
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 180, 400, 30), pseudoJoueur, style.textField);

            if (menu == "listeServeurs")
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 150, 400, 30), LanguageManager.GetText("createARoom"), style.button))
                {
                    menu = "creationServeur";
                }
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 200, 400, 30), LanguageManager.GetText("filtres"), style.button))
                {
                    menu = "filtres";
                }
            }

            if (menu == "filtres")
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 150, 400, 30), LanguageManager.GetText("createARoom"), style.button))
                {
                    menu = "creationServeur";
                }
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 200, 400, 30), LanguageManager.GetText("appliquerFiltres"), style.button))
                {
                    menu = "listeServeurs";
                    //APPLIQUER LES FILTRES ICI
                }
            }

            if (menu == "creationServeur")
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 150, 400, 30), LanguageManager.GetText("roomList"), style.button))
                {
                    menu = "listeServeurs";
                }
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 200, 400, 30), LanguageManager.GetText("filtres"), style.button))
                {
                    menu = "filtres";
                }
            }
        }

        if(PauseMenuGUI.pausemenu == "" && menu != "")
        {
            switch (err)
            {
                default:
                case 0:
                    break;
                case 1:
                    GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 100, 400, 20), LanguageManager.GetText("invalidNameOrMDP"), style.textField);
                    break;
                case 2:
                    GUI.Label(new Rect(Screen.width / 2 - 370, Screen.height / 2 + 45, 740, 70), LanguageManager.GetText("lowPassword"), style.textField);
                    break;
                case 3:
                    GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 100, 400, 20), LanguageManager.GetText("MDPNoMatche"), style.textField);
                    break;
                case 4:
                    GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 + 100, 500, 20), LanguageManager.GetText("validRegister"), style.textField);
                    break;
                case 5:
                    GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 100, 400, 20), LanguageManager.GetText("alreadyTake"), style.textField);
                    break;
            }
        }
	}

	void OnJoinedRoom(){
        isLoadingScene = false;
	}


/////////////////////////////////////////////////////////////////////////////////////////////////
//_REGISTER_//
    public static bool isValidPasswd(string passwd)
    {

        string pattern = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{6,64})";
        return Regex.IsMatch(passwd, pattern);
    }

    public static bool isValidUserName(string username)
    {

        string pattern = @"^[a-zA-Z0-9_]*$";
        return Regex.IsMatch(username, pattern);
    }

    IEnumerator registerPlayer(string name, string passwd)
    {
        ObscuredString hash = "3fc10317ff24163cddf54f47fb2838a3029cdbfee119095317d44dfeb73ccef2dbed305a71169521285675b847372056c1ba7584148497365a802bcc16589547";
        ObscuredString pseudo = md5Protection1.Md5Sum(name);
        ObscuredString newpasswd = sha512Protection.SHA512Sum(passwd);

        WWW www = new WWW("http://login.proelium.cf/register.php?user=" + name.ToUpper() + "&passwd=" + pseudo.ToString() + newpasswd.ToString() + "&hash=" + hash.ToString());
        yield return www;

        if (www.error == null)
        {
            if (www.text == "0")
            {
                menu = "login";
                rpasswd = "";
                passwd = "";
                CursorGestion.setInMenu();
                err = 4;
            }
            if (www.text == "1")
            {
                err = 5;
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }

//_LOGIN_//
    IEnumerator logIn(string name, string passwd)
    {
        if (withLogin)
        {

            ObscuredString hash = "3fc10317ff24163cddf54f47fb2838a3029cdbfee119095317d44dfeb73ccef2dbed305a71169521285675b847372056c1ba7584148497365a802bcc16589547";
            ObscuredString pseudo = md5Protection1.Md5Sum(name);
            ObscuredString newpasswd = sha512Protection.SHA512Sum(passwd);

            print("Send...");

            WWW www = new WWW("http://login.proelium.cf/login.php?user=" + name.ToUpper() + "&passwd=" + pseudo.ToString() + newpasswd.ToString() + "&hash=" + hash.ToString());
            yield return www;

            if (www.error == null)
            {
                print(www.text);

                if (www.text == "0")
                {
                    err = 0;
                    pseudoJoueur = name;
                    menu = "listeServeurs";
                    isLogged = true;
                    PhotonNetwork.playerName = pseudoJoueur;
                    PhotonNetwork.player.name = pseudoJoueur;
                    CursorGestion.setInMenu();
                }
                if (www.text == "1")
                {
                    err = 1;
                }
            }
            else
            {
                Debug.Log(www.error);
            }
        }
        else
        {
            err = 0;
            CursorGestion.setInMenu();
            pseudoJoueur = name;
            menu = "listeServeurs";
            isLogged = true;
            PhotonNetwork.playerName = pseudoJoueur;
            PhotonNetwork.player.name = pseudoJoueur;
        }
    }
}