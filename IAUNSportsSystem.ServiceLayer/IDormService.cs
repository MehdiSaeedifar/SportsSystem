using IAUNSportsSystem.DomainClasses;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IDormService
    {
        Task<IList<DormModel>> GetAll();
        void Add(Dorm dorm);
        void Edit(Dorm dorm);
        void Delete(int dormId);
    }

    public class DormModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}