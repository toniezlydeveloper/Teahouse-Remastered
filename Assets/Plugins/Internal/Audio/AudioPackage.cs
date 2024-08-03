using System;
using UnityEngine;

namespace Internal.Audio
{
    [Serializable]
    public struct AudioPackage
    {
        [field:SerializeField] public AudioSoundType Type { get; set; }
        [field:SerializeField] public AudioClip Clip { get; set; }
    }
}