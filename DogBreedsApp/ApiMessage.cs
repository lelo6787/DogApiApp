using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DogBreedsApp
{
    class ApiMessage
    {
        [JsonProperty("message")]
        public dynamic message { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

    }
}
