using UnityEngine;

namespace Organization
{
    public class OrganizationPoint : MonoBehaviour
    {
        public bool IsTaken => TryGetOrganizable(out Organizable _);
        
        public void RotateLeft()
        {
            if (!TryGetOrganizable(out Organizable organizable))
                return;
            
            RotateLeft(organizable);
            ApplyOffset(organizable);
        }

        public void RotateRight()
        {
            if (!TryGetOrganizable(out Organizable organizable))
                return;
            
            RotateRight(organizable);
            ApplyOffset(organizable);
        }

        private void RotateLeft(Organizable organizable) => organizable.transform.Rotate(0f, -90f, 0f);
        
        private void RotateRight(Organizable organizable) => organizable.transform.Rotate(0f, 90f, 0f);

        private static void ApplyOffset(Organizable organizable) => organizable.transform.localPosition = organizable.transform.forward * organizable.ForwardOffset;

        private bool TryGetOrganizable(out Organizable organizable)
        {
            organizable = GetComponentInChildren<Organizable>();
            return organizable != null;
        }
    }
}