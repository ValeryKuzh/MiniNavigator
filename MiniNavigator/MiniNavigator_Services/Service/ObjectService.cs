using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using MiniNavigator_DB.Repository;
using MiniNavigator_Services.Mapper;
using MiniNavigator_UI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_Services.Service
{
    public class ObjectService
    {
        private readonly IMapper<NavObjectDTO, BaseObject> _objectMapper;

        private readonly IRepository<BaseObject> _objectRepository;

        public ObjectService(IMapper<NavObjectDTO, BaseObject> objectMapper, IRepository<BaseObject> objectRepository)
        {
            _objectMapper = objectMapper;
            _objectRepository = objectRepository;
        }
    }
}
