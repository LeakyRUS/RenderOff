using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace RenderOff
{
    public class Mod : MelonMod
    {
        private MelonPreferences_Category _approachCategory;
        private MelonPreferences_Entry<string> _approachEntry;

        private IOffMethod _approach;

        private LinkedList<IOffMethod> _approaches;

        public override void OnInitializeMelon()
        {
            FillApproaches();

            _approachCategory = MelonPreferences.CreateCategory("RenderOff_Approach");
            _approachEntry = _approachCategory.CreateEntry("Current", _approaches.First.Value.GetType().FullName);
        }

        public override void OnLateInitializeMelon()
        {
            _approach = GetApproachFromName(_approachEntry.Value);

            Application.focusChanged += _approach.FocusChanged;
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                ChangeApproach();
                MelonLogger.Msg($"New method is {_approach.GetType().FullName.Split('.').Last()}");
            }
        }

        private void FillApproaches()
        {
            var types = System.Reflection.Assembly.GetAssembly(typeof(IOffMethod))?.GetTypes()
                .Where(x => typeof(IOffMethod).IsAssignableFrom(x) && x.IsClass)
                .Select(x => (IOffMethod)Activator.CreateInstance(x)).ToList();

            _approaches = new LinkedList<IOffMethod>(types);
        }

        private IOffMethod GetApproachFromName(string name)
        {
            return _approaches.ToList().FirstOrDefault(x => x.GetType().FullName == name)
                ?? _approaches.First.Value;
        }

        private void ChangeApproach()
        {
            IOffMethod foundApproach = _approach;
            {
                var node = _approaches.First;

                while (node.Value != foundApproach)
                {
                    var nextNode = node.Next;
                    if (nextNode == null)
                    {
                        node = _approaches.First;
                        break;
                    }
                    else
                    {
                        node = nextNode;
                    }
                }

                foundApproach = node.Next?.Value ?? _approaches.First.Value;
            }

            Application.focusChanged -= _approach.FocusChanged;

            _approachEntry.Value = foundApproach.GetType().FullName;
            OnLateInitializeMelon();

            MelonPreferences.Save();
        }
    }
}
