using System.Net.Http.Json;

namespace SeeThroughWindows.Services
{
    /// <summary>
    /// Interface for checking application updates
    /// </summary>
    public interface IUpdateChecker
    {
        /// <summary>
        /// Event fired when an update is available
        /// </summary>
        event EventHandler<UpdateAvailableEventArgs>? UpdateAvailable;

        /// <summary>
        /// Check for updates asynchronously
        /// </summary>
        Task CheckForUpdatesAsync();
    }

    /// <summary>
    /// Event arguments for update available event
    /// </summary>
    public class UpdateAvailableEventArgs : EventArgs
    {
        public string Version { get; }
        public string DownloadUrl { get; }

        public UpdateAvailableEventArgs(string version, string downloadUrl)
        {
            Version = version;
            DownloadUrl = downloadUrl;
        }
    }

    /// <summary>
    /// Implementation of update checker using GitHub API
    /// </summary>
    public class GitHubUpdateChecker : IUpdateChecker
    {
        private const string GitHubApiUrl = "https://api.github.com/repos/MOBZystems/SeeThroughWindows/releases/latest";
        private readonly HttpClient _httpClient;

        public event EventHandler<UpdateAvailableEventArgs>? UpdateAvailable;

        public GitHubUpdateChecker()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SeeThroughWindows");
        }

        public async Task CheckForUpdatesAsync()
        {
            try
            {
                var release = await _httpClient.GetFromJsonAsync<GitHubRelease>(GitHubApiUrl);
                if (release != null)
                {
                    var currentVersion = GetCurrentVersion();
                    var latestVersion = ParseVersion(release.TagName);

                    if (latestVersion > currentVersion)
                    {
                        UpdateAvailable?.Invoke(this, new UpdateAvailableEventArgs(release.TagName, release.HtmlUrl));
                    }
                }
            }
            catch
            {
                // Ignore update check errors
            }
        }

        private Version GetCurrentVersion()
        {
            var versionString = Application.ProductVersion;
            if (Version.TryParse(versionString, out var version))
            {
                return version;
            }
            return new Version(1, 0, 0, 0);
        }

        private Version ParseVersion(string versionString)
        {
            // Remove 'v' prefix if present
            if (versionString.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                versionString = versionString.Substring(1);
            }

            if (Version.TryParse(versionString, out var version))
            {
                return version;
            }
            return new Version(0, 0, 0, 0);
        }

        /// <summary>
        /// GitHub release JSON structure
        /// </summary>
        private class GitHubRelease
        {
            public string TagName { get; set; } = "";
            public string HtmlUrl { get; set; } = "";
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
