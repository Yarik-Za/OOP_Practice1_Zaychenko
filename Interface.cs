using System.Globalization;
using System.Text.Json;

namespace Lab
{
    public class Interface
    {
        private int QuantityMax;
        private int count = Storage.GetTasks().Count;

        #region Input
        private int Input_Task_Number()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter task number: ");
                    int taskNumber;
                    int.TryParse(Console.ReadLine(), out taskNumber);
                    return taskNumber;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }
        }

        private DateTime Inptut_Task_Deadline()
        {
            while (true)
            {
                try
                {
                    Console.Write($"Enter deadline in format {Homework.DateFormat}: ");
                    string input_date = Console.ReadLine();
                    Console.WriteLine();
                    if (DateTime.TryParseExact(input_date, Homework.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                    {
                        return result;
                    }
                    else throw new Exception($"Date input error. Try correct format {Homework.DateFormat}");

                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }
        }

        private string Inptut_Task_Subject()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter subject: ");
                    string subject = Console.ReadLine();
                    return subject;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }
        }

        private TaskType Input_Task_Type()
        {
            while (true)
            {
                Console.WriteLine($"***[Task Types]****************************************\r\n" +
                    $" 0 - Comp \r\n" +
                    $" 1 - Oral \r\n" +
                    $" 2 - Write");
                Console.WriteLine($"*******************************************************");

                while (true)
                {
                    try
                    {
                        byte select;
                        select = Convert.ToByte(Input_range("Select task type: ", 2, 0));
                        switch (select)
                        {
                            case 0: return TaskType.Comp; break;
                            case 1: return TaskType.Oral; break;
                            case 2: return TaskType.Write; break;
                            default: throw new Exception("Input error. Try again"); continue;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine($"{ex.Message}"); continue; }
                }
            }
        }

        private string Input_Task_Text()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter task description: ");
                    string taskText = Console.ReadLine();
                    Console.WriteLine();
                    return taskText;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }
        }
        #endregion

        public void Create()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        QuantityMax = Input_range("Enter the quantity of hometasks: ", 10, 1);

                        if (count >= QuantityMax) throw new Exception("Reached the max quantity of tasks creation");
                        break;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); break; }
                }

                while (count < QuantityMax)
                {
                    ConstructorModes();
                    bool back = false;
                    switch (Input_range("Enter choise: ", 4, 0))
                    {
                        case 0:
                            {
                                int n = Input_Task_Number();
                                DateTime inp = Inptut_Task_Deadline();
                                string s = Inptut_Task_Subject();

                                Console.WriteLine();

                                TaskType t = Input_Task_Type();
                                string txt = Input_Task_Text();

                                Homework task = new(n, inp, s, t, txt);
                                Console.WriteLine("Full task was created successfuly\n++++++++++++++++++++++++++");

                                PressEnter();
                                count++;
                                break;
                            }

                        case 1:
                            {
                                string txt = Input_Task_Text();
                                Homework task = new(txt);
                                Console.WriteLine("Text task was created successfuly\n++++++++++++++++++++++++++");
                                PressEnter();
                                count++;
                                break;
                            }
                        case 2:
                            Homework default_t = new(); Console.WriteLine("Default created successfuly\n++++++++++++++++++++++++++"); PressEnter();
                            count++; break;
                        case 3:
                            bool ok = false;
                            while (!ok)
                            {
                                try
                                {
                                    Console.WriteLine($"Input string with:\nTask Number,Deadline (in format {Homework.DateFormat}),Subject,Task Type(Comp,Oral,Write),TaskText\nseparated by \"|\" symbol");
                                    string input = Console.ReadLine();
                                    Exception error;
                                    if (Homework.TryParse(input, out Homework homeworkObject, out error))
                                    {
                                        //немає необхідності тут створювати об'єкт, оскілки в завданні треба щоб трай парс та трай створювали об'єкт класу
                                        //new Homework(homeworkObject.TaskNumber, homeworkObject.Deadline, homeworkObject.Subject, homeworkObject.taskType, homeworkObject.TaskText);
                                        Console.WriteLine("String input by \"|\" separator was created successfuly\n++++++++++++++++++++++++++");
                                        count++; ok = true;
                                        break;
                                    }
                                    else if (error != null)
                                    {
                                        ok = false;
                                        Console.WriteLine("Failed to parse the input. Please check the format and try again.");
                                        throw error; break;
                                    }
                                }
                                catch (Exception ex) { Console.WriteLine(ex.Message.ToString() + "\n"); }
                            }
                            break;
                        case 4: MenuModes(); back = true; break;
                    }
                    if (back == true) break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void Output()
        {
            for (int i = 0; i < Storage.GetTasks().Count; i++)
            {
                if (!(Storage.GetTasks().Any()))
                    throw new Exception("No tasks exist in list");

                Print_task_info(Storage.GetTasks(), i, 1);
            }
        }

        #region find

        public void Find()
        {
            while (true)
            {
                Console.WriteLine($"Choose search option: \n" +
                    $" 0 - By task number\n" +
                    $" 1 - By deadline\n" +
                    $" 2 - Return to Menu\n");

                Console.Write("Select option: ");
                byte select = byte.Parse(Console.ReadLine());
                if (select >= 3) Console.WriteLine("Value should be in range 0-2");

                switch (select)
                {
                    case 0: FindByNumber(); break;
                    case 1: FindByDeadline(); break;
                    case 2: return;
                }
                break;
            }
        }

        public void FindByNumber()
        {
            Console.Write("Enter task number: ");
            int inputNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            int i = 0;
            foreach (var task in Storage.GetTasks())
            {
                if (inputNumber == task.TaskNumber)
                {
                    Print_task_info(Storage.GetTasks(), i, 1);
                }
                i++;
            }
        }

        public void FindByDeadline()
        {
            bool inputDateValid = false;
            while (!inputDateValid)
            {
                Console.Write($"Enter task deadline (in the format {Homework.DateFormat}): ");
                string inputDateString = Console.ReadLine();
                DateTime inputDate;

                if (DateTime.TryParseExact(inputDateString, Homework.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out inputDate))
                {
                    inputDateValid = true;
                }
                else if (!DateTime.TryParseExact(inputDateString, Homework.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out inputDate))
                {
                    Console.WriteLine($"Invalid date format. Please enter the date in {Homework.DateFormat} format.");
                    return;
                }

                Console.WriteLine();

                int i = 0;
                foreach (var task in Storage.GetTasks())
                {
                    if (inputDate == task.Deadline)
                    {
                        Print_task_info(Storage.GetTasks(), i, 1);
                    }
                    i++;
                }
            }
        }
        #endregion

        #region delete
        public void Delete()
        {
            while (true)
            {
                Console.WriteLine($"Choose delete option: \n" +
                    $" 0 - By task number\n" +
                    $" 1 - By deadline\n" +
                    $" 2 - Return to Menu\n");

                switch (Input_range("Select option: ", 2, 0))
                {
                    case 0: DeleteByNumber(); break;
                    case 1: DeleteByDeadline(); break;
                    case 2: return;
                }
                break;
            }
        }

        public void DeleteByNumber()
        {
            int inputNumber;
            while (true)
            {
                try
                {
                    Console.Write("Enter task number: ");
                    inputNumber = Convert.ToUInt16(Console.ReadLine());
                    if (inputNumber > 0)
                    {
                        Console.WriteLine(); break;
                    }
                    else throw new Exception("Number shold be above zero");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }

            List<Homework> tasksToDelete = new List<Homework>();

            // Find tasks with the same numbers and add them to tasksToDelete list
            for (int i = 0; i < Storage.GetTasks().Count; i++)
            {
                Homework task = Storage.GetTasks()[i];
                if (inputNumber == task.TaskNumber)
                {
                    tasksToDelete.Add(task);
                }
            }

            if (tasksToDelete.Count == 0)
            {
                Console.WriteLine("No tasks found with the specified number.");
                return;
            }

            Console.WriteLine("Tasks with the specified number:");
            for (int i = 0; i < tasksToDelete.Count; i++)
            {
                Print_task_info(tasksToDelete, i, 2);
            }

            Console.Write("Enter the number of the task to delete: ");
            if (int.TryParse(Console.ReadLine(), out int selectedTaskIndex) && selectedTaskIndex >= 1 && selectedTaskIndex <= tasksToDelete.Count)
            {
                // Remove the selected task
                Storage.GetTasks().Remove(tasksToDelete[selectedTaskIndex - 1]);
                Console.WriteLine("Task deleted successfully.");
                Homework.ObjectCount--;
            }
            else
            {
                Console.WriteLine("Invalid input. Task was not deleted.");
            }
        }

        public void DeleteByDeadline()
        {
            Console.Write($"Enter task deadline (in the format {Homework.DateFormat}): ");
            string inputDateString = Console.ReadLine();
            DateTime inputDate;

            if (!DateTime.TryParseExact(inputDateString, Homework.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out inputDate))
            {
                Console.WriteLine($"Invalid date format. Please enter the date in {Homework.DateFormat} format.");
                return;
            }

            Console.WriteLine();
            List<Homework> tasksToDelete = new List<Homework>();

            // Find tasks with the same deadline and add them to tasksToDelete list
            for (int i = 0; i < Storage.GetTasks().Count; i++)
            {
                Homework task = Storage.GetTasks()[i];
                if (inputDate == task.Deadline)
                {
                    tasksToDelete.Add(task);
                }
            }

            if (tasksToDelete.Count == 0)
            {
                Console.WriteLine("No tasks found with the specified deadline.");
                return;
            }

            Console.WriteLine("Tasks with the specified deadline:");
            for (int i = 0; i < tasksToDelete.Count; i++)
            {
                Print_task_info(tasksToDelete, i, 2);
            }

            Console.Write("Enter the number of the task to delete: ");
            if (int.TryParse(Console.ReadLine(), out int selectedTaskIndex) && selectedTaskIndex >= 1 && selectedTaskIndex <= tasksToDelete.Count)
            {
                // Remove the selected task
                Storage.GetTasks().Remove(tasksToDelete[selectedTaskIndex - 1]);
                Console.WriteLine("Task deleted successfully.");
                Homework.ObjectCount--;
            }
            else
            {
                Console.WriteLine("Invalid input. Task was not deleted.");
            }
        }

        public void Clear_Colection()
        {
            Storage.GetTasks().Clear();
        }
        #endregion

        #region overloaded_methods

        int tasksMarkedAsDone = 0;

        private void Done_task_choose()
        {
            List<Homework> undone_tsks = CreateList(task => task.Done == false);
            Console.WriteLine("Undone tasks list:");
            for (int i = 0; i < undone_tsks.Count; i++)
            {
                Print_task_info(undone_tsks, i, 2);
            }
            Console.Write("Enter the number of the task to mark as done: ");
            if (int.TryParse(Console.ReadLine(), out int selectedTaskIndex) && selectedTaskIndex >= 1 && selectedTaskIndex <= undone_tsks.Count)
            {
                if (Done_task_choose_logic(selectedTaskIndex))
                    Console.WriteLine("Task marked as done successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input. Task was not marked as done. Try again");
            }
        }

        //логіка винесена в окремі методи задля позбавлення залежності методу Done_task_... від класу Console
        public bool Done_task_choose_logic(int selection)
        {
            Homework selected_task = Storage.GetTasks()[selection - 1];
            selected_task.DoTask(selected_task.TaskNumber);// Done the selected task
            return true;
        }

        private void Done_task_subj()
        {
            List<Homework> undone_tsks = CreateList(task => task.Done == false);

            Console.WriteLine("Undone tasks list:");
            for (int i = 0; i < undone_tsks.Count; i++)
                Print_task_info(undone_tsks, i, 2);

            Console.Write("Enter the subject of the task/s to mark as done: ");
            string input_subj = Console.ReadLine();

            if (!int.TryParse(input_subj, out int selected_subj) && !input_subj.Any(c => char.IsDigit(c)))
            {
                // Введений рядок не не містить числа (це можуть бути символи або комбінація символів і цифр)

                //Done_task_subj_logic(input_subj, undone_tsks);
                if (Done_task_subj_logic(input_subj, undone_tsks) > 0)
                    Console.WriteLine($"Marked {tasksMarkedAsDone} task(s) as done successfully.");
                else Console.WriteLine("No matching tasks found for the entered deadline.");
            }
            else Console.WriteLine("Invalid input. Any task was not marked as done. Try again");
        }

        public int Done_task_subj_logic(string selected_subj, List<Homework> undone)
        {
            tasksMarkedAsDone = 0;
            foreach (var task in undone)
                if (task.DoTask(selected_subj)) // Done the selected subject task
                    tasksMarkedAsDone++;

            return tasksMarkedAsDone;
        }

        private void Done_task_deadline()
        {
            List<Homework> undone_tsks = CreateList(task => task.Done == false);

            Console.WriteLine("Undone tasks list:");
            for (int i = 0; i < undone_tsks.Count; i++)
            {
                Print_task_info(undone_tsks, i, 2);
            }

            Console.Write($"Enter the deadline in format {Homework.DateFormat} of the task/s to mark as done: ");
            string input_date = Console.ReadLine();

            if (DateTime.TryParseExact(input_date, Homework.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime inputDate))
            {
                // Введена дата в правильному форматі

                if (Done_task_deadline_logic(undone_tsks, inputDate) > 0)
                    Console.WriteLine($"Marked {tasksMarkedAsDone} task(s) as done successfully.");
                else Console.WriteLine("No matching tasks found for the entered deadline.");
            }
            else Console.WriteLine("Invalid date format. No tasks were marked as done. Please use the correct date format.");

        }

        public int Done_task_deadline_logic(List<Homework> undone, DateTime selected_date)
        {
            tasksMarkedAsDone = 0;
            foreach (var task in undone)
                if (task.DoTask(selected_date))
                    tasksMarkedAsDone++;

            return tasksMarkedAsDone;
        }

        public List<Homework> CreateList(Func<Homework, bool> condition)
        {
            List<Homework> filtered_tasks = new List<Homework>();

            foreach (Homework task in Storage.GetTasks())
            {
                if (condition(task))
                {
                    filtered_tasks.Add(task);
                }
            }

            return filtered_tasks;
        }

        #endregion

        #region Serialization/Deserialization

        public static void SaveToTxt(List<Homework> tasks, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
                foreach (Homework task in tasks)
                    writer.WriteLine(task.SaveString());
        }

        public static void SaveToJson(List<Homework> tasks, string path)
        {
            try
            {
                string jsonstring = "";
                foreach (var t in tasks)
                    jsonstring += JsonSerializer.Serialize<Homework>(t) + "\n";

                File.WriteAllText(path, jsonstring);
                Console.WriteLine($"Check out the JSON file at: {Path.GetFullPath(path)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static List<Homework> LoadFromTxt(string filePath)
        {
            List<Homework> tasks = new List<Homework>();

            if (!File.Exists(filePath))
                throw new Exception($"File doesn't exist at path {filePath}");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    if (Homework.TryParse(line, out Homework task, out Exception errorMessage))
                        tasks.Add(task);
                    else throw new Exception($"Error parsing line: {errorMessage.Message}");
            }
            return tasks;
        }

        public static List<Homework> LoadFromJson(string path)
        {
            List<Homework> r_json = new List<Homework>();
            try
            {
                List<string> lines = new List<string>();
                lines = File.ReadAllLines(path).ToList();
                // або
                // string[] lines = File.ReadAllLines(path);
                Console.WriteLine("\nContents of JSON Account file:\n");
                foreach (var item in lines)
                    Console.WriteLine(item);

                foreach (var item in lines)
                {
                    Homework? hw = JsonSerializer.Deserialize<Homework>(item);
                    if (hw != null) r_json.Add(hw);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Reading JSON file error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return r_json;
        }

        #endregion

        #region UI

        public void MenuModes()
        {
            Console.Clear();

            Console.WriteLine("" +
                   $"***************************************[Modes menu]***\n" +
                   $" 0 - Create task\n" +
                   $" 1 - Output tasks\n" +
                   $" 2 - Find task\n" +
                   $" 3 - Delete task\n" +
                   $" 4 - Overloaded Methods menu\n" +
                   $" 5 - Static Methods menu\n" +
                   $" 6 - Save the collection of objects to a file\n" +
                   $" 7 - Read the collection of objects from a file\n" +
                   $" 8 - Clear the collection of objects\n" +
                   $" 9 - Close app\n" +
                   $"*******************************************************");
        }

        #region serialization_modes
        public void Save_modes()
        {
            Console.Clear();

            Console.WriteLine("" +
            $"**********************************[Saving File Menu]***\n" +
            $" 1 - Save to *.txt\n" +
            $" 2 - Save to *.json\n" +
            $" 0 - Return to Main Menu\n" +
            $"*******************************************************");

            switch (Input_range("Enter choice: ", 2, 0))
            {
                case 1:
                    List<Homework> save_txt_list = Storage.GetTasks();
                    SaveToTxt(save_txt_list, "tasks_list.txt"); break;
                case 2:
                    List<Homework> save_json_list = Storage.GetTasks();
                    SaveToJson(save_json_list, "tasks_list.json"); break;
                case 0: break;
            }
        }

        public void Read_modes()
        {
            Console.Clear();

            Console.WriteLine("" +
            $"**********************************[Loading File Menu]***\n" +
            $" 1 - Load from *.txt\n" +
            $" 2 - Load from *.json\n" +
            $" 0 - Return to Main Menu\n" +
            $"*******************************************************");

            switch (Input_range("Enter choice: ", 2, 0))
            {
                case 1: List<Homework> loadedTasksTxt = LoadFromTxt("tasks_list.txt"); break;

                case 2: List<Homework> loadedTasksJson = LoadFromJson("tasks_list.json"); break;

                case 0: break;
            }
        }
        #endregion

        #region modes
        public void ConstructorModes()
        {
            Console.Clear();

            Console.WriteLine("" +
                  $"*****************************[Task`s constuctor menu]***\n" +
                  $" 0 - Input all values\n" +
                  $" 1 - Input only Task Text\n" +
                  $" 2 - Debug default constructor\n" +
                  $" 3 - Create by \"|\" separated string in order\n\"Task Number,Deadline (in  format{Homework.DateFormat}),Subject,Task Type(Comp,Oral,Write),TaskText\"\n" +
                  $" -----------------------------\n" +
                  $" 4 - Return to Main Menu\n" +
                  $"*******************************************************");
        }

        public void OverloadedMethodsModes()
        {
            Console.Clear();

            Console.WriteLine("" +
                  $"**************************************[Overloaded Methods menu]***\n" +
                  $" 0 - Done task by choose\n" +
                  $" 1 - Done task by Subject\n" +
                  $" 2 - Done task by Deadline\n" +
                  $" 3 - Return to Main Menu\n" +
                  $"*******************************************************");
            switch (Input_range("Enter choise: ", 3, 0))
            {
                case 0: Done_task_choose(); break;
                case 1: Done_task_subj(); break;
                case 2: Done_task_deadline(); break;
                case 3: break;
            }
        }

        public void StaticMethodsModes()
        {
            Console.Clear();

            Console.WriteLine("" +
                  $"*******************************[Static Methods menu]***\n" +
                  $" 0 - Get quantity of all homeworks\n" +
                  $" 1 - Get statistics\n" +
                  $" 2 - Return to Main Menu\n" +
                  $"*******************************************************");
            switch (Input_range("Enter choise: ", 3, 0))
            {
                case 0: int q = Homework.ObjectCount; Console.WriteLine($"Quantity of hometask is {q}"); break;
                case 1:
                    {
                        List<Homework> tasks = Storage.GetTasks(); // Отримуємо список завдань з Storage
                        Homework.GetStatistics(tasks); // Оновлюємо статистику для завдань

                        Console.WriteLine("Statistics:"); // Виводимо статистику
                        Console.WriteLine($"Total amount of tasks: {Homework.TotalTasks}");
                        Console.WriteLine($"Amount of completed tasks: {Homework.CompletedTasks}");
                        Console.WriteLine($"Amount of INcompleted tasks: {Homework.IncompleteTasks}");

                        TimeSpan averageTime = Homework.AverageCompletionTime; // Отримуємо середній термін виконання

                        averageTime = TimeSpan.FromTicks(Math.Abs(averageTime.Ticks)); // модуль для завжди позитивного значення

                        int days = averageTime.Days; // Отримуємо кількість днів та годин
                        int hours = averageTime.Hours;
                        Console.WriteLine($"The average completion time: {days} days and {hours} hours"); break;
                    }
                case 2: break;
            }
        }
        #endregion

        #region ui_interaction
        private void Print_task_info(List<Homework> t, int id, int var)
        {

            if (var == 1)
                Console.Write($"*****************************************[Index:{id}]***\n    ");

            else if (var == 2)
                Console.Write($"[{id + 1}] ");
            Console.WriteLine($"Task number: {t[id].TaskNumber}\n" +
                              $"    Task deadline: {t[id].Deadline.ToString(Homework.DateFormat)}\n" +
                              $"    Task subject: {t[id].Subject} \n" +
                              $"    Task description: {t[id].taskText}\n" +
                              $"    Task type: {t[id].taskType}\n" +
                              $"    Task done: {t[id].Done}\n" +
                              $"    Task expired: {t[id].IsExpired}");

            if (var == 1) Console.WriteLine($"*****************************************************\n");
            else if (var == 2)
                Console.WriteLine();
        }

        public ushort Input_range(string text, ushort up_range, ushort down_range)
        {
            while (true)
            {
                ushort input;

                try
                {
                    Console.Write(text);
                    input = Byte.Parse(Console.ReadLine());

                    if (input > up_range || input < down_range)
                        throw new Exception($"Value should be in range {down_range}-{up_range}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    continue;
                }
                Console.WriteLine();
                return input;
            }
        }

        public void PressEnter()
        {
            while (true)
            {
                Console.WriteLine("To continiue press Enter...");
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    break; // Выход из цикла, если нажата клавиша Enter
                }
                else Console.WriteLine("Pressed another key");
            }
        }
        #endregion

        #endregion

    }
}