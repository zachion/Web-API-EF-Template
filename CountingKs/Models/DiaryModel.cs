using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CountingKs.Data.Entities;

namespace CountingKs.Models
{
    public class DiaryModel
    {
        public string Url {get; set; }
        public DateTime CurrentDate { get; set; }
        //public IEnumerable<DiaryEntryModel> Entries { get; set; }

    }
}