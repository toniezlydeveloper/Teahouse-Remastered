using Internal.Dependencies.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shared
{
    public class DialogData
    {
        public Sprite Icon { get; set; }
        public string Text { get; set; }
    }
    
    public interface IDialogPanel : IDependency
    {
        void Present(DialogData data);
    }
    
    public class DialogPanel : MonoBehaviour, IDialogPanel
    {
        [SerializeField] private TextMeshProUGUI textContainer;
        [SerializeField] private Image iconContainer;
        [SerializeField] private GameObject holder;

        private void Start() => holder.SetActive(false);
        
        public void Present(DialogData data)
        {
            holder.SetActive(data != null);
            
            if (data == null)
            {
                return;
            }
            
            iconContainer.sprite = data.Icon;
            textContainer.text = data.Text;
        }
    }
}