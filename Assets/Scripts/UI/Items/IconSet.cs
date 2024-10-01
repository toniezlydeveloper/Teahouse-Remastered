using System.Collections.Generic;
using UnityEngine;

namespace UI.Items
{
    [CreateAssetMenu(menuName = "Config/Icons")]
    public class IconSet : ScriptableObject
    {
        [field: SerializeField] public List<Sprite> Value { get; set; }
    }
}