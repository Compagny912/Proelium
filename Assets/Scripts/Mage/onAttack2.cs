using UnityEngine;
using System.Collections;

public class onAttack2 : Photon.MonoBehaviour {

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
        if (anim.GetBool("Attack2") == true && attackrealised == false)
        {
            attackrealised = true;
            go = PhotonNetwork.Instantiate("MageAttack2", GetComponent<Rigidbody>().transform.position, GetComponent<Rigidbody>().transform.rotation, 0);
            go.name = "Tourbillon";
            go.transform.SetParent(player.transform);
        }
    }
}
