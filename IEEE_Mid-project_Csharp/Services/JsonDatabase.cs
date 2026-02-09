using Newtonsoft.Json;

namespace MID_PROJ.Services;
public interface IIdentifiable
{
    int Id { get; set; }
}
public class JsonDatabase<T> where T : class, IIdentifiable
{
    private readonly string _filePath;

    public JsonDatabase(string filePath)
    {
        _filePath = filePath;
        if(!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public List<T> GetAll()
    {
        string json =File.ReadAllText(_filePath);
        List<T> models =JsonConvert.DeserializeObject<List<T>>(json) ?? [];
        return models;
    }

    public void Add(T item)
    {
        var items =GetAll();
        item.Id = items.Count > 0 ? items.Max(x => x.Id) + 1 : 0;
        items.Add(item);
        Save(items);
    }

    public T? GetById(int id)
    {
        var items =GetAll();
        return items.FirstOrDefault(item => item.Id ==id);
    }

    public void Update(int id, T data)
    {
        var items =GetAll();
        var item = items.FirstOrDefault(item => item.Id ==id);
        if(item ==null)
        {
            throw new Exception($"{typeof(T).Name} with id({id}) not found");
        }

        items.Remove(item);
        data.Id = id;
        items.Add(data);
        Save(items);
    }

    public void Delete(int id)
    {
        var items =GetAll();
        var item =items.FirstOrDefault(item => item.Id == id);

        if (item !=null)
        {
            items.Remove(item);
            Save(items);
            return;
        }
        throw new Exception($"{typeof(T).Name} with id({id}) not found");
    }

    private void Save(List<T> items)
    {
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(items, Formatting.Indented));
    }
}