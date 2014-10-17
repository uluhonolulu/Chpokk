using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace $safeprojectname$
{
    internal static class $safeprojectname$ClassificationDefinition
    {
        /// <summary>
        /// Defines the "$safeprojectname$" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("$safeprojectname$")]
        internal static ClassificationTypeDefinition $safeprojectname$Type = null;
    }
}
