﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;

namespace CountingKs.Controllers
{
    public class TokenController : BaseApiController
    {
        public TokenController(ICountingKsRepository repo) : base(repo)
        {

        }

        public HttpResponseMessage Post([FromBody] TokenRequestModel model)
        {
            try
            {
                var user = TheRepository.GetApiUsers().FirstOrDefault(u => u.AppId == model.ApiKey);

                if (user != null)
                {
                    var secret = user.Secret;

                    // simplistic  implementation do not use
                    var key = Convert.FromBase64String(secret);
                    var provider = new System.Security.Cryptography.HMACSHA256(key);
                    var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(user.AppId));

                    var signature = Convert.ToBase64String(hash);

                    if (signature == model.Signature)
                    {
                        var rawTokenInfo = string.Concat(user.AppId + DateTime.UtcNow.ToString("d"));
                        var rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
                        var token = provider.ComputeHash(rawTokenByte);
                        var authToken = new AuthToken()
                        {
                            Token = Convert.ToBase64String(token),
                            Expiration = DateTime.UtcNow.AddDays(7),
                            ApiUser = user
                        };
                        if (TheRepository.Insert(authToken) && TheRepository.SaveAll())
                        {
                            return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(authToken));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not authorised");
        }
    }
}