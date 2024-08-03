using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Internal.Audio
{
    [Serializable]
    public struct AudioSourcePackage
    {
        [field:SerializeField] public AudioMixerGroup Group { get; set; }
        [field:SerializeField] public AudioSoundType[] Types { get; set; }
    }
}