using Lab;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests7
{
    [TestClass]
    public class UnitTestLab7
    {
        [TestMethod]
        public void SaveToTxt_SaveTasksToFile_FileContainsTasks()
        {
            Storage.GetTasks().Clear();

            // Arrange
            List<Homework> tasks = new List<Homework>
            {
                new Homework(1, DateTime.Today.AddDays(7), "Math", TaskType.Default, "Solve equations"),
                new Homework(2, DateTime.Today.AddDays(5), "History", TaskType.Default, "Write an essay")
            };

            string filePath = "Unit_test7-1_tasks.txt";

            // Act
            Interface.SaveToTxt(tasks, filePath);

            // Assert
            Assert.IsTrue(File.Exists(filePath));

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void SaveToTxt_SaveEmptyListToFile_FileIsEmpty()
        {
            Storage.GetTasks().Clear();

            // Arrange
            List<Homework> tasks = new List<Homework>();
            string filePath = "Unit_test7-2_empty_tasks.txt";

            // Act
            Interface.SaveToTxt(tasks, filePath);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            Assert.AreEqual(0, new FileInfo(filePath).Length);

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void SaveToTxt_SaveTasksToFile_FileContainsError()
        {
            Storage.GetTasks().Clear();

            // Arrange
            List<Homework> tasks = new List<Homework>
            {
                new Homework(1, DateTime.Today.AddDays(7), "Math", TaskType.Default, "Solve equations"),
                new Homework(2, DateTime.Today.AddDays(5), "History", TaskType.Default, "Write an essay")
            };

            tasks[0].taskText = null;
            string filePath = "Unit_test7-3_error_tasks.txt";

            // Act
            try
            {
                Interface.SaveToTxt(tasks, filePath);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, typeof(FormatException));
                return;
            }

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void SaveToJson_SaveTasksToJsonFile_FileContainsTasks()
        {
            Storage.GetTasks().Clear();
            // Arrange
            List<Homework> tasks = new List<Homework>
            {
                new Homework(1, DateTime.Today.AddDays(7), "Math", TaskType.Default, "Solve equations"),
                new Homework(2, DateTime.Today.AddDays(5), "History", TaskType.Default, "Write an essay")
            };

            string filePath = "Unit_test7-4_tasks.json";

            // Act
            Interface.SaveToJson(tasks, filePath);

            // Assert
            Assert.IsTrue(File.Exists(filePath));

            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void SaveToJson_SaveEmptyListToJsonFile_FileIsEmpty()
        {
            Storage.GetTasks().Clear();

            // Arrange
            List<Homework> tasks = new List<Homework>();
            string filePath = "Unit_test7-5_empty_tasks.json";

            // Act
            Interface.SaveToJson(tasks, filePath);

            // Assert
            Assert.IsTrue(File.Exists(filePath));

            // ��������, �� ���� JSON ������ ������� ����� []
            string jsonContents;
            using (StreamReader reader = new StreamReader(filePath))
            {
                jsonContents = reader.ReadToEnd().Trim();
            }

            Assert.AreEqual("", jsonContents);
            // Clean up
            File.Delete(filePath);
        }

        [TestMethod]
        public void LoadFromTxt_ValidTxtFile_LoadsTasksSuccessfully()
        {
            Storage.GetTasks().Clear();

            // Arrange
            List<Homework> expectedTasks = new List<Homework>
            {
                new Homework(1, DateTime.Today.AddDays(7), "Math", TaskType.Default, "Solve equations"),
                new Homework(2, DateTime.Today.AddDays(5), "History", TaskType.Default, "Write an essay")
            };

            // Save tasks to a txt file
            Interface.SaveToTxt(expectedTasks, "Unit_test7-6_tasks.txt");

            try
            {
                // Act
                List<Homework> loadedTasks = Interface.LoadFromTxt("Unit_test7-6_tasks.txt");

                // Assert
                Assert.AreEqual(expectedTasks.Count, loadedTasks.Count, "Counts of tasks do not match.");

                for (int i = 0; i < expectedTasks.Count; i++)
                {
                    Assert.AreEqual(
                        expectedTasks[i].ToString().Replace(Environment.NewLine, string.Empty),
                        loadedTasks[i].ToString().Replace(Environment.NewLine, string.Empty),
                        $"Task at index {i} does not match."
                    );
                }
            }
            finally
            {
                // Clean up
                File.Delete("Unit_test7-6_tasks.txt");
            }
        }

        [TestMethod]
        public void LoadFromTxt_ErrorsTxtFile_LoadsTasksSuccessfully()
        {
            Storage.GetTasks().Clear();

            // Arrange
            List<Homework> expectedTasks = new List<Homework>
            {
                new Homework(1, DateTime.Today.AddDays(7), "Math", TaskType.Default, "Solve equations"),
                new Homework(2, DateTime.Today.AddDays(5), "History", TaskType.Default, "Write an essay")
            };

            Interface.SaveToTxt(expectedTasks, "Unit_test7-6_tasks.txt");

            string temp = File.ReadAllText("Unit_test7-6_tasks.txt");

            using (StreamWriter writer = new StreamWriter("Unit_test7-6_tasks.txt"))
            {
                writer.WriteLine("UnitTest for lab 7");            // Save tasks to a txt file
                writer.WriteLine(temp);
            }

            try
            {
                // Act
                List<Homework> loadedTasks = Interface.LoadFromTxt("Unit_test7-6_tasks.txt");

                // Assert
                Assert.AreEqual(expectedTasks.Count, loadedTasks.Count, "Counts of tasks do not match.");

                for (int i = 0; i < expectedTasks.Count; i++)
                {
                    Assert.AreEqual(
                        expectedTasks[i].ToString().Replace(Environment.NewLine, string.Empty),
                        loadedTasks[i].ToString().Replace(Environment.NewLine, string.Empty),
                        $"Task at index {i} does not match."
                    );
                }
            }
            finally
            {
                // Clean up
                File.Delete("Unit_test7-6_tasks.txt");
            }
        }

        [TestMethod]
        public void LoadFromJson_ValidJsonFile_LoadsTasksSuccessfully()
        {
            Storage.GetTasks().Clear();

            // Arrange
            List<Homework> tasks = new List<Homework>()
            {
                new Homework(1, DateTime.Today.AddDays(7), "Math", TaskType.Default, "Solve equations"),
                new Homework(2, DateTime.Today.AddDays(5), "History", TaskType.Default, "Write an essay")
            };

            // Save tasks to a json file
            Interface.SaveToJson(tasks, "Unit_test7-7_tasks.json");

            try
            {
                // Act
                List<Homework> loadedTasks = Interface.LoadFromJson("Unit_test7-7_tasks.json");

                // Assert
                Assert.AreEqual(tasks.Count, loadedTasks.Count, "Counts of tasks do not match.");

                for (int i = 0; i < tasks.Count; i++)
                {
                    string expected = tasks[i].ToString().Replace(Environment.NewLine, string.Empty);
                    string actual = loadedTasks[i].ToString().Replace(Environment.NewLine, string.Empty);

                    if (expected != actual)
                        Console.WriteLine($"Mismatch at index {i}:\nExpected: {expected}\nActual: {actual}");

                    Assert.AreEqual(expected, actual, $"Task at index {i} does not match.");
                }
            }
            finally
            {
                // Clean up
                File.Delete("Unit_test7-7_tasks.json");
            }
        }
    }
}
