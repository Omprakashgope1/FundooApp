using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLayer.Model
{
    public class EditLableReq
    {
        public long id { get; set; }
        public string NewName { get; set; }
        [JsonIgnore]
        public long UserId {  get; set; }
    }
}
