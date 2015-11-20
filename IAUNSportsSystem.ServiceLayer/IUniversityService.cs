using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IUniversityService
    {
        Task<IList<UniversityModel>> GetAll();
        void Add(University university);
        void Delete(int universityId);
        void Edit(University university);

    }

    public class UniversityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
