using De01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De01.BUS
{
    public class SinhVienService
    {
        public List<Sinhvien> GetAll()
        {
            StudentModel context = new StudentModel();
            return context.Sinhviens.ToList();
        }

        public Sinhvien FindById(string studentId)
        {
            StudentModel context = new StudentModel();
            return context.Sinhviens.FirstOrDefault(p => p.MaSV == studentId);
        }

        public void InsertUpdate(Sinhvien s)
        {
            StudentModel context = new StudentModel();
            context.Sinhviens.AddOrUpdate(s);
            context.SaveChanges();
        }

        public void InsertNew(Sinhvien s)
        {
            StudentModel context = new StudentModel();
            context.Sinhviens.Add(s);
            context.SaveChanges();
        }
    }
}