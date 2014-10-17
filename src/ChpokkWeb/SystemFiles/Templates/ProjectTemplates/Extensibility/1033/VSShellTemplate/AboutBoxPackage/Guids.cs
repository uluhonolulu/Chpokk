// Guids.cs
// MUST match guids.h
using System;

namespace $safeprojectname$
{
    static class GuidList
    {
        public const string guidAboutBoxPackagePkgString = "$guid2$";
        public const string guidAboutBoxPackageCmdSetString = "$guid3$";

        public static readonly Guid guidAboutBoxPackageCmdSet = new Guid(guidAboutBoxPackageCmdSetString);
    };
}