using AutoMapper;
using AutoMapper.Internal;
using ConnectingPeople.Data;
using ConnectingPeople.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectingPeople.Web.ViewModels.Chat
{
    public class MessageViewModel : IMapFrom<Message>
    {
        public string SenderUsername { get; set; }

        public string Text { get; set; }

    }
}
