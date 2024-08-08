using System.Collections.Generic;

namespace UI.Shared
{
    public class FurnishingPanelProxy : APanelsProxy<IFurnishingPanel>, IFurnishingPanel
    {
        public void Present(List<FurniturePieceData> pieces)
        {
            foreach (IFurnishingPanel panel in GetPanels())
                panel.Present(pieces);
        }

        public void Present(int selectedIndex)
        {
            foreach (IFurnishingPanel panel in GetPanels())
                panel.Present(selectedIndex);
        }

        public void Present(bool state)
        {
            foreach (IFurnishingPanel panel in GetPanels())
                panel.Present(state);
        }
    }
}