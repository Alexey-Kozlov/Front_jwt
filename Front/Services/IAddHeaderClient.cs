using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Front_jwt.Services
{
    public interface IAddHeaderClient
    {
        Task<string> GetdataFromApi();
    }
}
