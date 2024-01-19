using CommonLayer.Model;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Digests;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteBussiness
    {
        public NoteEntity MakeNote(Note note);
        public void DeleteNote(GetNoteReq req);
        public NoteEntity EditNote(EditNoteReq note);
        public NoteEntity Trash(long id, long UserId);
        public List<NoteEntity> GetAll(long userId);
        public NoteEntity SetColor(ColChangeReq req);
        public NoteEntity SetArchieve(SetArchieveReq req);
        public NoteEntity PinNote(PinNoteReq SetPin);
        public NoteEntity GetById(long id, long userId);
        public NoteEntity ChangeImage(AddImage Image, long UserId);
        public List<NoteEntity> GetByKeyWord(string word,long UserId);
    }
}
