using System;
using UnityEngine;

namespace Tutorial
{
    [Serializable]
    public class TutorialStep
    {
        [field: SerializeField] public TutorialCameraType CameraType { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] [field: TextArea] public string Text { get; set; }
    }
}