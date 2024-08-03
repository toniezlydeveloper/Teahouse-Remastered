using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace Interaction
{
    [Serializable]
    public class ModeHints
    {
        [field:SerializeField] public PlayerMode CorrespondingMode { get; set; }
        [field:SerializeField] public string SpaceHint { get; set; }
        [field:SerializeField] public string EHint { get; set; }
        [field:SerializeField] public string QHint { get; set; }
    }
    
    public class InteractionElement : MonoBehaviour
    {
        [SerializeField] private PlayerModeProxy modeProxy;
        [SerializeField] private PlayerMode handledMode;
        [SerializeField] private GameObject highlight;
        [SerializeField] private ModeHints[] hints;
        [SerializeField] private int order;
        
        public Dictionary<PlayerMode, ModeHints> HintsByMode { get; private set; }

        public PlayerMode HandledMode => handledMode;
        public int Order => order;

        private void Awake() => HintsByMode = hints.ToDictionary(hint => hint.CorrespondingMode, hint => hint);

        public void Highlight(bool state) => highlight.SetActive(state);
        
    }
}