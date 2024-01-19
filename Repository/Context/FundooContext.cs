using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Context
{
    public class FundooContext:DbContext
    {
        public FundooContext(DbContextOptions<FundooContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }   
        public DbSet<CollabEntity> Collabs { get; set; }
        public DbSet<LabelEntity> Labels { get; set; }
        
    }
}
