namespace UI.Shared
{
    public class CurrencyPanelProxy : APanelsProxy<ICurrencyPanel>, ICurrencyPanel
    {
        public void Present(int amount)
        {
            foreach (ICurrencyPanel panel in typedPanels)
                panel.Present(amount);
        }
    }
}