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
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Framework;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;
using ICG.Modules.ExpandableTextHtml.Components;

namespace ICG.Modules.ExpandableTextHtml
{
    public partial class ViewExpandableTextHtml : PortalModuleBase, IActionable
    {
        #region Private Members (For Css Styling/Templates)

        private string _titleCssClass = "Normal";
        private string _contentCssClass = "SubHead";
        private string _itemTemplate = "";
        private bool _forceExpand;
        private int _displayLimit = -1;
        private string _showAllText = "View Older";
        private readonly List<DisplayedEntries> _displayed = new List<DisplayedEntries>();

        #endregion

        /// <summary>
        /// On page load, when not a postback we will check to ensure that the module is configured
        /// and then will bind the data listing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Trigger DNN to request jQuery registration
                JavaScript.RequestRegistration(CommonJs.jQuery);

                if (!IsPostBack)
                {
                    //Check for configuration
                    ModuleSettingsInfo oInfo = ModuleSettingsController.SelectOne(ModuleId);
                    if (oInfo != null)
                    {
                        //Setup standard
                        ProcessModuleSettings(oInfo);
                        RegisterJavascript();
                        BindTextListing(oInfo.SortOrder);

                        if (oInfo.ShowExpandCollapseAll)
                        {
                            //Prepare expand/collapse all methods
                            RenderExpandAndCollapseAll();
                        }

                        //Auto-expand any needed items
                        Page.ClientScript.RegisterStartupScript(GetType(), "ICG_ETH_AutoExpand_" + ModuleId.ToString(),
                                                                "ICG_ETH_LoadFromUrl();", true);
                    }
                    else
                    {
                        //Hides the display and says not configured
                        HideDisplayAndDisplayNotConfigured();
                    }
                } //End postback block
            } //End catch block
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #region Helper Methods

        /// <summary>
        /// This helper method is used to load the module settings from the DNN settings
        ///  hash tables, any configuration elements that are needed are completed as well.
        /// </summary>
        /// <remarks>
        /// Jquery Method action stubs are included for future module use
        /// </remarks>
        private void ProcessModuleSettings(ModuleSettingsInfo oInfo)
        {
            _titleCssClass = oInfo.TitleCss;
            _contentCssClass = oInfo.ContentCss;

            //Get the item template
            _itemTemplate = GetLocalizeString("ItemTemplate");

            //If the user selected forced expanding, see if we are in print mode
            if (Request.Url.PathAndQuery.Contains("dnnprintmode") && oInfo.ExpandOnPrint)
                _forceExpand = true;

            //Get display limit, ONLY if we don't have the "show=all" querystring
            if (Request.QueryString["show"] == null)
            {
                _displayLimit = oInfo.DisplayLimit;
                _showAllText = oInfo.ShowAllText;
            }
        }


        /// <summary>
        /// Registers the javascript.
        /// </summary>
        private void RegisterJavascript()
        {
            //Register the ETH script
            var jQueryEthScriptPath = TemplateSourceDirectory + "/js/ETH_JQuery.js";
            Page.ClientScript.RegisterClientScriptInclude("ETH_jQuery", jQueryEthScriptPath);
        }

        /// <summary>
        /// This method will bind the text elements to the control
        /// </summary>
        /// <param name="sortOrder"></param>
        private void BindTextListing(object sortOrder)
        {
            var controller = new ExpandableTextHtmlController();
            var items = controller.GetExpandableTextHtmls(ModuleId, sortOrder.ToString());

            //If not editable, filter the list
            if (!IsEditable)
            {
                //Only show currently published items, as well as items that are either visible to all, OR to this users role
                items = items.Where(
                    x => x.PublishDate < DateTime.Now
                         && (x.RequiredRole.Equals("-1")
                             || UserInfo.IsInRole(x.RequiredRole))).ToList();
            }

            //See if we have items
            if (items != null && items.Count > 0)
            {
                //Check to see if limiting display, or see if we have less items than the limit
                if (_displayLimit == -1 || items.Count <= _displayLimit)
                {
                    //Bind list directly
                    rptListing.DataSource = items;
                    rptListing.DataBind();
                }
                else
                {
                    //Bind via Paged Data Source
                    var oDs = new PagedDataSource();
                    oDs.DataSource = items;
                    oDs.AllowPaging = true;
                    oDs.PageSize = _displayLimit;
                    rptListing.DataSource = oDs;
                    rptListing.DataBind();

                    //SHow all
                    pShowAll.Visible = true;
                    hlShowAll.Text = _showAllText;
                    hlShowAll.NavigateUrl = Globals.NavigateURL(TabId) + "?show=all";
                }
            }
            else
            {
                //Configured but no content, hide list
                HideDisplayAndDisplayNoContent();
            }
        }

        /// <summary>
        /// This method is used to help when we have a non-configured site.
        /// </summary>
        private void HideDisplayAndDisplayNotConfigured()
        {
            //Hide it
            rptListing.Visible = false;

            //Get message
            var message = GetLocalizeString(IsEditable ? "NotConfiguredAdmin" : "NotConfigured");

            //Show 
            Skin.AddModuleMessage(this, message, ModuleMessage.ModuleMessageType.YellowWarning);
        }

        /// <summary>
        /// This method is used to help when we have a module with no content
        /// </summary>
        private void HideDisplayAndDisplayNoContent()
        {
            //Hide it
            rptListing.Visible = false;

            //Get message
            var message = GetLocalizeString(IsEditable ? "NoContentAdmin" : "NoContent");

            //Show 
            Skin.AddModuleMessage(this, message, ModuleMessage.ModuleMessageType.YellowWarning);
        }

        #endregion

        #region Localization Helper

        /// <summary>
        /// This helper method will get localized strings to display.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetLocalizeString(string keyName)
        {
            return Localization.GetString(keyName, LocalResourceFile);
        }

        #endregion

        #region IActionable Members

        /// <summary>
        /// IActionable implementation, adds two menu items.  1.) Add New Item.  2.) Support Forum
        /// </summary>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                //Create the listing collection
                var actions = new ModuleActionCollection();

                //Ensure that we have configured the module
                var oInfo = ModuleSettingsController.SelectOne(ModuleId);
                if (oInfo != null)
                {
                    actions.Add(GetNextActionID(),
                                Localization.GetString(ModuleActionType.AddContent, LocalResourceFile),
                                ModuleActionType.AddContent, "", "", EditUrl(), false, SecurityAccessLevel.Edit,
                                true, false);
                }

                //Add support link, edit only, open to new window!
                actions.Add(GetNextActionID(), GetLocalizeString("SupportLink"), ModuleActionType.OnlineHelp, "", "",
                            "https://github.com/IowaComputerGurus/DNN-ExpandableText/issues",
                            false, SecurityAccessLevel.Edit, true, true);

                return actions;
            }
        }

        #endregion

        /// <summary>
        /// This method is called each time the datalist is bound.  This adds the code that will show/hide the elements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptListing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Get the data item
                var oInfo = (ExpandableTextHtmlInfo) e.Item.DataItem;

                //Get the placeholder
                var itemLiteral = (Literal) e.Item.FindControl("litItem");

                CreateItemJQuery(itemLiteral, oInfo);
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                var oHeader = (Literal) e.Item.FindControl("litHeader");

                //Determine if we have an additional header
                ModuleSettingsInfo oSettings = ModuleSettingsController.SelectOne(ModuleId);
                if (oSettings.HeaderText.Length > 0)
                    oHeader.Text = oSettings.HeaderText + GetLocalizeString("HeaderTemplate");
                else
                    oHeader.Text = GetLocalizeString("HeaderTemplate");

                //Append Expand/Collapse all if needed
                if (oSettings.ShowExpandCollapseAll)
                {
                    var expandAllText = GetLocalizeString("ExpandAll");
                    var collapseAllText = GetLocalizeString("CollapseAll");
                    var expandCollapseFormat =
                        "<a href=\"javascript:ICG_ETH_HideAll_{0}();\" class=\"normal CollapseLink\">{1}</a>&nbsp;<a href=\"javascript:ICG_ETH_ShowAll_{0}();\" class=\"normal ExpandLink\">{2}</a>";
                    oHeader.Text += string.Format(expandCollapseFormat, ModuleId, collapseAllText, expandAllText);
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                var oFooter = (Literal) e.Item.FindControl("litFooter");
                oFooter.Text = GetLocalizeString("FooterTemplate");
            }
        }

        #region Item Build Helpers

        /// <summary>
        /// This method will create an item using the jQuery methods for expanding/collapsing
        /// </summary>
        /// <param name="litContent">The literal control that the content item should be added to</param>
        /// <param name="oInfo">The <see cref="ExpandableTextHtmlInfo"/> object with information</param>
        private void CreateItemJQuery(Literal litContent, ExpandableTextHtmlInfo oInfo)
        {
            //Declare a string builder for the template
            var oItemBuilder = new StringBuilder();

            //Start with the template
            oItemBuilder.Append(_itemTemplate);

            //Setup Edit
            oItemBuilder.Replace("[EDIT]", BuildEditLink(oInfo.ItemId));

            //Create the content element's id for later use
            var contentId = "ICG_ETH_" + oInfo.ItemId.ToString();

            //Sub in the title to our template
            oItemBuilder.Replace("[TITLE]", BuildTitleLink(contentId, oInfo.ItemId.ToString(), oInfo.Title));

            //Put in expand collapse token
            oItemBuilder.Replace("[EXPANDCOLLAPSEICON]",
                                 BuildExpandCollapseIconJquery(contentId, oInfo.ItemId, oInfo.IsExpanded));

            //Sub in the content
            oItemBuilder.Replace("[CONTENT]", BuildContent(contentId, oInfo.Body, oInfo.IsExpanded));

            //Add in last modified
            oItemBuilder.Replace("[LASTMODIFIED]", oInfo.LastUpdated.ToShortDateString());

            //Put the content into the literal
            litContent.Text = oItemBuilder.ToString();

            //Add to the list
            _displayed.Add(new DisplayedEntries(oInfo.ItemId, contentId));
        }

        #endregion

        #region Content Build Helpers

        /// <summary>
        /// This method will build the edit link if needed or return an empty string if not needed
        /// </summary>
        /// <param name="itemId">The id of the item to potentially edit</param>
        /// <returns>Link string or empty string for template token removal</returns>
        private string BuildEditLink(int itemId)
        {
            //Find out if we are in edit mode
            if (IsEditable)
            {
                //Insert link for edit
                string editLink = "<a href='" + EditUrl("EntryId", itemId.ToString()) + "'>";

                //Add the edit image and end the link
                editLink += "<img src='" + Globals.ResolveUrl("~/images/edit.gif") +
                            "' alt='Edit' style='border-style: none;' /> </a>";

                //Send back the url
                return editLink;
            }
            else
            {
                //Return empty string
                return "";
            }
        }

        /// <summary>
        /// This method will build the title link for the content used for display
        /// </summary>
        /// <param name="contentId">the unique element ID</param>
        /// <param name="itemId">The unique element id</param>
        /// <param name="title">The title from the element</param>
        /// <returns></returns>
        private string BuildTitleLink(string contentId, string itemId, string title)
        {
            return
                string.Format(
                    "<a href=\"javascript:ShowOrHideContentJquery('{0}','{1}');\" class='{2}'>{3}</a><a name='{0}' ></a>",
                    contentId, itemId, _titleCssClass, title);
        }

        /// <summary>
        /// This method builds the expand collapse icon code for JQuery instances
        /// </summary>
        /// <param name="contentId">The id of the content</param>
        /// <param name="itemId">The id of the element</param>
        /// <param name="isExpanded">SHould it be expanded</param>
        /// <returns></returns>
        private string BuildExpandCollapseIconJquery(string contentId, int itemId, bool isExpanded)
        {
            //Create the expand collapse sections
            var oExpandCollapseBuilder = new StringBuilder();

            //Start with the link that will trigger the JS
            oExpandCollapseBuilder.Append("<a href=\"javascript:ShowOrHideContentJquery('" + contentId + "','" +
                                          itemId.ToString() + "');\"");
            oExpandCollapseBuilder.Append(" class='" + _titleCssClass + "' >");

            //Add the expand and collapse items
            oExpandCollapseBuilder.Append("<img style='border-style: none;' alt='" +
                                          Localization.GetString("ExpandAlt", LocalResourceFile) + "' src='" +
                                          Globals.ResolveUrl("~/images/max.gif") + "' id='ICG_ETH_EXPAND_" +
                                          itemId.ToString() + "' width='12' height='15'");

            //If the item IS expanded, hide expand
            if (isExpanded || _forceExpand)
                oExpandCollapseBuilder.Append(" class='hideContent' ");

            //Close out the tag
            oExpandCollapseBuilder.Append(" />");

            oExpandCollapseBuilder.Append("<img style='border-style: none;' alt='" +
                                          Localization.GetString("CollapseAlt", LocalResourceFile) + "' src='" +
                                          Globals.ResolveUrl("~/images/min.gif") + "' id='ICG_ETH_COLLAPSE_" +
                                          itemId.ToString() + "' width='12' height='15'");

            //If the item is NOT expanded, hide collapse
            if (!isExpanded && !_forceExpand)
                oExpandCollapseBuilder.Append(" class='hideContent' ");

            //Close out the tag
            oExpandCollapseBuilder.Append(" />");

            //End the link portion
            oExpandCollapseBuilder.Append("</a> &nbsp;");

            return oExpandCollapseBuilder.ToString();
        }

        /// <summary>
        /// This method will build the content portion of the display, both jQuery and JScript.
        /// </summary>
        /// <param name="contentId">The id of the content element</param>
        /// <param name="body">The body text</param>
        /// <param name="isExpanded">Is it expanded</param>
        /// <returns></returns>
        private string BuildContent(string contentId, string body, bool isExpanded)
        {
            string contentFormatString = "<div id='{0}' class='{1}' {2}>{3}</div>";

            if (!isExpanded && !_forceExpand)
            {
                //Due to a .NET error, must wrap the regular CSS outside
                contentFormatString = "<div class='{4}'>" + contentFormatString + "</div>";
                return string.Format(contentFormatString, contentId, "hideContent", "", body, _contentCssClass);
            }
            else
            {
                //Not hiding, no trickery needed
                return string.Format(contentFormatString, contentId, _contentCssClass, "", body);
            }
        }

        #endregion

        #region Expand and Collapse All

        private void RenderExpandAndCollapseAll()
        {
            var oBuilder = new StringBuilder();
            oBuilder.Append("<script type=\"text/javascript\">");
            oBuilder.Append(GetAllItemMethod("ICG_ETH_HideAll_" + ModuleId.ToString(), "ICG_ETH_HideContent"));
            oBuilder.Append(GetAllItemMethod("ICG_ETH_ShowAll_" + ModuleId.ToString(), "ICG_ETH_ShowContent"));
            oBuilder.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "ICG_ETH_SHOWALL_" + ModuleId.ToString(),
                                                        oBuilder.ToString());
        }

        private string GetAllItemMethod(string methodName, string actionMethod)
        {
            var oBuilder = new StringBuilder();
            oBuilder.AppendFormat("function {0} (){{", methodName);
            foreach (DisplayedEntries currentEntry in _displayed)
            {
                oBuilder.AppendLine(string.Format("{0}('{1}','{2}');", actionMethod, currentEntry.ContentId,
                                                  currentEntry.ItemId.ToString()));
            }
            oBuilder.AppendLine("}");
            return oBuilder.ToString();
        }

        #endregion

        private class DisplayedEntries
        {
            public int ItemId { get; set; }
            public string ContentId { get; set; }

            public DisplayedEntries(int item, string content)
            {
                ItemId = item;
                ContentId = content;
            }
        }
    }
}