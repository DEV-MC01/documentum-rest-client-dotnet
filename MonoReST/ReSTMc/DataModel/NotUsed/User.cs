﻿using Emc.Documentum.Rest.Net;
using Emc.Documentum.Rest.Http.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emc.Documentum.Rest.DataModel
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [DataContract(Name = "user", Namespace = "http://identifiers.emc.com/vocab/documentum")] 
    public partial class User : PersistentObject
    {
        /// <summary>
        /// Get the home cabinet (default folder) resource of the user
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Folder GetHomeCabinet(SingleGetOptions options)
        {
            return Client.GetSingleton<Folder>(this.Links, LinkUtil.DEFAULT_FOLDER.Rel, options);
        }

        /// <summary>
        /// Get the groups feed of which this user is a member
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public Feed<T> GetParentGroups<T>(FeedGetOptions options)
        {
            return Client.GetFeed<T>(
                this.Links,
                LinkUtil.PARENT.Rel,
                options);
        }
    }
}
