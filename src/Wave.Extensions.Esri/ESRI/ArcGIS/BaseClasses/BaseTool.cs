﻿using System.Drawing;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;

namespace ESRI.ArcGIS.BaseClasses
{
    /// <summary>
    ///     An abstract tool that is used in the engine environment.
    /// </summary>
    public abstract class BaseTool : ADF.BaseClasses.BaseTool
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseTool" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="tooltip">The tooltip.</param>
        protected BaseTool(string name, string caption, string category, string message, string tooltip)
            : base(null, caption, category, Cursors.Arrow, 0, null, message, name, tooltip)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseTool" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="bitmap">The bitmap.</param>
        protected BaseTool(string name, string caption, string category, string message, string tooltip, Bitmap bitmap)
            : base(bitmap, caption, category, Cursors.Arrow, 0, null, message, name, tooltip)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseTool" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="cursor">The cursor.</param>
        protected BaseTool(string name, string caption, string category, string message, string tooltip, Bitmap bitmap, Cursor cursor)
            : base(bitmap, caption, category, cursor, 0, null, message, name, tooltip)
        {
        }

        #endregion

        #region Protected Properties

        /// <summary>
        ///     Gets or sets the hook helper.
        /// </summary>
        /// <value>The hook helper.</value>
        protected IHookHelper HookHelper { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Called when the command is created inside the application.
        /// </summary>
        /// <param name="hook">
        ///     A reference to the application in which the command was created.
        ///     The hook may be an IApplication reference (for commands created in ArcGIS Desktop applications)
        ///     or an IHookHelper reference (for commands created on an Engine ToolbarControl).
        /// </param>
        /// <remarks>
        ///     Note to inheritors: classes inheriting from BaseCommand must always
        ///     override the OnCreate method. Use this method to store a reference to the host
        ///     application, passed in via the hook parameter.
        /// </remarks>
        public override void OnCreate(object hook)
        {
            if (hook is IHookHelper)
                this.HookHelper = hook as IHookHelper;
            else
                this.HookHelper = new HookHelperClass {Hook = hook};
        }

        #endregion
    }
}