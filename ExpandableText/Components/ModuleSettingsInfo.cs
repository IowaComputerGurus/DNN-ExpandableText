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

namespace ICG.Modules.ExpandableTextHtml.Components
{
    /// <summary>
    /// Infomation object for the ModuleSettings Database Table
    /// </summary>
    /// <remarks>Auto Generated Class
    /// Created With Code Smith
    /// Using the "ICG DotNetNuke .NET 3.5 Info Object Template V 1.0.0"</remarks>
    public class ModuleSettingsInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the module id.
        /// </summary>
        /// <value>The module id.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        public string SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the title CSS.
        /// </summary>
        /// <value>The title CSS.</value>
        public string TitleCss { get; set; }

        /// <summary>
        /// Gets or sets the content CSS.
        /// </summary>
        /// <value>The content CSS.</value>
        public string ContentCss { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [expand on print].
        /// </summary>
        /// <value><c>true</c> if [expand on print]; otherwise, <c>false</c>.</value>
        public bool ExpandOnPrint { get; set; }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>The header text.</value>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use jquery].
        /// </summary>
        /// <value><c>true</c> if [use jquery]; otherwise, <c>false</c>.</value>
        public bool UseJquery { get; set; }

        /// <summary>
        /// Gets or sets the display limit.
        /// </summary>
        /// <value>The display limit.</value>
        public int DisplayLimit { get; set; }

        /// <summary>
        /// Gets or sets the show all text.
        /// </summary>
        /// <value>The show all text.</value>
        public string ShowAllText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show expand collapse all].
        /// </summary>
        /// <value><c>true</c> if [show expand collapse all]; otherwise, <c>false</c>.</value>
        public bool ShowExpandCollapseAll { get; set; }

        #endregion
    }
}