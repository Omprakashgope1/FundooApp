using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLayer.Model
{
    public class PinNoteReq
    {
        public int Id { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }
    }
}
