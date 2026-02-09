namespace MID_PROJ.Models;
using MID_PROJ.Services;
using Newtonsoft.Json;

public class Subject : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> Topics { get; set; }

    [JsonConstructor]
    public Subject()
    {
        Name = "";
        Topics = new List<string>();
    }

    public Subject(string name, List<string> topics)
    {
        Name = name;
        Topics = topics ?? new List<string>();
    }
}
