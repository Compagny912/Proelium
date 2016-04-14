using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]

public class VueEtDeplacements : Photon.MonoBehaviour {

    private CharacterController controller;

    //INITFORCAMERA
    public static int health = 100;
    public sbyte xVue = 45;
    public Transform parentDeathCamera;
    public GameObject pivotCamera;
    private float sensi;
    private float invertY;
    private float rotationX = 0;

    ///////////////////////////////
    private GameObject display;
    ///////////////////////////////

    //INITOTHERSPARAMETERS
    public int vie;
    public GameObject cloak;
    public GameObject staff;
    public GameObject squeleton;
    private CursorGestion cursor;

    //INITFORMOVEMENTS
    private static Animator anim;
    public sbyte gravity = 5;
    public float walkspeed;
    public float runspeed;
    public int jumpspeed;
    public float jump = 2;
    private Vector3 d = Vector3.zero;
    public Camera cam;

    void Start () {

        if (GetComponent<PhotonView>().isMine == false)
        {
            GetComponent<CharacterController>().enabled = false;

            pivotCamera.SetActive(false);

            GetComponent<TeamScript>().enabled = false;
            staff.layer = 9;
            cloak.layer = 13;
            squeleton.layer = 13;

            string team = PhotonPlayer.Find(GetComponent<PhotonView>().ownerId).GetTeam().ToString();
            int intTeam = team == "blue" ? 17 : 18;
            this.gameObject.layer = intTeam;
            GetComponent<VueEtDeplacements>().enabled = false;
        }
        else if (GetComponent<PhotonView>().isMine == true)
        {
            cloak.layer = 11;
            squeleton.layer = 11;
            gameObject.layer = 11;
            squeleton.GetComponent<Renderer>().enabled = false;
            cloak.GetComponent<Renderer>().enabled = false;
            staff.layer = 8;
            display = GameObject.Find("Display2");
            display.SetActive(false);
            anim = this.GetComponent<Animator>();
            sensi = ObscuredPrefs.GetInt("MouseSensibility");
            invertY = PlayerPrefs.GetInt("InverseAxeY");
            controller = this.GetComponent<CharacterController>();
            cursor = GameObject.Find("Scripts").GetComponent<CursorGestion>();
            cursor.setInGame();
        }
    }
	
	void FixedUpdate () {

        vie = health;
        float ax;
        float az;
        float acc;
        
        rotationX += Input.GetAxisRaw("Mouse Y") * (invertY == 1 ? -sensi : sensi);
        rotationX = Mathf.Clamp(rotationX, -40, 65);
        pivotCamera.transform.localEulerAngles = new Vector3(-rotationX, 0, 0);
        if (anim.GetBool("Die"))
        {
            pivotCamera.transform.SetParent(parentDeathCamera);
            if (!controller.isGrounded)
            {
                d.y -= gravity * Time.deltaTime;
                controller.Move(d * Time.deltaTime * walkspeed);
            }
                return;
        }
        acc = Input.GetAxis("Speed");
        ax = Input.GetAxisRaw("Horizontal");
        ax *= acc > 0 && ax > 0 ? 1.5f : 1f;
        ax *= acc > 0 && ax < 0 ? 1.5f : 1f;
        az = Input.GetAxisRaw("Vertical");
        az *= acc > 0 && az > 0 ? runspeed : 1f;
        az *= acc > 0 && az < 0 ? 1.5f : 1f;

        transform.Rotate(0, Input.GetAxisRaw("Mouse X") * sensi, 0);
        d.x = ax;
        d.z = az;
        d = transform.TransformDirection(d);

        if (controller.isGrounded)
        {
            anim.SetBool("Jump", false);
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("Jump", true);
                d.y = jump;
            }
        }
        if (!controller.isGrounded)
        {
            d.y -= gravity * Time.deltaTime;
            anim.SetBool("Jump", true);
        }
        anim.SetFloat("AxeZ", az);
        anim.SetFloat("AxeX", ax);

        controller.Move(d * Time.deltaTime * walkspeed);

        if (Input.GetAxis("Fire1") > 0 && !anim.GetBool("Attack1"))
        {
            anim.SetBool("Attack1", true);
            attack1();
        }
        if (Input.GetAxis("Fire2") > 0 && !anim.GetBool("Attack1"))
        {
            anim.SetBool("Attack2", true);
            attack2();
        }
    }
    public void touchPlayer()
    {
        StartCoroutine(cursor.hitPlayer());
    }
    public void takeDegats(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            pivotCamera.transform.SetParent(parentDeathCamera);
            anim.SetBool("Die", true);
            GetComponent<CapsuleCollider>().enabled = false;
            showMessage.inputMessage("%PLAYER% as été tué par l'environnement");
        }
    }
    public void takeDegats(string[] a)
    {
        health -= int.Parse(a[0]);
        if (health <= 0)
        {
            anim.SetFloat("AxeX", 0);
            anim.SetFloat("AxeZ", 0);
            pivotCamera.GetComponentInChildren<MotionBlur>().enabled = true;
            pivotCamera.transform.SetParent(parentDeathCamera);
            anim.SetBool("Die", true);
            PhotonNetwork.player.AddDeaths(1);
            if (GetComponent<CapsuleCollider>()) GetComponent<CapsuleCollider>().enabled = false;
            if(GetComponent<SphereCollider>()) GetComponent<SphereCollider>().enabled = false;
            showMessage.inputMessage(this.name + " as été tué par " + a[1] + ".");
            if(GameObject.Find(a[1]).GetComponent<AI>()) GameObject.Find(a[1]).GetComponent<AI>().kill();
            StartCoroutine(isOnDeath());
        }
    }
    /*
    public void takeDegats(int damage, string name)
    {
        health -= damage;
        if (health <= 0)
        {
            PhotonNetwork.player.AddDeaths(1);
            pivotCamera.transform.SetParent(parentDeathCamera);
            anim.SetBool("Die", true);
            GetComponent<CapsuleCollider>().enabled = false;
            showMessage.inputMessage("%PLAYER% as été tué par " + name + ".");
            GameObject.Find("Scripts").GetComponent<testscript>().onDeath();
            StartCoroutine(isOnDeath());
        }
    }
    */
    public void kill()
    {
        PhotonNetwork.player.AddKills(1);
        PhotonNetwork.player.AddScore(100);
    }

    public void attack1()
    {
        this.Invoke("onAttack1", 0.5f);
    }
    void onAttack1()
    {
        GameObject g = PhotonNetwork.Instantiate(PhotonNetwork.player.GetTeam() == PunTeams.Team.blue ? "MageAttack1BLUE" : "MageAttack1RED", new Vector3(controller.transform.position.x,
                controller.transform.position.y + 0.8f,
                controller.transform.position.z),
            pivotCamera.transform.rotation, 0);
        GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, PhotonNetwork.playerName, g.name);
    }
    public void attack2()
    {
        this.Invoke("onAttack2", 0.5f);
    }
    public string getTeam()
    {
        return PhotonNetwork.player.GetTeam().ToString();
    }
    void onAttack2()
    {
        GameObject g = PhotonNetwork.Instantiate("MageAttack2", new Vector3(controller.transform.position.x,
                controller.transform.position.y,
                controller.transform.position.z),
            pivotCamera.transform.rotation, 0);
        GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, PhotonNetwork.playerName, g.name);
    }
    [PunRPC]
    void RefreshName(string futureName, string actualName)
    {
        GameObject.Find(actualName).name = futureName;
    }
    IEnumerator isOnDeath()
    {
        yield return new WaitForSeconds(4);
        GameObject.Find("Scripts").GetComponent<testscript>().onDeath();
        display.SetActive(true);
        Destroy(this.gameObject);
    }
}
