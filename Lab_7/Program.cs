using System.Text;

namespace Lab
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // можливість зчитування та виведення кирилічних символів
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.InputEncoding = System.Text.Encoding.GetEncoding(1251);
            //Console.OutputEncoding = System.Text.Encoding.GetEncoding(1251);
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            #region debug_info

            //debug list

            //var homework1 = new Homework(1, DateTime.Parse("2023-10-10"), "Math", TaskType.Comp, "Solve equations");
            //var homework2 = new Homework(2, DateTime.Parse("2023-10-11"), "History", TaskType.Oral, "Prepare a presentation");
            //var homework3 = new Homework(3, DateTime.Parse("2023-10-15"), "English", TaskType.Write, "Write an essay");
            //var homework4 = new Homework(4, DateTime.Parse("2023-10-17"), "Science", TaskType.Default, "Read a chapter");
            //var homework5 = new Homework(5, DateTime.Parse("2023-10-20"), "Art", TaskType.Oral, "Discuss an artwork");

            var homework1 = new Homework(1, DateTime.Parse("2023-10-10"), "Math", TaskType.Write, "Solve equations");
            var homework2 = new Homework(2, DateTime.Parse("2023-10-20"), "English", TaskType.Oral, "Write an essay");
            var hm3dbg = new Homework(45, DateTime.Parse("2023-11-21"), "Java", TaskType.Comp, "Debug mode");
            var hm3db = new Homework(45, DateTime.Parse("2023-11-22"), "Java", TaskType.Comp, "Debug mode2");
            var hm3 = new Homework(45, DateTime.Parse("2023-11-23"), "Java", TaskType.Comp, "Debug mode3");
            var d2 = new Homework("Debug mode WITH ONLY TASK TEXT");


            //debug separator "|" input   777 | 11.11.23 | Space Subj | Oral | Debug separator

            #endregion

            Interface ui = new();

            while (true)
            {
                ui.MenuModes();
                switch (ui.Input_range("Enter choise: ", 10, 0))
                {
                    case 0: ui.Create(); ui.PressEnter(); break;
                    case 1: ui.Output(); ui.PressEnter(); break;
                    case 2: ui.Find(); ui.PressEnter(); break;
                    case 3: ui.Delete(); ui.PressEnter(); break;
                    case 4: ui.OverloadedMethodsModes(); ui.PressEnter(); break;
                    case 5: ui.StaticMethodsModes(); ui.PressEnter(); break;
                    case 6: ui.Save_modes(); ui.PressEnter(); break;
                    case 7: ui.Read_modes(); ui.PressEnter(); break;
                    case 8: ui.Clear_Colection(); ui.PressEnter(); break;
                    case 9: Environment.Exit(0); ui.PressEnter(); break;
                }
            }
        }
    }
}