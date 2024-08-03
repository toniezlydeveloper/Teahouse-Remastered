using Internal.Dependencies.Core;
using UnityEngine;

namespace Customers
{
    public interface IEnvironmentSetup : IDependency
    {
        Transform QueuePoint { get; }
        Transform SpawnPoint { get; }
        Transform ExitPoint { get; }
    }
    
    public class EnvironmentSetup : ADependency<IEnvironmentSetup>, IEnvironmentSetup
    {
        [field:SerializeField] public Transform QueuePoint { get; set; }
        [field:SerializeField] public Transform SpawnPoint { get; set; }
        [field:SerializeField] public Transform ExitPoint { get; set; }
    }
}