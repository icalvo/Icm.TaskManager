using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Icm.TaskManager.Rest.DTOs
{

    public class ApiParameterDto
    {
        public string Name { get; set; }
        public bool IsUriParameter { get; set; }

        public ApiParameterDto(ApiParameterDescription apiParameterDescription)
        {
            Name = apiParameterDescription.Name;
            IsUriParameter = apiParameterDescription.Source == ApiParameterSource.FromUri;
        }
    }
}