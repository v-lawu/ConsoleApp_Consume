using System;
using System.Net.Http;

namespace ConsoleApp_Consume
{
    class Program
    {
        static void Main(string[] args)
        {
            PostStudent();
            Console.WriteLine();
            GetStudents();       
        }

        public static void GetStudents()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44341/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Students");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<Student[]>();
                    readTask.Wait();

                    var students = readTask.Result;

                    foreach (var student in students)
                    {
                        Console.WriteLine(student.Name + "  " + student.Age);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Test");
            Console.ReadLine();
        }

        public static void PostStudent()
        {
            var student = new Student() { Name = "Mei", Age = 14 };
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44341/api/");
            var postTask = client.PostAsJsonAsync<Student>("Students", student);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {

                var readTask = result.Content.ReadAsAsync<Student>();
                readTask.Wait();

                var insertedStudent = readTask.Result;

                Console.WriteLine("Student {0} inserted with id: {1}", insertedStudent.Name, insertedStudent.Id);
            }
            else
            {
                Console.WriteLine(result.StatusCode);
            }
        }
    }
}
