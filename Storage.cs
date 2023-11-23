
namespace Lab
{
    public static class Storage
    {
        private static List<Homework> Tasks = new List<Homework>();

        public static void AddTask(Homework task)
        {
            Tasks.Add(task);
        }

        public static List<Homework> GetTasks()
        {
            return Tasks;
        }
    }
}