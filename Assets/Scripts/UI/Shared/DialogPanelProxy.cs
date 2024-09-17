namespace UI.Shared
{
    public class DialogPanelProxy : APanelsProxy<IDialogPanel>, IDialogPanel
    {
        public void Present(DialogData data)
        {
            foreach (IDialogPanel dialogPanel in typedPanels)
            {
                dialogPanel.Present(data);
            }
        }
    }
}