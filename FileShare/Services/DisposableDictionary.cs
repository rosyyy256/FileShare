namespace FileShare.Services;

public class DisposableDictionary<TValue>
{
    private readonly Dictionary<string, TValue> _temporaryLinks = new();

    public string Create(TValue value)
    {
        var guid = Guid.NewGuid().ToString();
        _temporaryLinks.Add(guid, value);
        return guid.ToString();
    }

    public TValue GetAndDispose(string key)
    {
        if (_temporaryLinks.TryGetValue(key, out var value))
        {
            _temporaryLinks.Remove(key);
            return value;
        }
        else
        {
            throw new KeyNotFoundException("Disposable key not found");
        }
    }
}
