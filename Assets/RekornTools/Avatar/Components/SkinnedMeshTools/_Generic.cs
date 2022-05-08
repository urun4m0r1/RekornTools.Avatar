using System;
using UnityEngine;

namespace RekornTools.Avatar
{
#region List
    [Serializable] public sealed class SkinnedMeshRenderers : ComponentList<SkinnedMeshRenderer> { }

    [Serializable] public sealed class Transforms : ComponentList<Transform> { }

    [Serializable] public sealed class RigNamePairs : SerializedList<RigExclusion> { }
#endregion // List
}
