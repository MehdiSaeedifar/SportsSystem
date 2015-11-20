using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.Models;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface INewsService
    {
        Task<IList<NewsModel>> GetAll();
        void Add(News news);
        Task<NewsModel> Get(int newsId);
        void Edit(News news);
        void Delete(int newsId);
        IList<RecentNewsModel> GetRecentNewsList(int count);
        Task<IList<NewsModel>> GetNewsList(int page, int size);
        Task<int> Count();
    }


    public class RecentNewsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
