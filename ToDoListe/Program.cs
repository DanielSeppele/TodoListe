using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json;
namespace ToDoListe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ToDoListe todoListe = new ToDoListe();
            todoListe.AufgabeHinzufügen("Aufgabe1", 2, Prios.Important);
            todoListe.AufgabeHinzufügen("Aufgabe2", 3, Prios.NotImportant);
            string projektOrdner = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));

            string dateiPfad = Path.Combine(projektOrdner, "todos.json");

            todoListe.aufgabeSpeichern(dateiPfad);
            todoListe.AufgabeLaden(dateiPfad);
        }
    }


    public enum Prios
    {
        Important,
        Moderate,
        NotImportant
    }

    public class TodoItem
    {
        public String name;
        public int dauer;
        public Prios prio;
        public TodoItem(String name, int dauer, Prios prio)
        {
            this.name = name;
            this.dauer = dauer;
            this.prio = prio;
        }

        public string toString()
        {
            return $"Name:{name}\nDauer:{dauer}\nPrio:{prio}";
        }

        public string[] toStringInArray()
        {
            string[] todoItemAll = new string[3];
            todoItemAll[0] = $"Name:{this.name}";
            todoItemAll[1] = $"Dauer:{this.dauer}";
            todoItemAll[2] = $"Prio:{this.prio}";
            return todoItemAll;
        }
    }

    public class ToDoListe
    {

        List<TodoItem> todos = new List<TodoItem>();
        public void AufgabeHinzufügen(String name, int dauer, Prios prio)
        {
            try
            {
                TodoItem item = new TodoItem(name, dauer, prio);
                todos.Add(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public string AufgabeLöschen(string name)
        {
            int x = 0;
            while (x < todos.Count)
            {
                if (todos.ElementAt(x).name == name)
                {
                    todos.RemoveAt(x);
                    return "Removal erfolgreich";
                }
                x++;
            }

            return $"Das Todo mit dem Namen {name} gibt es nicht und konnte auch nicht removed werden.";

        }

        public void AufgabeAnzeigen(string name)
        {
            int x = 0;
            while (x < todos.Count) { 
                if(todos.ElementAt(x).name == name)
                {
                    Console.WriteLine(todos.ElementAt(x).toString());
                    return;
                }
                x++;
            }

        }


    public void aufgabeSpeichern(string path)
    {
        try
        {
            string json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void AufgabeLaden(string path)
    {
        if (!path.EndsWith(".json"))
        {
            Console.WriteLine("Wrong File Type.");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            todos = JsonSerializer.Deserialize<List<TodoItem>>(json);
            AufgabenAnzeigen();
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }


    private void AufgabenAnzeigen()
        {
            for (int i = 0; i < todos.Count; i++)
            {
                Console.WriteLine(todos.ElementAt(i).ToString());
            }
        }
    
    }
}
