using System.Collections.Generic;
using UnityEngine;

namespace Internal.Dependencies.Core
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DependencyList<TDependency> : List<TDependency>, IDependency
    {
    }
    
    public abstract class ADependencyElement<TDependency> : MonoBehaviour
    {
        private void Awake() => DependencyInjector.AddRecipeElement(GetComponent<TDependency>());

        private void OnDestroy() => DependencyInjector.RemoveRecipeElement(GetComponent<TDependency>());
    }
}