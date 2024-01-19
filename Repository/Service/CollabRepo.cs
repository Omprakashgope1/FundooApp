using CommonLayer.Model;
using Repository.Context;
using Repository.Entity;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Service
{
    public class CollabRepo:ICollabRepo
    {
        public FundooContext Context;
        public CollabRepo(FundooContext context)
        {
            this.Context = context;
        }

        public CollabEntity Collab(CollabReq collab)
        {
            CollabEntity collabEntity = new CollabEntity();
            CollabEntity CollabFind = Context.Collabs.FirstOrDefault(x=>x.Email == collab.Email &&
            x.NoteId == collab.NoteId && x.UserId == collab.UserId);
            if(CollabFind != null)
            {
                throw new Exception("Email already exists");
            }
            var Email = Context.Users.FirstOrDefault(x => x.Email == collab.Email);
            if(Email == null)
            {
                throw new Exception("Email with this particular name does not exists");
            }
            var Note = Context.Notes.FirstOrDefault(x => x.id == collab.NoteId && x.UserId == collab.UserId);
            if(Note == null)
            {
                throw new Exception("User does not contain the given NoteId");
            }
            collabEntity.Email = collab.Email;
            collabEntity.NoteId = collab.NoteId;
            collabEntity.UserId = collab.UserId;
            Context.Collabs.Add(collabEntity);
            Context.SaveChanges();
            return collabEntity;
        }
        public void DeleteCollabs(DeleteCollabReq collab)
        {
            CollabEntity entity = Context.Collabs.FirstOrDefault(x => x.Email == collab.Email && x.UserId == collab.UserId);
            Context.Remove(entity);
            Context.SaveChanges(true);
        }
        public List<CollabEntity> GetAllCollabs(long UserId) 
        {
            return Context.Collabs.Where(x => x.UserId == UserId).ToList();
        }
    }
}
