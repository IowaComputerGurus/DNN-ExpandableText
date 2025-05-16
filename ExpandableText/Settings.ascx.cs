/*
 * Copyright (c) 2009-2012 IowaComputerGurus Inc (http://www.iowacomputergurus.com)
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
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.UserControls;
using ICG.Modules.ExpandableTextHtml.Components;

namespace ICG.Modules.ExpandableTextHtml
{
    public partial class Settings : ModuleSettingsBase
    {
        #region Protected Members

        /// <summary>
        /// Header input control, declared here to ensure that IIS isn't needed for 
        /// proper control setup.
        /// </summary>
        protected TextEditor txtHeader;

        #endregion

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            JavaScript.RequestRegistration(CommonJs.DnnPlugins);
        }

        /// <summary>
        /// handles the loading of the module setting for this
        /// control
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {
                    var oSettings = ModuleSettingsController.SelectOne(ModuleId);
                    if (oSettings != null)
                    {
                        ListItem sortOrder = ddlSortOrder.Items.FindByValue(oSettings.SortOrder);
                        if (sortOrder != null)
                            sortOrder.Selected = true;
                        else
                            defaultSortOrderSetting();
                        txtTitleCssClass.Text = oSettings.TitleCss;
                        txtContentCssClass.Text = oSettings.ContentCss;
                        chkExpandOnPrint.Checked = oSettings.ExpandOnPrint;
                        txtHeader.Text = oSettings.HeaderText;
                        ddlDefaultShowLimit.SelectedValue = oSettings.DisplayLimit.ToString();
                        txtShowAllText.Text = oSettings.ShowAllText;
                        chkShowExpandCollapseAll.Checked = oSettings.ShowExpandCollapseAll;
                    }
                    else
                    {
                        defaultSortOrderSetting();
                        txtTitleCssClass.Text = "SubHead";
                        txtContentCssClass.Text = "Normal";
                        chkExpandOnPrint.Checked = false;
                        chkShowExpandCollapseAll.Checked = true;
                    }

                    ddlDefaultShowLimit_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #region Helper Methods

        /// <summary>
        /// This helper method will default the sort order setting, selecting the first item from the
        /// listing and saving it.  This will ensure that they have the setting.
        /// </summary>
        private void defaultSortOrderSetting()
        {
            //Default the value
            ddlSortOrder.SelectedIndex = 0;

            //Update the setting
            var oController = new ModuleController();
            oController.UpdateModuleSetting(ModuleId, "ICG_ETH_SortOrder", ddlSortOrder.SelectedValue);
        }

        #endregion

        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
                //Save the setting
                ModuleSettingsInfo oInfo = new ModuleSettingsInfo();
                oInfo.ContentCss = txtContentCssClass.Text;
                oInfo.DisplayLimit = int.Parse(ddlDefaultShowLimit.SelectedValue);
                oInfo.ExpandOnPrint = chkExpandOnPrint.Checked;
                txtHeader.HtmlEncode = false;
                if (txtHeader.Text.Length > 10 && !txtHeader.Text.Equals("<p>&#160;</p>"))
                    oInfo.HeaderText = txtHeader.Text;
                else
                    oInfo.HeaderText = string.Empty;
                oInfo.ModuleId = ModuleId;
                oInfo.ShowAllText = txtShowAllText.Text;
                oInfo.SortOrder = ddlSortOrder.SelectedValue;
                oInfo.TitleCss = txtTitleCssClass.Text;
                oInfo.UseJquery = true;
                oInfo.ShowExpandCollapseAll = chkShowExpandCollapseAll.Checked;
                ModuleSettingsController.Save(oInfo);

                //Purge the cache
                ModuleController.SynchronizeModule(ModuleId);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// If any option other than, all is selected, display the text input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDefaultShowLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            divShowAll.Visible = ddlDefaultShowLimit.SelectedIndex > 0;
        }
    }
}