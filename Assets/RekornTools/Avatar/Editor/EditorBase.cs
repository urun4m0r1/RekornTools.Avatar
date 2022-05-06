using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class EditorButton : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var mono = target as MonoBehaviour;
            if (mono == null) return;

            var methods = mono.GetType()
                              .GetMembers(ReflectionExtensions.Everything)
                              .Where(x => Attribute.IsDefined(x, typeof(ButtonAttribute)));

            foreach (var memberInfo in methods)
            {
                if (GUILayout.Button(memberInfo.Name))
                {
                    (memberInfo as MethodInfo)?.Invoke(mono, null);
                }
            }
        }
    }
#endif
}
