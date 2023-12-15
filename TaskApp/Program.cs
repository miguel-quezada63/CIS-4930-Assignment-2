// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;
using System.Linq;
using TaskApp.model;
using TaskApp.util;

namespace TaskApp
{
    class Program {
        private static class MenuCommand
        {
            internal const string CREATE = "create";
            internal const string DELETE = "delete";
            internal const string EDIT = "edit";
            internal const string COMPLETE = "complete";
            internal const string LIST_OUTSTANDING = "listout";
            internal const string LIST_ALL = "listall";
            internal const string SEARCH = "search";
            internal const string EXIT = "exit";
        }
        static void Main(string[] args)
        {
            JsonData.Load(); // load JSON data into Items from Roaming folder
            for (string input = ""; input != MenuCommand.EXIT;)
            {
                PrintMenu();
                input = Console.ReadLine().ToLower();
                var inputTrimmed = string.Concat(input.Where(c => !char.IsWhiteSpace(c))); // trims white space from the input string
                Console.WriteLine();
                switch (inputTrimmed)
                {
                    case MenuCommand.CREATE:
                        Create();
                        break;
                    case MenuCommand.EDIT:
                        Edit();
                        break;
                    case MenuCommand.DELETE:
                        Delete();
                        break;
                    case MenuCommand.COMPLETE:
                        Complete();
                        break;
                    case MenuCommand.LIST_OUTSTANDING:
                        TaskManager.ListOutstandingTasks();
                        break;
                    case MenuCommand.LIST_ALL:
                        TaskManager.ListAllTasks();
                        break;
                    case MenuCommand.SEARCH:
                        Search();
                        break;
                    case MenuCommand.EXIT:
                        JsonData.Save();
                        break;
                    default:
                        Console.Write("Invalid selection, please try again.\n");
                        break;
                }
                Console.WriteLine();
            }
        }
        private static void Create()
        {
            Console.Write("Is this task an appointment (y/N): ");
            var isAppointment = char.ToLower(char.Parse(Console.ReadLine() ?? "")) == 'y';
            var typeOfTask = isAppointment ? "appointment" : "task";
            Console.Write("Please enter the name of the {0}: ", typeOfTask);
            var name = Console.ReadLine();
            Console.Write("Please enter the description of the {0}: ", typeOfTask);
            var description = Console.ReadLine();
            if (isAppointment)
            {
                Console.Write("Please enter the start date of the {0} (MM/DD/YYYY): ", typeOfTask);
                DateTime start, stop;
                while (!DateTime.TryParse(Console.ReadLine(), out start))
                    Console.Write("Invalid date entered. Please enter the start date of the {0} (MM/DD/YYYY): ", typeOfTask);
                Console.Write("Please enter the stop date of the {0} (MM/DD/YYYY): ", typeOfTask);
                while (!DateTime.TryParse(Console.ReadLine(), out stop) || stop < start)
                    Console.Write("Invalid date entered. Please enter the stop date of the {0} (MM/DD/YYYY): ", typeOfTask);
                Console.Write("Please enter the attendees of this {0} (separate attendees with a single comma, no spaces): ", typeOfTask);
                var attendees = Console.ReadLine() ?? "";
                var id = TaskManager.CreateAppointment(name, description, start, stop, attendees.Split(',').ToList());
                Console.WriteLine("Appointment with Id {1} successfully created", typeOfTask, id);
            }
            else
            {
                Console.Write("Please enter the deadline of the {0} (MM/DD/YYYY): ", typeOfTask);
                DateTime deadline;
                while (!DateTime.TryParse(Console.ReadLine(), out deadline))
                    Console.Write("Invalid date entered. Please enter the deadline of the {0} (MM/DD/YYYY): ", typeOfTask);
                Console.Write("Is the {0} completed (y/N): ", typeOfTask);
                var isCompleted = Char.ToLower(Char.Parse(Console.ReadLine() ?? "")) == 'y';
                var id = TaskManager.CreateTask(name, description, deadline, isCompleted);
                Console.WriteLine("{0} with Id {1} successfully created", typeOfTask, id);
            }
        }
        private static void Edit()
        {
            Console.Write("Please enter the ID of the task you would like to edit: ");
            if (TaskManager.Items.Count <= 0)
            {
                Console.WriteLine("There are currently no tasks to edit, please create a task first before editing.");
                return;
            }
            var id = Console.ReadLine();
            if (TaskManager.FindItem(id) == default(Item))
            {
                Console.WriteLine("Task with ID {0} not found.", id);
                return;
            }
            Console.WriteLine("Please enter the new information for each respective attribute. If you do not want to edit an attribute, simply leave the input blank.");
            Console.Write("Please enter the new name of the task (leave empty if no change): ");
            var name = Console.ReadLine();
            Console.Write("Please enter the new description of the task (leave empty if no change): ");
            var description = Console.ReadLine();
            if (TaskManager.FindItem(id) is Appointment a)
            {
                DateTime start = default;
                DateTime stop = default;
                Console.Write("Please enter the new start date of the appointment (MM/DD/YYYY) (leave empty if no change): ");
                var temp = Console.ReadLine() ?? "";
                if (temp.Length != 0 && !DateTime.TryParse(temp, out start))
                {
                    while (!DateTime.TryParse(Console.ReadLine(), out start))
                        Console.Write("Invalid date entered. Please enter the new start date of the appointment (MM/DD/YYYY): ");
                }
                Console.Write("Please enter the new stop date of the appointment (MM/DD/YYYY) (leave empty if no change): ");
                temp = Console.ReadLine() ?? "";
                if (temp.Length != 0 && !DateTime.TryParse(temp, out stop))
                {
                    while (!DateTime.TryParse(Console.ReadLine(), out stop))
                        Console.Write("Invalid date entered. Please enter the new stop date of the appointment (MM/DD/YYYY): ");
                }
                Console.Write("Please enter the attendees of this appointment (separate attendees with a single comma, no spaces. Leave empty if no change): ");
                string attendees = Console.ReadLine() ?? default;
                TaskManager.EditTask(id, name, description, start, stop, attendees);
            }
            else
            {            
                Console.Write("Please enter the new deadline of the task (MM/DD/YYYY) (leave empty if no change): ");
                DateTime deadline = default;
                var temp = Console.ReadLine() ?? "";
                if (temp.Length != 0 && !DateTime.TryParse(temp, out deadline))
                {
                    while (!DateTime.TryParse(Console.ReadLine(), out deadline))
                        Console.Write("Invalid date entered. Please enter the new deadline of the task (MM/DD/YYYY): ");
                }
                TaskManager.EditTask(id, name, description, newDeadline: deadline);
            }
        }
        private static void Delete()
        {
            Console.Write("Please enter the ID of the task you would like to delete: ");
            var id = Console.ReadLine();
            var isSuccessfullyDeleted = TaskManager.DeleteItem(id);
            Console.WriteLine(
                isSuccessfullyDeleted ? "\nTask with ID {0} successfully deleted." : "\nTask with ID {0} not found.",
                id);
        }
        private static void Complete()
        {
            Console.Write("Please enter the ID of the task you would like to complete: ");
            var id = Console.ReadLine();
            var isSuccessfullyCompleted = TaskManager.CompleteTask(id);
            Console.WriteLine(
                isSuccessfullyCompleted
                    ? "\nTask with ID {0} successfully set to complete."
                    : "\nTask with ID {0} unsuccessfully set to complete.", id);
        }
        private static void Search()
        {
            Console.Write("Please enter the query you would like to use to search for tasks: ");
            var input = Console.ReadLine();
            Console.WriteLine();
            TaskManager.Search(input);
        }
        private static void PrintMenu()
        {
            Console.Write(
                "Please select an option from the following menu:\n\n" +
                "Create a new task: {0}\n" +
                "Delete an existing task: {1}\n" +
                "Edit an existing task: {2}\n" +
                "Set an existing task to completed: {3}\n" +
                "List all outstanding tasks: {4}\n" +
                "List all tasks: {5}\n" +
                "Search for tasks: {6}\n" +
                "Exit: {7}\n\n" +
                "Input: ",
                MenuCommand.CREATE,
                MenuCommand.DELETE,
                MenuCommand.EDIT,
                MenuCommand.COMPLETE,
                MenuCommand.LIST_OUTSTANDING,
                MenuCommand.LIST_ALL,
                MenuCommand.SEARCH,
                MenuCommand.EXIT
            );
        }
    }
}