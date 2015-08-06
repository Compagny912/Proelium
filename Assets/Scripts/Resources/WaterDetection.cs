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

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<MageController>().waterEffect = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.gameObject.tag == "Player")
        {
            col.GetComponent<MageController>().waterEffect = false;
        }
    }
}