// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;
using System.Collections.Generic;
namespace TaskApp.model
{
    class Appointment : Item
    {
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public List<string> Attendees { get; set; }
        public Appointment() : base() { }
        public Appointment(string name, string description, DateTime start, DateTime stop, List<string> attendees) : base(name, description)
        {
            Name = name;
            Description = description;
            Start = start;
            Stop = stop;
            Attendees = attendees;
        }
        public override bool Contains(string query) => base.Contains(query) || Attendees.Contains(query);
        public override void Log()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("Type        : Id");
            Console.WriteLine("--------------------");
            Console.WriteLine("Appointment : {0}", Id);
            Console.WriteLine("--------------------");
            base.Log();
            Console.WriteLine("Start: {0}\nStop: {1}\nAttendees: {2}\n", Start.ToShortDateString(), Stop.ToShortDateString(), String.Join(", ", Attendees));
        }
    }
}
