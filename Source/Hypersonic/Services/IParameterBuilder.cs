using System.Collections.Generic;
using System.Data.Common;

namespace Hypersonic.Services
{
    public interface IParameterBuilder
    {
        List<DbParameter> GetParameters<T>(T paramters);
    }
}
