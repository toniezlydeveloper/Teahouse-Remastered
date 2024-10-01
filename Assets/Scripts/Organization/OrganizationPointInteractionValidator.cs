using Interaction;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Organization
{
    public class OrganizationPointInteractionValidator : MonoBehaviour, IInteractionValidator
    {
        public bool CanModify => GetComponentInChildren<Organizable>() != null != DependencyInjector.GetRecipe<IOrganizer>().Value.HoldsItem;
    }
}