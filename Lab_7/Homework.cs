using System.Globalization;
using System.Text.Json.Serialization;

namespace Lab
{
    public class Homework
    {
        public const string DateFormat = "dd.MM.yy";

        [JsonPropertyName("Number")]
        private int taskNumber;
        [JsonPropertyName("Deadline")]
        private DateTime deadline;
        [JsonPropertyName("Subject")]
        private string subject;
        [JsonPropertyName("Done")]
        private bool done;
        [JsonPropertyName("Text")]
        public string taskText { get; set; }
        [JsonPropertyName("Type")]
        public TaskType taskType { get; set; } = TaskType.Default; //автовластивість

        // Статичне поле для лічильника об'єктів
        private static int objectCount = 0;

        // поля для моєї властивості - статистики по виконанню
        public static int TotalTasks { get; private set; } = Storage.GetTasks().Count();
        public static int CompletedTasks { get; private set; } = 0;
        public static int IncompleteTasks => TotalTasks - CompletedTasks;
        public static TimeSpan AverageCompletionTime { get; private set; } = TimeSpan.Zero;

        public Homework(int taskNumber, DateTime deadline, string subject, TaskType taskType, string taskText)
        {
            this.taskNumber = taskNumber;
            this.deadline = deadline;
            this.subject = subject;
            this.taskType = taskType;
            this.taskText = taskText;
            this.AddToStorage();
            objectCount++;
        }

        //конструктор тільки з текстом завдання
        public Homework(string taskText)//конструктор, який викликає інший конструктор класу
        : this(GenerateDefaultTaskNumber(), DateTime.Today, "---", TaskType.Default, taskText)
        {
            this.done = false;
        }

        // без параметрів
        public Homework() //конструктор, який викликає інший конструктор класу
        : this(GenerateDefaultTaskNumber(), DateTime.Today, "no subject", TaskType.Default, "nothing ")
        { }

        public static int GenerateDefaultTaskNumber()
        {
            int max_num = -1;

            for (int i = 0; i < Storage.GetTasks().Count; i++)
            {
                Homework tsk = Storage.GetTasks()[i];
                if (max_num < tsk.TaskNumber)
                    max_num = tsk.TaskNumber;
            }
            return max_num + 1;
        }

        // Статична властивість для отримання кількості створених об'єктів
        public static int ObjectCount
        {
            get { return objectCount; }
            set
            {
                if (value >= 0)
                    objectCount = value;
                else
                    objectCount = 0;
            }
        }

        public int TaskNumber
        {
            get { return taskNumber; }
            set
            {
                if (value > 0)
                    taskNumber = value;
                else throw new Exception("This value should be above 0");
            }
        }

        DateTime mindeadline = new DateTime(2023, 09, 01);
        DateTime maxdeadline = new DateTime(2024, 12, 31);
        public DateTime Deadline
        {
            get { return deadline; }
            set
            {
                if (DateTime.TryParseExact(value.ToString(DateFormat), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    if (parsedDate >= mindeadline && parsedDate <= maxdeadline)
                        deadline = parsedDate;
                    else throw new Exception($"Date of deadline should be in range {mindeadline.ToString(DateFormat)} - {maxdeadline.ToString(DateFormat)}.");
                }
                else throw new Exception($"Invalid date format. Please use the '{DateFormat}' for input.");
            }
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new Exception("Subject can`t be empty.");
                else if (value.Any(c => char.IsDigit(c)))
                    throw new Exception("Subject can`t contain numbers.");
                else subject = value;
            }
        }

        public bool IsExpired
        {
            get { return Deadline < DateTime.Today; }
        }

        public bool Done
        {
            get { return done; }
            private set
            {
                if (Deadline < DateTime.Today)
                    done = true;
            }
        }

        #region overloaded_methods

        public void DoTask(int nuber_of_tsk)// Реалізація методу з параметром номеру завдання
        {
            if (this.TaskNumber == nuber_of_tsk)
                this.done = true;
        }

        public bool DoTask(string subj)// Реалізація методу з параметром предмету
        {
            if (this.Subject == subj)
            {
                this.done = true;
                return true;
            }
            else return false;
        }

        public bool DoTask(DateTime date)// Реалізація методу з параметром терміну виконання
        {
            if (this.Deadline == date)
            {
                this.done = true;
                return true;
            }
            else return false;
        }
        #endregion

        private void AddToStorage()
        {
            Storage.AddTask(this);
        }

        public static Homework Parse(string s)
        {
            s = s.Trim();
            string[] parts = s.Split('|'); // Розділити рядок за роздільником

            if (parts.Length != 5)
                throw new FormatException("Invalid input format. Expected 5 \"|\"-separated values.");
         

            // Розбір параметрів та створення об'єкта Homework
            int taskNumber;
            if (!int.TryParse(parts[0], out taskNumber))
            {
                throw new FormatException("Invalid Task Number format.");
            }

            DateTime deadline;
            //string tempdate = parts[1].Trim();
            if (!DateTime.TryParseExact(parts[1].Trim(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out deadline))
                throw new FormatException($"Invalid deadline format. Please use the '{DateFormat}' format.");

            string subject = parts[2].Trim(); // Видалення зайвих пробілів

            TaskType taskType;
            if (!Enum.TryParse(parts[3], out taskType))
                throw new FormatException("Invalid Task Type format.");

            string taskText = parts[4].Trim(); // Видалення зайвих пробілів

            // Створення та ініціалізація об'єкта Homework
            Homework hw = new Homework(taskNumber, deadline, subject, taskType, taskText);

            return hw;
        }

        public static bool TryParse(string s, out Homework obj, out Exception errorMessage)
        {
            try
            {
                obj = Parse(s);
                errorMessage = null; // Успешный разбор, ошибка отсутствует
                return true;
            }
            catch (Exception ex)
            {
                obj = null;
                errorMessage = ex;
                return false;
            }
        }

        public override string ToString()
        {
            return $"Task Number: {TaskNumber} | Deadline: {Deadline.ToString(DateFormat)} | Subject: {Subject} | Task Type: {taskType} | Task Text: {taskText}";
        }

        public string SaveString()
        {
            return $"{TaskNumber}|{Deadline.ToString(DateFormat)}|{Subject}|{taskType}|{taskText}";
        }

        //static method
        public static void GetStatistics(List<Homework> tasks)
        {
            TotalTasks = Storage.GetTasks().Count();
            CompletedTasks = tasks.Count(task => task.done);
            if (CompletedTasks > 0)
            {
                AverageCompletionTime = new TimeSpan(tasks.Where(task => task.done)
                    .Select(task => (DateTime.Today - task.Deadline))
                    .Sum(timeSpan => timeSpan.Ticks) / CompletedTasks);
            }
        }

    }
}