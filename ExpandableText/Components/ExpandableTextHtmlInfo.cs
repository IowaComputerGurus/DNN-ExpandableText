/*
 * Copyright (c) 2006-2012 IowaComputerGurus Inc (http://www.iowacomputergurus.com)
 * Copyright Contact: webmaster@iowacomputergurus.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights to use, 
 * copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial 
 * portions of the Software. 
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
 * NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE
 * */

using System;

namespace ICG.Modules.ExpandableTextHtml.Components
{
    /// <summary>
    /// Represents a single item of content within the module.
    /// </summary>
    public class ExpandableTextHtmlInfo
    {
        /// <summary>
        /// The id of the module that the entry is associated with
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// The id of a specific entry
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// The title of the entry, what will be clickable
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The body of the entry which will be displayed int he collapsable section
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The last updated date
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Should this module be shown as expanded?
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// This is a field that can be used for an admin to force
        /// and alternative sort order, rather than by title or by date
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>
        /// The publish date.
        /// </value>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the required role.
        /// </summary>
        /// <value>
        /// The required role.
        /// </value>
        public string RequiredRole { get; set; }
    }
}