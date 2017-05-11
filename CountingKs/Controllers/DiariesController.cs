using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Filters;
using CountingKs.Models;
using CountingKs.Services;

namespace CountingKs.Controllers
{
    [CountingKsAuthorize]
    public class DiariesController : BaseApiController
    {
        private ICountingKsIdentityService _identityService;

        public DiariesController(ICountingKsRepository repo,
            ICountingKsIdentityService identityService) : base(repo)
        {
            _identityService = identityService;
        }
        
        //api/user/diaries
        public IEnumerable<DiaryModel> Get()
        {
            var username = _identityService.CurrentUser;
            var results = TheRepository
                .GetDiaries(username)
                .OrderByDescending(d => d.CurrentDate)
                .Take(10)
                .ToList()
                .Select(d => TheModelFactory.Create(d));

            return results;

        }

        //api/user/diaries/2017-05-04
        public HttpResponseMessage Get(DateTime diaryId)
        {
            var username = _identityService.CurrentUser;
            var result = TheRepository.GetDiary(username, diaryId);

            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, 
                TheModelFactory.Create(result));
        }
        
        //api/user/diaries
        public HttpResponseMessage Post([FromBody] DiaryModel model)
        {
            try
            {
                if (TheRepository.GetDiaries(_identityService.CurrentUser)
                        .Count(e => e.CurrentDate == model.CurrentDate.Date) > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, "A diary exists for that date");
                }

                var entity = TheModelFactory.Parse(model);
                entity.UserName = _identityService.CurrentUser;

                if (TheRepository.Insert(entity) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
                }
            }
            catch (Exception ex)
            {
                // TODO addloging
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        //api/user/diaries/2017-05-04
        public HttpResponseMessage Delete(DateTime diaryId)
        {
            try
            {
                var entity = TheRepository.GetDiary(_identityService.CurrentUser, diaryId);
                if (entity == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if (TheRepository.DeleteDiary(entity.Id) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch
            {
                // TODO Add Logging
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
