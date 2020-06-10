﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaCar.Data.Requests.Comments;
using RentACar.WebAPI.Interface;

namespace RentACar.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseCRUDController<RentaCar.Data.Models.MComment, CommentSearchRequest, CommentUpsert, CommentUpsert>
    {
        public CommentController(ICRUDService<RentaCar.Data.Models.MComment, CommentSearchRequest, CommentUpsert, CommentUpsert> service) : base(service)
        {
        }
    }
}