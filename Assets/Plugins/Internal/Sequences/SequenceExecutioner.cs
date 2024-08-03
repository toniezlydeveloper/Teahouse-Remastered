using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Internal.Sequences
{
    public class SequenceExecutioner : MonoBehaviour
    {
        [field:SerializeField] public bool ShouldExecuteOnStart { get; set; }
        [field:SerializeField] public List<ASequenceStep> Steps { get; set; }

        private List<ASequenceStep> _individualSteps;

        private void Awake() => InitBehaviours();

        private void Start()
        {
            if (!ShouldExecute())
            {
                return;
            }
            
            StartCoroutine(C_ExecuteBehaviours());
        }
        
        private bool ShouldExecute() => ShouldExecuteOnStart;

        private IEnumerator C_ExecuteBehaviours()
        {
            foreach (ASequenceStep behaviour in _individualSteps)
            {
                yield return behaviour.Execute();
            }
        }

        private void InitBehaviours()
        {
            _individualSteps = Steps.Select(Instantiate).ToList();

            foreach (ASequenceStep action in _individualSteps)
            {
                action.Init(gameObject);
            }
        }
    }
}