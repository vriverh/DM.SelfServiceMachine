using DM.SelfServiceMachine.LocalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Repository.IRepository
{
    public interface IReadRepository
    {
        ReadInfo GetReadInfo(int timeOut);

        void CancelGetReadInfo();
    }
}
