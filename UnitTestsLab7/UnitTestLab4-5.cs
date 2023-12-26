using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Lab;

namespace UnitTests4_5
{
    [TestClass]
    public class UnitTests_Lab4_5
    {
        [TestMethod]
        public void HomeworkConstructor_ValidParameters_SetsProperties()
        {
            // Arrange
            int taskNumber = 1;
            DateTime deadline = DateTime.Today;
            string subject = "Math";
            TaskType taskType = TaskType.Default;
            string taskText = "Complete the math assignment";

            // Act
            Homework homework = new Homework(taskNumber, deadline, subject, taskType, taskText);

            // Assert
            Assert.AreEqual(taskNumber, homework.TaskNumber);
            Assert.AreEqual(deadline, homework.Deadline);
            Assert.AreEqual(subject, homework.Subject);
            Assert.AreEqual(taskType, homework.taskType);
            Assert.AreEqual(taskText, homework.taskText);
        }

        [TestMethod]
        public void HomeworkConstructor_INValidParameters_SetsProperties()
        {
            // Arrange
            int taskNumber = -5;
            DateTime deadline = DateTime.ParseExact("11.08.23", Homework.DateFormat, CultureInfo.InvariantCulture);
            string subjectn = "";
            TaskType taskType = TaskType.Default;
            string subject = "20th century";

            Homework homeworkn = new Homework(taskNumber, deadline, subjectn, taskType, "empty txt");
            Homework homework = new Homework(taskNumber, deadline, subject, taskType, "empty txt");

            // Act + Assert 
            Assert.ThrowsException<Exception>(() => homeworkn.TaskNumber = taskNumber);
            Assert.ThrowsException<Exception>(() => homeworkn.Deadline = deadline);
            Assert.ThrowsException<Exception>(() => homeworkn.Subject = subjectn);

            Assert.ThrowsException<Exception>(() => homework.TaskNumber = taskNumber);
            Assert.ThrowsException<Exception>(() => homework.Deadline = deadline);
            Assert.ThrowsException<Exception>(() => homework.Subject = subject);
        }

        [TestMethod]
        public void GenerateDefNum_NoTasks_Returns0()
        {  // Arrange
            Storage.GetTasks().Clear();
            int expected = 0;
            // Act
            int d = Storage.GetTasks().Count;
            int actual = Homework.GenerateDefaultTaskNumber();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HomeworkIsExpired_CheckIfDeadlineIsExpired()
        {
            // Arrange
            DateTime pastDeadline = DateTime.Today.AddDays(-1);
            DateTime today = DateTime.Today;
            DateTime futureDeadline = DateTime.Today.AddDays(1);


            Homework homework = new Homework();
            // Act
            homework.Deadline = pastDeadline;
            // Assert
            Assert.IsTrue(homework.IsExpired, "Deadline is in the past, so IsExpired should be true.");

            // Act
            homework.Deadline = today;
            // Assert
            Assert.IsFalse(homework.IsExpired, "Deadline is in the past, so IsExpired should be false.");

            // Act
            homework.Deadline = futureDeadline;
            // Assert
            Assert.IsFalse(homework.IsExpired, "Deadline is in the future, so IsExpired should be false.");
        }

        [TestMethod]
        public void DoTask_WithTaskNumber_SetsDoneToTrue()
        {
            // Arrange
            Homework homework = new Homework(1, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment");

            // Act
            homework.DoTask(1);

            // Assert
            Assert.IsTrue(homework.Done);
        }

        [TestMethod]
        public void DoTask_WithDifferentTaskNumber_KeepsDoneFalse()
        {
            // Arrange
            Homework homework = new Homework(1, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment");

            // Act
            homework.DoTask(2);

            // Assert
            Assert.IsFalse(homework.Done);
        }

        [TestMethod]
        public void DoTask_WithSubject_SetsDoneToTrue()
        {
            // Arrange
            Homework homework = new Homework(1, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment");

            // Act
            bool actual = homework.DoTask("Math");

            // Assert
            Assert.AreEqual(actual, true);
            Assert.IsTrue(homework.Done);
        }

        [TestMethod]
        public void DoTask_WithDifferentSubject_ReturnsFalse()
        {
            // Arrange
            Homework homework = new Homework(1, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment");

            // Act
            bool actual = homework.DoTask("English");

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(homework.Done);
        }

        [TestMethod]
        public void DoTask_WithDeadline_SetsDoneToTrue()
        {
            // Arrange
            DateTime tommorow = DateTime.Today.AddDays(1);
            Homework homework = new Homework(1, tommorow, "Math", TaskType.Default, "Complete the math assignment");

            // Act
            bool result = homework.DoTask(tommorow);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(homework.Done);
        }

        [TestMethod]
        public void DoTask_WithDifferentDeadline_ReturnsFalse()
        {
            // Arrange
            DateTime deadline = DateTime.Today.AddDays(1);
            Homework homework = new Homework(1, deadline, "Math", TaskType.Default, "Complete the math assignment");

            // Act
            bool result = homework.DoTask(DateTime.Today);

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(homework.Done);
        }

        [TestMethod]
        public void Parse_ValidInput_ParsesHomework()
        {
            // Arrange
            string input = "1|12.11.23|Math|Write|Complete the math assignment";

            // Act
            Homework homework = Homework.Parse(input);

            // Assert
            Assert.AreEqual(1, homework.TaskNumber);
            Assert.AreEqual(new DateTime(2023, 11, 12), homework.Deadline);
            Assert.AreEqual("Math", homework.Subject);
            Assert.AreEqual(TaskType.Write, homework.taskType);
            Assert.AreEqual("Complete the math assignment", homework.taskText);
        }

        [TestMethod]
        public void TryParse_ValidInput_ParsesHomework()
        {
            // Arrange
            string expected = "1|12.11.23|Math|Default|Complete the math assignment";
            Homework actual;
            Exception error;

            // Act
            bool result = Homework.TryParse(expected, out actual, out error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.TaskNumber);
            Assert.AreEqual(new DateTime(2023, 11, 12), actual.Deadline);
            Assert.AreEqual("Math", actual.Subject);
            Assert.AreEqual(TaskType.Default, actual.taskType);
            Assert.AreEqual("Complete the math assignment", actual.taskText);
        }

        [TestMethod]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            Homework homework = new Homework(1, new DateTime(2023, 11, 12), "Math", TaskType.Default, "Complete the math assignment");

            // Act
            string result = homework.ToString();

            // Assert
            string expected = "Task Number: 1 | Deadline: 12.11.23 | Subject: Math | Task Type: Default | Task Text: Complete the math assignment";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetStatistics_CalculatesStatistics_NoINcomplete()
        {
            Storage.GetTasks().Clear();
            int d = Storage.GetTasks().Count;
            int b = Homework.ObjectCount;
            // Arrange
            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };

            foreach (var t in db)
            {
                t.DoTask(t.TaskNumber);
            }

            // Act
            Homework.GetStatistics(Storage.GetTasks());

            // Assert
            Assert.AreEqual(3, Homework.TotalTasks);
            Assert.AreEqual(3, Homework.CompletedTasks);
            Assert.AreEqual(0, Homework.IncompleteTasks);
            Assert.AreEqual(TimeSpan.FromDays(2), Homework.AverageCompletionTime);
        }

        [TestMethod]
        public void GetStatistics_CalculatesStatistics_ONEcompleted()
        {
            Storage.GetTasks().Clear();
            // Arrange
            List<Homework> db = new List<Homework>
            {
                //new Homework(111, new DateTime(2023, 11, 5), "Math", TaskType.Default, "Complete the math assignment"),
                //new Homework(222, new DateTime(2023, 11, 3), "Science", TaskType.Default, "Complete the science assignment"),
                //new Homework(333, new DateTime(2023, 11, 1), "History", TaskType.Default, "Complete the history assignment")

                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")

            };

            db[2].DoTask(db[2].TaskNumber);

            // Act
            Homework.GetStatistics(Storage.GetTasks());

            // Assert
            Assert.AreEqual(3, Homework.TotalTasks);
            Assert.AreEqual(1, Homework.CompletedTasks);
            Assert.AreEqual(2, Homework.IncompleteTasks);
            Assert.AreEqual(TimeSpan.FromDays(4), Homework.AverageCompletionTime);
        }

    }
}

