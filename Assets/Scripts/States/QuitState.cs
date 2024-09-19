using Internal.Flow.States;
using UnityEngine.Device;

namespace States
{
    public class QuitState : AState
    {
        public override void OnEnter()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif            
            Application.Quit();
        }
    }
}