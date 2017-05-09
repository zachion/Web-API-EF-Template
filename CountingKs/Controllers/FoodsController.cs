using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.UI;
using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;
using Microsoft.Ajax.Utilities;

namespace CountingKs.Controllers
{
    public class FoodsController : BaseApiController
    {
       
        public FoodsController(ICountingKsRepository repo) :base(repo)
        {
        }

        const int PAGE_SIZE = 40;

        public object Get(bool includeMeasures = true, int page = 0)
        {
            IQueryable<Food> query;

            if (includeMeasures)
            {
                query = TheRepository.GetAllFoodsWithMeasures();
            }
            else
            {
                query = TheRepository.GetAllFoods();
            }

            var baseQuesry = query.OrderBy(f => f.Description);
            var totalCount = baseQuesry.Count();

            var totalPages = Math.Ceiling((double) totalCount / PAGE_SIZE);

            var helper = new UrlHelper(Request);

            var prevPage = page > 0 ? helper.Link("Food", new { page = page - 1 }) : "";
            var nextPage = page < totalPages -1 ? helper.Link("Food", new { page = page + 1 }) : "";


            var results = baseQuesry.Skip(PAGE_SIZE * page)
                .Take(PAGE_SIZE)
                .ToList()
                .Select(f => TheModelFactory.Create(f));

            return new
            {
                TotalCount = totalCount,
                TotalPage = totalPages,
                PrevPageUrl = prevPage,
                NextPageUrl = nextPage,
                Results = results
            };
            
        }

        public FoodModel Get(int foodid)
        {
            return TheModelFactory.Create(TheRepository.GetFood(foodid));
        }
    }
}
