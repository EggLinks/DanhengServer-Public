namespace EggLink.DanhengServer.Util;

/// <summary>
///     A list that can be used to randomly select an element with a certain weight from it.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RandomList<T>
{
    private readonly List<T> _List = [];

    public RandomList()
    {
    }

    public RandomList(IEnumerable<T> collection)
    {
        _List.AddRange(collection);
    }

    public void Add(T item, int weight)
    {
        for (var i = 0; i < weight; i++) _List.Add(item);
    }

    public void Remove(T item)
    {
        var temp = _List.Clone().ToList();
        _List.Clear();
        foreach (var i in temp)
            if (i?.Equals(item) == false)
                _List.Add(i);
    }

    public void AddRange(IEnumerable<T> collection, IEnumerable<int> weights)
    {
        var list = collection.ToList();
        for (var i = 0; i < list.Count; i++) Add(list[i], weights.ElementAt(i));
    }

    public T? GetRandom()
    {
        if (_List.Count == 0) return default;
        return _List[Random.Shared.Next(_List.Count)];
    }

    public void Clear()
    {
        _List.Clear();
    }

    public int GetCount()
    {
        return _List.Count;
    }
}