using System;

namespace HITS.Blazor.Grid
{
    /// <summary>
    /// This class contains properties to controls the features of grid columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class GridAttributes : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridAttributes()
        {
        }

        /// <summary>
        /// This property controls if a list object property is displayed in the grid.
        /// </summary>
        public bool ShowInGrid { get; set; } = true;

        /// <summary>
        /// This property controls if a list object property can be sorted.
        /// </summary>
        public bool SortBy { get; set; } = true;

        /// <summary>
        /// This property controls if a list object property can be filtered.
        /// </summary>
        public bool FilterBy { get; set; } = false;

        /// <summary>
        /// This property controls the list object property value alignment.
        /// </summary>
        public string Align { get; set; } = "left";

        /// <summary>
        /// This property controls the list object property presentation format.
        /// </summary>
        public string FormatString { get; set; }

    }
}
