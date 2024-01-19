using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repository.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        [ForeignKey("Note")]
        public long NoteId {  get; set; }
        [JsonIgnore]
        public UserEntity User { get; set; }
        [JsonIgnore]
        public NoteEntity Note { get; set; }
    }
}
