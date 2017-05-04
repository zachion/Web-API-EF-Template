﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;

namespace CountingKs.Controllers
{
    public class FoodsController : ApiController
    {

        public IEnumerable<Food> Get()
        {
            var repo = new CountingKsRepository(
                new CountingKsContext());

            var results = repo.GetAllFoods()
                .OrderBy(f => f.Description)
                .Take(25)
                .ToList();
            return results;
        }
    }
}
