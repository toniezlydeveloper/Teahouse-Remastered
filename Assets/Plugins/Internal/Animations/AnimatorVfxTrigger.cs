using UnityEngine;

namespace Internal.Animations
{
    public class AnimatorVfxTrigger : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private Vector3 spawnOffset;
        [SerializeField] private float vfxLifeTime;

        public void SpawnVfx()
        {
            Destroy(Instantiate(vfxPrefab, spawnPoint.position + spawnOffset, Quaternion.identity), vfxLifeTime);
        }
    }
}