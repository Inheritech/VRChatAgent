namespace Agent.Client.Services {
    public interface IStorage {
        Task SetValueAsync(string key, string value);
        Task<string?> GetValueAsync(string key);
    }
}
