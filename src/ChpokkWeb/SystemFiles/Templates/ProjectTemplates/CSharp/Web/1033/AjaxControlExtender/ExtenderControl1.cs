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
    /// Summary description for ExtenderControl1
    /// </summary>
    [
        TargetControlType(typeof(Control))
    ]
    public class ExtenderControl1 : ExtenderControl
    {
	    public ExtenderControl1()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
	    }
        protected override IEnumerable<ScriptDescriptor>
                GetScriptDescriptors(System.Web.UI.Control targetControl)
        {
            ScriptBehaviorDescriptor descriptor = new ScriptBehaviorDescriptor("$safeprojectname$.ClientBehavior1", targetControl.ClientID);
            yield return descriptor;
        }

        // Generate the script reference
        protected override IEnumerable<ScriptReference>
                GetScriptReferences()
        {
            yield return new ScriptReference("$safeprojectname$.ClientBehavior1.js", this.GetType().Assembly.FullName);
        }
    }
}