using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.CloudSave;

using Newtonsoft.Json;

using CoreSystems;


public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance {get; private set;}

    [Header("Settings")]
    [SerializeField] private Logger _Logger;
    [SerializeField] private bool _UseLocalPath = false;
    [SerializeField] private bool _UseCloudSave;

    private const string SAVES_PATH = "/Saves/";
    private CloudSaveClient _Client;
    private string _Path;

    private void Awake ()
    {
        if (!Instance) Instance = this;
        else Destroy(this);

        if (_UseLocalPath) _Path = Application.dataPath;
        else _Path = Application.persistentDataPath;
    }

    public void EnableCloudSave ()
    {
        _UseCloudSave = true;
        CloudSaveClient _Client = new CloudSaveClient();
    }

    public static void Save<T> (T obj, string directory, string file)
    {
        if (Instance._UseCloudSave)
        {
            Instance.CloudSave(obj, string.Concat(directory, file));
        }


        string path = GetPath(directory, file);
        Instance?._Logger.Log(Instance.gameObject, "Saving Data @ " + path);

        var json = JsonUtility.ToJson(obj);
        using StreamWriter writer = new StreamWriter(path);
        writer.Write(json);
    }

    public static T Load<T> (string directory, string file)
    {
        string path = GetPath(directory, file);
        T obj = default;

        try
        {
            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            obj = JsonUtility.FromJson<T>(json);
            
            Instance?._Logger.Log(Instance.gameObject, "Loading Data @ " + path);

            return obj;
        }
        catch (System.Exception)
        {
            Instance?._Logger.Log(Instance.gameObject, "Error Loading - path not found @ " + path);
        }

        return default(T);
    }


    public void CloudSave (object obj, string path) => _Client.Save(path, obj);

    public async Task<T> CloudLoad<T> (string path)
    {
        Dictionary<string, string> data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string>{path});

        if (data.ContainsKey(path)) return JsonConvert.DeserializeObject<T>(data[path]);
        else return default;
    }

    private static string GetPath (string directory, string file)
    {
        string path;
        if (Instance) path = string.Concat(Instance._Path, SAVES_PATH, directory);
        else path = string.Concat(Application.persistentDataPath, SAVES_PATH, directory);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        return string.Concat(path, file, ".json");
    }
}