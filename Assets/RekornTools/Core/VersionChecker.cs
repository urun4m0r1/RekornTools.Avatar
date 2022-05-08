using System;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace RekornTools
{
    [Serializable]
    public struct VersionInfo
    {
        [SerializeField] public string version;
        [SerializeField] public string server;
        [SerializeField] public string github;
        [SerializeField] public string booth;
    }

    [InitializeOnLoad]
    public class VersionChecker
    {
        static VersionChecker() => CheckNewVersion();

        static void CheckNewVersion()
        {
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/RekornTools/Core/VERSION.json");
            if (textAsset == null)
            {
                Debug.LogError("VersionChecker: Could not find server file.");
                return;
            }

            var localVersion = JsonUtility.FromJson<VersionInfo>(textAsset.text);

            var www = UnityWebRequest.Get(localVersion.server);

            var request = www?.SendWebRequest();
            if (request == null)
            {
                Debug.LogError("VersionChecker: Can't reach server.");
                return;
            }

            request.completed += x =>
            {
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log($"VersionChecker: {www.error}");
                }
                else
                {
                    Debug.Log(www.downloadHandler?.text);
                    var remoteVersion = JsonUtility.FromJson<VersionInfo>(www.downloadHandler?.text?.Trim());
                    if (remoteVersion.version != localVersion.version)
                    {
                        EditorUtility.DisplayDialog("New version available"
                                                  , $"New version {remoteVersion.version} is available.\n"
                                                  + $"You can download it from here.\n"
                                                  + $"Github: {remoteVersion.github}\n"
                                                  + $"Booth: {remoteVersion.booth}"
                                                  , "OK");
                    }
                }

                www.Dispose();
            };
        }
    }
}
