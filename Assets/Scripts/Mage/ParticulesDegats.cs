using UnityEngine;
using System.Collections;

public class ParticulesDegats : Photon.MonoBehaviour {

    public int degats;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject other)
    {
        string[] a = { degats + "", this.gameObject.name };
        other.SendMessage("takeDegats", a, SendMessageOptions.DontRequireReceiver);
        if(other.tag == "Player")
        {
            if(GameObject.Find(this.name).GetComponent<VueEtDeplacements>())
            GameObject.Find(this.name).GetComponent<VueEtDeplacements>().touchPlayer();
        }
    }

}
