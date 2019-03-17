using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TranslationTool.Models
{
    [Table("Translation")]
    public class Translation
    {

        [Key]
        public int IdTranslation { get; set; }

        public string SerbianTranslation { get; set; }

        public string EnglishTranslation { get; set; }

        public DateTime SavedDateAndTime { get; set; }

        public DateTime LastTimeCalledDateAndTime { get; set; }

    }
}