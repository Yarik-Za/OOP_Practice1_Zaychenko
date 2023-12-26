# Лабораторна робота № 7
з дисципліни Об’єктно орієнтоване програмування  
на тему: Збереження та завантаження(зчитування) обʼєктів.  
Serialization/ Deserialization

### ПОСТАНОВКА ЗАДАЧІ

На основі отриманого на лекції 7 теоретичного матеріалу скорегувати програму для лабораторної роботи № 6 наступним чином:

1.	В основній програмі додати методи для збереження (серіалізації) колекції List<T> об’єктів предметної області у файли з форматом *.csv (*.txt) та *.json, а також методи для читання (десеріалізації) колекції з відповідних файлів.

2.	Модифікувати меню таким чином (з’являються нові пункти!):

1 – додати об’єкт  
2 – вивести на екран об’єкти  
3 – знайти об’єкт  
4 – видалити об’єкт  
5 – демонстрація поведінки об’єктів  
6 – демонстрація роботи static методів  
7 – зберегти колекцію об’єктів у файлі  
8 – зчитати колекцію об’єктів з файлу  
9 – очистити колекцію об’єктів  
0 – вийти з програми  

У пункті меню «7 – зберегти колекцію об’єктів у файлі» необхідно реалізувати підменю:  
&nbsp;&nbsp;&nbsp;1 – зберегти у файл \*.csv (\*.txt)  
&nbsp;&nbsp;&nbsp;2 – зберегти у файл *.json

У пункті меню «8 – зчитати колекцію об’єктів з файлу» необхідно реалізувати підменю:

&nbsp;&nbsp;&nbsp;1 – зчитати з файлу \*.csv (\*.txt)  
&nbsp;&nbsp;&nbsp;2 – зчитати з файлу *.json

Якщо на момент зчитування з файлу у колекції List<T> є наявні об’єкти, то десеріалізовані об’єкти мають додаватися до списку.
До колекції List<T> об’єктів додаємо тільки коректно десеріалізовані об’єкти, інші – пропускаємо.

3.	Для нових/перероблених методів додати/скорегувати unit-тести.

4.	Запустити виконання всіх наявних unit-тестів (як нових, так і з попередньої лабораторної роботи) і досягти повного їх проходження.
5.	Детально протестувати програму. Мають бути протестовані 7-9 пункти меню. При тестуванні десеріалізації перевіряємо процес перетворення не тільки на коректних файлах *.csv (*.txt) і *.json, а також не забуваємо перевірити і файли з пропущеними даними і невірними типами даних.

6.	Оформити звіт:

-	Титульний аркуш
-	Завдання 
-	Сlass diagram (для основного проєкту і тест-проєкту)
-	Реалізація класу 
-	Реалізація тест-класів
-	Код програми файлу Program.cs 
-	Результати запуску всіх розроблених unit-тестів
-	Результати детального тестування функціональності програми (навести скріншоти виконання тестування програми або скопіювати і вставити у звіт вивід програми на екран)

## ХІД РОБОТИ

__Опис програми:__
Мова програмування: С#, операційна система Windows 11 Prо, Версія 23H2, Збірка ОС 22621.1325, процесор: Apple Silicon M1 Pro 3.20 GHz (ядер: 4), компілятор: Microsoft Visual Studio Community 2022 (64-розрядна версія ARM).

**Class Diagram основної програми**
![1](readme_source/dgrm/ClassDiagram.png)

**Class Diagram класу для Unit-тестування**
![2](readme_source/dgrm/ClassDiagram1.png)

**Реалізація класу з методами серіалізації та дереалізації**
```
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
            int suces_count = 0;
            int line_num = 1;
            List<Homework> tasks = new List<Homework>();

            if (!File.Exists(filePath))
                throw new Exception($"File doesn't exist at path {filePath}");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && line != "")
                    if (Homework.TryParse(line, out Homework task, out Exception errorMessage))
                    {
                        tasks.Add(task); suces_count++;
                    }
                    else Console.WriteLine($"Error parsing line {line_num++}: {errorMessage.Message}");
            }
            Console.WriteLine("Added objects from txt file: " + suces_count);
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
                Console.WriteLine("\nContents of JSON file:\n");
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

```

**Реалізація підменю для збереження/зчитування**
```
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

```

**Реалізація тест-класу для нових методів в програмі**

[Переглянути тести](UnitTestsLab7/UnitTestLab7.cs)

### Тестування функціональності програми

Меню вибору запису списку в текстовий файл
![4](readme_source/tests/4.png)

Результат запису в текстовий файл
![5](readme_source/tests/5.png)

Зчитування об’єктів файлу в програму та їх виведення
![6](readme_source/tests/6.png)

Виведення завантажених елементів з текстового файлу
![7](readme_source/tests/7.png)

Результат спроби запису неправильного файлу з вмістом:
```
UnitTest for lab 7
1|02.01.24|Math|Default|Solve equations
2|31.12.23|History|Default|Write an essay
```
подано нижче на рисунку

Зчитування файлу з некоректними даними
![8](readme_source/tests/8.png)

Очищення списку об’єктів в програмі та виведення для перевірки
![9](readme_source/tests/9.png)

Запис списку в файл формату json
![10](readme_source/tests/10.png)

Додавання обʼєктів з файлу json
![12](readme_source/tests/11.png)

Виведення елементів на екран, зчитаних з файлу json
![13](readme_source/tests/13.png)

## ВИСНОВОК

&nbsp; &nbsp;&nbsp;&nbsp;В основній програмі додано методи для збереження (серіалізації) колекції List<T> об’єктів предметної області у файли з форматом \*.csv (\*.txt) та *.json. Додано також методи для читання (десеріалізації) колекції з відповідних файлів.  
&nbsp; &nbsp;&nbsp;&nbsp;Меню програми було розширено новими функціями: збереженням та читанням колекції об’єктів з файлів у форматах \*.csv (\*.txt) та *.json, а також можливістю очищення колекції.  
&nbsp; &nbsp;&nbsp;&nbsp;Підменю для пунктів "Зберегти у файл \*.csv (\*.txt)" та "Зберегти у файл *.json" реалізовано відповідно.  
&nbsp; &nbsp;&nbsp;&nbsp;Підменю для пунктів "Зчитати з файлу \*.csv (\*.txt)" та "Зчитати з файлу *.json" враховує наявність об’єктів у колекції List<T> на момент зчитування. Десеріалізовані об’єкти додаються до колекції лише у випадку коректного десеріалізації.  
&nbsp; &nbsp;&nbsp;&nbsp;Для нових та перероблених методів були додані та кореговані unit-тести, і вони успішно пройшли всі перевірки.  
&nbsp; &nbsp;&nbsp;&nbsp;Програма була детально протестована, включаючи збереження та читання з файлів у різних форматах, а також обробку різних сценаріїв вводу даних. Всі тести та етапи виконані успішно, забезпечуючи стабільну та надійну роботу програми.