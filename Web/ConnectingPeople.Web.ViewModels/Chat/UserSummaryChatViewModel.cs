using AutoMapper;
using ConnectingPeople.Data;
using ConnectingPeople.Data.Models;
using ConnectingPeople.Services.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Chat
{
    public class UserSummaryChatViewModel : IMapFrom<Data.Chat>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string About { get; set; }

        public string LastMessageSenderUsername { get; set; }

        public string LastMessageText { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ChatPartnerImageName { get; set; }

        public string ChatPartner { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Chat, UserSummaryChatViewModel>()
                .ForMember(
                    x => x.LastMessageSenderUsername,
                    opt => opt.MapFrom(
                        map => map.Messages.OrderBy(x => x.CreatedOn).LastOrDefault().SenderUsername))
                .ForMember(
                    x => x.LastMessageText,
                    opt => opt.MapFrom(
                    map => map.Messages.OrderBy(x => x.CreatedOn).LastOrDefault().Text));
        }
    }
}
