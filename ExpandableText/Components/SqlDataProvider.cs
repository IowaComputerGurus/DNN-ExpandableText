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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace ICG.Modules.ExpandableTextHtml.Components
{
    public class SqlDataProvider : DataProvider
    {
        #region vars

        private const string providerType = "data";
        private const string moduleQualifier = "ICG_ETH_";

        private readonly ProviderConfiguration providerConfiguration =
            ProviderConfiguration.GetProviderConfiguration(providerType);

        private readonly string connectionString;
        private readonly string providerPath;
        private readonly string objectQualifier;
        private readonly string databaseOwner;

        #endregion

        #region cstor

        /// <summary>
        /// cstor used to create the sqlProvider with required parameters from the configuration
        /// section of web.config file
        /// </summary>
        public SqlDataProvider()
        {
            var provider = (Provider) providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            connectionString = Config.GetConnectionString();

            if (connectionString == string.Empty)
                connectionString = provider.Attributes["connectionString"];

            providerPath = provider.Attributes["providerPath"];

            objectQualifier = provider.Attributes["objectQualifier"];
            if (objectQualifier != string.Empty && !objectQualifier.EndsWith("_"))
                objectQualifier += "_";

            databaseOwner = provider.Attributes["databaseOwner"];
            if (databaseOwner != string.Empty && !databaseOwner.EndsWith("."))
                databaseOwner += ".";
        }

        #endregion

        #region properties

        public string ConnectionString
        {
            get { return connectionString; }
        }


        public string ProviderPath
        {
            get { return providerPath; }
        }

        public string ObjectQualifier
        {
            get { return objectQualifier; }
        }


        public string DatabaseOwner
        {
            get { return databaseOwner; }
        }

        #endregion

        #region private methods

        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + moduleQualifier + name;
        }

        private object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region override methods

        public override IDataReader GetExpandableTextHtmls(int moduleId, string orderBy)
        {
            return SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GetExpandableTextHtmls"), moduleId,
                                           orderBy);
        }

        public override IDataReader GetExpandableTextHtml(int moduleId, int itemId)
        {
            return SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GetExpandableTextHtml"), moduleId,
                                           itemId);
        }

        public override void AddExpandableTextHtml(int moduleId, string title, string body, DateTime lastUpdated,
                                                   bool isExpanded, int sortOrder, DateTime publishDate,
                                                   string requiredRole)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("AddExpandableTextHtml"), moduleId, title,
                                      body, lastUpdated, isExpanded, sortOrder, publishDate, requiredRole);
        }

        public override void UpdateExpandableTextHtml(int moduleId, int itemId, string title, string body,
                                                      DateTime lastUpdated, bool isExpanded, int sortOrder,
                                                      DateTime publishDate, string requiredRole)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("UpdateExpandableTextHtml"), moduleId,
                                      itemId, title, body, lastUpdated, isExpanded, sortOrder, publishDate, requiredRole);
        }

        public override void DeleteExpandableTextHtml(int moduleId, int itemId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DeleteExpandableTextHtml"), moduleId,
                                      itemId);
        }

        public override IDataReader ModuleSettingsSelectOne(int moduleId)
        {
            return SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("ModuleSettingsSelectOne"), moduleId);
        }

        public override void ModuleSettingsSave(int moduleId, string sortOrder, string titleCss, string contentCss,
                                                bool expandOnPrint, string headerText, bool useJquery, int displayLimit,
                                                string showAllText, bool showExpandCollapseAll)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("ModuleSettingsSave"), moduleId, sortOrder,
                                      titleCss, contentCss, expandOnPrint, headerText, useJquery, displayLimit,
                                      showAllText, showExpandCollapseAll);
        }

        #endregion
    }
}