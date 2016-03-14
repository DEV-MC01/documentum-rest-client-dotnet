﻿using Emc.Documentum.Rest.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emc.Documentum.Rest.Http.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class AtomUtil
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static string ID = "id";

        public static string TITLE = "title";
        public static string SUMMARY = "summary";
        public static string AUTHOR = "author";
        public static string UPDATED = "updated";
        public static string PUBLISHED = "published";
        public static string ENTRIES = "entries";
        public static string LINKS = "links";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="feed"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string FindEntryHref<T>(Feed<T> feed, string title)
        {
            string href = null;
            List<Entry<T>> entries = feed.Entries;
            if (entries != null)
            {
                foreach (Entry<T> entry in entries)
                {
                    if (title.Equals(entry.Title) && entry.Content is OutlineAtomContent)
                    {
                        OutlineAtomContent ct = entry.Content as OutlineAtomContent;
                        href = ct.Src;
                        if (href == null)
                        {
                            href = LinkUtil.FindLinkAsString(entry.Links, LinkUtil.SELF.Rel);
                        }
                        break;
                    }
                }
            }
            return href;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="feed"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static T FindInlineEntry<T>(Feed<T> feed, string title)
        {
            T inlineContent = default (T);
            List<Entry<T>> entries = feed.Entries;
            if (entries != null)
            {
                foreach (Entry<T> entry in entries)
                {
                    if (title.Equals(entry.Title))
                    {
                        inlineContent = entry.Content;
                        break;
                    }
                }
            }
            return inlineContent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="feed"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static T FindInlineEntryBySummary<T>(Feed<T> feed, string title)
        {
            T inlineContent = default(T);
            List<Entry<T>> entries = feed.Entries;
            if (entries != null)
            {
                foreach (Entry<T> entry in entries)
                {
                    if (title.Equals(entry.Summary))
                    {
                        inlineContent = entry.Content;
                        break;
                    }
                }
            }
            return inlineContent;
        }
    }
}
