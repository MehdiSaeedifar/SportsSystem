using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUNSportsSystem.DataLayer
{
    public class SportSystemDbConfiguration : DbConfiguration
    {
        public SportSystemDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlServerExecutionStrategy());
        }
    }
}
