using ConnectingPeople.Data.Common.Repositories;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectingPeople.Services.Data
{
    public class ItemService : IItemService
    {
        private readonly IDeletableEntityRepository<Item> itemRepo;

        public ItemService(IDeletableEntityRepository<Item> itemRepository)
        {
            this.itemRepo = itemRepository;
        }

        public ICollection<Item> GetAll()
        {
            return this.itemRepo.AllAsNoTracking().ToList();
        }

        public IList<T> MapAll<T>()
        {
            return this.itemRepo.AllAsNoTracking()
                .To<T>()
                .ToList<T>();
        }
    }
}
