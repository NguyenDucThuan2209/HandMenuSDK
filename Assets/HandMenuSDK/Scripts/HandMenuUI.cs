using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMenuSDK
{
    public class HandMenuUI : MonoBehaviour
    {
        private static HandMenuUI m_instance;
        public static HandMenuUI Instance => m_instance;

        [Header("MENU OPERATOR")]
        [SerializeField] int m_pinchAmount;
        [SerializeField] float m_thresholdTime;
        [SerializeField] GameObject m_poketableCanvas;
        [SerializeField] SpawnGuideline m_spawnGuideline;

        [Header("TELEPORT OPERATOR")]
        [SerializeField] Transform m_locationPointer;
        [SerializeField] Transform[] m_teleportDestinations;

        private float m_pinchTime;
        private bool m_isPinching;
        private int m_pinchCount;

        // Start is called before the first frame update
        void Start()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckForPinch();
        }

        #region Pinch Operator
        private void CheckForPinch()
        {
            if (m_isPinching)
            {
                if (m_pinchCount < m_pinchAmount)
                {
                    m_pinchTime += Time.deltaTime;
                    
                    if (m_pinchTime > m_thresholdTime) ResetPinch();
                }
                else
                {
                    m_poketableCanvas.SetActive(!m_poketableCanvas.activeSelf);
                    ResetPinch();
                }
            }
        }

        private void ResetPinch()
        {
            m_isPinching = false;
            m_pinchCount = 0;
            m_pinchTime = 0;
        }

        public void Pinch()
        {            
            m_pinchCount++;
            m_isPinching = true;
        }
        #endregion

        #region Map Operator
        public void ShowPath(string pathName, Vector3 targetPoint)
        {                        
            m_spawnGuideline.Spawn(pathName, new Vector3[] { transform.position, Vector3.zero, targetPoint });
        }
        public void ShowPath(string pathName)
        {
            Vector3 targetPoint = new Vector3();

            for (int i = 0; i < m_teleportDestinations.Length; i++)
            {
                if (m_teleportDestinations[i].name == pathName)
                {
                    targetPoint = m_teleportDestinations[i].position;
                    break;
                }
            }
            
            ShowPath(pathName, targetPoint);
        }
        public void TeleportToDestination(string destinationName)
        {
            Debug.LogWarning("Teleport to: " + destinationName);
            for (int i = 0; i < m_teleportDestinations.Length; i++)
            {
                if (m_teleportDestinations[i].name == destinationName)
                {
                    GetComponentInParent<OVRCameraRig>().transform.position = m_teleportDestinations[i].position;
                    GetComponentInParent<OVRCameraRig>().transform.rotation = m_teleportDestinations[i].rotation;
                    break;
                }
            }
        }
        #endregion
    }
}