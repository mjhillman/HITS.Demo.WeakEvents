using System;
using System.Collections.Generic;

namespace HITS.Blazor.Grid
{
    /// <summary>
    /// This class contains properties to control the features of the grid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GridParameters<T> : IDisposable
    {
        private bool disposedValue;

        public enum SummaryTypeEnum { Count, Sum, Average };

        /// <summary>
        /// The grid title text shown at the top of the grid.
        /// </summary>
        public string GridTitle { get; set; } = "List";

        /// <summary>
        /// The grid title and summary text color.
        /// </summary>
        public string GridTitleColor { get; set; } = "darkblue";

        /// <summary>
        /// The table text color.
        /// </summary>
        public string GridTableColor { get; set; } = "darkblue";

        /// <summary>
        /// The List of type T containing the data to be displayed in the grid.
        /// </summary>
        public List<T> DataList { get; set; }

        /// <summary>
        /// This property determines if the "Edit" button will be displayed in the grid row.
        /// </summary>
        public bool ShowEditColumn { get; set; } = false;

        /// <summary>
        /// This property determines if the filter text box will be displayed.
        /// </summary>
        public bool ShowFilterRow { get; set; } = false;

        /// <summary>
        /// This property determines if the pager will be displayed.
        /// </summary>
        public bool ShowPager { get; set; } = false;

        /// <summary>
        /// This property determines if the Add New button will be displayed at the top of the grid.
        /// </summary>
        public bool ShowAddNew { get; set; } = false;

        /// <summary>
        /// This property determines the number of rows displayed per page.
        /// </summary>
        public int PageSize { get; set; } = 5;

        /// <summary>
        /// This property identifies the primary key property.
        /// </summary>
        public string PrimaryKeyName { get; set; }

        /// <summary>
        /// This property determines the text that will be shown on the edit button.
        /// </summary>
        public string EditButtonText { get; set; } = "Edit";

        /// <summary>
        /// This property determines the CSS class for the container.
        /// </summary>
        public string BootstrapContainerClass { get; set; } = "container-fluid";

        /// <summary>
        /// This property determines the CSS class for grid columns.
        /// </summary>
        public string BootstrapColumnClass { get; set; } = "col-md";

        /// <summary>
        /// This property identifies the column to be summarized.
        /// </summary>
        public string SummaryColumnName { get; set; }

        /// <summary>
        /// This property contains the summary title text to be shown at the top of the grid for the summary value.
        /// </summary>
        public string SummaryTitle { get; set; }

        /// <summary>
        /// This property sets the type of aggregate function to be applied to the summary column.
        /// </summary>
        public SummaryTypeEnum SummaryType { get; set; }

        /// <summary>
        /// This property sets formatting of the summary value.
        /// </summary>
        public string SummaryFormat { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    DataList?.Clear();
                    DataList = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GridParameters()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
