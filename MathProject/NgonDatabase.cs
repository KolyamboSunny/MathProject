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
    public class NgonDatabase: DbContext
    {
        public DbSet<Ngon> Ngons { get; set; }
        public DbSet<SignMatrix> PluckerSignMatrices { get; set; }
    }
}
