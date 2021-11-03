using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emc.Documentum.Rest.DocClass
{
    public class Properties
    {
        public string object_name { get; set; }
        public string r_object_id { get; set; }
        public string title { get; set; }
        public string eif_date_due { get; set; }
        public string eif_date_completed { get; set; }
        public string eif_alt_doc_number { get; set; }
        public string eif_historic_state { get; set; }
        public string eif_revision { get; set; }
        public string eif_issue_reason { get; set; }
        public string eif_discipline { get; set; }
        public string eif_responsible { get; set; }
        public string eif_originator { get; set; }
        public string eif_type_of_doc { get; set; }
        public string eif_contract_number { get; set; }
        public string sib_revision_date { get; set; }
        public string sib_language { get; set; }
        public string sib_unit_title { get; set; }
        public string sib_vendor { get; set; }
        public string sib_doc_type_code { get; set; }
        public string sib_discipline_code { get; set; }
        public string arctic_area_code { get; set; }
        public string arctic_document_class { get; set; }
        public string arctic_number { get; set; }
        public string arctic_document_title { get; set; }
        public string arctic_mark_stamp { get; set; }
        public string eif_acceptance_code { get; set; }
        public int sib_building_seq_number { get; set; }
        public string sib_construction_area { get; set; }
        public string sib_deliverable_reference { get; set; }
        public string arctic_document_title_eng { get; set; }
        public string sib_customer { get; set; }
        public string sib_plant_unit { get; set; }
        public string sib_name_project_nipi { get; set; }
        public string sib_key { get; set; }
        public string sib_status { get; set; }
        public string sib_purchase_claim_number { get; set; }
        public string sib_name_acrs { get; set; }
        public string sib_restriction_level { get; set; }
        public string sib_restriction_modify_date { get; set; }
        public string r_creation_date { get; set; }
        public string r_modify_date { get; set; }
    }

    public class CP_Document
    {
        public string definition { get; set; }
        public Properties properties { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 


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

            public string r_object_id { get; set; }
            public string object_name { get; set; }
            public string title { get; set; }
            public string subject { get; set; }
            public string ecs_lc_state { get; set; }
            public string eif_date_completed { get; set; }
            public string eif_date_due { get; set; }
            public string eif_date_received { get; set; }
            public string eif_date_sent { get; set; }
            public string ecs_last_chronicle_id { get; set; }
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
        

    }

    public class TrmClass
    {
        public string type { get; set; }
        public string definition { get; set; }
        public Properties properties { get; set; }
    }


}