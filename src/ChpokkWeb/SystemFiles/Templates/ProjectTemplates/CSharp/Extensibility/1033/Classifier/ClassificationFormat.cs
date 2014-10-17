using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace $safeprojectname$
{
    #region Format definition
    /// <summary>
    /// Defines an editor format for the $safeprojectname$ type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "$safeprojectname$")]
    [Name("$safeprojectname$")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class $safeprojectname$Format : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "$safeprojectname$" classification type
        /// </summary>
        public $safeprojectname$Format()
        {
            this.DisplayName = "$safeprojectname$"; //human readable version of the name
            this.BackgroundColor = Colors.BlueViolet;
            this.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
    #endregion //Format definition
}
