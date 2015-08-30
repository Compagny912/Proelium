#pragma warning disable 618

using UnityEngine;
using System.Collections;

public class onAttack1 : Photon.MonoBehaviour {

    Animator anim;
    public static bool attackrealised;

    void Start()
    {
        attackrealised = false;
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetBool("Attack1") == true && attackrealised == false)
        {
            attackrealised = true;
            this.Invoke("attack", 0.5f);
        }
    }
    void attack()
    {
        GameObject go = (GameObject) PhotonNetwork.Instantiate(
            "MageAttack1", 
            new Vector3(GetComponent<Rigidbody>().transform.position.x, 
                GetComponent<Rigidbody>().transform.position.y + 0.5f, 
                GetComponent<Rigidbody>().transform.position.z), 
            this.GetComponentInChildren<Camera>().transform.rotation, 0);

        GetComponent<PhotonView>().RPC("RefreshName", PhotonTargets.AllBuffered, PhotonNetwork.playerName, go.name);
    }

    [PunRPC]
    void RefreshName(string pseudo, string name)
    {
        GameObject.Find(name).name = pseudo;
    }
}
