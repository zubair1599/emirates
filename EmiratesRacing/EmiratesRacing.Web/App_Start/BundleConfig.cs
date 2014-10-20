using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EmiratesRacing.Web.App_Start
{
    public class BundleConfig
    {
        public static void SetBundles(BundleCollection collection)
        {
            Bundle scripts =
               new ScriptBundle("~/bootstrapAngularScripts")
                   .Include("~/Scripts/angular-*")
                   .Include("~/Scripts/bootstrap.*")
                   .Include("~/Scripts/jquery-*");

            Bundle styles = new StyleBundle("~/bootstrapAngularStyles")
            .Include("~/Content/*.css")
            .Include("~/Scripts/*.css");
                
            
            collection.Add(scripts);
            collection.Add(styles); 
        }
    }
}