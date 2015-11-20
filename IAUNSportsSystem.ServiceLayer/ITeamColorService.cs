using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.ServiceLayer
{
    public interface ITeamColorService
    {
        Task Add(int participationId,int representativeUserId, string[] colors);
        Task<bool> CanAddTemColor(int participationId, int representativeUserId);
    }
}
