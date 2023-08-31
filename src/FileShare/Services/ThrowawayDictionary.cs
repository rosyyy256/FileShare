namespace FileShare.Services;

public class ThrowawayDictionary<TValue> where TValue : struct
{
    private readonly Dictionary<string, TValue> _temporaryLinks = new();

    public string Create(TValue value)
    {
        RemoveByValue(value);
        var guid = Guid.NewGuid().ToString();
        _temporaryLinks.Add(guid, value);
        return guid.ToString();
    }

    public bool TryGetAndThrowaway(string key, out TValue value) => _temporaryLinks.Remove(key, out value);

    private void RemoveByValue(TValue value)
    {
        if (_temporaryLinks.ContainsValue(value))
        {
            var found = _temporaryLinks.First(v => v.Value.Equals(value));
            _temporaryLinks.Remove(found.Key);
        }
    }
}
