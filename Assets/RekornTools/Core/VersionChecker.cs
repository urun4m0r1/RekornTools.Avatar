using UnityEditor;
using UnityEngine;

namespace RekornTools
{
    [InitializeOnLoad]
    public class VersionChecker
    {
        static VersionChecker()
        {
            CheckNewVersion();
        }

        static void CheckNewVersion()
        {
            Debug.Log("Checking for new version...");
        }
    }
}
