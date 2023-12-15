// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;
using System.Collections.Generic;
using System.Linq;
using TaskApp.model;
using TaskApp.util;

namespace TaskApp
{
    internal static class TaskManager
    {
        public const int MAX_PAGE_SIZE = 3;
        public static List<Item> Items { get; set; } = new();

        public static string CreateTask(string name, string description, DateTime deadline, bool isCompleted)
        {
            var item = new Task(name, description, deadline, isCompleted);
            Items.Add(item);
            return item.Id;
        }
        public static string CreateAppointment(string name, string description, DateTime start, DateTime stop, List<string> attendees)
        {
            var item = new Appointment(name, description, start, stop, attendees);
            Items.Add(item);
            return item.Id;
        }
        public static bool DeleteItem(string id) => Items.Remove(FindItem(id));
        public static bool EditTask(string id, string newName, string newDesc, DateTime newStart = default, DateTime newStop = default, string newAttendees = default, DateTime newDeadline = default)
        {
            var found = FindItem(id);
            if (found == default) return false;
            if (newName != default && newName.Length != 0) found.Name = newName;
            if (newDesc != default && newDesc.Length != 0) found.Description = newDesc;
            if (found is Task t && newDeadline != default) t.Deadline = newDeadline;
            if (found is Appointment a)
            {
                if (newStart != default) a.Start = newStart; 
                if (newStop != default) a.Stop = newStop;
                if (newAttendees != default && newAttendees.Length != 0) a.Attendees = newAttendees.Split(',').ToList<string>();
            }
            return true;
        }
        public static void Search(string query)
        {
            var items = Items.FindAll(item => item.Contains(query));
            if (items.Count > MAX_PAGE_SIZE)
                NavigateTaskList(items);
            else if (items.Count > 0)
                items.ForEach(item => item.Log());
            else
                Console.WriteLine("There are no tasks or appointments in which the search query matches.");
        }
        public static bool CompleteTask(string id)
        {
            var task = FindItem(id) as Task;
            if (task == default(Task)) return false;
            task.IsCompleted = true;
            return true;
        }
        public static void ListOutstandingTasks()
        {
            bool FindAllOutstanding(Item item)
            {
                if (item is Task t) return !t.IsCompleted;
                return false;
            }
            var outstandingTasks = Items.FindAll(FindAllOutstanding);
            if (outstandingTasks.Count > MAX_PAGE_SIZE)
                NavigateTaskList(outstandingTasks);
            else if (outstandingTasks.Count > 0)
                outstandingTasks.ForEach(task => task.Log());
            else
                Console.WriteLine("There are no outstanding tasks to display.");
        }
        public static void ListAllTasks()
        {
            if (Items.Count > MAX_PAGE_SIZE)
                NavigateTaskList(Items);
            else if (Items.Count > 0)
                Items.ForEach(task => task.Log());
            else
                Console.WriteLine("There are no tasks to display.");
        }
        public static void NavigateTaskList<T>(List<T> items)
        {
            var navigator = new ListNavigator<T>(items);
            var isNavigating = true;
            while (isNavigating)
            {
                navigator.GetCurrentPage().Values.ToList().ForEach(item => (item as Item)?.Log());
                if(navigator.CurrentPage != 1) Console.WriteLine("F. Go to first page");
                if(navigator.CurrentPage != navigator.lastPage) Console.WriteLine("L. Go to last page");
                if(navigator.HasPreviousPage) Console.WriteLine("P. Previous");
                if(navigator.HasNextPage) Console.WriteLine("N. Next");
                Console.WriteLine("E. Exit navigator\n");
                Console.Write("Please enter selection: ");
                var input = Console.ReadLine() ?? "";
                Console.WriteLine();
                if (input.Equals("P", StringComparison.InvariantCultureIgnoreCase)) navigator.GoBackward();
                else if (input.Equals("N", StringComparison.InvariantCultureIgnoreCase)) navigator.GoForward();
                else if (input.Equals("F", StringComparison.InvariantCultureIgnoreCase)) navigator.GoToFirstPage();
                else if (input.Equals("L", StringComparison.InvariantCultureIgnoreCase)) navigator.GoToLastPage();
                else if(input.Equals("E", StringComparison.InvariantCultureIgnoreCase)) isNavigating = false;
                else Console.WriteLine("Invalid selection, please try again.\n");
            }
            Console.WriteLine();
        }
        public static Item FindItem(string id) => Items.Find(task => task.Id == id);
    }
}