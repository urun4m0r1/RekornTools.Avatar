using System;
using UnityEngine;

namespace RekornTools.Avatar
{
#region List
    [Serializable] public sealed class SkinnedMeshRenderers : ComponentList<SkinnedMeshRenderer> { }

    [Serializable] public sealed class Transforms : ComponentList<Transform> { }

    [Serializable] public sealed class BonePairs : SerializedList<BonePair> { }

    [Serializable] public sealed class NamePairs : SerializedList<NamePair> { }

    [Serializable] public sealed class BonePair : RigPair<Transform> { }

    [Serializable] public sealed class NamePair : RigPair<string> { }
#endregion // List
}
