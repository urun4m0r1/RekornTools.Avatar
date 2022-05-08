using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [System.Serializable]
    public class ObjectList<T> : SerializedList<T> where T : Object
    {
        public void SelectComponents()
        {
            if (TryGetSelections(out var selections)) Selection.objects = selections;
        }

        public virtual bool TryGetSelections([NotNull] out Object[] selections)
        {
            selections = this.Select(x => x == null ? null : x as Object).ToArray();
            return selections.Length != 0;
        }
    }
}
