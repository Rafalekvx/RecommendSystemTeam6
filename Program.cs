using ConsoleTables;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace RecommenderTeam6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string currentPath = Environment.CurrentDirectory;
            string fullpath = Path.Combine(currentPath, "Database.json");
            List<ItemModel> WriteList = new List<ItemModel>();

            if (!(File.Exists(fullpath)) || string.IsNullOrEmpty(File.ReadAllText(fullpath)))
            {
                    WriteList.Add(new ItemModel() { Id = 1, Name = "Perfum", Description = "Dutch perfumes", MainCategory = "Perfums", Categories = { "Aroma" }, Price = 200 });
                    WriteList.Add(new ItemModel() { Id = 2, Name = "Bronzer", Description = "Bronzing Powder with cocoa seeds extract", MainCategory = "Makeup", Categories = { "Facecare","Visual" }, Price = 50 });
                    WriteList.Add(new ItemModel() { Id = 3, Name = "Foundation", Description = "Foundation makeup", MainCategory = "Makeup", Categories = { "Facecare", "Visual", "Powdered" }, Price = 70 });
                    WriteList.Add(new ItemModel() { Id = 4, Name = "Lipstick", Description = "lipstick in cherry red color", MainCategory = "Makeup", Categories = { "Facecare", "Visual", "Flavor"}, Price = 70 });
                    WriteList.Add(new ItemModel() { Id = 5, Name = "Shampoon", Description = "Coconut cream shampoon ", MainCategory = "Skincare", Categories = { "Aroma", "Flavor" }, Price = 30 });
                    WriteList.Add(new ItemModel() { Id = 6, Name = "Hand cream", Description = "Hand cream against viruses", MainCategory = "Skincare", Categories = { "Resistence", "Aroma" }, Price = 500 });

                    var serialized = JsonConvert.SerializeObject(WriteList);

                    File.WriteAllText(fullpath, serialized);
            }

            List<ItemModel> ReadedItems = new List<ItemModel>();

            using (var stream = new StreamReader(fullpath))
            {
                string json = stream.ReadToEnd();
                ReadedItems = JsonConvert.DeserializeObject<List<ItemModel>>(json);
            }


            var table = new ConsoleTable("Id", "Name", "MainCategory");

            foreach (var item in ReadedItems)
            {
                table.AddRow(item.Id, item.Name, item.MainCategory);
            }

            table.Write();

            Console.WriteLine("Insert id of item to get more information, 2137 for exit, 2136 for recommendation");

            List<string> ItemFlow = new List<string>();
            List<string> ItemsAlreadyLooked = new List<string>();


            bool exit = false;

            while (exit == false)
            {
                string PickItem = Console.ReadLine();
                if (!(string.IsNullOrEmpty(PickItem)))
                {
                    if (int.TryParse(PickItem, out int id))
                    {
                        var index = ReadedItems.FindIndex(x => x.Id == id);
                        if (index != -1)
                        {
                            string stringCategories = string.Empty;

                            for (int i = 0; i < ReadedItems[id - 1].Categories.Count - 1; i++)
                            {
                                if (i == ReadedItems[id - 1].Categories.Count - 2)
                                {
                                    stringCategories += ReadedItems[id - 1].Categories[i];
                                    ItemFlow.Add(ReadedItems[id - 1].Categories[i]);
                                }
                                else
                                {
                                    stringCategories += $"{ReadedItems[id - 1].Categories[i]} ,";
                                    ItemFlow.Add(ReadedItems[id - 1].Categories[i]);
                                }
                            }

                            ItemFlow.Add(ReadedItems[id - 1].MainCategory);
                            ItemFlow.Add(ReadedItems[id - 1].MainCategory);

                            ItemsAlreadyLooked.Add(ReadedItems[id-1].Name);

                            Console.WriteLine($"Name : {ReadedItems[id - 1].Name} \nDescription : {ReadedItems[id - 1].Description} \nCategories : {stringCategories} \nPrice : {ReadedItems[id - 1].Price}");
                        }
                        else if (id == 2136)
                        {

                            var max = (from x in ItemFlow
                                       group x by x into grp
                                       orderby grp.Count() descending
                                       select grp.Key).First();

                            foreach(var item in ReadedItems)
                            {
                                if (!(ItemsAlreadyLooked.Contains(item.Name)))
                                {
                                    if (item.Categories.Contains(max) || item.MainCategory == max)
                                    {
                                        Console.WriteLine($"Ours recomended item for you {item.Name}");
                                    }
                                }
                            } 


                            Console.WriteLine($"Category : {max} ");

                        }
                        else if (id == 2137)
                        {
                            exit = true;
                        }
                        }
                    }


                }

                Console.ReadKey();
            }
        }
    }