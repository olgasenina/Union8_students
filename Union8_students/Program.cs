using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Union8_students
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                На вход программа получает бинарный файл, предположительно, это база данных студентов.

                Свойства сущности Student:

                Имя — Name (string);
                Группа — Group (string);
                Дата рождения — DateOfBirth (DateTime).

                Ваша программа должна:
                    Создать на рабочем столе директорию Students.
                    Внутри раскидать всех студентов из файла по группам (каждая группа-отдельный текстовый файл), в файле группы студенты перечислены построчно в формате "Имя, дата рождения".
             */

            string fPath = "C:/_МОЯ/Студенты/Students.dat";

            //SerializeList(fPath);
            DeserializeList(fPath);


            Console.WriteLine("Для завершения работы программы нажмите любую клавишу");
            Console.ReadKey();
        }
        static void SerializeList(string fPath)
        {
            var students = new Student[]
            {
                new Student("Иван", "П-89", new DateTime(2002, 1, 23)),
                new Student("Петр", "П-89", new DateTime(2002, 7, 4)),
                new Student("Игорь", "П-90", new DateTime(2002, 5, 20)),
                new Student("Маша", "П-90", new DateTime(2002, 3, 7)),
                new Student("Лена", "П-91", new DateTime(2002, 8, 30)),
                new Student("Виктор", "П-91", new DateTime(2002, 7, 8))
            };

            using (FileStream fs = new FileStream(fPath, FileMode.OpenOrCreate))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, students);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Не удалось выполнить сериализацию. Причина: " + e.Message);
                }
            }
        }
        static void DeserializeList(string fPath)
        {
            var students = new Student[] { };

            using (FileStream fs = new FileStream(fPath, FileMode.Open))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    students = (Student[])formatter.Deserialize(fs);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Не удалось выполнить десериализацию. Причина: " + e.Message);
                }
            }

            // Создать на рабочем столе директорию Students.

            string rPath = @"/Users/Ольга/Desktop/Students";

            DirectoryInfo dirInfo = new DirectoryInfo(rPath);

            try
            {
                if (dirInfo.Exists)
                {
                    dirInfo.Delete(true); // Удаление со всем содержимым
                }

                dirInfo.Create(); // Создать папку
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании каталога Students: {ex.Message}");
            }

            foreach (Student aStudent in students)
            {
                try
                {
                    var fileInfo = new FileInfo(rPath + "/" + aStudent.Group + ".txt");

                    if (fileInfo.Exists)
                    {
                        using (StreamWriter sw = fileInfo.AppendText()) // Добавляем запись
                        {
                            sw.WriteLine($"{aStudent.Name}, {aStudent.DateOfBirth}");
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = fileInfo.CreateText()) // Создаем файл
                        {
                            sw.WriteLine($"{aStudent.Name}, {aStudent.DateOfBirth}");
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка при записи данных студента: {ex.Message}");
                }
                
            } 
                
        }
    }
}
