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
using System.Collections;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace ICG.Modules.ExpandableTextHtml.Components
{
    public static class ModuleSettingsController
    {
        #region Methods

        /// <summary>
        /// This method will retreive the settings information for the module
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static ModuleSettingsInfo SelectOne(int moduleId)
        {
            var cacheKey = moduleId.ToString() + "_ICG_ETH_SETTINGS";
            var item = DataCache.GetCache(cacheKey);
            if (item != null)
            {
                return (ModuleSettingsInfo) item;
            }
            else
            {
                var oInfo = CBO.FillObject<ModuleSettingsInfo>(DataProvider.Instance().ModuleSettingsSelectOne(moduleId));
                UpdateCache(oInfo);
                return oInfo;
            }
        }

        /// <summary>
        /// Saves the new settings
        /// </summary>
        /// <param name="oInfo"></param>
        public static void Save(ModuleSettingsInfo oInfo)
        {
            DataProvider.Instance().ModuleSettingsSave(oInfo.ModuleId, oInfo.SortOrder, oInfo.TitleCss, oInfo.ContentCss,
                                                       oInfo.ExpandOnPrint, oInfo.HeaderText, oInfo.UseJquery,
                                                       oInfo.DisplayLimit, oInfo.ShowAllText,
                                                       oInfo.ShowExpandCollapseAll);
            UpdateCache(oInfo);
        }

        /// <summary>
        /// This method is used to load settings from the DNN Settings collection, for upgrade sites
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="Settings"></param>
        public static ModuleSettingsInfo LoadSettingsFromDnn(int moduleId, Hashtable Settings)
        {
            var validSettings = true;
            var oNewSettings = new ModuleSettingsInfo();
            var oModController = new ModuleController();
            oNewSettings.ModuleId = moduleId;
            //If we have CSS settings use them
            object titleCssSetting = Settings["ICG_ETH_TitleCss"];
            object contentCssSetting = Settings["ICG_ETH_ContentCss"];
            object sortOrderSettings = Settings["ICG_ETH_SortOrder"];

            if (titleCssSetting != null)
            {
                oNewSettings.TitleCss = titleCssSetting.ToString();
                oModController.DeleteModuleSetting(moduleId, "ICG_ETH_TitleCss");
            }
            else
                validSettings = false;

            if (contentCssSetting != null)
            {
                oNewSettings.TitleCss = contentCssSetting.ToString();
                oModController.DeleteModuleSetting(moduleId, "ICG_ETH_ContentCss");
            }
            else
                validSettings = false;

            if (sortOrderSettings != null)
            {
                oNewSettings.SortOrder = sortOrderSettings.ToString();
                oModController.DeleteModuleSetting(moduleId, "ICG_ETH_SortOrder");
            }
            else
                validSettings = false;

            if (Settings["ICG_ETH_ExpandOnPrint"] != null)
            {
                oNewSettings.ExpandOnPrint = bool.Parse(Settings["ICG_ETH_ExpandOnPrint"].ToString());
                oModController.DeleteModuleSetting(moduleId, "ICG_ETH_ExpandOnPrint");
            }
            else
                validSettings = false;


            //Load the jquerysettings, if enable
            object jquerySetting = Settings["ICG_ETH_JQuery"];
            if (jquerySetting != null)
            {
                oNewSettings.UseJquery = bool.Parse(jquerySetting.ToString());
                oModController.DeleteModuleSetting(moduleId, "ICG_ETH_JQuery");
            }
            else
                validSettings = false;

            object displayLimitSetting = Settings["ICG_ETH_DisplayLimit"];
            if (displayLimitSetting != null)
            {
                oNewSettings.DisplayLimit = int.Parse(displayLimitSetting.ToString());
                oModController.DeleteModuleSetting(moduleId, "ICG_ETH_DisplayLimit");
            }
            else
                validSettings = false;

            //Also get show all text
            object showAllTextSetting = Settings["ICG_ETH_ShowAllText"];
            if (showAllTextSetting != null)
            {
                oNewSettings.ShowAllText = showAllTextSetting.ToString();
                oModController.DeleteModuleSetting(moduleId, "ICG_ETH_ShowAllText");
            }
            else
                validSettings = false;

            if (validSettings)
            {
                oNewSettings.ShowExpandCollapseAll = true;
                Save(oNewSettings);
                return oNewSettings;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Cache Helpers

        /// <summary>
        /// Updates the cache with the module settings for 20 minutes
        /// </summary>
        /// <param name="oinfo"></param>
        private static void UpdateCache(ModuleSettingsInfo oinfo)
        {
            if (oinfo != null)
            {
                string cacheKey = oinfo.ModuleId.ToString() + "_ICG_ETH_SETTINGS";
                DataCache.SetCache(cacheKey, oinfo, new TimeSpan(0, 10, 0));
            }
        }

        #endregion
    }
}