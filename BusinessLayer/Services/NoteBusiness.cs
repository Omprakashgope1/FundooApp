using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Repository.Entity;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBusiness : INoteBussiness
    {
        public readonly INoteRepo _NoteRepo;
        public NoteBusiness(INoteRepo NoteRepo)
        {
            _NoteRepo = NoteRepo;
        }

        public void DeleteNote(GetNoteReq req)
        {
            _NoteRepo.DeleteNote(req);
        }

        public NoteEntity MakeNote(Note note)
        {
            return _NoteRepo.MakeNote(note);
        }
        public NoteEntity EditNote(EditNoteReq EditReq)
        {
            return _NoteRepo.EditNote(EditReq);
        }
        public NoteEntity Trash(long id, long UserId)
        {
            return _NoteRepo.Trash(id, UserId);
        }
        public List<NoteEntity> GetAll(long userId)
        {
            return _NoteRepo.GetAll(userId);
        }
        public NoteEntity SetColor(ColChangeReq req)
        {
            return _NoteRepo.SetColor(req);
        }
        public NoteEntity SetArchieve(SetArchieveReq req)
        {
            return _NoteRepo.SetArchieve(req);
        }
        public NoteEntity PinNote(PinNoteReq SetPin)
        {
            return _NoteRepo.PinNote(SetPin);
        }
        public NoteEntity GetById(long id, long userId)
        {
            return _NoteRepo.GetById(id,userId);
        }

        public NoteEntity ChangeImage(AddImage Image, long UserId)
        {
            return _NoteRepo.ChangeImage(Image, UserId);
        }
        public List<NoteEntity> GetByKeyWord(string word, long UserId)
        {
            return _NoteRepo.GetByKeyWord(word,UserId);
        }
    }
}
