using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAUNSportsSystem.DomainClasses;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ITechnicalStaffRoleService
    {
        Task<IList<TechnicalStaffModel>> GetAllParticipationRoles();
        Task<IList<TechnicalStaffModel>> GetAllCommonRoles();
        Task<IList<TechnicalStaffModel>> GetAllRoles();
        void Add(TechnicalStaffRole technicalStaffRole);
        void Edit(TechnicalStaffRole technicalStaffRole);
        void Delete(int technicalStaffRoleId);
    }

    public class TechnicalStaffModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCommon { get; set; }
    }
}
