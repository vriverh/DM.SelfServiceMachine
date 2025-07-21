using DM.LocalServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.Repository.IRepository
{
    public interface IReadRepository
    {
        ReadInfo GetReadInfo(int timeOut);

        void CancelGetReadInfo();
    }
}
