using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        private static SamuraiContext _contextNT = new SamuraiContextNoTracking();

        private static void Main(string[] args)
        {
            /*AddSamuraisByName("Rishank", "Sumit", "Aakanksha", "Abhigyaan", "Aryan", "Soumya");
            GetSamurais();
            AddVariousType();
            Console.Write("Press Any Key...");
            Console.ReadKey();*/
            //QueryFilters();
            //QueryAggregates();
            //RetrieveAndUpdate();
            //RetrieveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();
            //RetrieveAndDeleteSamurai();
            //QueryAndUpdateBattles_Disconnected();
        }

        private static void AddVariousType()
        {
            _context.AddRange(
                new Samurai { Name = "Mitashi" },
                new Samurai { Name = "Hilary" },
                new Battle { Name = "Battle of Nagamoto" },
                new Battle { Name = "Battle of Nakasaki" });

            /*_context.Samurais.AddRange(
                new Samurai { Name = "Shimada"},
                new Samurai { Name = "Okamoto"});

            _context.Battles.AddRange(
                new Battle { Name = "Battle of Anegawa" },
                new Battle { Name = "Battle of Nagashino" });*/

            _context.SaveChanges();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach(string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name});
            }
            _context.SaveChanges();
        }

        private static void GetSamurais()
        {
            var samurais = _contextNT.Samurais.TagWith("ConsoleApp.Program.GetSamurais method").ToList();
            Console.WriteLine($"Samurai Count is: {samurais.Count}");
            foreach(var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters()
        {
            //var name = "Sampson";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();

            var samurais = _contextNT.Samurais.Where(s => EF.Functions.Like(s.Name, "J%")).ToList();
        }

        private static void QueryAggregates()
        {
            //var name = "Sampson";
            //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);

            var samurai = _contextNT.Samurais.Find(2);
        }

        private static void RetrieveAndUpdate()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();

        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurai = _context.Samurais.Skip(1).Take(4).ToList();
            samurai.ForEach(s => s.Name += "San");
            _context.SaveChanges();

        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();

        }

        private static void RetrieveAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(17);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            }

            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });

            using(var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }
    }
}
