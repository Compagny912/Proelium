using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Scripts.Mage
{
	[RequireComponent(typeof (CharacterController))]
	[RequireComponent(typeof (AudioSource))]
	public class MageController : Photon.MonoBehaviour
	{
		[SerializeField] private bool m_IsWalking;
		[SerializeField] private float m_WalkSpeed;
		[SerializeField] private float m_RunSpeed;
		[SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
		[SerializeField] private float m_JumpSpeed;
		[SerializeField] private float m_StickToGroundForce;
		[SerializeField] private float m_GravityMultiplier;
		[SerializeField] private MouseLook m_MouseLook;
		[SerializeField] private bool m_UseFovKick;
		[SerializeField] private FOVKick m_FovKick = new FOVKick();
		[SerializeField] private bool m_UseHeadBob;
		[SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
		[SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
		[SerializeField] private float m_StepInterval;
		[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
		[SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
		[SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] private AudioClip[] m_FootstepWaterSounds;
        [SerializeField] private int[] previousStateBeforeJump;

		private Camera m_Camera;
		private bool m_Jump;
		private float m_YRotation;
        public bool waterEffect;
		private Vector2 m_Input;
		private Vector3 m_MoveDir = Vector3.zero;
		private CharacterController m_CharacterController;
		private CollisionFlags m_CollisionFlags;
		private bool m_PreviouslyGrounded;
		private Vector3 m_OriginalCameraPosition;
		private float m_StepCycle;
		private float m_NextStep;
		private bool m_Jumping;
		private AudioSource m_AudioSource;
		public Animator m_anim;
		public GameObject squeleton;
		public GameObject cloak;
		public GameObject staff;

		// Use this for initialization
		private void Start()
		{
            GameObject mage = this.gameObject;
			m_CharacterController = GetComponent<CharacterController>();
			m_Camera = Camera.main;
			m_OriginalCameraPosition = m_Camera.transform.localPosition;
			m_FovKick.Setup(m_Camera);
			m_HeadBob.Setup(m_Camera, m_StepInterval);
			m_StepCycle = 0f;
			m_NextStep = m_StepCycle/2f;
			m_Jumping = false;
			m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            m_anim = GetComponent<Animator>(); 
            waterEffect = false;


			if (GetComponent<PhotonView>().isMine == false) {

				//MAGE
				GetComponent<CharacterController>().enabled = false;
				GetComponent<AudioSource>().enabled = false;
				GetComponent<MageController>().enabled = false;
                //GetComponent<NetworkCharacter>().enabled = false;

				//CAMERAS
				Camera[] cams;
				cams = GetComponentsInChildren<Camera>();
				foreach(Camera cam in cams){
					cam.enabled = false;
				}
				GetComponentInChildren<FlareLayer>().enabled = false;
				GetComponentInChildren<GUILayer>().enabled = false;
				GetComponentInChildren<AudioListener>().enabled = false;
				GetComponentInChildren<Skybox>().enabled = false;
				staff.layer = 9;
                cloak.layer = 13;
                squeleton.layer = 13;
                mage.layer = 13;

			} else if (GetComponent<PhotonView>().isMine == true){

                cloak.layer = 11;
                squeleton.layer = 11;
                mage.layer = 11;
				squeleton.GetComponent<Renderer>().enabled = false;
				cloak.GetComponent<Renderer>().enabled = false;
                staff.layer = 8;
			}
		}

		// Update is called once per frame
		private void Update()
		{
			if(!m_anim.GetBool("IsDead") && PauseMenuGUI.pausemenu == ""){
				RotateView();
			}
			// the jump state needs to read here to make sure it is not missed
			if (!m_Jump && !m_Jumping) {
				m_Jump = CrossPlatformInputManager.GetButtonDown ("Jump");
			}
			if (!m_PreviouslyGrounded && m_CharacterController.isGrounded) {
				StartCoroutine (m_JumpBob.DoBobCycle ());
				PlayLandingSound ();
				m_MoveDir.y = 0f;
				m_Jumping = false;
				m_anim.SetBool ("OnTheAir", false);
			}
			if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded) {
				m_MoveDir.y = 0f;
			}
			m_PreviouslyGrounded = m_CharacterController.isGrounded;
		}

		private void PlayLandingSound()
		{
			m_AudioSource.clip = m_LandSound;
			m_AudioSource.Play();
			m_NextStep = m_StepCycle + .5f;
		}
		
		
		private void FixedUpdate()
		{
			float speed;
			GetInput(out speed);
			// always move along the camera forward as it is the direction that it being aimed at
			Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;
			
			// get a normal for the surface that is being touched to move along it
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
			                   m_CharacterController.height/2f);
			desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
			
			m_MoveDir.x = desiredMove.x*speed;
			m_MoveDir.z = desiredMove.z*speed;
			
			if (m_CharacterController.isGrounded && m_anim.GetBool("IsDead") == false)
			{

                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump && !m_Jumping && m_anim.GetBool("OnTheAir") == false)
                {
                    PlayJumpSound();
                    m_Jumping = true;
                    m_anim.SetBool("OnTheAir", true);

                    m_MoveDir.y = m_JumpSpeed;
                    m_Jump = false;
                }
			}           

			else
			{
				m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
			}
			m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
			
			ProgressStepCycle(speed);
			UpdateCameraPosition(speed);
		}
		
		
		private void PlayJumpSound()
		{
			m_AudioSource.clip = m_JumpSound;
			m_AudioSource.Play();
		}
		
		
		private void ProgressStepCycle(float speed)
		{
			if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
			{
				m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
					Time.fixedDeltaTime;
			}
			
			if (!(m_StepCycle > m_NextStep))
			{
				return;
			}
			
			m_NextStep = m_StepCycle + m_StepInterval;
			
			PlayFootStepAudio();
		}
		
		
		private void PlayFootStepAudio()
		{
			if (!m_CharacterController.isGrounded)
			{
				return;
			}
			// pick & play a random footstep sound from the array,
			// excluding sound at index 0
			
            if (waterEffect == false)
            {
                int n = Random.Range(1, m_FootstepSounds.Length);
                m_AudioSource.clip = m_FootstepSounds[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepSounds[n] = m_FootstepSounds[0];
                m_FootstepSounds[0] = m_AudioSource.clip;
            }
            else if (waterEffect == true)
            {
                int n = Random.Range(1, m_FootstepWaterSounds.Length);
                m_AudioSource.clip = m_FootstepWaterSounds[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepWaterSounds[n] = m_FootstepWaterSounds[0];
                m_FootstepWaterSounds[0] = m_AudioSource.clip;
            }
		}
		
		
		private void UpdateCameraPosition(float speed)
		{
			Vector3 newCameraPosition;
			if (!m_UseHeadBob)
			{
				return;
			}
			if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
			{
				m_Camera.transform.localPosition =
					m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
					                    (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
			}
			else
			{
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
			}
			m_Camera.transform.localPosition = newCameraPosition;
		}

		private void GetInput(out float speed)
		{
			// Read input
			float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			float vertical = CrossPlatformInputManager.GetAxis("Vertical");
			
			bool waswalking = m_IsWalking;
			
			#if !MOBILE_INPUT
			// On standalone builds, walk/run speed is modified by a key press.
			// keep track of whether or not the character is walking or running
			m_IsWalking = !(Input.GetAxis("Speed") > 0);
			speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
			#endif

			// set the desired speed to be walking or running
			if (!m_anim.GetBool ("IsDead") && PauseMenuGUI.pausemenu == "") {
                m_Input = new Vector2(horizontal, vertical);
                /////////////////

                if (m_Jumping) return;

                if (vertical == 0)
                {
                    m_anim.SetInteger("AxeX", 0);
                }

                if (vertical > 0)
                {
                    m_anim.SetInteger("AxeX", speed == m_WalkSpeed ? 10 : 20);
                }

                if (vertical < 0)
                {
                    speed = m_WalkSpeed;
                    m_anim.SetInteger("AxeX", -10);
                }


                if (horizontal == 0)
                {
                    m_anim.SetInteger("AxeY", 0);
                }
                
                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Q))
                {
                    horizontal = 0;
                    m_anim.SetInteger("AxeY", 0);
                    return;
                }

                if (horizontal < 0)
                {
                    m_anim.SetInteger("AxeY", -1);
                    //speed = m_RunSpeed;
                }
                if (horizontal > 0)
                {
                    m_anim.SetInteger("AxeY", 1);
                    //speed = m_RunSpeed;
                }

				// normalize input if it exceeds 1 in combined length:
				if (m_Input.sqrMagnitude > 1) {
					m_Input.Normalize ();
				}
			
				// handle speed change to give an fov kick
				// only if the player is going to a run, is running and the fovkick is to be used
				if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0) {
					StopAllCoroutines ();
					StartCoroutine (!m_IsWalking ? m_FovKick.FOVKickUp () : m_FovKick.FOVKickDown ());
				}

            }
            else if (PauseMenuGUI.pausemenu != "")
            {
                m_anim.SetInteger("AxeX", 0);
                m_anim.SetInteger("AxeY", 0);
                speed = 0;
            }
            else
            {
                //WHEN THE PLAYER IS DEAD !!!
            }
		}
		
		private void RotateView()
		{
			m_MouseLook.LookRotation (transform, m_Camera.transform);
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it

            if (m_CollisionFlags == CollisionFlags.Below)
			{
				return;
			}
			
			if (body == null || body.isKinematic)
			{
				return;
			}
			body.AddForceAtPosition(m_CharacterController.velocity*10f, hit.point, ForceMode.Impulse);
		}

        private void Attaquer()
        {
            if (!m_anim.GetBool("IsDead"))
            {
                if ((Input.GetAxis("Fire1") > 0 && m_anim.GetBool("onAttack2") == false) && PauseMenuGUI.pausemenu == "")
                {
                    m_anim.SetBool("Attack1", true);
                }

                else if ((Input.GetAxis("Fire2") > 0 && m_anim.GetBool("onAttack1") == false) && PauseMenuGUI.pausemenu == "")
                {
                    m_anim.SetBool("Attack2", true);
                }
            }
        }
	}
}
