using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OKC_LOTTERY
{
    class Program
    {
        HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Program program = new Program();
            await program.getPlays();

        }

        private async Task getPlays()
        {   
            //Connects to data source
            string response = await client.GetStringAsync("https://www.lottery.ok.gov/plays.json");

            //Maps Json data to usable objects
            List<Plays> plays = JsonConvert.DeserializeObject<List<Plays>>(response);

            //Add all games played to new list
            List<string> gamesList = new List<string>();
            foreach (var items in plays)
            {
                foreach(string games in items.games_played)
                {
                    gamesList.Add(games);
                }
            }

            //Group and count values then pair them into a dictionary
            var results = gamesList.GroupBy(x => x).ToDictionary(y => y.Key, z => z.Count());
            foreach(var result in results)
            {
                Console.WriteLine($"{result.Key} was played {result.Value} times");
            }
            
        }
    }



    // Represents the Json objects
    class Plays {
        public DateTime date_played { get; set; }
        public List<string> games_played { get; set; }
    }

}
