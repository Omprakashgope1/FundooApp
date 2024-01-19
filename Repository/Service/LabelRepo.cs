using CommonLayer.Model;
using GreenPipes.Internals.Mapping;
using MimeKit.Encodings;
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
    public class LabelRepo:ILabelRepo
    {
        public FundooContext Context;
        public LabelRepo(FundooContext context)
        {
            Context = context;
        }
        public LabelEntity AddLabel(LabelReq req) 
        {
            LabelEntity label = new LabelEntity();
            label.UserId = req.UserId;
            label.Name = req.Name;
            label.NoteId = req.NoteId;
            Context.Labels.Add(label);
            Context.SaveChanges();
            return label;
        }
        public void DeleteLabel(DeleteLabelReq DeleteLabel)
        {
            LabelEntity label = Context.Labels.FirstOrDefault(x => x.Id == DeleteLabel.id && x.UserId == DeleteLabel.UserId);
            if(label == null)
            {
                throw new Exception("Label of given id does not exists");
            }
            Context.Labels.Remove(label);

            Context.SaveChanges();
            return;
        }

        public LabelEntity EditLabel(EditLableReq req)
        {
            LabelEntity label = Context.Labels.FirstOrDefault(x => x.UserId == req.UserId && x.Id == req.id);
            if (label == null)
            {
                throw new Exception("Label by given id does not exits");
            }
            label.Name = req.NewName;
            Context.SaveChanges();
            return label;
        }

        public List<LabelEntity> GetAll(long userId)
        {
            return Context.Labels.Where(x => x.UserId == userId).ToList();
        }
        public LabelEntity GetByTitle(LabelIdReq req,long UserId)
        {
            LabelEntity entity = Context.Labels.FirstOrDefault(x => x.Name == req.Name && x.UserId == UserId);
            if(entity == null) 
            {
                throw new Exception("Label by given id");
            }
            return entity;

        }
    }
}
