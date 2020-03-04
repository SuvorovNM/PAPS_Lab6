using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAPS_Lab6
{
    interface IObserver
    {
        void Update(Object ob);
    }

    interface IObservable
    {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers();
    }

    class Journal : IObservable
    {
        Dictionary<string, double> marks;

        List<IObserver> observers;

        public Journal()
        {
            marks = new Dictionary<string, double>();
            observers = new List<IObserver>();
        }
        public void NotifyObservers()
        {
            foreach (IObserver o in observers)
            {
                o.Update(marks);
            }
        }

        public void RegisterObserver(IObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            try
            {
                observers.Remove(o);
            }
            catch { }
        }

        public void AddStudent(string _name, List<double> _marks)
        {
            marks.Add(_name, (double)_marks.Average());
            NotifyObservers();
        }
        public void RemoveStudent(string _name)
        {
            if (marks.ContainsKey(_name))
            {
                marks.Remove(_name);
            }
            NotifyObservers();
        }
        public void UpdateStudent(string _name, List<double> _marks)
        {
            if (marks.ContainsKey(_name))
            {
                marks[_name] = (double)_marks.Average();
            }
            NotifyObservers();
        }
    }

    class Teacher : IObserver
    {
        public string name;
        IObservable markbook;
        public Teacher(string _name, IObservable obs)
        {
            name = _name;
            markbook = obs;
            markbook.RegisterObserver(this);
        }
        public void Update(object ob)
        {
            Console.WriteLine("*** Информация преподавателя ***");
            var markInfo = (Dictionary<string, double>)ob;

            double avgMark = markInfo.Values.Average();
            Console.WriteLine("Средняя оценка: "+ avgMark.ToString());

            var atRisk = new Dictionary<string, double>();

            Console.WriteLine("Студенты в зоне риска: ");
            foreach(string key in markInfo.Keys)
            {
                if (markInfo[key] < 3)
                {
                    atRisk.Add(key, markInfo[key]);
                    Console.WriteLine(key+ " : "+ markInfo[key]);
                }
            }
            Console.WriteLine("***  ***");
        }
    }

    class Student : IObserver
    {
        public string name;
        IObservable markbook;
        public Student(string _name, IObservable obs)
        {
            name = _name;
            markbook = obs;
            markbook.RegisterObserver(this);
        }
        public void Update(object ob)
        {
            Console.WriteLine("*** Информация студента " + name +" ***");
            var markInfo = (Dictionary<string, double>)ob;

            Console.WriteLine("Обновлена оценка : "+ markInfo[name]);
            Console.WriteLine("***  ***");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            Teacher teacher = new Teacher("Зеленский В.А.", journal);
            Student student = new Student("Порошенко П.А.", journal);
            journal.AddStudent("Порошенко П.А.", new List<double>() { 1, 4, 5, 5, 3, 4 });
            Console.ReadLine();
            journal.AddStudent("Янукович В.Ф.", new List<double>() { 2, 3, 1, 2, 3, 5 });
            Console.ReadLine();
            journal.UpdateStudent("Порошенко П.А.", new List<double>() { 1, 2, 3, 2, 3, 1 });
            Console.ReadLine();
            Console.WriteLine("End!");
            Console.ReadLine();
        }
    }
}
