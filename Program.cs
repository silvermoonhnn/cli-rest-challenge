using System.Text;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;


namespace Rest
{
    [HelpOption("--hlp")]
    [Subcommand(
        typeof(GetList),
        typeof(PostTodo),
        typeof(Update),
        typeof(Delete),
        typeof(Clear),
        typeof(Done),
        typeof(UnDone)
    )]

    class Program
    {
        public static Task<int> Main(string[] args)
        {
            return CommandLineApplication.ExecuteAsync<Program>(args);
        }

        [Command(Description = "Get list", Name = "getlist")]
        class GetList
        {
            public async Task OnExecuteAsync()
            {
                var client = new HttpClient();
                var result = await client.GetStringAsync("http://localhost:3000/todo");
                var js = JsonConvert.DeserializeObject<List<Todo>>(result);

                foreach (var i in js){
                    string stat = null;
                    if (i.status)
                    {
                        stat = "Done";
                    }
                    Console.WriteLine($"{i.id}. {i.activity} {i.keterangan} - {stat}");
                }
            }
        }

        [Command(Description = "Add new todo", Name = "add")]
        class PostTodo
        {
            [Argument(0)]
            public string dt1 { get; set; }
            [Argument(1)]
            public string dt2 { get; set; }
            public async Task OnExecuteAsync()
            {
                
                var req = new Activity(){activity = dt1, keterangan = dt2, status = false};
                var client = new HttpClient();
                var todo = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                var result = await client.PostAsync("http://localhost:3000/todo", todo);

                // Console.WriteLine(result);
            }
        }

        [Command(Description = "Update a todo item", Name = "update")]
        class Update
        {
            [Argument(0)]
            public string idTodo { get; set; }
            [Argument(1)]
            public string dt1 { get; set; } 
            [Argument(2)]
            public string dt2 { get; set; } 

            public async Task OnExecuteAsync()
            {
                var req = new Todo(){id = Convert.ToInt32(idTodo), activity = dt1, keterangan = dt2, status = false};
                var client = new HttpClient();
                var todo = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                var result = await client.PatchAsync($"http://localhost:3000/todo/{idTodo}", todo);
            }
        }

        [Command(Description = "Delete a todo item", Name = "del")]
        class Delete
        {
            [Argument(0)]
            public string idTodo { get; set; }

            public async Task OnExecuteAsync()
            {
                var req = new {id = Convert.ToInt32(idTodo)};
                var client = new HttpClient();
                var todo = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                var result = await client.DeleteAsync($"http://localhost:3000/todo/{idTodo}");
            }
        }

        [Command(Description = "Clear all todo list", Name = "clear")]
        class Clear
        {
            public async Task OnExecuteAsync()
            {
                var quest = Prompt.GetYesNo("Are you sure want to delete?", false, ConsoleColor.Blue);
                var client = new HttpClient();

                if (quest)
                {
                    var list = await client.GetStringAsync("http://localhost:3000/todo");
                    var js = JsonConvert.DeserializeObject<List<Todo>>(list);
                    var id = new List<int>();

                    foreach(var x in js)
                    {
                        id.Add(x.id);
                    }
                    foreach (var y in id)
                    {
                         var result = await client.DeleteAsync($"http://localhost:3000/todo/{y}");
                    }
                } 
            }
        }

        [Command(Description="Set a todo item to completed", Name = "done")]
        class Done
        {
            [Argument(0)]
            public string idTodo { get; set; }

            public async Task OnExecuteAsync()
            {
                var req = new {id = Convert.ToInt32(idTodo), status = true};
                var client = new HttpClient();
                var todo = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                var result = await client.PatchAsync($"http://localhost:3000/todo/{idTodo}", todo);
            }
        }

        [Command(Description="Set a todo item to uncompleted", Name = "undone")]
        class UnDone
        {
            [Argument(0)]
            public string idTodo { get; set; }

            public async Task OnExecuteAsync()
            {
                var req = new {id = Convert.ToInt32(idTodo), status = false};
                var client = new HttpClient();
                var todo = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                var result = await client.PatchAsync($"http://localhost:3000/todo/{idTodo}", todo);
            }
        }

    }
}
