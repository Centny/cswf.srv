using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace io.vty.cswf.srv
{
    public class Proc : Process
    {
        private StreamWriter outf, errf;
        public string Name { get; set; }
        protected IDictionary<string, string> conf;
        protected EvnLog L;

        public Proc(EvnLog elog,string name, IDictionary<string, string> conf)
        {
            this.L = elog;
            L.I("creating Proc({0}) by conf->{1}", conf);
            this.StartInfo.UseShellExecute = "true".Equals(conf["shell"]);
            this.StartInfo.FileName = conf["exec"];
            this.StartInfo.Arguments = conf["args"];
            this.StartInfo.CreateNoWindow = true;
            this.ErrorDataReceived += this.ErrDataReceived;
            this.OutputDataReceived += this.OutDataReceived;
            this.StartInfo.WorkingDirectory = conf["cws"];
            this.outf = new StreamWriter(new FileStream(conf["out"], FileMode.OpenOrCreate | FileMode.Append));
            this.errf = new StreamWriter(new FileStream(conf["err"], FileMode.OpenOrCreate | FileMode.Append));
            this.Name = name;
            this.conf = conf;
        }
        protected void ErrDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.errf.Write(e.Data);
        }
        protected void OutDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.outf.Write(e.Data);
        }
        protected void OnExit(object sender, EventArgs e)
        {
            this.outf.Close();
            this.errf.Close();
        }

    }
}
