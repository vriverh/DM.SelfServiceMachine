using DM.SelfServiceMachine.LocalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Repository.IRepository
{
    /// <summary>
    /// Defines a repository for retrieving developer information.
    /// </summary>
    /// <remarks>This interface provides a method to retrieve developer information encapsulated in a <see
    /// cref="DevInfo"/> object. Implementations of this interface are responsible for defining the source and retrieval
    /// logic for the developer information.</remarks>
    public interface IDevInfoRepository
    {
        DevInfo GetDevInfo();
    }
}
