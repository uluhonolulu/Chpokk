using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;

namespace $safeprojectname$
{
    class Program
    {
        static void Main(string[] args)
        {
            using(WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) {waitHandle.Set();};
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Console.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(typeof($safeprojectname$.$SafeClassName$));
                instance.Start();

                waitHandle.WaitOne();
            }
        }
    }
}
