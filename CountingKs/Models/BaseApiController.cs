using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CountingKs.Data;

namespace CountingKs.Models
{
    public class BaseApiController : ApiController
    {
        private ICountingKsRepository _repo;
        private ModelFactory _modelFactory;
        public BaseApiController(ICountingKsRepository repo)
        {
            _repo = repo;
        }

        protected ICountingKsRepository TheRepository
        {
            get
            {
                return _repo;
            }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, TheRepository);
                }
                return _modelFactory;
            }
        }

    }
}