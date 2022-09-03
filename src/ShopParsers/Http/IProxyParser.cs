using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopParsers.Http
{
    public interface IProxyParser
    {
        public Task<IEnumerable<ProxyContainer>> GetProxies();
    }
}
