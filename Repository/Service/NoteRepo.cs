using CommonLayer.Model;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Utilities;
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
    public class NoteRepo : INoteRepo
    {
        public readonly FundooContext context;
        public NoteRepo(FundooContext context)
        {
            this.context = context;
        }
        //Documentation-Ghost docs
        /// <summary>
        /// 
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public NoteEntity MakeNote(Note note)
        {
            NoteEntity notes = new NoteEntity();
            notes.Title = note.Title;
            notes.Reminder = note.Reminder;
            notes.Image = note.Image;
            notes.IsArchieve = note.IsArchieve;
            notes.IsPin = note.IsPin;
            notes.Color = note.Color;
            notes.Description = note.Description;
            notes.UserId = note.UserId;
            notes.Reminder = note.Reminder;
            notes.IsTrash = note.IsTrash;
            context.Notes.Add(notes);
            context.SaveChanges();
            return notes;
        }
        public void DeleteNote(GetNoteReq req)
        {
            NoteEntity note = context.Notes.FirstOrDefault(x => x.id == req.id && x.UserId == req.UserId);
            context.Remove(note);
            context.SaveChanges();
        }
        public NoteEntity EditNote(EditNoteReq note) 
        {
            NoteEntity notes = context.Notes.FirstOrDefault(x => x.id == note.Id && x.UserId == note.UserId);
            if (notes == null) 
            {
                throw new Exception("user not found");
            }
            notes.Title = note.Title;
            notes.Reminder = note.Reminder;
            notes.IsArchieve = note.IsArchieve;
            notes.IsPin = note.IsPin;
            notes.Color = note.Color;
            notes.Description = note.Description;
            notes.UserId = note.UserId;
            notes.IsTrash = note.IsTrash;
            context.SaveChanges();
            return notes;
        }
        public NoteEntity Trash(long id,long UserId)
        {
            NoteEntity notes = context.Notes.FirstOrDefault(x => x.id == id && x.UserId == UserId);
            if (notes == null) 
            {
                throw new Exception("Note not found");
            }
            if (notes.IsTrash == false)
            {
                notes.IsTrash = true;
            }
            else
            notes.IsTrash = false;
            context.SaveChanges();
            return notes;
        }

        public List<NoteEntity> GetAll(long userId)
        {
            List<NoteEntity> notes = context.Notes.Where(x => x.UserId == userId).ToList();
            return notes;
        }
        public NoteEntity SetColor(ColChangeReq req)
        {
            NoteEntity note = context.Notes.FirstOrDefault(x => x.id == req.id && x.UserId == req.UserId);
            if (note == null)
            {
                throw new Exception("Not found");
            }
            note.Color = req.color;
            context.SaveChanges();
            return note;
        }
        public NoteEntity SetArchieve(SetArchieveReq req) 
        {
            NoteEntity note = context.Notes.FirstOrDefault(x => x.id == req.id &&  req.UserId == req.UserId);
            if(note == null) 
            {
                throw new Exception("Not found");
            }
            if(note.IsArchieve == true)
            {
                note.IsArchieve = false;
            }
            else
                note.IsArchieve = true;
            context.SaveChanges();
            return note;
        }
        public NoteEntity ChangeImage(AddImage Image,long UserId)
        {
            NoteEntity note = context.Notes.FirstOrDefault(x => x.UserId == UserId && x.id == Image.id);
            if(note == null) 
            {
                throw new Exception("Note of given id does not exists");
            }
            note.Image = UploadClass.UploadPhoto(Image.Image.OpenReadStream());
            return note;
        }
        public NoteEntity PinNote(PinNoteReq req)
        {
            NoteEntity note = context.Notes.FirstOrDefault(x => x.id == req.Id & req.UserId == req.UserId);
            if (note == null)
            {
                throw new Exception("Note Not found");
            }
            if (note.IsPin == true) 
            {
                note.IsPin = false;
            }
            else
                note.IsPin = true;
            context.SaveChanges();
            return note;
        }

        public NoteEntity GetById(long id, long userId)
        {
            NoteEntity note = context.Notes.FirstOrDefault(x => x.id == id && x.UserId == userId);
            return note;
        }
        public List<NoteEntity> GetByKeyWord(string word, long UserId) 
        {
            List<NoteEntity> ans = new List<NoteEntity>();
            var list = context.Notes.Where(x => x.UserId == UserId).ToList();
            foreach(var item in list) 
            {
                NoteEntity note = item;
                string title = item.Title;
                string description = item.Description;
                if(title.Contains(word) || description.Contains(word)) 
                {
                    ans.Add(note);
                }
            }
            return ans;
        }
    }
}
