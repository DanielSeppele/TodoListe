using System.IO;
using System.Reflection;

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
                
                using StreamWriter writer = new StreamWriter(path);
                int anzahlParameters = typeof(TodoItem).GetFields().Length;
                string[] varNames = new string[anzahlParameters];
                int x = 0;
                foreach (FieldInfo field in typeof(TodoItem).GetFields()) {
                    varNames[x] = field.Name;    
                    x++;
                }

                for(int i = 0; i < todos.Count; i++)
                {
                    writer.WriteLine("{");
                    TodoItem todoItem = todos.ElementAt(i);
                    for (int y = 0; y < anzahlParameters; y++)
                    {
                        var feld = todoItem.GetType().GetField(varNames[y]);
                        string varName = '"' + varNames[y] + '"';
                        writer.WriteLine(varName + ":" + feld.GetValue(todoItem));
                    }
                    writer.WriteLine("}");
                }
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
                using StreamReader reader = new StreamReader(path);
                string line = reader.ReadLine();
                todos.Clear();
                while (line != null)
                {
                    TodoItem item = new TodoItem(null, -1, Prios.NotImportant);
                    
                    while (line != "}") {
                        if (line == "{")
                        {
                            continue;
                        }
                        string[] splitLine = reader.ToString().Split(':');
                        Console.WriteLine(reader);


                        Console.WriteLine(splitLine[0] + ":" + splitLine[1]);
                        line = reader.ReadLine();
                    }
                    line = reader.ReadLine();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
