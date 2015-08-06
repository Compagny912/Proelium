using UnityEngine;
using System.Collections;

public class onAttack1 : Photon.MonoBehaviour {

    Animator anim;
    GameObject go;
    GameObject player;
    public static bool attackrealised = true;

    void Start()
    {
        attackrealised = false;
        anim = GetComponent<Animator>();
        player = this.gameObject;
    }

    void Update()
    {
        if (anim.GetBool("Attack1") == true && attackrealised == false)
        {
            attackrealised = true;
            Invoke("attack", 0.5f);
        }
    }
    void attack()
    {
        go = PhotonNetwork.Instantiate("MageAttack1", new Vector3(GetComponent<Rigidbody>().transform.position.x, GetComponent<Rigidbody>().transform.position.y + 0.5f, GetComponent<Rigidbody>().transform.position.z), this.GetComponentInChildren<Camera>().transform.rotation, 0);
        go.name = "Fireball";
    }
}
