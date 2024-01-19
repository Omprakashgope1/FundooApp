using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLayer.Model
{
    public class EditNoteReq
    {
        public long Id { get; set; }    
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string Color { get; set; }
        public bool IsArchieve { get; set; }
        public bool IsTrash {  get; set; }  
        public bool IsPin { get; set; }
        [JsonIgnore]
        public long UserId {  get; set; }
    }
}
