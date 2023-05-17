using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMenuSDK.UI
{
    public class MapPanel : UIPanel
    {
        [SerializeField] Transform m_cylinderLocation;
        [SerializeField] Transform m_capsuleLocation;
        [SerializeField] Transform m_sphereLocation;
        [SerializeField] Transform m_cubeLocation;

        public void ShowPath(string pathName)
        {
            Vector3 targetPoint = new Vector3();
            switch (pathName)
            {
                case "Cylinder":
                    targetPoint = m_cylinderLocation.position;
                    break;
                case "Capsule":
                    targetPoint = m_capsuleLocation.position;
                    break;
                case "Sphere":
                    targetPoint = m_sphereLocation.position;
                    break;
                case "Cube":
                    targetPoint = m_cubeLocation.position;
                    break;
            }
            HidePanel();
            HandMenuUI.Instance.ShowUIPanel("Main");
            HandMenuUI.Instance.ShowPath(pathName, targetPoint);
        }
        public void TeleportToDestination(string destinationName)
        {
            Debug.LogWarning("Teleport to: " + destinationName);
            switch (destinationName)
            {
                case "Cylinder":
                    GetComponentInParent<OVRCameraRig>().transform.position = m_cylinderLocation.position;
                    break;
                case "Capsule":
                    GetComponentInParent<OVRCameraRig>().transform.position = m_capsuleLocation.position;
                    break;
                case "Sphere":
                    GetComponentInParent<OVRCameraRig>().transform.position = m_sphereLocation.position;
                    break;
                case "Cube":
                    GetComponentInParent<OVRCameraRig>().transform.position = m_cubeLocation.position;
                    break;
            }
        }
    }
}