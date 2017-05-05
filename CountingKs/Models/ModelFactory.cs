using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using CountingKs.Data.Entities;

namespace CountingKs.Models
{
    public class ModelFactory
    {
        private UrlHelper _urlHelper;

        public ModelFactory(HttpRequestMessage request)
        {
            _urlHelper = new UrlHelper(request);
        }
        public FoodModel Create(Food food)
        {
            return new FoodModel()
            {
                Url = _urlHelper.Link("Food", new {foodid = food.Id}),
                Description = food.Description,
                Measures = food.Measures.Select(m => Create(m))
            };
        }
        public MeasureModel Create(Measure measure)
        {
            return new MeasureModel()
            {
                Url = _urlHelper.Link("Measure", new { foodid = measure.Food.Id ,id=measure.Id}),
                Description = measure.Description,
                Calories = measure.Calories
            };
        }

        public DiaryModel Create(Diary d)
        {
            return new DiaryModel()
            {
                Url = _urlHelper.Link("Diaries", new { diaryid = d.CurrentDate.ToString("yyyy-MM-dd") }),
                CurrentDate = d.CurrentDate,
                Entries = d.Entries.Select(e => Create(e))
            };
        }


        public DiaryEntryModel Create(DiaryEntry entry)
        {
            return new DiaryEntryModel()
            {
                Url = _urlHelper.Link("DiaryEntries", new { diaryid = entry.Diary.CurrentDate.ToString("yyyy-MM-dd"), id = entry.Id }),
                Quantity = entry.Quantity,
                FoodDescription = entry.FoodItem.Description,
                MeasureDescription = entry.Measure.Description,
                MeasureUrl = _urlHelper.Link("Measures", new { foodid = entry.FoodItem.Id, id = entry.Measure.Id })
            };
        }
    }
}