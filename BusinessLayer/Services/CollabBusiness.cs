using BusinessLayer.Interface;
using CommonLayer.Model;
using Repository.Entity;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CollabBusiness: ICollabBusiness
    {
        public readonly ICollabRepo _Collab;
        public CollabBusiness(ICollabRepo collab)
        {
            this._Collab = collab;
        }

        public CollabEntity Collab(CollabReq collab)
        {
            return _Collab.Collab(collab);
        }
        public void DeleteCollabs(DeleteCollabReq collab)
        {
            _Collab.DeleteCollabs(collab);
            return;
        }
        public List<CollabEntity> GetAllCollabs(long UserId)
        {
            return _Collab.GetAllCollabs(UserId);
        }
    }

}
