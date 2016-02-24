using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace io.vty.cswf.srv
{
    public class Proc : Process
    {
        private StreamWriter outf, errf;
        public string Name { get; set; }
        protected IDictionary conf;
        protected EvnLog L;

        public Proc(EvnLog elog, string name, IDictionary conf)
        {
            this.L = elog;
            var cws = conf["cws"].ToString();
            var exec = conf["exec"].ToString();
            var args = conf["args"].ToString();
            var shell = "true".Equals(conf["shell"]);
            if (!Path.IsPathRooted(exec))
            {
                exec = cws + "\\" + exec;
            }
            L.I("creating Proc({0}) by \n cws:\t{1}\n exec:\t{2}\n args:\t{3}\n shell:\t{4}\n", name, cws, exec, args, shell);
            this.StartInfo.UseShellExecute = shell;
            this.StartInfo.FileName = exec;
            this.StartInfo.Arguments = args;
            this.StartInfo.CreateNoWindow = true;
            this.StartInfo.WorkingDirectory = cws;
            this.StartInfo.RedirectStandardError = true;
            this.StartInfo.RedirectStandardOutput = true;
            this.Name = name;
            this.conf = conf;
        }
        public bool Run()
        {
            var cws = conf["cws"].ToString();
            var ofp = conf["out"].ToString();
            var efp = conf["err"].ToString();
            if (!Path.IsPathRooted(ofp))
            {
                ofp = cws + "\\" + ofp;
            }
            if (!Path.IsPathRooted(efp))
            {
                efp = cws + "\\" + efp;
            }
            L.I("start run Proc({0}) by \n cws:\t{1}\n out:\t{2}\n err:\t{3}\n", this.Name, cws, ofp, efp);
            this.outf = new StreamWriter(new FileStream(ofp, FileMode.OpenOrCreate | FileMode.Append));
            this.errf = new StreamWriter(new FileStream(efp, FileMode.OpenOrCreate | FileMode.Append));
            if (!this.Start())
            {
                return false;
            }
            this.ErrorDataReceived += this.ErrDataReceived;
            this.OutputDataReceived += this.OutDataReceived;
            this.Exited += this.OnExited;
            this.EnableRaisingEvents = true;
            this.BeginErrorReadLine();
            this.BeginOutputReadLine();
            return true;
        }
        protected void ErrDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.errf.Write(e.Data + "\n");
            this.outf.Flush();
        }
        protected void OutDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.outf.Write(e.Data + "\n");
            this.outf.Flush();
        }
        protected void OnExited(object sender, EventArgs e)
        {
            L.I("the Proc({0}) is exit with code({1})", this.Name, this.ExitCode);
            this.outf.Close();
            this.errf.Close();
        }

    }
}
