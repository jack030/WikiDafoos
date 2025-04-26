
namespace WikiDafoos.Models.ViewModel
{
    public class HomeIndexViewModel
    {
        public List<Article> LatestArticles { get; set; }
        public List<Article> SuggestedArticles { get; set; }
        public List<Article> TopViewArticles { get; set; }
    }
}
