using UnityEngine;

namespace Internal.Sequences
{
    [CreateAssetMenu(menuName = "Data/Sequence Point")]
    public class SequencePoint : ScriptableObject
    {
        [field:SerializeField] public Vector3 ViewportPoint { get; set; }

        private static Camera _mainCamera;

        public Vector3 WorldPoint
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera!.ViewportToWorldPoint(ViewportPoint);
            }
        }
    }
}