using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emc.Documentum.Rest
{

    public class Rootobject
    {
        public string id { get; set; }
        public string title { get; set; }
        public DateTime updated { get; set; }
        public int total { get; set; }
        public int itemsperpage { get; set; }
        public int page { get; set; }
        public Author[] author { get; set; }
        public Entry[] entries { get; set; }
        public Link[] links { get; set; }
    }

    public class Author
    {
        public string name { get; set; }
    }

    public class Entry
    {
        public string id { get; set; }
        public string title { get; set; }
        public DateTime updated { get; set; }
        public object[] author { get; set; }
        public Content content { get; set; }
        public float score { get; set; }
    }

    public class Content
    {
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public string arctic_document_class { get; set; }
        public string arctic_document_title_eng { get; set; }
        public string arctic_document_title { get; set; }
        public string sib_doc_type_code { get; set; }
        public string sib_discipline_code { get; set; }
        public string sib_contr_transm_inc_number { get; set; }
        public string eif_issue_reason { get; set; }
        public string eif_acceptance_code { get; set; }
        public string arctic_mark_stamp { get; set; }
        public string eif_revision { get; set; }
        public string sib_unit_title { get; set; }
        public string r_object_id { get; set; }
        public string object_name { get; set; }
        public string ctrm_number { get; set; }
        public string ctrm_stage { get; set; }
        public string itrm_number { get; set; }
        public string itrm_stage { get; set; }
        public string ctrm_category { get; set; }
        public string itrm_category { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }


}
