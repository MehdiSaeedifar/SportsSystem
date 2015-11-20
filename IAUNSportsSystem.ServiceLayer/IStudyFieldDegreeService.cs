using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface IStudyFieldDegreeService
    {
        Task<IList<StudyFieldDegreeModel>> GetAllAsync();
        void Add(StudyFieldDegree studyFieldDegree);
        void Delete(int studyFieldDegreeId);
        void Edit(StudyFieldDegree studyFieldDegree);


    }

    public class StudyFieldDegreeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
