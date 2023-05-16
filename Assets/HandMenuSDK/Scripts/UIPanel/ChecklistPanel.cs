using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandMenuSDK.UI
{
    public class ChecklistPanel : UIPanel
    {
        [SerializeField] List<Toggle> m_toggleList;        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < m_toggleList.Count; i++)
            {
                if (!m_toggleList[i].isOn) return;
            }
            HidePanel();
        }

        public override void HidePanel()
        {
            base.HidePanel();
            HandMenuUI.Instance.ShowUIPanel("Main");
            for (int i = 0; i < m_toggleList.Count; i++)
            {
                m_toggleList[i].isOn = false;
            }
        }
    }
}