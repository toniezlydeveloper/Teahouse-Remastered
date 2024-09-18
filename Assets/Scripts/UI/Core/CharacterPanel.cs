using System;
using Internal.Dependencies.Core;
using Internal.Flow.UI;
using UnityEngine;

namespace UI.Core
{
    public interface ICharacterPanel : IDependency
    {
        void Init(CharacterData data);
    }

    public class CharacterData
    {
        public Action PreviousSpeciesCallback { get; set; }
        public Action NextSpeciesCallback { get; set; }
        public Action PreviousOutfitCallback { get; set; }
        public Action NextOutfitCallback { get; set; }
        public Action<Color> ColorCallback { get; set; }
    }
    
    public class CharacterPanel : AUIPanel, ICharacterPanel
    {
        public void Init(CharacterData data)
        {
        }
    }
}