using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService
{
    public interface ISaves
    {
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
