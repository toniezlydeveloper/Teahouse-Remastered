using Interaction;
using UnityEngine;

namespace Organization
{
    public class OrganizationPointInteractionValidator : MonoBehaviour, IInteractionValidator
    {
        public bool CanModify => GetComponentInChildren<Organizable>() != null;
    }
}