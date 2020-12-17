
namespace ConnectingPeople.Services.Data
{
using System;
using System.Collections.Generic;
using System.Text;

using ConnectingPeople.Data.Models;

public interface IItemService
    {
        ICollection<Item> GetAll();

        IList<T> MapAll<T>();
    }
}
