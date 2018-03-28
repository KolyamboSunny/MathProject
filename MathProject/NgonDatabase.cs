using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MathProject.Entities;
using MathProject.Tools;

namespace MathProject
{
    public class NgonDatabase : DbContext
    {
        public IQueryable<Ngon> Ngons;
        public IQueryable<SignMatrix> PluckerSignMatrices;
       
        public NgonDatabase(int dimensions=5) :base("name=NgonContext")//:base("name=NgonContext")
        {
            Database.CreateIfNotExists();
            Database.Connection.Open();            
            Ngons = NgonStorage.Where(n => n.Verticies.Count == dimensions);
            PluckerSignMatrices = PluckerSignMatricesStorage.Where(n => n.dimensions == dimensions);                       
        }
        public void Add(Ngon ngon)
        {
            ngon.PluckerSignMatrix.Ngons.Add(ngon);
            this.NgonStorage.Add(ngon);
            this.SaveChanges();
        }
        public void Add(IEnumerable<Ngon> ngons)
        {
            foreach (var ngon in ngons)
            {
                ngon.PluckerSignMatrix.Ngons.Add(ngon);
            }                    
            this.NgonStorage.AddRange(ngons);
            this.SaveChanges();
        }
        public void Add(SignMatrix pluckerMatrix)
        {
            this.PluckerSignMatricesStorage.Add(pluckerMatrix);
            this.SaveChanges();
        }
        public void Add(IEnumerable<SignMatrix> pluckerMatrices)
        {
            this.PluckerSignMatricesStorage.AddRange(pluckerMatrices);
            this.SaveChanges();
        }
        public DbSet<Ngon> NgonStorage{get;set;}
        public DbSet<SignMatrix> PluckerSignMatricesStorage { get; set; }     
    }
}
