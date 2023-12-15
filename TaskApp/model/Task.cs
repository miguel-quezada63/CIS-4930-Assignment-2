// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;
namespace TaskApp.model
{
    class Task : Item
    {
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }

        public Task() : base() { }

        public Task(string name, string description, DateTime deadline, bool isCompleted) : base(name, description)
        {
            Deadline = deadline;
            IsCompleted = isCompleted;
        }
        public override void Log()
        {
            Console.WriteLine("-------------");
            Console.WriteLine("Type : Id");
            Console.WriteLine("-------------");
            Console.WriteLine("Task : {0}", Id);
            Console.WriteLine("-------------");
            base.Log();
            Console.WriteLine("Deadline: {0}\nIsCompleted: {1}\n", Deadline.Date.ToShortDateString(), IsCompleted);
        }
    }
}
