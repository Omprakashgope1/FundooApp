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
    public class LabelBusiness:ILabelBusiness
    {
        public readonly ILabelRepo LabelRepo;
        public LabelBusiness(ILabelRepo labelRepo)
        {
            LabelRepo = labelRepo;
        }

        public LabelEntity AddLabel(LabelReq req)
        {
            return LabelRepo.AddLabel(req);
        }

        public void DeleteLabel(DeleteLabelReq DeleteLabel)
        {
            LabelRepo.DeleteLabel(DeleteLabel);
        }

        public LabelEntity EditLabel(EditLableReq req)
        {
            return LabelRepo.EditLabel(req);
        }

        public List<LabelEntity> GetAll(long userId)
        {
            return LabelRepo.GetAll(userId);
        }
        public LabelEntity GetByTitle(LabelIdReq req, long UserId)
        {
            return LabelRepo.GetByTitle(req, UserId);
        }
    }
}
