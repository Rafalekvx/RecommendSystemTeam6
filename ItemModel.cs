using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderTeam6
{
    public class ItemModel
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string MainCategory { get; set; }

        public List<string> Categories = new();
    }
}
