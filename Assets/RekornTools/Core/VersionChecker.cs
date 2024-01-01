#if UNITY_EDITOR
using System;
using System.Linq;
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

        public bool IsEmpty() =>
            string.IsNullOrWhiteSpace(version)
         && string.IsNullOrWhiteSpace(server)
         && string.IsNullOrWhiteSpace(github)
         && string.IsNullOrWhiteSpace(booth);
    }

    [InitializeOnLoad]
    public class VersionChecker
    {
        [NotNull] static string PrefPath => $"{Application.identifier}/{nameof(VersionChecker)}";

        static VersionChecker() => CheckNewVersion();

        static void CheckNewVersion()
        {
            var regex = new System.Text.RegularExpressions.Regex(@"(?<=/RekornTools/)\w+/.+\.json");
            var path  = AssetDatabase.FindAssets("VERSION t:TextAsset")
                                     .Select(AssetDatabase.GUIDToAssetPath)
                                     .FirstOrDefault(x => regex.IsMatch(x));

            var local = ParseLocalVersion(path);

            if (local.IsEmpty())
            {
                Debug.LogWarning("Failed to parse update server info.");
                return;
            }

            var skipVersion = EditorPrefs.GetString(PrefPath);
            if (skipVersion == local.version) return;

            GetRemoteVersion(
                local.server
              , remote => CheckVersion(local, remote)
              , message => CheckNetwork(local, message ?? "Unknown"));
        }

        static VersionInfo ParseLocalVersion([CanBeNull] string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.LogWarning("VersionChecker: Could not find local version file.");
                return default;
            }

            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if (textAsset == null)
            {
                Debug.LogWarning("VersionChecker: Could not find local version file.");
                return default;
            }

            return JsonUtility.FromJson<VersionInfo>(textAsset.text);
        }

        static void GetRemoteVersion([NotNull] string server, [CanBeNull] Action<VersionInfo> onSuccess, [CanBeNull] Action<string> onError)
        {
            var www = UnityWebRequest.Get(server);

            var request = www?.SendWebRequest();
            if (request == null)
            {
                onError?.Invoke("Can't reach server.");
                return;
            }

            request.completed += x =>
            {
                if (www.isNetworkError || www.isHttpError)
                {
                    onError?.Invoke(www.error);
                    return;
                }

                var rawData = www.downloadHandler?.data;
                if (rawData == null)
                {
                    onError?.Invoke("Failed to get data from server.");
                    return;
                }

                var jsonString = Encoding.UTF8.GetString(rawData, 3, rawData.Length - 3);
                var remote     = JsonUtility.FromJson<VersionInfo>(jsonString);

                onSuccess?.Invoke(remote);

                www.Dispose();
            };
        }

        static void CheckVersion(VersionInfo local, VersionInfo remote)
        {
            if (remote.version == local.version) return;

            var response = EditorUtility.DisplayDialogComplex(
                $"RekornTools {local.version}"
              , $"New version {remote.version} is available.\n"
              + $"You can download it with buttons below."
              , "Download", "Close", "Skip this version");

            ShowDownloadDialog(remote, response);
        }

        static void CheckNetwork(VersionInfo local, [NotNull] string message)
        {
            var response = EditorUtility.DisplayDialogComplex(
                $"RekornTools {local.version}"
              , $"Update server unreachable. ({message})\n"
              + $"Maybe you are offline or server is down.\n"
              + $"You can try download latest version with buttons below."
              , "Download", "Close", "Skip this version");

            ShowDownloadDialog(local, response);
        }

        static void ShowDownloadDialog(VersionInfo info, int response)
        {
            switch (response)
            {
                case 0:
                    var link = GetDownloadLink(info);
                    if (!string.IsNullOrWhiteSpace(link)) Application.OpenURL(link);
                    break;
                case 1:
                    break;
                case 2:
                    EditorPrefs.SetString(PrefPath, info.version);
                    break;
            }
        }

        static string GetDownloadLink(VersionInfo info)
        {
            var response = EditorUtility.DisplayDialogComplex(
                $"RekornTools {info.version}"
              , $"Choose where to download the new version."
              , "Github", "Close", "Booth");

            switch (response)
            {
                case 0: return info.github;
                case 1: return null;
                case 2: return info.booth;
            }

            return null;
        }
    }
}
#endif
