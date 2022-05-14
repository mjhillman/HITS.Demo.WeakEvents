using HITS.Demo.WeakEvents.Services;
using HITS.Extensions.Object;
using HITS.LIB.DataAccess;
using HITS.LIB.WeakEvents;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace HITS.Demo.WeakEvents.Shared
{
    public class LogDataManager
    {
        private static volatile LogDataManager instance;
        private static object syncRoot = new Object(); 
        public enum LogEvents { GetLogDataResponse, GetLogDataRequest, EraseLogRequest, SessionLogRequest }

        private LogDataManager()
        {

        }

        public static LogDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LogDataManager();                            
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Call this method to have this class subscribe to a log event.
        /// </summary>
        /// <remarks>Normally there would be a dispose event where we would call the UnregisterAll method
        /// but since this is a singleton there is one instance of this class until the program ends so a 
        /// dispose pattern in unneccesary.
        /// </remarks>
        public void Subscribe()
        {
            WeakReferenceMessenger.Default.Register<StandardMessage>(this, WeakEventHandler);
        }

        /// <summary>
        /// When any event is published for this class, this method processes the event.
        /// </summary>
        /// <param name="r">this class</param>
        /// <param name="m">instance of the StandardMessage class containing the EventData</param>
        /// <remarks>This method demonstrates how isolation can be achieved from the calling class.
        /// Rather than calling a method directly, an event is published by an external class to invoke the desired method.
        /// Using this approach you could change the logging code being used with minimal changes.</remarks>
        public async void WeakEventHandler(object r, StandardMessage m)
        {
            EventData eventData = m.Value as EventData;

            if (eventData != null)
            {
                //process request to erase log
                if (eventData.Token == nameof(LogEvents.EraseLogRequest))
                {
                    await EraseLogAsync();
                }
                
                //process request to retrieve all the log data
                else if (eventData.Token == nameof(LogEvents.GetLogDataRequest))
                {
                    //reusing eventData with the addition of data and a new token
                    eventData.Token = nameof(LogEvents.GetLogDataResponse);
                    eventData.Data = GetLogListAsync().Result;
                    WeakReferenceMessenger.Default.Send(new StandardMessage(eventData));
                }

                //process a request to make a session log entry
                else if (eventData.Token == nameof(LogEvents.SessionLogRequest))
                {
                    await WriteLogEntry((SessionModel)eventData.Data);
                }
            }
        }

        private async Task<List<SessionModel>> GetLogListAsync()
        {
            using (SqliteAsync dal = new SqliteAsync())
            {
                string sql = "SELECT * FROM Session ORDER BY CreatedOn DESC;";
                string connectionString = $"DataSource={IoHelper.GetFilePath("LocalLog.sdb")}";
                return await dal.GetDataTableAsListAsync<SessionModel>(connectionString, sql);
            }
        }

        private async Task<int> EraseLogAsync()
        {
            using (SqliteAsync dal = new SqliteAsync())
            {
                string sql = "DELETE FROM Session;";
                string connectionString = $"DataSource={IoHelper.GetFilePath("LocalLog.sdb")}";
                return await dal.ExecuteNonQueryAsync(connectionString, sql);
            }
        }

        private async Task<int> WriteLogEntry(SessionModel model)
        {
            if (model != null)
            {
                using (SqliteAsync dal = new SqliteAsync())
                {
                    string sql = model.AsSqlInsert<SessionModel>("Session");
                    string connectionString = $"DataSource={IoHelper.GetFilePath("LocalLog.sdb")}";
                    return await dal.ExecuteNonQueryAsync(connectionString, sql);
                }
            }

            return 0;
        }

    }
}
