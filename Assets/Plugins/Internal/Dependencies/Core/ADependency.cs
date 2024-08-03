using UnityEngine;

namespace Internal.Dependencies.Core
{
    public abstract class ADependency<TDependency> : MonoBehaviour
    {
        private void Awake() => DependencyInjector.InjectRecipe(GetComponent<TDependency>());

        private void OnDestroy() => DependencyInjector.DejectRecipe<TDependency>();
    }
}