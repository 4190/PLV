using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plv.BlockModels
{
    public class SecondBlock
    {
        public int Id { get; set; }

        public string PreviousDocIdHash { get; set; }
        public string DocIdHash { get; set; }

        public string PreviousAddedByHash { get; set; }
        public string AddedByHash { get; set; }

        public string PreviousCurrentUserHash { get; set; }
        public string CurrentUserHash { get; set; }

        public string PreviousReceiverHash { get; set; }
        public string ReceiverHash { get; set; }

        public string PreviousSenderHash { get; set; }
        public string SenderHash { get; set; }

        public string PreviousShortOptionalDescriptionHash { get; set; }
        public string ShortOptionalDescriptionHash { get; set; }

        public string PreviousDateAddedHash { get; set; }
        public string DateAddedHash { get; set; }

        public string PreviousDateReceivedHash { get; set; }
        public string DateReceivedHash { get; set; }
    }
}
