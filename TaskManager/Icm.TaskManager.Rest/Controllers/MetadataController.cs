using Icm.TaskManager.Rest.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Icm.TaskManager.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MetadataController : ApiController
    {
        private class MetadataDto
        {
            public IEnumerable<ApiMethodDto> Methods { get; set; }
        }
        // GET api/metadata
        [HttpGet]
        [ResponseType(typeof(MetadataDto))]
        public IHttpActionResult Get()
        {
            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            var apiMethods = apiExplorer.ApiDescriptions.Select(ad => new ApiMethodDto(ad)).ToList();

            return Ok(new MetadataDto { Methods = apiMethods });
        }
    }
}
