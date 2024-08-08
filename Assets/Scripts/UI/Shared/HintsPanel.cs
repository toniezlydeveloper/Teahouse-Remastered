using System.Collections.Generic;
using Interaction;
using Internal.Dependencies.Core;
using Player;
using TMPro;
using UnityEngine;

namespace UI.Shared
{
    public interface IHintsPanel : IDependency
    {
        void Present(InteractionElement interactionElement = null);
    }
    
    public class HintsPanel : MonoBehaviour, IHintsPanel
    {
        [SerializeField] private TextMeshProUGUI spaceHintContainer;
        [SerializeField] private TextMeshProUGUI eHintContainer;
        [SerializeField] private TextMeshProUGUI qHintContainer;
        [SerializeField] private PlayerModeProxy playerModeProxy;
        [SerializeField] private GameObject spaceHint;
        [SerializeField] private GameObject eHint;
        [SerializeField] private GameObject qHint;
        [SerializeField] private GameObject hints;
        [SerializeField] private bool isAlwaysActive;
        
        private Dictionary<PlayerMode, ModeHints> _hintsByMode;

        private void OnEnable() => playerModeProxy.OnChanged += Present;

        private void OnDisable() => playerModeProxy.OnChanged -= Present;
        
        public void Present(InteractionElement interactionElement)
        {
            if (!TryStayingActive())
                ToggleHintsPanel(interactionElement);
            
            if (!TryGettingHints(interactionElement, out ModeHints modeHints))
                return;

            Present(modeHints);
        }

        private void Present(PlayerMode _)
        {
            if (!TryGettingHints(out ModeHints modeHints))
                return;
            
            Present(modeHints);
        }

        private bool TryStayingActive()
        {
            if (!isAlwaysActive)
                return false;
            
            spaceHint.SetActive(false);
            eHint.SetActive(false);
            qHint.SetActive(false);
            return true;
        }

        private void ToggleHintsPanel(InteractionElement interactionElement) => hints.SetActive(interactionElement != null);

        private void Present(ModeHints modeHints)
        {
            spaceHint.SetActive(!string.IsNullOrEmpty(modeHints.SpaceHint));
            eHint.SetActive(!string.IsNullOrEmpty(modeHints.EHint));
            qHint.SetActive(!string.IsNullOrEmpty(modeHints.QHint));

            spaceHintContainer.text = modeHints.SpaceHint;
            eHintContainer.text = modeHints.EHint;
            qHintContainer.text = modeHints.QHint;
        }

        private bool TryGettingHints(InteractionElement interactionElement, out ModeHints modeHints)
        {
            _hintsByMode = null;
            modeHints = null;

            if (!interactionElement)
                return false;
            
            _hintsByMode = interactionElement.HintsByMode;
            return _hintsByMode.TryGetValue(playerModeProxy.Value, out modeHints);
        }

        private bool TryGettingHints(out ModeHints modeHints)
        {
            modeHints = null;

            if (_hintsByMode == null)
                return false;
            
            return _hintsByMode.TryGetValue(playerModeProxy.Value, out modeHints);
        }
    }
}