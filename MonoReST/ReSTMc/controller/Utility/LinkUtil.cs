﻿using Emc.Documentum.Rest.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emc.Documentum.Rest.Http.Utility
{

    /// <summary>
    /// Class used for resolving document action links
    /// </summary>
    public class LinkUtil
    {
        /// <summary>
        /// The location to append to the repositoryBaseUri for the mail import service
        /// </summary>
        public static string EMAILIMPORT = "/mailimport";
        /// <summary>
        /// The location to append to the repositoryBaseUri for the retention event date service
        /// </summary>
        public static string RETENTION = "/retention";
        /// <summary>
        /// The relection/self link of the current object
        /// </summary>
        public static LinkUtil SELF = new LinkUtil("self");
        /// <summary>
        /// The link used to post changes to the object
        /// </summary>
        public static LinkUtil EDIT = new LinkUtil("edit");
        /// <summary>
        /// The object used to delete an object
        /// </summary>
        public static LinkUtil DELETE = new LinkUtil("delete", true);
        /// <summary>
        /// The link used to return the document/object content
        /// </summary>
        public static LinkUtil CONTENTS = new LinkUtil("contents");
        /// <summary>
        /// Link used to return a list of versions
        /// </summary>
        public static LinkUtil VERSIONS = new LinkUtil("version-history");

        public static LinkUtil PARENT = new LinkUtil("parent");
        public static LinkUtil ALTERNATE = new LinkUtil("alternate");
        public static LinkUtil PAGING_NEXT = new LinkUtil("next");
        public static LinkUtil PAGING_PREV = new LinkUtil("previous");
        public static LinkUtil PAGING_FIRST = new LinkUtil("first");
        public static LinkUtil PAGING_LAST = new LinkUtil("last");
        public static LinkUtil ENCLOSURE = new LinkUtil("enclosure");
        public static LinkUtil CANONICAL = new LinkUtil("canonical");
        public static LinkUtil PREDECESSOR_VERSION = new LinkUtil("predecessor-version");
        public static LinkUtil TYPE = new LinkUtil("type");
        public static LinkUtil TYPES = new LinkUtil("types", true);

        // Documentum specific link relations
        public static LinkUtil FOLDERS = new LinkUtil("folders", true);
        public static LinkUtil DOCUMENTS = new LinkUtil("documents", true);
        public static LinkUtil OBJECTS = new LinkUtil("objects", true);
        public static LinkUtil CABINET = new LinkUtil("cabinet", true);
        public static LinkUtil CONTENT_MEDIA = new LinkUtil("content-media", true);
        public static LinkUtil PRIMARY_CONTENT = new LinkUtil("primary-content", true);
        public static LinkUtil CHECKED_OUT_OBJECTS = new LinkUtil("checked-out-objects", true);
        public static LinkUtil REPOSITORIES = new LinkUtil("repositories", true);
        public static LinkUtil ABOUT = new LinkUtil("about", false);
        public static LinkUtil RELATIONS = new LinkUtil("relations", true);    
        public static LinkUtil USERS = new LinkUtil("users", true);
        public static LinkUtil USER = new LinkUtil("user", true);
        public static LinkUtil GROUPS = new LinkUtil("groups", true);
        public static LinkUtil CURRENT_USER = new LinkUtil("current-user", true);
        public static LinkUtil DEFAULT_FOLDER = new LinkUtil("default-folder", true);
        public static LinkUtil MODIFIER = new LinkUtil("modifier", true);
        public static LinkUtil CREATOR = new LinkUtil("creator", true);
        public static LinkUtil OWNER = new LinkUtil("owner", true);
        public static LinkUtil LOCK_OWNER = new LinkUtil("lock-owner", true); 
        public static LinkUtil FORMAT = new LinkUtil("format", true);
        public static LinkUtil CHECKIN_NEXT_MAJOR = new LinkUtil("checkin-next-major", true);
        public static LinkUtil CHECKIN_NEXT_MINOR = new LinkUtil("checkin-next-minor", true);
        public static LinkUtil CHECKIN_BRANCH_VERSION = new LinkUtil("checkin-branch", true);
        public static LinkUtil CHECKOUT = new LinkUtil("checkout", true);
        public static LinkUtil CANCEL_CHECKOUT = new LinkUtil("cancel-checkout", true);
        public static LinkUtil CURRENT_VERSION = new LinkUtil("current-version", true);
        public static LinkUtil OBJECT_ID = new LinkUtil("object-id", true);
        public static LinkUtil FOLDER_PATH = new LinkUtil("folder-path", true);
        public static LinkUtil FOLDER_CHILDREN = new LinkUtil("folder-children", true);
        public static LinkUtil DQL = new LinkUtil("dql", true);
        //Full text mjm
        public static LinkUtil SEARCH = new LinkUtil(@"search", true);
        public static LinkUtil CABINETS = new LinkUtil("cabinets", true);
        public static LinkUtil FORMATS = new LinkUtil("formats", true);
        public static LinkUtil NETWORK_LOCATIONS = new LinkUtil("network-locations", true);
        public static LinkUtil PARENT_LINKS = new LinkUtil("parent-links", true);
        public static LinkUtil CHILD_LINKS = new LinkUtil("child-links", true);
    
        private static string PREFIX = "http://identifiers.emc.com/linkrel/";
        private bool isDocumentum;
        private string rawRel;



        /// <summary>
        /// 
        /// </summary>
        public string Rel 
        {
            get
            {
                return isDocumentum ? (PREFIX + rawRel) : rawRel;
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rel"></param>
        private LinkUtil(string rel) {
            this.rawRel = rel;
            this.isDocumentum = false;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="isDocumentum"></param>
        private LinkUtil(string rel, bool isDocumentum) {
            this.rawRel = rel;
            this.isDocumentum = isDocumentum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="links"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public static string FindLinkAsString(List<Link> links, string rel)
        {
            if (links != null)
            {

                foreach(Link link in links)
                {
                    //if (links.Count == 1 && ! rel.Equals("self"))
                    //{
                    //    String service = "";
                    //    if (rel != null && !rel.Trim().Equals(""))
                    //    {
                    //        String[] relPieces = rel.Split(new char[] {'/'});
                    //        service = relPieces[relPieces.Length - 1];
                    //    }
                    //    if (!service.Equals(""))
                    //        return link.Href + "/" + service;
                    //    else
                    //        return link.Href;
                    //}

                   
                    //http://identifiers.emc.com/linkrel/checkout: http://localhost:8080/dctm-rest/repositories/Process/objects/xxxxxxxxxxxxxxxx/lock
                    if (link.Rel.StartsWith(rel)) 
                    {
                        return String.IsNullOrEmpty(link.Href) ? link.Hreftemplate : link.Href;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="links"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public static Link FindLink(List<Link> links, string rel)
        {
            Link retLink = null;
            if (links != null)
            {

                foreach (Link link in links)
                {
                    if (link.Rel.StartsWith(rel))
                    {
                        return link;
                    }
                }
                if (retLink == null)
                {
                    // At the repository level, link rel is the URL so we cannot find by REL name, we have to find it by 
                    // what the REL ENDS with
                    foreach (Link link in links)
                    {
                        if (link.Rel.EndsWith(rel))
                        {
                            retLink = link;
                            retLink.Rel = rel;
                            return retLink;
                        }
                    }
                }
                return retLink;
            }
            return null;
        }
    }
}
