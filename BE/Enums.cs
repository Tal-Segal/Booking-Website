using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{

    #region Enums
    public enum type { Zimmer, HotelRoom, Camp, Igloo, Boat };
    public enum area { North, South, Center, Jerusalem };
    public enum RequestStatus { Open, Expired, MailWasSent, Approved, Unanswered };
    public enum Preference { Must, Possible, Unwanted };
    #endregion

}
