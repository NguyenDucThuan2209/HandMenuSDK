using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandMenuSDK.UI
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] GameObject m_panel;
        public GameObject Panel => m_panel;
        public bool IsShow => Panel.activeSelf;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void ShowPanel()
        {
            m_panel.SetActive(true);
        }
        public virtual void HidePanel()
        {
            m_panel.SetActive(false);
        }
    }
}