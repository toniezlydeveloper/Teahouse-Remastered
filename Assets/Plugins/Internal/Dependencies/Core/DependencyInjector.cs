using System;
using System.Collections.Generic;
using System.Linq;

namespace Internal.Dependencies.Core
{
    public static class DependencyInjector
    {
        private static readonly Dictionary<Type, IDependency> DependenciesByType = new();
        private static readonly Dictionary<Type, object> DependencyRecipesByType = new();

        public static void Inject(params IDependency[] dependencies)
        {
            foreach (IDependency dependency in dependencies)
            {
                Inject(dependency);
            }
        }

        public static TDependency Get<TDependency>() where TDependency : IDependency
        {
            if (DependenciesByType.ContainsKey(typeof(TDependency)))
            {
                return (TDependency)DependenciesByType[typeof(TDependency)];
            }

            return default;
        }

        public static void AddRecipeElement<TDependency>(TDependency element) => GetRecipe<DependencyList<TDependency>>().Value.Add(element);

        public static void RemoveRecipeElement<TDependency>(TDependency element) => GetRecipe<DependencyList<TDependency>>().Value.Remove(element);

        public static void InjectListRecipe<TDependency>() where TDependency : IDependency =>
            DependencyRecipesByType.Add(typeof(DependencyList<TDependency>), new DependencyRecipe<DependencyList<TDependency>>
            {
                Value = new DependencyList<TDependency>()
            });

        public static void InjectRecipe<TDependency>(TDependency dependency)
        {
            DependencyRecipesByType.TryAdd(typeof(TDependency), new DependencyRecipe<TDependency>());
            DependencyRecipesByType.GetTyped<TDependency>().Value = dependency;
        }

        public static void DejectRecipe<TDependency>()
        {
            if (!DependencyRecipesByType.ContainsKey(typeof(TDependency)))
            {
                return;
            }

            DependencyRecipesByType.GetTyped<TDependency>().Value = default;
        }

        public static DependencyRecipe<TDependency> GetRecipe<TDependency>() where TDependency : IDependency
        {
            DependencyRecipesByType.TryAdd(typeof(TDependency), new DependencyRecipe<TDependency>());
            return DependencyRecipesByType.GetTyped<TDependency>();
        }

        private static void Inject(IDependency dependency)
        {
            foreach (Type type in dependency.GetType().GetInterfaces().Where(type => type != typeof(IDependency)))
            {
                DependenciesByType.TryAdd(type, dependency);
            }
        }

        private static DependencyRecipe<TDependency> GetTyped<TDependency>(this Dictionary<Type, object> recipes)
        {
            object recipe = recipes[typeof(TDependency)];
            return (DependencyRecipe<TDependency>)recipe;
        }
    }
}