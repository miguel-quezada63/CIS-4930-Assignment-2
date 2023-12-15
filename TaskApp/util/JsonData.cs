// Miguel Quezada
// Assignment #2
// 10/02/2021
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using TaskApp.model;
namespace TaskApp.util
{
    static class JsonData
    {
        static readonly string Roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static readonly string DataPath = Path.Combine(Roaming, "TaskApp\\data.json");
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
        public static void Load()
        {
            Directory.CreateDirectory(Roaming + "\\TaskApp");
            if (File.Exists(DataPath)) TaskManager.Items = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(DataPath), Settings);
        }
        public static void Save() => File.WriteAllText(DataPath, JsonConvert.SerializeObject(TaskManager.Items, Settings));
    }
}
