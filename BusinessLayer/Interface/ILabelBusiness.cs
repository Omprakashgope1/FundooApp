using CommonLayer.Model;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ILabelBusiness
    {
        public LabelEntity AddLabel(LabelReq req);
        public void DeleteLabel(DeleteLabelReq DeleteLabel);
        public LabelEntity EditLabel(EditLableReq req);
        public List<LabelEntity> GetAll(long userId);
        public LabelEntity GetByTitle(LabelIdReq req, long UserId);
    }
}
