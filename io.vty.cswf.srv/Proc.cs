using io.vty.cswf.log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.srv
{
    public class Proc : Process
    {
        private static readonly ILog L = Log.New();
        private StreamWriter outf, errf;
        public string Name { get; set; }
        protected IDictionary<string, string> conf;

        public Proc(string name, IDictionary<string, string> conf)
        {
            L.D("creating Proc({0}) by conf->{1}", conf);
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
