using Agent.Client.Services;
using System.Text.Json;

namespace Agent.API.Services {
    public class LocalStorage : IStorage {
        private const string FilePath = "state.json";
        private Dictionary<string, string> _data;
        private bool _isLoaded = false;

        private async Task EnsureLoadedAsync() {
            if (!_isLoaded) {
                _data = await LoadAsync();
                _isLoaded = true;
            }
        }

        private async Task SaveAsync() {
            var json = JsonSerializer.Serialize(_data);
            await File.WriteAllTextAsync(FilePath, json);
        }

        private async Task<Dictionary<string, string>> LoadAsync() {
            if (!File.Exists(FilePath)) {
                return [];
            }

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
        }

        public async Task SetValueAsync(string key, string value) {
            await EnsureLoadedAsync();
            _data[key] = value;
            await SaveAsync();
        }

        public async Task<string?> GetValueAsync(string key) {
            await EnsureLoadedAsync();
            _data.TryGetValue(key, out var value);
            return value;
        }
    }
}
