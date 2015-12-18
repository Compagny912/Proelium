using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class VueEtDeplacements : MonoBehaviour {

    //INIT
    public sbyte xVue = 45;
    public GameObject pivotCamera;
    public GameObject head;
    private GameObject go;
    private float sensi;
    private float rotationY;
    private float rotationX;

    public Vector3 actualPosition;
    public Vector3 nextPosition;
    public Quaternion actualViewCam;
    public Quaternion nextViewCam;
    private Vector3 v = Vector3.zero;

    void Start () {
        sensi = ObscuredPrefs.GetInt("MouseSensibility");
        go = this.gameObject;
	}
	
	void Update () {
        /*
        actualPosition = pivotCamera.transform.position;
        nextPosition = head.transform.position;
        actualViewCam = pivotCamera.transform.localRotation;
        nextViewCam = head.transform.localRotation;
        
        actualPosition = Vector3.SmoothDamp(actualPosition, nextPosition, ref v, 0.1f);

        */

        //CAMERA
        rotationY += Input.GetAxis("Mouse Y") * (PlayerPrefs.GetInt("InverseAxeY") == 1 ? -sensi : sensi);
        rotationY = Mathf.Clamp(rotationY, -xVue, xVue + 10);
        pivotCamera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        rotationX += Input.GetAxis("Mouse X") * (PlayerPrefs.GetInt("InverseAxeX") == 1 ? -sensi : sensi);
        go.transform.localRotation = new Quaternion(0, 0, 0, -rotationX);

	}
}
