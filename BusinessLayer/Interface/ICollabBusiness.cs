using CommonLayer.Model;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICollabBusiness
    {
        public CollabEntity Collab(CollabReq collab);
        public void DeleteCollabs(DeleteCollabReq collab);
        public List<CollabEntity> GetAllCollabs(long UserId);
    }
}
