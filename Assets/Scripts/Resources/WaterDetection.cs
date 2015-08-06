using UnityEngine;
using System.Collections;
using UnityStandardAssets.Scripts.Mage;

public class WaterDetection : Photon.MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.tag == "Player")
        {
            col.collider.GetComponent<MageController>().waterEffect = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.tag == "Player")
        {
            col.collider.GetComponent<MageController>().waterEffect = false;
        }
    }
}