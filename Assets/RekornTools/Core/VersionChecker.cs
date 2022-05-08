#if UNITY_EDITOR
using System;
using System.Text;
using JetBrains.Annotations;
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
        [SerializeField] public string store;
    }

    [InitializeOnLoad]
    public class VersionChecker
    {
        [NotNull] static string PrefPath => $"{Application.identifier}/{nameof(VersionChecker)}";

        static VersionChecker() => CheckNewVersion();

        static void CheckNewVersion()
        {
            var local = ParseLocalVersion("Assets/RekornTools/Core/VERSION.json");

            var skipVersion = EditorPrefs.GetString(PrefPath);
            if (skipVersion == local.version) return;

            GetRemoteVersion(local.server, remote => CheckVersion(local, remote));
        }

        static VersionInfo ParseLocalVersion([NotNull] string path)
        {
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if (textAsset == null)
            {
                Debug.LogError("VersionChecker: Could not find server file.");
                return default;
            }

            return JsonUtility.FromJson<VersionInfo>(textAsset.text);
        }

        static void GetRemoteVersion([NotNull] string server, [CanBeNull] Action<VersionInfo> callback)
        {
            var www = UnityWebRequest.Get(server);

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
                    Debug.LogError($"VersionChecker: {www.error}");
                }
                else
                {
                    var rawData = www.downloadHandler?.data;
                    if (rawData == null)
                    {
                        Debug.LogError("VersionChecker: Could not get data from server.");
                        return;
                    }

                    var jsonString = Encoding.UTF8.GetString(rawData, 3, rawData.Length - 3);
                    var remote     = JsonUtility.FromJson<VersionInfo>(jsonString);

                    callback?.Invoke(remote);
                }

                www.Dispose();
            };
        }

        static void CheckVersion(VersionInfo local, VersionInfo remote)
        {
            if (remote.version == local.version) return;


            var response = EditorUtility.DisplayDialogComplex(
                $"RekornTools {local.version}"
              , $"New version {remote.version} is available.\n"
              + $"You can download it with button below."
              , "Download", "Close", "Skip this version");

            switch (response)
            {
                case 0:
                    var link = GetDownloadLink(remote);
                    if (!string.IsNullOrWhiteSpace(link)) Application.OpenURL(link);
                    break;
                case 1:
                    break;
                case 2:
                    EditorPrefs.SetString(PrefPath, local.version);
                    break;
            }
        }

        static string GetDownloadLink(VersionInfo remote)
        {
            var response = EditorUtility.DisplayDialogComplex(
                $"RekornTools {remote.version}"
              , $"Choose where to download the new version."
              , "Github", "Booth", "Asset Store");

            switch (response)
            {
                case 0: return remote.github;
                case 1: return remote.booth;
                case 2: return remote.store;
            }

            return null;
        }
    }
}
#endif
