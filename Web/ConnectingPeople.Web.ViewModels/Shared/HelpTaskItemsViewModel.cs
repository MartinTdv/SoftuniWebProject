
namespace ConnectingPeople.Web.ViewModels.Profile
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using ConnectingPeople.Data.Common.Enums;
    using ConnectingPeople.Data.Models;
    using ConnectingPeople.Services.Mapping;

    public class HelpTaskItemsViewModel : IMapFrom<HelpTaskItems>, IHaveCustomMappings
    {
        public string NameInCyrillic { get; set; }

        public string FAIconClass { get; set; }

        public ItemUseType ItemUseType { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<HelpTaskItems, HelpTaskItemsViewModel>()
                .ForMember(x => x.FAIconClass, options =>
                {
                    options.MapFrom(x => x.Item.FAIconClass);
                })
                .ForMember(x => x.NameInCyrillic, options =>
                {
                    options.MapFrom(x => x.Item.NameInCyrillic);
                });
        }
    }
}
