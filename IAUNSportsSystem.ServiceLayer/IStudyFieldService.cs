using IAUNSportsSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IStudyFieldService
    {
        Task<IList<StudyFieldModel>> GetAllAsync();
        void Add(StudyField studyField);
        void Delete(int studyFiledId);
        void Edit(StudyField studyField);
    }

    public class StudyFieldModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
