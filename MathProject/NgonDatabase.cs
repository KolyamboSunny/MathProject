using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MathProject.Entities;

namespace MathProject
{
    public class NgonDatabase: DbContext
    {
        public DbSet<Ngon> Ngons { get; set; }
        //public DbSet<Edge> Edges { get; set; }
    }
}
