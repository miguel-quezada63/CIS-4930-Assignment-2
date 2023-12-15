// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;
using System.Collections.Generic;
namespace TaskApp.util
{
    public class ListNavigator<T>
    {
        public int pageSize { get; }
        private int currentPage;
        public int CurrentPage => currentPage;
        private List<T> state;
        public int lastPage
        {
            get
            {
                var val = state.Count / pageSize;

                if (state.Count % pageSize > 0)
                {
                    //if there is a partial page at the end, that is the actual last page.
                    val++;
                }

                return val;
            }
        }
        public bool HasPreviousPage => currentPage > 1;

        public bool HasNextPage => currentPage < lastPage;

        public ListNavigator(List<T> list, int pageSize = TaskManager.MAX_PAGE_SIZE)
        {
            this.pageSize = pageSize;
            this.currentPage = 1;
            state = list;
        }
        public Dictionary<int, T> GoForward()
        {
            if (currentPage + 1 > lastPage)
            {
                Console.WriteLine("No next page to display! Type anything other than N to exit the navigator. \n");
                return GetWindow();
            }
            currentPage++;
            return GetWindow();
        }
        public Dictionary<int, T> GoBackward()
        {
            if (currentPage - 1 <= 0)
            {
                Console.WriteLine("No previous page to display! Type anything other than P to exit the navigator.\n");
                return GetWindow();
            }
            currentPage--;
            return GetWindow();
        }
        public Dictionary<int, T> GoToPage(int page)
        {
            if (page <= 0 || page > lastPage)
            {
                throw new PageFaultException("Cannot navigate to a page outside of the bounds of the list!");
            }
            currentPage = page;
            return GetWindow();
        }
        public Dictionary<int, T> GetCurrentPage()
        {
            return GoToPage(currentPage);
        }
        public Dictionary<int, T> GoToFirstPage()
        {
            currentPage = 1;
            return GetWindow();
        }
        public Dictionary<int, T> GoToLastPage()
        {
            currentPage = lastPage;
            return GetWindow();
        }
        private Dictionary<int, T> GetWindow()
        {//(currentPage*pageSize) + pageSize
            var window = new Dictionary<int, T>();
            for (int i = (currentPage - 1) * pageSize; i < (currentPage - 1) * pageSize + pageSize && i < state.Count; i++)
                window.Add(i + 1, state[i]);
            return window;
        }
    }
    public class PageFaultException : Exception
    {
        public PageFaultException(string message) : base(message)
        {

        }
    }
}
