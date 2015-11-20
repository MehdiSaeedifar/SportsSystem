using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFSecondLevelCache;
using IAUNSportsSystem.DataLayer;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer.EntityFramework
{
    public class SlideShowService : ISlideShowService
    {
        private readonly IDbContext _dbContext;
        private readonly IDbSet<SlideShowItem> _slideShowItems;

        public SlideShowService(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _slideShowItems = dbContext.Set<SlideShowItem>();
        }

        public async Task<IList<SlideShowItemModel>> GetAll()
        {
            return await _slideShowItems.Select(s => new SlideShowItemModel()
            {
                Id = s.Id,
                Image = s.Image,
                Link = s.Link,
                Order = s.Order,
                Text = s.Text,
                Title = s.Title
            }).ToListAsync();
        }

        public void Add(DomainClasses.SlideShowItem slideShowItem)
        {
            _slideShowItems.Add(slideShowItem);
        }

        public void Edit(DomainClasses.SlideShowItem slideShowItem)
        {
            _slideShowItems.Attach(slideShowItem);

            _dbContext.Entry(slideShowItem).State = EntityState.Modified;

        }

        public void Delete(int slideShowItemId)
        {
            var slideShowItem = new SlideShowItem() { Id = slideShowItemId };
            _slideShowItems.Attach(slideShowItem);
            _slideShowItems.Remove(slideShowItem);
        }

        public async Task<SlideShowItemModel> Get(int slideShowItemId)
        {
            return await _slideShowItems.Where(s => s.Id == slideShowItemId).Select(s => new SlideShowItemModel()
            {
                Id = s.Id,
                Image = s.Image,
                Link = s.Link,
                Order = s.Order,
                Text = s.Text,
                Title = s.Title
            }).FirstOrDefaultAsync();
        }


        public IList<SlideShowItemModel> GetSliderImage()
        {
            return  _slideShowItems.AsNoTracking().Select(s => new SlideShowItemModel()
            {
                Id = s.Id,
                Image = s.Image,
                Link = s.Link,
                Order = s.Order,
                Text = s.Text,
                Title = s.Title
            }).OrderByDescending(s => s.Order).Cacheable().ToList();
        }
    }
}
