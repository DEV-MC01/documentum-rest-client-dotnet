using System;
using System.Collections.Generic;

namespace Emc.Documentum.Rest.DocClass
{
    public class CP_Document
    {
        public string definition { get; set; }
        public IDictionary<string, string> properties { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
}

namespace Emc.Documentum.Rest.LinksClass
{
    public class Properties
    {
        public string r_object_id { get; set; }
        public string parent_id { get; set; }
        public string child_id { get; set; }
    }

    public class TRMtoDocLink
    {
        public Properties properties { get; set; }
    }

}

namespace Emc.Documentum.Rest.TRM_Class
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Properties
    {

        public string object_name { get; set; }
        public string subject { get; set; }
        public string ecs_lc_state { get; set; }
        public string ecs_last_chronicle_id { get; set; }
        public string eif_date_due { get; set; }
        public string eif_date_received { get; set; }
        public string eif_date_sent { get; set; }
        public string ecs_last_object_id { get; set; }
        public string ecs_last_security_hash { get; set; }
        public string ecs_last_lc_state { get; set; }
        public string ecs_last_primary_fdr_id { get; set; }
        public string ecs_content_modify_date { get; set; }
        public string ecs_content_modifier { get; set; }
        public string ecs_last_contents_id { get; set; }
        public string ecs_internal_ref { get; set; }
        public string ecs_doc_type_name { get; set; }
        public string ecs_id_number { get; set; }
        public string eif_issue_reason { get; set; }
        public string eif_notes { get; set; }
        public string eif_from { get; set; }
        public string eif_to { get; set; }
        public string eif_include_cmt_statuses { get; set; }
        public string eif_tr_category { get; set; }
        public string sib_sender { get; set; }
        public string sib_contr_transm_number { get; set; }
        public string sib_sent_to_contractor { get; set; }
        public string sib_tr_class_code { get; set; }
        public string sib_delivery_type { get; set; }
        public string sib_tr_stage { get; set; }
        public string sib_contract_number { get; set; }
        public string eif_revision { get; set; }
        public string r_object_type { get; set; }
        public string r_creation_date { get; set; }
        public string r_modify_date { get; set; }
        public string sib_tr_area_code { get; set; }
        public string r_object_id { get; set; }


    }

    public class TrmClass
    {
        public string type { get; set; }
        public string definition { get; set; }
        public Properties properties { get; set; }
    }
}