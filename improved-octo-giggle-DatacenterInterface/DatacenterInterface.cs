using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace improved_octo_giggle_DatacenterInterface
{
    public partial interface DataCenter
    {
        public enum DataActionResultType
        {
            Success,
            Etc
        }

        public class DataActionResultBase
        {
            public bool IsSuccess;
            public DataActionResultType ActionResultType;
            public string Message;
        }
    }
}
