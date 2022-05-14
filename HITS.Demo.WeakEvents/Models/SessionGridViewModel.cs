using HITS.Blazor.Grid;
using HITS.Demo.WeakEvents.Services;
using HITS.Demo.WeakEvents.Shared;

namespace HITS.Demo.WeakEvents
{
    /// <summary>
    /// This class contains the methods for getting the data required by the grid,
    /// </summary>
    public class SessionGridViewModel : IDisposable
    {
        /// <summary>
        /// The list of products
        /// </summary>
        public List<SessionModel> SessionList { get; set; }

        /// <summary>
        /// The grid parameters/settings
        /// </summary>
        public GridParameters<SessionModel> GridParameters { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SessionGridViewModel()
        {
            SessionList = new List<SessionModel>();

            GridParameters = new GridParameters<SessionModel>()
            {
                GridTitle = $"{SessionList.Count.ToString("N0")} entries",
                GridTitleColor = "darkblue",
                GridTableColor = "darkblue",
                DataList = SessionList,
                BootstrapColumnClass = "col-lg",
                BootstrapContainerClass = "container-fluid",
                ShowEditColumn = false,
                PrimaryKeyName = "SessionId",
                ShowFilterRow = false,
                ShowAddNew = false,
                ShowPager = true,
                PageSize = 10,
            };
        }

        #region dispose
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    SessionList?.Clear();
                    SessionList = null;
                    GridParameters?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ProductViewModel()
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

        #endregion
    }
}
