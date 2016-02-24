using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.srv
{
    public class EvnLog 
    {
        protected EventLog elog;
        public EvnLog(EventLog elog)
        {
            this.elog = elog;
        }

        public void I(string format, params Object[] args)
        {
            this.elog.WriteEntry(string.Format(format, args),EventLogEntryType.Information);
        }

        public void W(string format, params Object[] args)
        {
            this.elog.WriteEntry(string.Format(format, args), EventLogEntryType.Warning);
        }

        public void E(string format, params Object[] args)
        {
            this.elog.WriteEntry(string.Format(format, args), EventLogEntryType.Error);
        }
    }
}
