using ConnectingPeople.Data;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Data.Models;
using ConnectingPeople.Web.ViewModels.Forms;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingPeople.Services.Data
{
    public interface IChatService
    {
        ICollection<T> MapUserChats<T>(string username);

        UsersNameAndImageDTO GetUserChatByUserId(int id);

        Task<Chat> BindConnectionIdToUserAsync(int chatId, string currentlyLoggedUsername, string connectionId);

        Task SetGroupNameAsync(string groupName, int chatId);

        Task<int> CreateChatAsync(ApplyFormInputModel input);

        Chat GetChatById(int id);

        Task SaveMessageToChat(Message message);

        T MapChatById<T>(int chatId);

    }
}
