using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFSecondLevelCache;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.Models;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class NewsService : INewsService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<News> _newses;

        public NewsService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _newses = dbContext.Set<News>();
        }

        public async Task<IList<NewsModel>> GetAll()
        {
            return await _newses.Select(a => new NewsModel()
            {
                Id = a.Id,
                CreatedDate = a.CreatedDate,
                Title = a.Title,
                Body = a.Body
            }).ToListAsync();
        }

        public void Add(News news)
        {
            _newses.Add(news);
        }

        public async Task<NewsModel> Get(int newsId)
        {
            return await _newses.Where(n => n.Id == newsId).Select(n => new NewsModel()
            {
                Id = n.Id,
                Title = n.Title,
                Body = n.Body
            }).FirstOrDefaultAsync();
        }

        public void Edit(News news)
        {
            _newses.Attach(news);

            //_dbContext.Entry(news).State = EntityState.Modified;
            _dbContext.Entry(news).Property(n => n.Title).IsModified = true;
            _dbContext.Entry(news).Property(n => n.Body).IsModified = true;
        }


        public void Delete(int newsId)
        {
            var news = new News() { Id = newsId };
            _newses.Attach(news);
            _newses.Remove(news);
        }


        public IList<RecentNewsModel> GetRecentNewsList(int count)
        {
            return
                _newses.AsNoTracking().OrderByDescending(news => news.CreatedDate).Select(news => new RecentNewsModel()
                {
                    Id = news.Id,
                    Title = news.Title
                }).Take(count).Cacheable().ToList();
        }

        public async Task<IList<NewsModel>> GetNewsList(int page, int size)
        {
            return await
                _newses.AsNoTracking().OrderByDescending(news => news.CreatedDate).Select(news => new NewsModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    Body = news.Body,
                    CreatedDate = news.CreatedDate
                }).Skip(page * size).Take(size).Cacheable().ToListAsync();
        }

        public async Task<int> Count()
        {
            return await _newses.Cacheable().CountAsync();
        }

    }
}
