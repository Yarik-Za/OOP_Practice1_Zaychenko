using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab;

namespace UnitTests6
{
    [TestClass]
    public class UnitTests_Lab6
    {
        [TestMethod]
        public void Done_task_choose_logic_ValidSelection_ReturnsTrue()
        {
            // Arrange
            var selection = 2; // Правильное значение

            Storage.GetTasks().Clear();

            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };

            Interface ui = new();

            // Act
            var result = ui.Done_task_choose_logic(selection);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(db[1].Done);
        }

        [TestMethod]
        public void Done_task_choose_logic_InvalidSelection()
        {
            // Arrange
            int selection = 0; // Неправильное значение

            Storage.GetTasks().Clear();

            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };

            Interface ui = new();

            // Act & Assert

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ui.Done_task_choose_logic(selection));

            foreach (Homework el in db)
                Assert.IsFalse(el.Done);
        }

        [TestMethod]
        public void Done_task_choose_logic_OutOfBoundsSelection_ReturnsFalse()
        {
            // Arrange
            var selection = 100; // Значение вне допустимых пределов

            Storage.GetTasks().Clear();

            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };
            Interface ui = new();

            // Act & Assert
            foreach (Homework el in db)
                Assert.IsFalse(el.Done);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ui.Done_task_choose_logic(selection));
        }

        [TestMethod]
        public void Done_task_subj_logic_NoTasks_ReturnsZero()
        {
            // Arrange
            string selectedSubj = "DEBUG";

            Storage.GetTasks().Clear();
            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };

            Interface ui = new();

            // Act
            int result = ui.Done_task_subj_logic(selectedSubj, db);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Done_task_subj_logic_OneMatchingTask_ReturnsOne()
        {
            // Arrange
            string selectedSubj = "Math";
            Storage.GetTasks().Clear();
            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };

            Interface ui = new();

            // Act
            int result = ui.Done_task_subj_logic(selectedSubj, db);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Done_task_subj_logic_MultipleMatchingTasks_ReturnsCorrectCount()
        {
            // Arrange
            string selectedSubj = "Science";

            Storage.GetTasks().Clear();
            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(222, DateTime.Today, "Science", TaskType.Default, "Complete another science assignment"),
                new Homework(333, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the chemistry test"),
            };

            Interface ui = new();

            // Act
            int result = ui.Done_task_subj_logic(selectedSubj, db);

            // Assert
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Done_task_deadline_logic_NoTasks_ReturnsZero()
        {
            Storage.GetTasks().Clear();
            // Arrange
            List<Homework> db = new List<Homework>();
            DateTime selectedDate = DateTime.Today;

            Interface ui = new();

            // Act
            int result = ui.Done_task_deadline_logic(db, selectedDate);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Done_task_deadline_logic_OneMatchingTask_ReturnsOne()
        {
            Storage.GetTasks().Clear();
            // Arrange
            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today.AddDays(-2), "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };


            DateTime selectedDate = DateTime.Today.AddDays(-4);

            Interface ui = new();

            // Act
            int result = ui.Done_task_deadline_logic(db, selectedDate);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Done_task_deadline_logic_MultipleMatchingTasks_ReturnsCorrectCount()
        {
            Storage.GetTasks().Clear();
            // Arrange
            List<Homework> db = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today, "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today.AddDays(-4), "History", TaskType.Default, "Complete the history assignment")
            };
            DateTime selectedDate = DateTime.Today;

            Interface ui = new();

            // Act
            int result = ui.Done_task_deadline_logic(db, selectedDate);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void CreateList_NoTasks_ReturnsEmptyList()
        {
            Storage.GetTasks().Clear();
            // Arrange
            List<Homework> noTasks = new List<Homework>();
            Func<Homework, bool> condition = task => false;

            Interface ui = new();

            // Act
            List<Homework> result = ui.CreateList(condition);

            // Assert
            CollectionAssert.AreEqual(noTasks, result);
        }

        [TestMethod]
        public void CreateList_AllTasks_ReturnsList_with_one_object()
        {
            // Arrange
            Storage.GetTasks().Clear();
            List<Homework> allTasks = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today, "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today, "History", TaskType.Default, "Complete the history assignment"),
            };

            Func<Homework, bool> condition = task => task.TaskNumber == 222;

            Interface ui = new();
            List<Homework> expected = new List<Homework> { allTasks[1] };

            // Act
            List<Homework> result = ui.CreateList(condition);

            // Assert
            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void CreateList_AllTasks_ReturnsList_DATES()
        {
            // Arrange
            Storage.GetTasks().Clear();
            List<Homework> allTasks = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today, "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today, "History", TaskType.Default, "Complete the history assignment"),
            };

            Func<Homework, bool> condition = task => task.Deadline == DateTime.Today;

            Interface ui = new();
            List<Homework> expected = allTasks;

            // Act
            List<Homework> result = ui.CreateList(condition);

            // Assert
            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void CreateList_AllTasks_ReturnsList_Types()
        {
            // Arrange
            Storage.GetTasks().Clear();
            List<Homework> allTasks = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today, "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today, "History", TaskType.Default, "Complete the history assignment"),
            };

            Func<Homework, bool> condition = task => task.taskType == TaskType.Default;

            Interface ui = new();
            List<Homework> expected = allTasks;

            // Act
            List<Homework> result = ui.CreateList(condition);

            // Assert
            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void CreateList_one_Task_ReturnsList_SUBJ()
        {
            // Arrange
            Storage.GetTasks().Clear();
            List<Homework> allTasks = new List<Homework>
            {
                new Homework(111, DateTime.Today, "Math", TaskType.Default, "Complete the math assignment"),
                new Homework(222, DateTime.Today, "Science", TaskType.Default, "Complete the science assignment"),
                new Homework(333, DateTime.Today, "History", TaskType.Default, "Complete the history assignment"),
            };

            Func<Homework, bool> condition = task => task.Subject == "History";

            Interface ui = new();
            List<Homework> expected = new List<Homework> { allTasks[2] };

            // Act
            List<Homework> result = ui.CreateList(condition);

            // Assert
            CollectionAssert.AreEqual(expected, result);
            Assert.AreEqual(1, result.Count);
        }
    }
}

