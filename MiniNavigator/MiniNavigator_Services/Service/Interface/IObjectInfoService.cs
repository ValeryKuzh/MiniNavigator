using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service.Interface
{
    public interface IObjectInfoService<Type> where Type : class
    {
        string GetTitle();
    }
}
