using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emc.Documentum.Rest
{
    #region old class implementation
    //public class TrmRootobject
    //{
    //    public string id { get; set; }
    //    public string title { get; set; }
    //    public DateTime updated { get; set; }
    //    public int total { get; set; }
    //    public int itemsperpage { get; set; }
    //    public int page { get; set; }
    //    public TrmAuthor[] author { get; set; }
    //    public TrmEntry[] entries { get; set; }
    //    public TrmLink2[] links { get; set; }
    //}

    //public class TrmAuthor
    //{
    //    public string name { get; set; }
    //}

    //public class TrmEntry
    //{
    //    public string id { get; set; }
    //    public string title { get; set; }
    //    public DateTime updated { get; set; }
    //    public object[] author { get; set; }
    //    public TrmContent content { get; set; }
    //    public float score { get; set; }
    //    public TrmLink1[] links { get; set; }
    //}

    //public class TrmContent
    //{
    //    public string type { get; set; }
    //    public string definition { get; set; }
    //    public TrmProperties properties { get; set; }
    //    public TrmLink[] links { get; set; }
    //}

    //public class TrmProperties
    //{
    //    public string r_object_id { get; set; }
    //    public string object_name { get; set; }
    //    public string title { get; set; }
    //    public string subject { get; set; }
    //    public string resolution_label { get; set; }
    //    public string owner_name { get; set; }
    //    public int owner_permit { get; set; }
    //    public string group_name { get; set; }
    //    public int group_permit { get; set; }
    //    public int world_permit { get; set; }
    //    public string log_entry { get; set; }
    //    public string acl_domain { get; set; }
    //    public string acl_name { get; set; }
    //    public string language_code { get; set; }
    //    public string ecs_doc_type { get; set; }
    //    public int ecs_template_level { get; set; }
    //    public string ecs_security_profile { get; set; }
    //    public string ecs_ref_no { get; set; }
    //    public string ecs_lc_name { get; set; }
    //    public string ecs_lc_state { get; set; }
    //    public int ecs_bypass_tbo { get; set; }
    //    public string ecs_last_chronicle_id { get; set; }
    //    public string ecs_last_object_id { get; set; }
    //    public string ecs_last_security_hash { get; set; }
    //    public string ecs_last_lc_state { get; set; }
    //    public string ecs_last_primary_fdr_id { get; set; }
    //    public object ecs_content_modify_date { get; set; }
    //    public string ecs_content_modifier { get; set; }
    //    public string ecs_last_contents_id { get; set; }
    //    public string ecs_internal_ref { get; set; }
    //    public string ecs_doc_type_name { get; set; }
    //    public int ecs_numbering_applied { get; set; }
    //    public int ecs_manual_seq_num { get; set; }
    //    public string ecs_id_number { get; set; }
    //    public string ecs_internal_chron_ref { get; set; }
    //    public int ecs_is_creation_finished { get; set; }
    //    public string ecs_activation_status { get; set; }
    //    public string eif_issue_reason { get; set; }
    //    public string eif_notes { get; set; }
    //    public object eif_date_due { get; set; }
    //    public object eif_date_completed { get; set; }
    //    public int eif_to_ack { get; set; }
    //    public object eif_date_ack { get; set; }
    //    public object eif_date_sent { get; set; }
    //    public object eif_date_received { get; set; }
    //    public string eif_project_ref { get; set; }
    //    public string eif_from { get; set; }
    //    public string eif_to { get; set; }
    //    public string eif_include_cmt_statuses { get; set; }
    //    public int eif_include_cmts { get; set; }
    //    public int eif_include_doc_content { get; set; }
    //    public string eif_tr_category { get; set; }
    //    public int eif_review_flow { get; set; }
    //    public int eif_auto_forward { get; set; }
    //    public string eif_handover_target { get; set; }
    //    public string sib_is_pem_exist { get; set; }
    //    public string sib_sender_email { get; set; }
    //    public string sib_sender { get; set; }
    //    public string sib_subproject_code { get; set; }
    //    public string sib_contr_transm_number { get; set; }
    //    public string sib_sent_to_contractor { get; set; }
    //    public string sib_tr_class_code { get; set; }
    //    public string sib_delivery_type { get; set; }
    //    public int sib_hard_copy_quantity { get; set; }
    //    public int sib_disc_copy_quantity { get; set; }
    //    public string sib_tr_area_code { get; set; }
    //    public string sib_tr_stage { get; set; }
    //    public int sib_number_of_copies { get; set; }
    //    public string sib_name_project_nipi { get; set; }
    //    public string sib_set_type_number { get; set; }
    //    public string sib_discipline_code { get; set; }
    //    public string sib_deliverable_reference { get; set; }
    //    public string sib_contract_number { get; set; }
    //    public string eif_revision { get; set; }
    //    public string sib_document_title { get; set; }
    //    public object sib_processing_date { get; set; }
    //    public string sib_remark { get; set; }
    //    public object sib_revision_date { get; set; }
    //    public string sib_stage { get; set; }
    //    public string sib_status { get; set; }
    //    public string sib_title { get; set; }
    //    public int sib_external_approve { get; set; }
    //    public object sib_external_approve_date { get; set; }
    //    public int sib_is_locked { get; set; }
    //    public object sib_lock_date { get; set; }
    //    public string sib_loading_config_name { get; set; }
    //    public int sib_send_crs { get; set; }
    //    public string sib_customer_project_number { get; set; }
    //    public string eif_template_name { get; set; }
    //    public string eif_template_r_object_id { get; set; }
    //    public int eif_to_respond { get; set; }
    //    public string sib_vendor { get; set; }
    //    public string eif_transfer_id { get; set; }
    //    public string eif_transfer_state { get; set; }
    //    public string sib_chief_project_engineer { get; set; }
    //    public string sib_cn_planning_engineer { get; set; }
    //    public string sib_project_class { get; set; }
    //    public string r_object_type { get; set; }
    //    public object r_creation_date { get; set; }
    //    public object r_modify_date { get; set; }
    //    public string a_content_type { get; set; }
    //}

    //public class TrmLink
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    //public class TrmLink1
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    //public class TrmLink2
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    //public class DocRootobject
    //{
    //    public string id { get; set; }
    //    public string title { get; set; }
    //    public DateTime updated { get; set; }
    //    public int total { get; set; }
    //    public int itemsperpage { get; set; }
    //    public int page { get; set; }
    //    public DocAuthor[] author { get; set; }
    //    public DocEntry[] entries { get; set; }
    //    public TrmLink2[] links { get; set; }
    //}

    //public class DocAuthor
    //{
    //    public string name { get; set; }
    //}

    //public class DocEntry
    //{
    //    public string id { get; set; }
    //    public string title { get; set; }
    //    public DateTime updated { get; set; }
    //    public object[] author { get; set; }
    //    public DocContent content { get; set; }
    //    public float score { get; set; }
    //    public DocLink1[] links { get; set; }
    //}

    //public class DocContent
    //{
    //    public string type { get; set; }
    //    public string definition { get; set; }
    //    public DocProperties properties { get; set; }
    //    public DocLink[] links { get; set; }
    //}

    //public class DocProperties
    //{
    //    public string r_object_id { get; set; }
    //    public string object_name { get; set; }
    //    public string title { get; set; }
    //    public string subject { get; set; }
    //    public string resolution_label { get; set; }
    //    public string owner_name { get; set; }
    //    public int owner_permit { get; set; }
    //    public string group_name { get; set; }
    //    public int group_permit { get; set; }
    //    public int world_permit { get; set; }
    //    public string log_entry { get; set; }
    //    public string acl_domain { get; set; }
    //    public string acl_name { get; set; }
    //    public string language_code { get; set; }
    //    public string ecs_doc_type { get; set; }
    //    public int ecs_template_level { get; set; }
    //    public string ecs_security_profile { get; set; }
    //    public string ecs_ref_no { get; set; }
    //    public string ecs_lc_name { get; set; }
    //    public string ecs_lc_state { get; set; }
    //    public int ecs_bypass_tbo { get; set; }
    //    public string ecs_last_chronicle_id { get; set; }
    //    public string ecs_last_object_id { get; set; }
    //    public string ecs_last_security_hash { get; set; }
    //    public string ecs_last_lc_state { get; set; }
    //    public string ecs_last_primary_fdr_id { get; set; }
    //    public object ecs_content_modify_date { get; set; }
    //    public string ecs_content_modifier { get; set; }
    //    public string ecs_last_contents_id { get; set; }
    //    public string ecs_internal_ref { get; set; }
    //    public string ecs_doc_type_name { get; set; }
    //    public int ecs_numbering_applied { get; set; }
    //    public int ecs_manual_seq_num { get; set; }
    //    public string ecs_id_number { get; set; }
    //    public string ecs_internal_chron_ref { get; set; }
    //    public int ecs_is_creation_finished { get; set; }
    //    public string ecs_activation_status { get; set; }
    //    public string eif_from { get; set; }
    //    public string eif_to { get; set; }
    //    public object eif_date_due { get; set; }
    //    public object eif_date_completed { get; set; }
    //    public string eif_acceptance_code { get; set; }
    //    public string eif_function { get; set; }
    //    public string eif_asset { get; set; }
    //    public string eif_area { get; set; }
    //    public string eif_system { get; set; }
    //    public string eif_subsystem { get; set; }
    //    public string eif_alt_doc_number { get; set; }
    //    public int eif_is_latest_rev_version { get; set; }
    //    public int eif_is_formally_issued { get; set; }
    //    public string eif_historic_state { get; set; }
    //    public string eif_revision { get; set; }
    //    public string eif_issue_reason { get; set; }
    //    public string eif_discipline { get; set; }
    //    public string eif_responsible { get; set; }
    //    public string eif_originator { get; set; }
    //    public string eif_type_of_doc { get; set; }
    //    public string eif_prepared_by { get; set; }
    //    public object eif_prepared_on { get; set; }
    //    public string eif_verified_by { get; set; }
    //    public object eif_verified_on { get; set; }
    //    public string eif_approved_by { get; set; }
    //    public object eif_approved_on { get; set; }
    //    public string eif_superseded_by { get; set; }
    //    public string eif_project_ref { get; set; }
    //    public int eif_is_only_rev { get; set; }
    //    public object eif_handover_planned_date { get; set; }
    //    public object eif_handover_actual_date { get; set; }
    //    public string eif_contract_number { get; set; }
    //    public string eif_facility { get; set; }
    //    public string eif_responded_by { get; set; }
    //    public string eif_related_object { get; set; }
    //    public string eif_email_from { get; set; }
    //    public object eif_email_date { get; set; }
    //    public string eif_message_id { get; set; }
    //    public object sib_revision_date { get; set; }
    //    public string sib_section { get; set; }
    //    public string sib_subsection { get; set; }
    //    public string sib_language { get; set; }
    //    public string sib_phase { get; set; }
    //    public string sib_subphase { get; set; }
    //    public int sib_number_of_sheets { get; set; }
    //    public string sib_subcontractor { get; set; }
    //    public string sib_doc_class_code { get; set; }
    //    public string sib_unit_title { get; set; }
    //    public string sib_unit_title_en { get; set; }
    //    public string sib_type_of_work { get; set; }
    //    public int sib_number_of_copies { get; set; }
    //    public string sib_construction_type { get; set; }
    //    public int sib_sheet_number { get; set; }
    //    public string sib_arcs_status { get; set; }
    //    public string sib_vendor { get; set; }
    //    public string sib_set_type { get; set; }
    //    public string sib_set_type_rus { get; set; }
    //    public string sib_set_type_seq { get; set; }
    //    public string sib_set_type_number { get; set; }
    //    public string sib_operation_complex { get; set; }
    //    public string sib_operation_complex_en { get; set; }
    //    public string sib_startup_complex { get; set; }
    //    public string sib_startup_complex_en { get; set; }
    //    public string sib_unit { get; set; }
    //    public string sib_unit_en { get; set; }
    //    public string sib_discipline_en { get; set; }
    //    public string sib_subproject_code { get; set; }
    //    public string sib_title_en { get; set; }
    //    public string sib_type_of_doc { get; set; }
    //    public string sib_acrs { get; set; }
    //    public string sib_linde_unit { get; set; }
    //    public string sib_linde_area { get; set; }
    //    public string sib_linde_object { get; set; }
    //    public string sib_linde_discipline { get; set; }
    //    public string sib_linde_discipline_rus { get; set; }
    //    public string sib_linde_unit_rus { get; set; }
    //    public string sib_linde_unit_en { get; set; }
    //    public string sib_linde_area_rus { get; set; }
    //    public string sib_linde_area_en { get; set; }
    //    public string sib_doc_type_code { get; set; }
    //    public string sib_discipline_code { get; set; }
    //    public string sib_is_stamped { get; set; }
    //    public string sib_linde_doc_type_code { get; set; }
    //    public string sib_linde_type_of_doc_en { get; set; }
    //    public string sib_linde_type_of_doc_rus { get; set; }
    //    public string sib_contr_transm_inc_number { get; set; }
    //    public string sib_po_vendor { get; set; }
    //    public string sib_package_revision { get; set; }
    //    public string sib_revised_crs_code { get; set; }
    //    public object sib_date_revised_crs_code { get; set; }
    //    public string arctic_stage { get; set; }
    //    public string arctic_originator_number { get; set; }
    //    public string arctic_company_number { get; set; }
    //    public string arctic_area_code { get; set; }
    //    public string arctic_document_class { get; set; }
    //    public string arctic_branch { get; set; }
    //    public string arctic_ii_code_type { get; set; }
    //    public string arctic_part { get; set; }
    //    public string arctic_volume { get; set; }
    //    public string arctic_approve_stamp { get; set; }
    //    public object arctic_stamp_date { get; set; }
    //    public string arctic_number { get; set; }
    //    public string arctic_document_title { get; set; }
    //    public string arctic_document_title_eng { get; set; }
    //    public string arctic_gost_number { get; set; }
    //    public object sib_contr_transm_inc_date { get; set; }
    //    public string arctic_mark_stamp { get; set; }
    //    public string sib_construction_area { get; set; }
    //    public string sib_customer { get; set; }
    //    public string sib_num_code_design { get; set; }
    //    public string sib_originator_type { get; set; }
    //    public string sib_plant_unit { get; set; }
    //    public string sib_originator_book_number { get; set; }
    //    public string sib_package_doc_label { get; set; }
    //    public object sib_plan_date { get; set; }
    //    public string sib_book { get; set; }
    //    public string sib_equip_type { get; set; }
    //    public string sib_order_code { get; set; }
    //    public string sib_name_project_nipi { get; set; }
    //    public string sib_deliverable_reference { get; set; }
    //    public string sib_purchase_type { get; set; }
    //    public string sib_equip_item { get; set; }
    //    public string sib_power { get; set; }
    //    public object sib_adoption_date { get; set; }
    //    public string sib_book_key { get; set; }
    //    public float sib_book_weight { get; set; }
    //    public string sib_condition { get; set; }
    //    public string sib_department { get; set; }
    //    public int sib_dlv_from_mdr_feed { get; set; }
    //    public string sib_doc_pd_key { get; set; }
    //    public float sib_document_weight { get; set; }
    //    public object sib_finished_date { get; set; }
    //    public object sib_forecast_plan_100 { get; set; }
    //    public object sib_forecast_plan_5 { get; set; }
    //    public object sib_forecast_plan_60 { get; set; }
    //    public string sib_from { get; set; }
    //    public object sib_issue_fact_date { get; set; }
    //    public object sib_issue_plan_100 { get; set; }
    //    public object sib_issue_plan_5 { get; set; }
    //    public object sib_issue_plan_60 { get; set; }
    //    public object sib_issue_plan_date { get; set; }
    //    public string sib_issue_reason_task { get; set; }
    //    public string sib_key { get; set; }
    //    public int sib_key_task { get; set; }
    //    public string sib_key1 { get; set; }
    //    public string sib_lc_manual_seq_num { get; set; }
    //    public string sib_notes { get; set; }
    //    public int sib_preliminary { get; set; }
    //    public object sib_prepare_date { get; set; }
    //    public string sib_progress_discipline { get; set; }
    //    public string sib_progress_form { get; set; }
    //    public string sib_remark { get; set; }
    //    public string sib_responsible { get; set; }
    //    public string sib_responsible_deputy_pm { get; set; }
    //    public string sib_responsible_stage_pd { get; set; }
    //    public int sib_send_confirm { get; set; }
    //    public object sib_send_confirm_date { get; set; }
    //    public object sib_start_fact_date { get; set; }
    //    public string sib_status { get; set; }
    //    public string sib_task_title { get; set; }
    //    public object sib_transmittal_sheet_date { get; set; }
    //    public string sib_transmittal_sheet_num { get; set; }
    //    public float sib_weight { get; set; }
    //    public string sib_doc_dlv_feed_key { get; set; }
    //    public int sib_is_hard_copy { get; set; }
    //    public string sib_factual_storage { get; set; }
    //    public string sib_purchase_coordinator { get; set; }
    //    public object sib_tb_receipt_plan_date { get; set; }
    //    public object sib_tb_receipt_fact_date { get; set; }
    //    public object sib_tb_answer_plan_date { get; set; }
    //    public object sib_tb_answer_fact_date { get; set; }
    //    public object sib_vd_receipt_plan_date { get; set; }
    //    public object sib_vd_receipt_fact_date { get; set; }
    //    public object sib_vd_answer_plan_date { get; set; }
    //    public object sib_vd_answer_fact_date { get; set; }
    //    public string sib_object_code { get; set; }
    //    public string sib_equip_code { get; set; }
    //    public string sib_purchase_stage { get; set; }
    //    public string sib_lead_department { get; set; }
    //    public object sib_mto_transfer_plan_date { get; set; }
    //    public object sib_mto_transfer_fact_date { get; set; }
    //    public object sib_mto_answer_plan_date { get; set; }
    //    public object sib_mto_answer_fact_date { get; set; }
    //    public string sib_inv_number { get; set; }
    //    public string sib_main_constr_contractor { get; set; }
    //    public string sib_chapter_of_estimate { get; set; }
    //    public string sib_estimate_number { get; set; }
    //    public string sib_lot { get; set; }
    //    public string sib_estimated_cost { get; set; }
    //    public string sib_elabor_claim_number { get; set; }
    //    public object sib_ksg_delivery_date { get; set; }
    //    public string sib_purchase_claim_number { get; set; }
    //    public string sib_respons_purchaser_mto { get; set; }
    //    public string tq_doc_revision { get; set; }
    //    public string tq_precomissioning { get; set; }
    //    public string tq_submitted_purpose { get; set; }
    //    public string tq_priority { get; set; }
    //    public string tq_prior_justification { get; set; }
    //    public string tq_query_classification { get; set; }
    //    public string tq_response_type { get; set; }
    //    public string tq_contract_number { get; set; }
    //    public string tq_state { get; set; }
    //    public object tq_newrev_date { get; set; }
    //    public string tq_expense_class_add { get; set; }
    //    public string tq_expense_class { get; set; }
    //    public string tq_expense_amount { get; set; }
    //    public string sib_subphase_name { get; set; }
    //    public object tq_responded_date { get; set; }
    //    public object tq_approved_date { get; set; }
    //    public object tq_client_approved_date { get; set; }
    //    public string tq_contractor { get; set; }
    //    public string tq_review_result { get; set; }
    //    public string tq_introduction_changes { get; set; }
    //    public string tq_responsible_type { get; set; }
    //    public string sib_contractor_code { get; set; }
    //    public string sib_rfp_rfq_package_number { get; set; }
    //    public string sib_plan_status_name { get; set; }
    //    public int sib_plan_status_code { get; set; }
    //    public string sib_set_type_code { get; set; }
    //    public string sib_labor_expenditures { get; set; }
    //    public string sib_pd_book_name { get; set; }
    //    public int sib_chain_index { get; set; }
    //    public string sib_chain_prev { get; set; }
    //    public int sib_chain_task { get; set; }
    //    public int sib_chain_last { get; set; }
    //    public string sib_to { get; set; }
    //    public string sib_work_order { get; set; }
    //    public string sib_customer_project_number { get; set; }
    //    public string sib_name_acrs { get; set; }
    //    public int sib_issue_pln_date_accepted { get; set; }
    //    public int sib_doc_key { get; set; }
    //    public int sib_can_modify_current { get; set; }
    //    public string sib_rendition_format_str { get; set; }
    //    public string sib_rendition_str { get; set; }
    //    public int sib_rendition_update { get; set; }
    //    public int sib_number_of_sheets_a4 { get; set; }
    //    public string sib_object_name { get; set; }
    //    public string sib_building_type { get; set; }
    //    public int sib_building_seq_number { get; set; }
    //    public string sib_hold { get; set; }
    //    public string sib_previous_doc_number { get; set; }
    //    public object sib_stamp_date { get; set; }
    //    public string dss_sib_constr_status { get; set; }
    //    public string sib_subunit_title { get; set; }
    //    public string sib_change_number { get; set; }
    //    public object sib_change_date { get; set; }
    //    public string sib_originator_name { get; set; }
    //    public string sib_subsystem { get; set; }
    //    public string eif_transfer_id { get; set; }
    //    public string eif_transfer_state { get; set; }
    //    public string sib_archived_condition { get; set; }
    //    public string sib_status_before_trm_start { get; set; }
    //    public string sib_building_code { get; set; }
    //    public string sib_alternate_name { get; set; }
    //    public string sib_pack_sign { get; set; }
    //    public object sib_sign_date { get; set; }
    //    public string sib_hash { get; set; }
    //    public string sib_verify_status { get; set; }
    //    public string r_object_type { get; set; }
    //    public object r_creation_date { get; set; }
    //    public object r_modify_date { get; set; }
    //    public string a_content_type { get; set; }
    //}

    //public class DocLink
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    //public class DocLink1
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}
    #endregion

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

        public string sib_key { get; set; }
        public string sib_construction_area { get; set; }
        public string eif_contract_number { get; set; }
        public string sib_deliverable_reference { get; set; }
        public string sib_discipline_code { get; set; }
        public int sib_building_seq_number { get; set; }
        public string arctic_number { get; set; }
        public string arctic_document_class { get; set; }
        public string sib_doc_type_code { get; set; }
        public string arctic_document_title_eng { get; set; }
        public object sib_contr_transm_inc_date { get; set; }
        public string sib_contr_transm_inc_number { get; set; }
        public string eif_issue_reason { get; set; }
        public string sib_language { get; set; }
        public string arctic_area_code { get; set; }
        public string eif_acceptance_code { get; set; }
        public string arctic_mark_stamp { get; set; }
        public string sib_purchase_claim_number { get; set; }
        public string eif_alt_doc_number { get; set; }
        public string eif_revision { get; set; }
        public object sib_revision_date
        {
            get; set;
            //get { return sib_revision_date; }
            //set
            //{
            //    try
            //    {
            //        DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //        sib_revision_date = startDate.AddSeconds(Double.Parse(value.ToString())).ToString();

            //    }
            //    catch (Exception)
            //    {

            //        sib_revision_date = "";
            //    }
            //}
        }
        public string arctic_document_title { get; set; }
        public string eif_originator { get; set; }
        public string sib_unit_title { get; set; }
        public string sib_vendor { get; set; }
        public string r_object_id { get; set; }
        public string transmittalnumber { get; set; }
        public string object_name { get; set; }
        public string title { get; set; }
        public string dm_attr_0030 { get; set; }
        public string sib_tr_stage { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }


}
