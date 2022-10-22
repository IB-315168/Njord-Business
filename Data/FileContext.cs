using System.Text.Json;
using Domain.Models;

namespace Data;

public class FileContext
{
    private const string filePath = "UserData.json";
    private DataContainer? dataContainer;

    public ICollection<User> Users
    {
        get
        {
            LoadData();
            return dataContainer!.Users;
        }
    }
    public ICollection<Team> Teams
    {
        get
        {
            LoadDataTeam();
            return dataContainer!.Teams;
        }
    }
    private void LoadDataTeam()
    {
        if (dataContainer != null) return;
    
        if (!File.Exists(filePath))
        {
            dataContainer = new ()
            {
                Teams = new List<Team>()
            };
            return;
        }
        string content = File.ReadAllText(filePath);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }
    
    private void LoadData()
    {
        if (dataContainer != null) return;
    
        if (!File.Exists(filePath))
        {
            dataContainer = new ()
            {
                Users = new List<User>()
            };
            return;
        }
        string content = File.ReadAllText(filePath);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }

    public void SaveChanges()
    {
        string serialized = JsonSerializer.Serialize(dataContainer, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(filePath, serialized);
        dataContainer = null;
    }
}