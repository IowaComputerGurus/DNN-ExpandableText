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
using System.Data;
using DotNetNuke.Framework;

namespace ICG.Modules.ExpandableTextHtml.Components
{
    public abstract class DataProvider
    {
        #region common methods

        /// <summary>
        /// var that is returned in the this singleton
        /// pattern
        /// </summary>
        private static readonly DataProvider instance;

        /// <summary>
        /// private static cstor that is used to init an
        /// instance of this class as a singleton
        /// </summary>
        static DataProvider()
        {
            instance = (DataProvider) Reflection.CreateObject("data", "ICG.Modules.ExpandableTextHtml.Components", "");
        }

        /// <summary>
        /// Exposes the singleton object used to access the database with
        /// the conrete dataprovider
        /// </summary>
        /// <returns></returns>
        public static DataProvider Instance()
        {
            return instance;
        }

        #endregion

        #region Abstract methods

        /* implement the methods that the dataprovider should */

        public abstract IDataReader GetExpandableTextHtmls(int moduleId, string orderBy);
        public abstract IDataReader GetExpandableTextHtml(int moduleId, int itemId);

        public abstract void AddExpandableTextHtml(int moduleId, string title, string body, DateTime lastUpdated,
                                                   bool isExpanded, int sortOrder, DateTime publishDate,
                                                   string requiredRole);

        public abstract void UpdateExpandableTextHtml(int moduleId, int itemId, string title, string body,
                                                      DateTime lastUpdated, bool isExpanded, int sortOrder,
                                                      DateTime publishDate, string requiredRole);

        public abstract void DeleteExpandableTextHtml(int moduleId, int itemId);

        public abstract IDataReader ModuleSettingsSelectOne(int moduleId);

        public abstract void ModuleSettingsSave(int moduleId, string sortOrder, string titleCss, string contentCss,
                                                bool expandOnPrint, string headerText, bool useJquery, int displayLimit,
                                                string showAllText, bool showExpandCollapseAll);

        #endregion
    }
}