using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLayer.Model
{
    public class LabelReq
    {
        public string Name {  get; set; }
        public long NoteId {  get; set; }
        [JsonIgnore]
        public long UserId {  get; set; }
    }
}
