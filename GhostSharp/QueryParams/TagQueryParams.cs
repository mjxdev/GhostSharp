﻿using GhostSharp.Enums;
using System;
using System.Collections.Generic;

namespace GhostSharp.QueryParams
{
    /// <summary>
    /// Query parameters that modify the payload that's returned, including
    /// limiting to or filtering by certain fields, including extra data, etc.
    /// </summary>
    public class TagQueryParams
    {
        /// <summary>
        /// Include a count of posts associated with the tag.
        /// </summary>
        public bool IncludePostCount { get; set; }

        /// <summary>
        /// Choose which resource fields you want to retrieve values for.
        /// </summary>
        /// <value>The resource fields to retrieve values for.</value>
        public TagFields Fields { get; set; }

        /// <summary>
        /// A filter string using NQL syntax.
        /// </summary>
        /// <example>
        /// See documentation: https://docs.ghost.org/api/content/#filtering
        /// </example>
        public string Filter { get; set; }

        /// <summary>
        /// How many tags to retrieve. (default is 15)
        /// </summary>
        /// <value>The number of tags to retrieve.</value>
        public int Limit { get; set; }

        /// <summary>
        /// Return all tags (no limit) - use carefully! If set to True, then Limit is ignored.
        /// </summary>
        public bool NoLimit { get; set; }

        /// <summary>
        /// Which page of paginated results to retrieve. (default is 1)
        /// </summary>
        /// <value>The page of results to retrieve.</value>
        public int Page { get; set; }

        /// <summary>
        /// A list of fields to order by and direction of ordering.
        /// </summary>
        /// <value>The field(s) to order by and direction. (default is "name ASC")</value>
        public List<Tuple<TagFields, OrderDirection>> Order { get; set; }
    }
}
