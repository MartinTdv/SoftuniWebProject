using ConnectingPeople.Data.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingPeople.Data.Seeding
{
    public class ItemSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Items.Any())
            {
                return;
            }

            var items = new List<(string Name, string IconClass)>()
            {
                ("кола", "fas fa-car"),
                ("ремарке", "fas fa-trailer"),
                ("лаптоп", "fas fa-laptop"),
                ("бус", "fas fa-truck"),
                ("колело", "fas fa-bicycle"),
                ("метла", "fas fa-broom"),
                ("фотоапарат", "fas fa-camera"),
            };

            foreach (var item in items)
            {
                await dbContext.Items.AddAsync(new Item
                {
                    NameInCyrillic = item.Name,
                    FAIconClass = item.IconClass,
                });
            }
        }
    }
}
