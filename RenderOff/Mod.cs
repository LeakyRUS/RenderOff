using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenderOff
{
    public class Mod : MelonMod
    {
        private MelonPreferences_Category _methodCategory;
        private MelonPreferences_Entry<string> _methodEntry;

        private IOffMethod _selectedMethod;
        private LinkedList<IOffMethod> _methods;

        private string _GUIMessage = string.Empty;
        private DateTime _GUIMessageCreated;

        public override void OnInitializeMelon()
        {
            FillMethods();

            _methodCategory = MelonPreferences.CreateCategory("RenderOff_Method");
            _methodEntry = _methodCategory.CreateEntry("Current", _methods.First.Value.GetType().FullName);
        }

        public override void OnLateInitializeMelon()
        {
            SetUp();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                ChangeMethod();
                var msg = $"{_selectedMethod.GetType().FullName.Split('.').Last()} selected";
                _GUIMessage = msg;
                _GUIMessageCreated = DateTime.Now;
                MelonLogger.Msg(msg);
            }
        }

        public override void OnGUI()
        {
            if (!string.IsNullOrWhiteSpace(_GUIMessage) && _GUIMessageCreated.AddSeconds(3) > DateTime.Now)
            {
                GUI.Label(new Rect(10, 10, 300, 25), _GUIMessage);
            }
            else
            {
                _GUIMessage = string.Empty;
            }
        }

        public override void OnApplicationQuit()
        {
            Application.focusChanged -= _selectedMethod.FocusChanged;
            _selectedMethod.FocusChanged(true);
        }

        private void FillMethods()
        {
            var types = typeof(IOffMethod).Assembly.GetTypes()
                .Where(x => typeof(IOffMethod).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => (IOffMethod)Activator.CreateInstance(x)).ToList();

            _methods = new LinkedList<IOffMethod>(types);
        }

        private IOffMethod GetMethodFromName(string name)
        {
            return _methods.ToList().FirstOrDefault(x => x.GetType().FullName == name)
                ?? _methods.First.Value;
        }

        private void SetUp()
        {
            _selectedMethod = GetMethodFromName(_methodEntry.Value);

            Application.focusChanged += _selectedMethod.FocusChanged;
        }

        private void ChangeMethod()
        {
            IOffMethod foundApproach = _selectedMethod;
            {
                var node = _methods.First;

                while (node.Value != foundApproach)
                {
                    var nextNode = node.Next;
                    if (nextNode == null)
                    {
                        node = _methods.First;
                        break;
                    }
                    else
                    {
                        node = nextNode;
                    }
                }

                foundApproach = node.Next?.Value ?? _methods.First.Value;
            }

            Application.focusChanged -= _selectedMethod.FocusChanged;

            _methodEntry.Value = foundApproach.GetType().FullName;
            SetUp();

            MelonPreferences.Save();
        }
    }
}
