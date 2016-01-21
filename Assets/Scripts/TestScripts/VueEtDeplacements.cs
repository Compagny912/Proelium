using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

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

    public int vie;

    //INITFORMOVEMENTS
    private static Animator anim;
    public sbyte gravity = 5;
    public int walkspeed;
    public int jumpspeed;
    public float jump = 2;
    private Vector3 d = Vector3.zero;

    void Start () {
        anim = this.GetComponent<Animator>();
        sensi = ObscuredPrefs.GetInt("MouseSensibility");
        invertY = PlayerPrefs.GetInt("InverseAxeY");
        controller = this.GetComponent<CharacterController>();
	}
	
	void Update () {
        vie = health;
        float ax;
        float az;
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
        pivotCamera.transform.SetParent(parentDeathCamera);

        ax = Input.GetAxisRaw("Horizontal") / 1.2f;
        az = Input.GetAxisRaw("Vertical");

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

        if (Input.GetAxis("Fire1") > 0) anim.SetBool("Attack1", true);
        if (Input.GetAxis("Fire2") > 0) anim.SetBool("Attack2", true);
    }

    public void takeDegats(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            pivotCamera.transform.SetParent(parentDeathCamera);
            anim.SetBool("Die", true);
            showMessage.inputMessage("%PLAYER% as été tué par l'environnement");
        }
    }
    public void takeDegats(string[] a)
    {
        health -= int.Parse(a[0]);
        if (health <= 0)
        {
            pivotCamera.transform.SetParent(parentDeathCamera);
            anim.SetBool("Die", true);
            showMessage.inputMessage("%PLAYER% as été tué par " + a[1] + ".");
        }
    }

    public void takeDegats(int damage, string name)
    {
        health -= damage;
        if (health <= 0)
        {
            pivotCamera.transform.SetParent(parentDeathCamera);
            anim.SetBool("Die", true);
            showMessage.inputMessage("%PLAYER% as été tué par " + name + ".");
        }
    }
    public void attack1()
    {
        this.Invoke("onAttack1", 0.5f);
    }
    void onAttack1()
    {
        PhotonNetwork.playerName = "Antoine";
        GameObject g = PhotonNetwork.Instantiate("MageAttack1", new Vector3(controller.transform.position.x,
                controller.transform.position.y + 0.5f,
                controller.transform.position.z),
            pivotCamera.transform.rotation, 0);
        GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, PhotonNetwork.playerName, g.name);
    }
    public void attack2()
    {
        this.Invoke("onAttack2", 0.5f);
    }
    void onAttack2()
    {
        PhotonNetwork.playerName = "Jade";
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
}
