using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apt_Erp_Lib
{
    public class NoteEntity
    {
        public int Aid { get; set; }
        public string Sort { get; set; }
        public string Asort { get; set; }

        public string AptCode { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }

        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
        public string NoteEncoding { get; set; }

        public DateTime PostDate { get; set; }
        public string PostID { get; set; }
        public DateTime EditDate { get; set; }
        public string EditIP { get; set; }

        public int ReadCount { get; set; }
        public int CommentCount { get; set; }
        public int Ref { get; set; }
        public int Step { get; set; }
        public int RefOrder { get; set; }
    }

    public class NoteLib
    {

    }
}
