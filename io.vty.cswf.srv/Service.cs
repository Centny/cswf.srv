using io.vty.cswf.log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace io.vty.cswf.srv
{
    public partial class Service : ServiceBase
    {
        private static readonly ILog L = Log.New();
        protected IList<Proc> procs = new List<Proc>();
        public Service()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            var services = ConfigurationManager.AppSettings.Get("services");
            L.D("start services with names({0})", services);
            foreach(var name in services.Split(','))
            {
                try
                {
                    var conf = ConfigurationManager.GetSection(name) as IDictionary<string,string>;
                    if (conf == null)
                    {
                        L.W("the configure section by name({0}) is not found", name);
                        continue;
                    }
                    if ("false".Equals(conf["on"]))
                    {
                        continue;
                    }
                    var proc = new Proc(name, conf);
                    proc.Start();
                    this.procs.Add(proc);
                    L.D("start process({0}) success with conf->{1}", conf);
                }catch(Exception e)
                {
                    L.E(e, "start process({0}) error");
                    this.OnStop();
                    break;
                }
            }
            L.D("start services with {0} process is running", this.procs.Count);
        }

        protected override void OnStop()
        {
            L.D("stop services with {0} process is running", this.procs.Count);
            foreach (var proc in this.procs)
            {
                try
                {
                    proc.Kill();
                    proc.WaitForExit();
                    L.D("stop process({0}) success", proc.Name);
                }
                catch(Exception e)
                {
                    L.E(e, "stopping process({0}) error", proc.Name);
                }
            }
            this.procs.Clear();
        }
    }
}
