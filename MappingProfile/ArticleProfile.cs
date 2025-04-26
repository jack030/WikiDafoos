using AutoMapper;
using WikiDafoos.Models;
using WikiDafoos.Models.ViewModel;

namespace WikiDafoos.MappingProfile
{
    public class ArticleProfile:Profile
    {
        public ArticleProfile() 
        {
            CreateMap<CreateArticleViewModel, Article>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore()) // Manual tags mapping later
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // We'll assign category separately
        }
    }
}
