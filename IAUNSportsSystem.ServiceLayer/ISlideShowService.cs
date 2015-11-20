using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ISlideShowService
    {

        Task<SlideShowItemModel> Get(int slideShowItemId);
        Task<IList<SlideShowItemModel>> GetAll();
        void Add(SlideShowItem slideShowItem);
        void Edit(SlideShowItem slideShowItem);
        void Delete(int slideShowItemId);

        IList<SlideShowItemModel> GetSliderImage();
    }

    public class SlideShowItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
    }
}
