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
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using ICG.Modules.ExpandableTextHtml.Components;

namespace ICG.Modules.ExpandableTextHtml
{
    public partial class EditExpandableTextHtml : PortalModuleBase
    {
        #region Protected Members

        /// <summary>
        /// Title input control
        /// </summary>
        protected TextEditor txtTitle;

        /// <summary>
        /// Body input control
        /// </summary>
        protected TextEditor txtBody;

        #endregion

        private int itemId = Null.NullInteger; //Stores the item id for the curren titem

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                jQuery.RequestDnnPluginsRegistration();

                //Store the item id on every load
                if (Request.QueryString["EntryId"] != null)
                {
                    itemId = Int32.Parse(Request.QueryString["EntryId"]);
                }

                if (!IsPostBack)
                {
                    //Load the list of roles
                    var roleController = new RoleController();
                    ddlRequiredRole.DataSource = roleController.GetPortalRoles(PortalId);
                    ddlRequiredRole.DataTextField = "RoleName";
                    ddlRequiredRole.DataValueField = "RoleName";
                    ddlRequiredRole.DataBind();
                    ddlRequiredRole.Items.Insert(0,
                                                 new ListItem(
                                                     Localization.GetString("SameAsModule", LocalResourceFile),
                                                     "-1"));

                    //check we have an item to lookup
                    if (!Null.IsNull(itemId))
                    {
                        //load the item
                        var controller = new ExpandableTextHtmlController();
                        var item = controller.GetExpandableTextHtml(ModuleId, itemId);

                        //ensure we have an item
                        if (item != null)
                        {
                            txtTitle.Text = item.Title;
                            txtBody.Text = item.Body;
                            chkIsExpanded.Checked = item.IsExpanded;
                            txtSortOrder.Text = item.SortOrder.ToString();
                            litContentId.Text = string.Format("ICG_ETH_{0}", item.ItemId.ToString());
                            txtPublishDate.Text = item.PublishDate.ToShortDateString();
                            var foundItem = ddlRequiredRole.Items.FindByValue(item.RequiredRole);
                            if (foundItem != null)
                                ddlRequiredRole.SelectedValue = item.RequiredRole;
                        }
                        else
                            Response.Redirect(Globals.NavigateURL(), true);
                    }
                    else
                    {
                        cmdDelete.Visible = false;

                        //Default sort order to 0
                        txtSortOrder.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #region Localization Helper

        /// <summary>
        /// This method will return localized text
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetLocalizeString(string keyName)
        {
            return Localization.GetString(keyName, LocalResourceFile);
        }

        #endregion

        /// <summary>
        /// This method handles an insert or update!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var controller = new ExpandableTextHtmlController();
                var item = new ExpandableTextHtmlInfo();

                txtTitle.HtmlEncode = false;
                item.Title = txtTitle.Text;
                txtBody.HtmlEncode = false;
                item.Body = txtBody.Text;

                item.LastUpdated = DateTime.Now;
                item.ModuleId = ModuleId;
                item.ItemId = itemId;
                item.IsExpanded = chkIsExpanded.Checked;
                item.SortOrder = int.Parse(txtSortOrder.Text);
                item.RequiredRole = ddlRequiredRole.SelectedValue;

                //Do we need to default the publish date
                item.PublishDate = string.IsNullOrEmpty(txtPublishDate.Text) ? DateTime.Now.Date : DateTime.Parse(txtPublishDate.Text);

                //Determine if we need to clean the title text
                if (item.Title.StartsWith("<p>") && item.Title.EndsWith("</p>"))
                {
                    item.Title = item.Title.Substring(3, item.Title.Length - 7);
                }

                //determine if we are adding or updating
                if (Null.IsNull(item.ItemId))
                    controller.AddExpandableTextHtml(item);
                else
                    controller.UpdateExpandableTextHtml(item);

                //Purge the cache
                ModuleController.SynchronizeModule(ModuleId);

                //Call cancel to return
                cmdCancel_Click(sender, e);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// Handles a cancel, exits to the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// Handles a delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Null.IsNull(itemId))
                {
                    var controller = new ExpandableTextHtmlController();
                    controller.DeleteExpandableTextHtml(ModuleId, itemId);

                    //Purge the cache
                    ModuleController.SynchronizeModule(ModuleId);

                    //Call cancel to return
                    cmdCancel_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}