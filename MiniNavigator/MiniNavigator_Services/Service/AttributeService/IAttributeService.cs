using MiniNavigator_Services.DTO;
using System;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    public interface IAttributeService
    {
        Task CreateAttributeAsync(ObjectAttributeDTO objectAttributeDTO, Guid typeID);
    }
}
