using UnityEngine;
using System.Collections;

public class NetworkCharacter : MonoBehaviour {

    private float lastSynchronizationTime = 0f;
    public float syncDelay = 0f;
    public float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Quaternion syncStartRotation = Quaternion.identity;
    private Quaternion syncEndRotation = Quaternion.identity;

    Animator anim;

	void Start () {
        
        anim = GetComponent<Animator>();
	}

	void Update () {
        if (GetComponent<PhotonView>().isMine == false)
        {
            syncTime += Time.deltaTime;
            GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
            GetComponent<Rigidbody>().rotation = Quaternion.Lerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
        }
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(GetComponent<Rigidbody>().position);
            stream.SendNext(GetComponent<Rigidbody>().rotation);

            stream.SendNext(anim.GetBool("OnTheAir"));
            stream.SendNext(anim.GetInteger("AxeY"));
            stream.SendNext(anim.GetInteger("AxeX"));
            stream.SendNext(anim.GetBool("IsDead"));
            stream.SendNext(anim.GetBool("Attack1"));
            stream.SendNext(anim.GetBool("Attack2"));
            stream.SendNext(anim.GetBool("onAttack1"));
            stream.SendNext(anim.GetBool("onAttack2"));
            stream.SendNext(anim.GetBool("Jump"));
        } else {

            syncEndPosition = (Vector3)stream.ReceiveNext();
            syncStartPosition = GetComponent<Rigidbody>().position;
            syncEndRotation = (Quaternion)stream.ReceiveNext();
            syncStartRotation = GetComponent<Rigidbody>().rotation;

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
            
            anim.SetBool("OnTheAir", (bool)stream.ReceiveNext());
            anim.SetInteger("AxeY", (int)stream.ReceiveNext());
            anim.SetInteger("AxeX", (int)stream.ReceiveNext());
            anim.SetBool("IsDead", (bool)stream.ReceiveNext());
            anim.SetBool("Attack1", (bool)stream.ReceiveNext());
            anim.SetBool("Attack2", (bool)stream.ReceiveNext());
            anim.SetBool("onAttack1", (bool)stream.ReceiveNext());
            anim.SetBool("onAttack2", (bool)stream.ReceiveNext());
            anim.SetBool("Jump", (bool)stream.ReceiveNext());
        }
    }
}
