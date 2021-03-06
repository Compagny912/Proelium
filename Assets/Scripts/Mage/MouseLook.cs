using System;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Scripts.Mage
{
    [Serializable]
    public class MouseLook
    {
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        private float f;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;


        public void Init(Transform character, Transform camera)
        {
            f = ObscuredPrefs.GetInt("MouseSensibility");
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }


        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * f;
            float xRot = Input.GetAxis("Mouse Y") * (PlayerPrefs.GetInt("InverseAxeY") == 1 ? -f : f);

            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            //xRot = Mathf.Clamp(xRot, -90, 90);

            //camera.localEulerAngles = new Vector3(-xRot, 0, 0);

            if (clampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
                camera.localRotation = m_CameraTargetRot;
            }

            if(smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }


        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
