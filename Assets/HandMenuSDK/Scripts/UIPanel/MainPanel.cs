namespace HandMenuSDK.UI
{
    public class MainPanel : UIPanel
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnShowChecklist()
        {
            HandMenuUI.Instance.ShowUIPanel("Checklist");            
        }
        public void OnShowMapPanel()
        {
            HandMenuUI.Instance.ShowUIPanel("Map");            
        }
    }
}