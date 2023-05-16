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
    }
}