using improved_octo_giggle_DatacenterInterface;
using improved_octo_giggle_server.Data;
using Microsoft.EntityFrameworkCore;

namespace improved_octo_giggle_server.Services.DataCenter_LocalModule
{
    public partial class DataCenterLocal : DataCenter
    {
        private ApplicationDbContext dbContext;
        private ILogger logger;

        public static DataCenterLocal? Create(ApplicationDbContext Context, bool ApplyMigration, ILogger Logger)
        {
            if(Context == null)
            {
                return null;
            }

            if (Context.Database == null)
            {
                return null;
            }

            if (!Context.Database.CanConnect())
            {
                return null;
            }

            if (ApplyMigration)
            {
                Context.Database.Migrate();
            }

            DataCenterLocal dataCenterLocal = new DataCenterLocal();
            dataCenterLocal.logger = Logger;
            dataCenterLocal.dbContext = Context;
            dataCenterLocal._broadcaster = new DataBroadcaster();

            return dataCenterLocal;
        }

        public string HashPassword(string password)
        {
            throw new NotImplementedException();
        }
    }
}
