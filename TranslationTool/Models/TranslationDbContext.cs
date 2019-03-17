using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TranslationTool.Models
{
    public class TranslationDbContext : DbContext
    {

        public TranslationDbContext(): base("conn")
        {

        }

        public DbSet<Translation> Translations { get; set; }

    }
}