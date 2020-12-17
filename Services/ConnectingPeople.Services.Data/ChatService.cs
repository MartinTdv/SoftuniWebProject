using AutoMapper.Internal;
using ConnectingPeople.Data;
using ConnectingPeople.Data.Common.Repositories;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Data.Models;
using ConnectingPeople.Services.Mapping;
using ConnectingPeople.Web.ViewModels.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectingPeople.Services.Data
{
    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Chat> chatRepo;

        public ChatService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Chat> chatRepository)
        {
            this.userRepo = userRepository;
            this.chatRepo = chatRepository;
        }

        public async Task<Chat> BindConnectionIdToUserAsync(int chatId, string currentlyLoggedUsername, string connectionId)
        {
            var chat = this.chatRepo.All()
                .FirstOrDefault(x => x.Id == chatId);
            if (chat.OthersideUsername == currentlyLoggedUsername)
            {
                chat.OthersideConnectionId = connectionId;
            }
            else
            {
                chat.TaskCreatorConnectionId = connectionId;
            }
            await this.chatRepo.SaveChangesAsync();
            return chat;
        }

        public async Task<int> CreateChatAsync(ApplyFormInputModel input)
        {
            var groupName = this.Hash(input.TaskCreatorUsername + input.OthersideUsername + input.About);
            var existingChatId = this.chatRepo.AllAsNoTracking()
                .Where(c => c.ChatGroupName == groupName)
                .Select(c => c.Id)
                .FirstOrDefault();
            if (existingChatId != 0)
            {
                return existingChatId;
            }

            var chat = new Chat
            {
                About = input.About,
                TaskCreatorUsername = input.TaskCreatorUsername,
                OthersideUsername = input.OthersideUsername,
                ChatGroupName = groupName,
            };
            chat.Messages.Add(new Message
            {
                SenderUsername = chat.OthersideUsername,
                Text = input.MessageText,
            });
            await this.chatRepo.AddAsync(chat);
            await this.chatRepo.SaveChangesAsync();

            chat = this.chatRepo.AllAsNoTracking().OrderBy(x => x.CreatedOn).LastOrDefault();

            var bothUsersInChat = this.userRepo.All()
                .Where(x => x.UserName == input.OthersideUsername || x.UserName == input.TaskCreatorUsername)
                .ToList();
            foreach (var user in bothUsersInChat)
            {
                var userChat = new UserChat
                {
                    UserId = user.Id,
                    ChatId = chat.Id,
                };
                chat.Users.Add(userChat);
                user.Chats.Add(userChat);
            }
            await this.userRepo.SaveChangesAsync();
            await this.chatRepo.SaveChangesAsync();
            return chat.Id;
        }

        public Chat GetChatById(int id)
        {
            return this.chatRepo.AllAsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        public UsersNameAndImageDTO GetUserChatByUserId(int id)
        {
            return this.chatRepo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new UsersNameAndImageDTO
                {
                    User = x.Users.Select(z => z.User).ToList(),
                })
                .FirstOrDefault();
        }

        public T MapChatById<T>(int chatId)
        {
            return this.chatRepo.AllAsNoTracking()
                .Where(x => x.Id == chatId)
                .To<T>()
                .FirstOrDefault();
        }

        public ICollection<T> MapUserChats<T>(string username)
        {
            var userId = this.userRepo.AllAsNoTracking()
                .FirstOrDefault(u => u.UserName == username)
                .Id;

            var userChats = this.chatRepo.AllAsNoTracking()
                .Where(x => x.Users.Any(c => c.UserId == userId))
                .To<T>()
                .ToList();

            return userChats;
        }

        public async Task SaveMessageToChat(Message message)
        {
            var chat = this.chatRepo.All()
                .FirstOrDefault(x => x.Id == message.ChatId);
            chat.Messages.Add(message);
            await this.chatRepo.SaveChangesAsync();
        }

        public async Task SetGroupNameAsync(string groupName, int chatId)
        {
            this.chatRepo.All()
                .FirstOrDefault(x => x.Id == chatId)
                .ChatGroupName = groupName;
            await this.chatRepo.SaveChangesAsync();
        }
        private string Hash(string text)
        {
            HashAlgorithm sha = SHA256.Create();
            return Encoding.Default.GetString(sha.ComputeHash(Encoding.Default.GetBytes(text)));
        }
    }
}
