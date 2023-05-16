using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMenuSDK
{
    public class HandMenuUI : MonoBehaviour
    {
        private static HandMenuUI m_instance;
        public static HandMenuUI Instance => m_instance;

        [Header("MENU OPPERATOR")]
        [SerializeField] int m_pinchAmount;
        [SerializeField] float m_thresholdTime;
        [SerializeField] SpawnGuideline m_spawnGuideline;

        [Header("MENU PREFERENCES")]
        [SerializeField] GameObject m_poketableCanvas;
        [SerializeField] UI.UIPanel m_mainPanel;
        [SerializeField] UI.UIPanel m_mapPanel;
        [SerializeField] UI.UIPanel m_checklistPanel;                
        
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
            Debug.LogWarning("Is Pinching!!!");
            m_pinchCount++;
            m_isPinching = true;
        }
        #endregion

        #region UI Operator
        private void HideAllUIPanel()
        {
            if (m_mainPanel.IsShow) m_mainPanel.HidePanel();
            if (m_checklistPanel.IsShow) m_checklistPanel.HidePanel();            
            if (m_mapPanel.IsShow) m_mapPanel.HidePanel();         
        }
        public void ShowUIPanel(string panelName)
        {
            HideAllUIPanel();

            switch (panelName)
            {
                case "Main":
                    m_mainPanel.ShowPanel();
                    break;
                case "Map":
                    m_mapPanel.ShowPanel();
                    break;
                case "Checklist":
                    m_checklistPanel.ShowPanel();
                    break;
            }
        }
        public void ShowPath(string pathName, Vector3 targetPoint)
        {                        
            m_spawnGuideline.Spawn(pathName, new Vector3[] { transform.position, Vector3.zero, targetPoint });
        }
        #endregion
    }
}