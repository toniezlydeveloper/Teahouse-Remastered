using Interaction;

namespace UI.Shared
{
    public class HintsPanelProxy : APanelsProxy<IHintsPanel>, IHintsPanel
    {
        public void Present(InteractionElement interactionElement)
        {
            foreach (IHintsPanel panel in GetPanels())
                panel.Present(interactionElement);
        }
    }
}