﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ESRI.ArcGIS.Geodatabase
{
    /// <summary>
    ///     Provides extension methods for the <see cref="ESRI.ArcGIS.Geodatabase.IWorkspace" /> interface.
    /// </summary>
    public static class WorkspaceAsyncExtensions
    {
        #region Public Methods

        /// <summary>
        ///     Executes the specified query (SQL) and returns the results as a <see cref="ICursor" />
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="commandText">The command text.</param>
        /// <returns>Returns a <see cref="ICursor" /> representing the results of the query.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public static ICursor ExecuteReaderAsync(this IWorkspace source, string commandText)
        {
            return Task.Wait(() => source.ExecuteReader(commandText));
        }

        /// <summary>
        ///     Gets all of the <see cref="IDomain" /> values that have been configured in the workspace.
        /// </summary>
        /// <param name="source">The workspace.</param>
        /// <returns>
        ///     Returns a <see cref="IEnumerable{IDomain}" /> representing the domains available; otherwise <c>null</c>.
        /// </returns>
        public static IEnumerable<IDomain> GetDomainsAsync(this IWorkspace source)
        {
            return Task.Wait(source.GetDomains);
        }

        /// <summary>
        ///     Gets the changes (or edits) that have been made in the current edit session.
        /// </summary>
        /// <param name="source">The source workspace.</param>
        /// <param name="editDataChangesType">Type of the edit data changes.</param>
        /// <param name="func">The function used to determine if the differences should be determined for the specified table.</param>
        /// <param name="predicate">The predicate used to determine if the differences are returned.</param>
        /// <param name="differenceTypes">The type of differences.</param>
        /// <returns>
        ///     Returns a <see cref="Dictionary{TKey,TValue}" /> representing the differences for the table
        ///     (or key).
        /// </returns>
        /// <exception cref="System.ArgumentNullException">differenceTypes</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     The workspace must be within an edit session in order to determine the
        ///     edit changes.
        /// </exception>
        public static Dictionary<string, List<DifferenceRow>> GetEditChangesAsync(this IWorkspace source, esriEditDataChangesType editDataChangesType, Func<string, bool> func, Predicate<DifferenceRow> predicate, params esriDifferenceType[] differenceTypes)
        {
            return Task.Wait(() => source.GetEditChanges(editDataChangesType, func, predicate, differenceTypes));
        }

        /// <summary>
        ///     Gets the changes (or edits) that have been made in the current edit session.
        /// </summary>
        /// <param name="source">The source workspace.</param>
        /// <param name="editDataChangesType">Type of the edit data changes.</param>
        /// <param name="predicate">The predicate used to determine if the differences are returned.</param>
        /// <param name="differenceTypes">The type of differences.</param>
        /// <returns>
        ///     Returns a <see cref="Dictionary{TKey,TValue}" /> representing the differences for the table
        ///     (or key).
        /// </returns>
        /// <exception cref="System.ArgumentNullException">differenceTypes</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     The workspace must be within an edit session in order to determine the
        ///     edit changes.
        /// </exception>
        public static Dictionary<string, List<DifferenceRow>> GetEditChangesAsync(this IWorkspace source, esriEditDataChangesType editDataChangesType, Predicate<DifferenceRow> predicate, params esriDifferenceType[] differenceTypes)
        {
            return Task.Wait(() => source.GetEditChanges(editDataChangesType, func => true, predicate, differenceTypes));
        }

        /// <summary>
        ///     Gets the changes (or edits) that have been made in the current edit session.
        /// </summary>
        /// <param name="source">The source workspace.</param>
        /// <param name="editDataChangesType">Type of the edit data changes.</param>
        /// <param name="func">The function used to determine if the differences should be determined for the specified table.</param>
        /// <param name="differenceTypes">The type of differences.</param>
        /// <returns>
        ///     Returns a <see cref="Dictionary{TKey,TValue}" /> representing the differences for the table
        ///     (or key).
        /// </returns>
        /// <exception cref="System.ArgumentNullException">differenceTypes</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     The workspace must be within an edit session in order to determine the
        ///     edit changes.
        /// </exception>
        public static Dictionary<string, List<DifferenceRow>> GetEditChangesAsync(this IWorkspace source, esriEditDataChangesType editDataChangesType, Func<string, bool> func, params esriDifferenceType[] differenceTypes)
        {
            return Task.Wait(() => source.GetEditChanges(editDataChangesType, func, differenceTypes));
        }

        /// <summary>
        ///     Gets the changes (or edits) that have been made in the current edit session.
        /// </summary>
        /// <param name="source">The source workspace.</param>
        /// <param name="editDataChangesType">Type of the edit data changes.</param>
        /// <param name="differenceTypes">The type of differences.</param>
        /// <returns>
        ///     Returns a <see cref="Dictionary{TKey,TValue}" /> representing the differences for the table
        ///     (or key).
        /// </returns>
        /// <exception cref="System.ArgumentNullException">differenceTypes</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     The workspace must be within an edit session in order to determine the
        ///     edit changes.
        /// </exception>
        public static Dictionary<string, List<DifferenceRow>> GetEditChangesAsync(this IWorkspace source, esriEditDataChangesType editDataChangesType, params esriDifferenceType[] differenceTypes)
        {
            return Task.Wait(() => source.GetEditChanges(editDataChangesType, differenceTypes));
        }

        /// <summary>
        ///     Gets all of the feature classes in the workspace.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Returns a <see cref="IEnumerable{IFeatureClass}" /> representing the feature classes.</returns>
        public static IEnumerable<IFeatureClass> GetFeatureClassesAsync(this IWorkspace source)
        {
            return Task.Wait(source.GetFeatureClasses);
        }

        /// <summary>
        ///     Gets all of the feature classes in the workspace.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Returns a <see cref="IEnumerable{IFeatureClass}" /> representing the feature classes.</returns>
        public static IEnumerable<IRelationshipClass> GetRelationshipClassesAsync(this IWorkspace source)
        {
            return Task.Wait(source.GetRelationshipClasses);
        }

        /// <summary>
        ///     Gets all of the tables in the workspace.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Returns a <see cref="IEnumerable{ITable}" /> representing the feature classes.</returns>
        public static IEnumerable<string> GetTableNamesAsync(this IWorkspace source)
        {
            return Task.Wait(source.GetTableNames);
        }

        /// <summary>
        ///     Gets all of the tables in the workspace.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>Returns a <see cref="IEnumerable{ITable}" /> representing the feature classes.</returns>
        public static IEnumerable<ITable> GetTablesAsync(this IWorkspace source)
        {
            return Task.Wait(source.GetTables);
        }

        #endregion
    }
}
