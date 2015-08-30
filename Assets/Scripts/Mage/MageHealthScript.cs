using UnityEngine;
using System.Collections;

public class MageHealthScript : Photon.MonoBehaviour {

    public int health;
    public int maxHealth = 100;
    public Animator anim;
    public GameObject chat;
    string name;
    string killer;

	// Use this for initialization
	void Start () {
        health = 100;
        anim = this.GetComponent<Animator>();
        name = this.gameObject.name;
        chat = GameObject.Find("Chat");
	}
	
	// Update is called once per frame
	void Update () {
	    if(health >= maxHealth){
            health = maxHealth;
        }
        if (health <= 0)
        {
            onDead();
        }
	}

    void onDead()
    {
        Invoke("Mort", 2f);
        string phrase;
        if (killer == null)
        {
            phrase = " as été tué par l'environnement.";
        }
        else if(killer == this.gameObject.name){
            phrase = " s'est suicidé";
        }
        else
        {
            phrase = " as été tué par " + killer;
        }
        chat.SendMessage("Message", this.gameObject.name + phrase);
        anim.SetBool("IsDead", true);
    }

    void Mort()
    {
        Destroy(this.gameObject);
        Score.estMort = true;
    }

    void sendDamage(int degats)
    {
        health -= degats;
    }

    void getLastDegats(string tueur)
    {
        killer = tueur;
    }
}
