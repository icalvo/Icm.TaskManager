using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Icm.TaskManager.Rest.DTOs
{

    public class ApiMethodDto
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public IEnumerable<ApiParameterDto> Parameters { get; set; }

        public ApiMethodDto(ApiDescription apiDescription)
        {
            Method = apiDescription.HttpMethod.Method;
            Url = apiDescription.RelativePath;
            ControllerName = apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName;
            ActionName = apiDescription.ActionDescriptor.ActionName;
            Parameters = apiDescription.ParameterDescriptions.Select(pd => new ApiParameterDto(pd)).ToList();
        }
    }


}