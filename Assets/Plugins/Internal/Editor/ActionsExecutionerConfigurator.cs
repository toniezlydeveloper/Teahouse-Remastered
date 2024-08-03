using System.Linq;
using Internal.Sequences;
using UnityEditor;
using UnityEngine.InputSystem;

namespace Internal.Editor
{
    public class ActionsExecutionerConfigurator : EditorWindow
    {
        private SequenceExecutioner[] _executioners;
        private bool _releasedExecutionKey;
        private bool _pressedExecutionKey;
        
        [MenuItem("Tools/just Adam/Actions Executioner Configurator")]
        public static void Open() => GetWindow<ActionsExecutionerConfigurator>("Actions Executioner Configurator");

        private void OnGUI() => DrawFields();

        private void Update() => HandleOperations();

        private void DrawFields()
        {
            EditorGUILayout.HelpBox("Press S + E to assign Executioners", MessageType.Info);
            EditorGUILayout.HelpBox("Press N + E to remove null Actions", MessageType.Info);
            EditorGUILayout.HelpBox("Press A + F + E to add Action first", MessageType.Info);
            EditorGUILayout.HelpBox("Press A + L + E to add Action last", MessageType.Info);
            EditorGUILayout.HelpBox("Press R + F + E to remove Action first", MessageType.Info);
            EditorGUILayout.HelpBox("Press R + L + E to remove Action last", MessageType.Info);
            
            EditorGUILayout.LabelField("Selected Executioners", EditorStyles.boldLabel);

            SequenceExecutioner[] existingExecutioners = _executioners?.Where(executioner => executioner != null).ToArray();
            
            if (existingExecutioners == null || existingExecutioners.Length == 0)
            {
                EditorGUILayout.LabelField("There are no executioners selected");
            }
            else
            {
                foreach (SequenceExecutioner executioner in existingExecutioners)
                {
                    EditorGUILayout.LabelField(executioner.name);
                }
            }
        }

        private void HandleOperations()
        {
            if (!TryGetExecutionKeyClick())
            {
                return;
            }

            if (PressedAdditionKey())
            {
                if (TryGetStep(out ASequenceStep step))
                {
                    if (PressedFirstKey())
                    {
                        AddFirstAction(step);
                    }
                    else if (PressedLastKey())
                    {
                        AddLastAction(step);
                    }
                    
                    SaveExecutioners();
                }
            }

            if (PressedRemovalKey())
            {
                if (PressedFirstKey())
                {
                    RemoveFirstAction();
                }
                else if (PressedLastKey())
                {
                    RemoveLastAction();
                }
                
                SaveExecutioners();
            }

            if (PressedNullKey())
            {
                RemoveNulls();
                SaveExecutioners();
            }

            if (!PressedSelectionKey())
            {
                return;
            }
            
            SelectExecutioners();
            Repaint();
        }

        private static bool TryGetStep(out ASequenceStep step)
        {
            step = Selection.activeObject as ASequenceStep;
            return step != null;
        }

        private static bool PressedAdditionKey() => Keyboard.current.aKey.isPressed;

        private static bool PressedRemovalKey() => Keyboard.current.rKey.isPressed;

        private static bool PressedFirstKey() => Keyboard.current.fKey.isPressed;

        private static bool PressedLastKey() => Keyboard.current.lKey.isPressed;

        private static bool PressedNullKey() => Keyboard.current.nKey.isPressed;

        private static bool PressedSelectionKey() => Keyboard.current.sKey.isPressed;

        private void AddFirstAction(ASequenceStep step)
        {
            foreach (SequenceExecutioner executioner in _executioners)
            {
                executioner.Steps.Insert(0, step);
            }
        }

        private void AddLastAction(ASequenceStep step)
        {
            foreach (SequenceExecutioner executioner in _executioners)
            {
                executioner.Steps.Add(step);
            }
        }

        private void RemoveFirstAction()
        {
            foreach (SequenceExecutioner executioner in _executioners)
            {
                executioner.Steps.RemoveAt(0);
            }
        }

        private void RemoveLastAction()
        {
            foreach (SequenceExecutioner executioner in _executioners)
            {
                executioner.Steps.RemoveAt(executioner.Steps.Count - 1);
            }
        }

        private void RemoveNulls()
        {
            foreach (SequenceExecutioner executioner in _executioners)
            {
                executioner.Steps.RemoveAll(action => action == null);
            }
        }

        private void SaveExecutioners()
        {
            foreach (SequenceExecutioner executioner in _executioners)
            {
                EditorUtility.SetDirty(executioner);
            }
        }

        private void SelectExecutioners()
        {
            if (Selection.gameObjects == null)
            {
                return;
            }
            
            _executioners = Selection.gameObjects.Select(gameObject => gameObject.GetComponent<SequenceExecutioner>()) .ToArray();
        }

        private bool TryGetExecutionKeyClick()
        {
            if (!_releasedExecutionKey)
            {
                _releasedExecutionKey = !Keyboard.current.eKey.isPressed;
            }

            if (_releasedExecutionKey)
            {
                _pressedExecutionKey = Keyboard.current.eKey.isPressed;
            }
            else
            {
                _pressedExecutionKey = false;
            }
            
            if (!_pressedExecutionKey)
            {
                return false;
            }

            _releasedExecutionKey = false;
            return true;
        }
    }
}