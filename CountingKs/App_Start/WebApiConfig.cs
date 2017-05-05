using System.Data;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace CountingKs
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "Food",
                "api/nutrition/foods/{foodid}",
                new {controller = "foods", foodid = RouteParameter.Optional} 
            );
            config.Routes.MapHttpRoute(
                "Measures",
                "api/nutrition/foods/{foodid}/measure/{id}",
                new {controller = "measures", id = RouteParameter.Optional}
            );
            config.Routes.MapHttpRoute(
                "Diaries",
                "api/user/diaries/{diaryid}",
                new { controller = "diaries", diaryid = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DiaryEntries",
                routeTemplate: "api/user/diaries/{diaryid}/entries/{id}",
                defaults: new { controller = "diaryentries", id = RouteParameter.Optional }
            );


            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            //allow camel casing in the response objects
            var jsonFormater = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            if (jsonFormater != null)
                jsonFormater.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}