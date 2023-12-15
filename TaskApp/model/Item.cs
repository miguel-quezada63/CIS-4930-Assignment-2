// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;

namespace TaskApp.model
{
    class Item
    {
        private static string GenerateId()
        {
            var id = Guid.NewGuid().ToString("N").Substring(0, 6);
            while (TaskManager.Items.Find(task => task.Id == id) != default(Item))
                id = Guid.NewGuid().ToString("N").Substring(0, 6);
            return id;
        }
        public Item() => Id = GenerateId();
        public Item(string name, string description)
        {
            Id = GenerateId();
            Name = name;
            Description = description;
        }
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual bool Contains(string query) => Name.Contains(query) || Description.Contains(query);
        public virtual void Log() => Console.Write("Name: {0}\nDescription: {1}\n", Name, Description);
    }
}
