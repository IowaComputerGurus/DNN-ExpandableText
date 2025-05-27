/*
 * Copyright (c) 2006-2024 IowaComputerGurus Inc (http://www.iowacomputergurus.com)
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
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search.Entities;

namespace ICG.Modules.ExpandableTextHtml.Components
{
    /// <summary>
    /// Provides DNN search integration for Expandable Text/HTML module using the ModuleSearchBase approach
    /// for DNN 10.x and beyond.
    /// </summary>
    public class ExpandableTextHtmlModuleSearchBase : ModuleSearchBase
    {
        /// <summary>
        /// Gets the search documents for the module.
        /// </summary>
        /// <param name="moduleInfo">The module information.</param>
        /// <param name="beginDateUtc">The begin date in UTC.</param>
        /// <returns>A collection of search documents.</returns>
        public override IList<SearchDocument> GetModifiedSearchDocuments(ModuleInfo moduleInfo, DateTime beginDateUtc)
        {
            var searchDocuments = new List<SearchDocument>();
            var controller = new ExpandableTextHtmlController();
            
            // Get all expandable text items for this module, ordered by last updated date
            var items = controller.GetExpandableTextHtmls(moduleInfo.ModuleID, "ORDER BY LastUpdated");
            
            foreach (var item in items)
            {
                // Only include items that have been updated since the begin date
                if (item.LastUpdated.ToUniversalTime() >= beginDateUtc)
                {
                    var searchDoc = new SearchDocument
                    {
                        UniqueKey = $"ETH_{moduleInfo.ModuleID}_{item.ItemId}",
                        PortalId = moduleInfo.PortalID,
                        TabId = moduleInfo.TabID,
                        ModuleId = moduleInfo.ModuleID,
                        ModuleDefId = moduleInfo.ModuleDefID,
                        Title = item.Title,
                        Body = item.Body,
                        Description = item.Title,
                        ModifiedTimeUtc = item.LastUpdated.ToUniversalTime(),
                        AuthorUserId = -1, // Default author as system since we don't track the author in this module
                        IsActive = true, // Assuming all items are active
                        CultureCode = moduleInfo.CultureCode,
                        Keywords = string.Empty // No specific keywords for this module
                    };
                    
                    searchDocuments.Add(searchDoc);
                }
            }
            
            return searchDocuments;
        }
    }
}