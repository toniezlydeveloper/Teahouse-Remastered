namespace UI.Shared
{
    public class PausePanelProxy : APanelsProxy<IPausePanel>, IPausePanel
    {
        public void Present(PauseData data)
        {
            foreach (IPausePanel panel in typedPanels)
            {
                panel.Present(data);
            }
        }
    }
}