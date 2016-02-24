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
        protected IList<Proc> procs = new List<Proc>();
        protected EvnLog L;
        public Service()
        {
            InitializeComponent();
            if (!EventLog.SourceExists("cswf.srv.log.source"))
            {
                EventLog.CreateEventSource("cswf.srv.log.source", "cswf.srv.log");
            }
            this.evn_l.Source = "cswf.srv.log.source";
            this.evn_l.Log = "cswf.srv.log";
            this.L = new EvnLog(this.evn_l);

        }
        protected override void OnStart(string[] args)
        {
            var services = ConfigurationManager.AppSettings.Get("services");
            L.I("start services with names({0})", services);
            foreach(var name in services.Split(','))
            {
                try
                {
                    var conf = ConfigurationManager.GetSection(name) as IDictionary<string,string>;
                    if (conf == null)
                    {
                        L.I("the configure section by name({0}) is not found", name);
                        continue;
                    }
                    if ("false".Equals(conf["on"]))
                    {
                        L.I("the service name({0}) is not actived", name);
                        continue;
                    }
                    var proc = new Proc(L, name, conf);
                    proc.Start();
                    this.procs.Add(proc);
                    L.I("start process({0}) success with conf->{1}", conf);
                }catch(Exception e)
                {
                    L.I("start process({0}) error->{1}", name, e.Message);
                    this.OnStop();
                    break;
                }
            }
            L.I("start services done with {0} process is running", this.procs.Count);
        }

        protected override void OnStop()
        {
            L.I("stop services with {0} process is running", this.procs.Count);
            foreach (var proc in this.procs)
            {
                try
                {
                    proc.Kill();
                    proc.WaitForExit();
                    L.I("stop process({0}) success", proc.Name);
                }
                catch(Exception e)
                {
                    L.E("stopping process({0}) error->{1}", proc.Name, e.Message);
                }
            }
            this.procs.Clear();
        }
    }
}
