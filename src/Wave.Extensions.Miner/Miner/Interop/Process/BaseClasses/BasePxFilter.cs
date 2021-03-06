using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Miner.ComCategories;
using stdole;

namespace Miner.Interop.Process
{
    /// <summary>
    ///     A abstract filter that can be used for either Session Manager or Workflow Manager.
    /// </summary>
    /// <remarks>http://resources.arcfmsolution.com/10.1/EngineSDK/FiltersBuilders.html</remarks>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
    [ComVisible(true)]
    public abstract class BasePxFilter : IMMPxFilter, IMMPxFilterEx
    {
        #region Fields

        private readonly string _DisplayName;
        private readonly string _NodeTypeName;
        private readonly string _ProgID;
        private static readonly ILog Log = LogProvider.For<BasePxFilter>();

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasePxFilter" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="progID">The prog ID.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="category">The category (Session Manager or Workflow Manager).</param>
        /// <param name="extensionName">Name of the extension (MMSessionManager or MMWorkflowManager).</param>
        /// <param name="nodeTypeName">Name of the node type.</param>
        /// <param name="displayName">The display name of the top level node.</param>
        protected BasePxFilter(string name, string progID, int priority, string category, string extensionName, string nodeTypeName, string displayName)
        {
            Name = name;
            _ProgID = progID;
            Priority = priority;
            Category = category;
            ExtensionName = extensionName;
            _NodeTypeName = nodeTypeName;
            _DisplayName = displayName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the category.
        /// </summary>
        public string Category { get; }

        /// <summary>
        ///     Gets the name of the extension.
        /// </summary>
        /// <value>
        ///     The name of the extension.
        /// </value>
        public string ExtensionName { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the priority.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="BasePxFilter" /> is visible.
        /// </summary>
        /// <value>
        ///     <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Visible
        {
            get
            {
                if (PxApplication == null) return false;

                return PxApplication.FilterVisible(_NodeTypeName);
            }
        }

        /// <summary>
        ///     Gets the large image.
        /// </summary>
        public virtual IPictureDisp LargeImage { get; protected set; }

        /// <summary>
        ///     Gets the small image.
        /// </summary>
        public virtual IPictureDisp SmallImage { get; protected set; }

        #endregion

        #region Protected Properties

        /// <summary>
        ///     Gets the px application.
        /// </summary>
        protected IMMPxApplication PxApplication { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Executes the filter and returns the <see cref="ID8ListItem" /> of the results.
        /// </summary>
        /// <returns>
        ///     An <see cref="ID8ListItem" /> containing the results.
        /// </returns>
        public virtual ID8ListItem Execute()
        {
            try
            {
                return this.InternalExecute();
            }
            catch (Exception e)
            {
                Log.Error("Error Executing Filter " + this.Name, e);
            }

            return null;
        }

        /// <summary>
        ///     Gets the ProgID that has been assigned to the filter.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> of the ProgID assigned to the filter.
        /// </returns>
        public string FilterProgID()
        {
            return _ProgID;
        }

        /// <summary>
        ///     Initializes the filter using the initialization data.
        /// </summary>
        /// <param name="vInitData">The initialization data.</param>
        public void Initialize(object vInitData)
        {
            PxApplication = (IMMPxApplication) vInitData;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///     Registers the specified registry key.
        /// </summary>
        /// <param name="registryKey">The registry key.</param>
        [ComRegisterFunction]
        internal static void Register(string registryKey)
        {
            MMFilter.Register(registryKey);
        }

        /// <summary>
        ///     Unregisters the specified registry key.
        /// </summary>
        /// <param name="registryKey">The registry key.</param>
        [ComUnregisterFunction]
        internal static void Unregister(string registryKey)
        {
            MMFilter.Unregister(registryKey);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Gets the builder that is used to build the list of nodes.
        /// </summary>
        /// <param name="pxApp">The process application reference.</param>
        /// <returns>Returns the <see cref="Miner.Interop.Process.IMMListBuilder" /> representing the builder.</returns>
        protected abstract IMMListBuilder GetNodeBuilder(IMMPxApplication pxApp);

        /// <summary>
        ///     Executes the filter and returns the <see cref="ID8ListItem" /> of the results.
        /// </summary>
        /// <returns>
        ///     An <see cref="ID8ListItem" /> containing the results.
        /// </returns>
        protected virtual ID8ListItem InternalExecute()
        {
            int nodeTypeID = PxApplication.Helper.GetNodeTypeID(_NodeTypeName);
            if (nodeTypeID == 0) return null;

            // Create the top level node.
            IMMPxNodeEdit nodeEdit = new MMPxNodeListClass();
            ((IMMPxNodeEdit2) nodeEdit).IsPxTopLevel = true;
            nodeEdit.Initialize(nodeTypeID, _NodeTypeName, 0);
            nodeEdit.DisplayName = _DisplayName;
            ((IMMPxApplicationEx) PxApplication).HydrateNodeFromDB((IMMPxNode) nodeEdit);

            // Use the builder to create the list.
            IMMDynamicList list = (IMMDynamicList) nodeEdit;
            list.BuildObject = this.GetNodeBuilder(PxApplication);
            list.Build(false);

            return (ID8ListItem) nodeEdit;
        }

        #endregion
    }
}