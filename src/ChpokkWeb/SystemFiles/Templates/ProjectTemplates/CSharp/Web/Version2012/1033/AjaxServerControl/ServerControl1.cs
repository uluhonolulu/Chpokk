using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
$if$ ($targetframeworkversion$ >= 3.5)using System.Xml.Linq;
$endif$
namespace $safeprojectname$
{
    /// <summary>
    /// Summary description for ServerControl1
    /// </summary>
    public class ServerControl1 : ScriptControl
    {
        public ServerControl1()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        protected override IEnumerable<ScriptDescriptor>
                GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("$safeprojectname$.ClientControl1", this.ClientID);
            yield return descriptor;
        }

        // Generate the script reference
        protected override IEnumerable<ScriptReference>
                GetScriptReferences()
        {
            yield return new ScriptReference("$safeprojectname$.ClientControl1.js", this.GetType().Assembly.FullName);
        }
    }
}