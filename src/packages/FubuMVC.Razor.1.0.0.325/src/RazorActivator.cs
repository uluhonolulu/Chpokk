using System.Collections.Generic;
using System.Web;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using HtmlTags;

namespace FubuMVC.Razor
{
    public class RazorActivator : IActivator
    {
        private readonly CommonViewNamespaces _namespaces;

        public RazorActivator(CommonViewNamespaces namespaces)
        {
            _namespaces = namespaces;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            log.Trace("Running {0}".ToFormat(GetType().Name));

            configureRazorSettings(log);
        }

        private void configureRazorSettings(IPackageLog log)
        {
            _namespaces.Add(typeof(VirtualPathUtility).Namespace); // System.Web)
            _namespaces.AddForType<RazorViewFacility>(); // FubuMVC.Razor
            _namespaces.AddForType<IPartialInvoker>(); // FubuMVC.Core.UI
            _namespaces.AddForType<HtmlTag>(); // HtmlTags 
            _namespaces.AddForType<string>();

            log.Trace("Adding namespaces to RazorSettings:");
            _namespaces.Namespaces.Each(x => log.Trace("  - {0}".ToFormat(x)));
        }
    }
}