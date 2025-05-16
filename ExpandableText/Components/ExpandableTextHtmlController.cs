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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace ICG.Modules.ExpandableTextHtml.Components
{
    /// <summary>
    /// This is the controller class providing functionality to the module
    /// </summary>
    public class ExpandableTextHtmlController : IPortable
    {
        #region Public Methods

        /// <summary>
        /// Gets all the ExpandableTextHtmlInfo objects for items matching the this moduleId
        /// </summary>
        /// <param name="moduleId">The id of the module that we are looking to get data on</param>
        /// <param name="orderBy">This is the order by clause, used for dynamic sorting</param>
        /// <returns>A listing of all entries</returns>
        public List<ExpandableTextHtmlInfo> GetExpandableTextHtmls(int moduleId, string orderBy)
        {
            return
                CBO.FillCollection<ExpandableTextHtmlInfo>(DataProvider.Instance().GetExpandableTextHtmls(moduleId,
                                                                                                          orderBy));
        }

        /// <summary>
        /// This method will get one specific entry from the database
        /// </summary>
        /// <param name="moduleId">the id of th emodule</param>
        /// <param name="itemId">The id of the item</param>
        /// <returns>The specific record</returns>
        public ExpandableTextHtmlInfo GetExpandableTextHtml(int moduleId, int itemId)
        {
            return
                CBO.FillObject<ExpandableTextHtmlInfo>(DataProvider.Instance().GetExpandableTextHtml(moduleId, itemId));
        }


        /// <summary>
        /// Adds a new ExpandableTextHtmlInfo object into the database
        /// </summary>
        /// <param name="info"></param>
        public void AddExpandableTextHtml(ExpandableTextHtmlInfo info)
        {
            DataProvider.Instance().AddExpandableTextHtml(info.ModuleId, info.Title, info.Body, info.LastUpdated,
                                                          info.IsExpanded, info.SortOrder, info.PublishDate,
                                                          info.RequiredRole);
        }

        /// <summary>
        /// update a info object already stored in the database
        /// </summary>
        /// <param name="info"></param>
        public void UpdateExpandableTextHtml(ExpandableTextHtmlInfo info)
        {
            DataProvider.Instance().UpdateExpandableTextHtml(info.ModuleId, info.ItemId, info.Title, info.Body,
                                                             info.LastUpdated, info.IsExpanded, info.SortOrder,
                                                             info.PublishDate, info.RequiredRole);
        }


        /// <summary>
        /// Delete a given item from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        public void DeleteExpandableTextHtml(int moduleId, int itemId)
        {
            DataProvider.Instance().DeleteExpandableTextHtml(moduleId, itemId);
        }

        #endregion

        //#region ISearchable Members

        ///// <summary>
        ///// Implements the search interface required to allow DNN to index/search the content of your
        ///// module
        ///// </summary>
        ///// <param name="modInfo"></param>
        ///// <returns></returns>
        //public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        //{
        //    var searchItems = new SearchItemInfoCollection();

        //    var infos = GetExpandableTextHtmls(modInfo.ModuleID, "ORDER BY LastUpdated");

        //    //Add each item from the contents into the search index
        //    foreach (var info in infos)
        //    {
        //        var searchInfo = new SearchItemInfo(modInfo.ModuleTitle, info.Title, 2, info.LastUpdated,
        //                                            modInfo.ModuleID, info.ItemId.ToString(), info.Body);
        //        searchItems.Add(searchInfo);
        //    }

        //    return searchItems;
        //}

        //#endregion

        #region IPortable Members

        ///// <summary>
        ///// Allows the module to export content 
        ///// </summary>
        ///// <param name="ModuleID">The id of the module that should be exported</param>
        ///// <returns>Formatted XML for the export</returns>
        public string ExportModule(int moduleID)
        {
            var sb = new StringBuilder();

            var infos = GetExpandableTextHtmls(moduleID, "ORDER BY title");

            if (infos.Count > 0)
            {
                sb.Append("<ExpandableTextHtmls>");
                foreach (var info in infos)
                {
                    sb.Append("<ExpandableTextHtml>");
                    sb.Append("<title>");
                    sb.Append(XmlUtils.XMLEncode(info.Title));
                    sb.Append("</title>");
                    sb.Append("<body>");
                    sb.Append(XmlUtils.XMLEncode(info.Body));
                    sb.Append("</body>");
                    sb.Append("<isExpanded>");
                    sb.Append(XmlUtils.XMLEncode(info.IsExpanded.ToString()));
                    sb.Append("</isExpanded>");
                    sb.Append("<sortOrder>");
                    sb.Append(XmlUtils.XMLEncode(info.SortOrder.ToString()));
                    sb.Append("</sortOrder>");
                    sb.Append("<publishDate>");
                    sb.Append(XmlUtils.XMLEncode(info.PublishDate.ToShortDateString()));
                    sb.Append("</publishDate>");
                    sb.Append("<requiredRole>");
                    sb.Append(XmlUtils.XMLEncode(info.RequiredRole));
                    sb.Append("</requiredRole>");
                    sb.Append("</ExpandableTextHtml>");
                }
                sb.Append("</ExpandableTextHtmls>");
            }

            return sb.ToString();
        }

        ///// <summary>
        ///// imports a module from an xml file
        ///// </summary>
        ///// <param name="ModuleID">The id of the module</param>
        ///// <param name="Content">The XML content</param>
        ///// <param name="Version"></param>
        ///// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            var infos = Globals.GetContent(Content, "ExpandableTextHtmls");

            //If no items found, short circuit
            if (infos == null)
                return;

            //Load each
            foreach (XmlNode info in infos.SelectNodes("ExpandableTextHtml"))
            {
                var oInfo = new ExpandableTextHtmlInfo
                                {
                                    ModuleId = ModuleID,
                                    Title = info.SelectSingleNode("title").InnerText,
                                    Body = info.SelectSingleNode("body").InnerText,
                                    IsExpanded =
                                        bool.Parse(info.SelectSingleNode("isExpanded").InnerText),
                                    SortOrder =
                                        int.Parse(info.SelectSingleNode("sortOrder").InnerText),
                                    PublishDate =
                                        DateTime.Parse(
                                            info.SelectSingleNode("publishDate").InnerText),
                                    LastUpdated = DateTime.Now,
                                    RequiredRole = info.SelectSingleNode("requiredRole").InnerText
                                };


                AddExpandableTextHtml(oInfo);
            }
        }

        #endregion
    }
}