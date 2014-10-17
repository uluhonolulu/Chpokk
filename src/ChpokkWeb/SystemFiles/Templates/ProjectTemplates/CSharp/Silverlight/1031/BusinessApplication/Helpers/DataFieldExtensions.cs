namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     Stellt Erweiterungsmethoden zum Ausführen von Vorgängen auf einem <see cref="DataField"/> bereit.
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// Ersetzt das <see cref="TextBox" />-Steuerelement eines <see cref="DataField" /> mit einem anderen Steuerelement und aktualisiert die Bindungen.
        /// </summary>
        /// <param name="field">Das <see cref="DataField"/>, dessen <see cref="TextBox"/> ersetzt wird.</param>
        /// <param name="newControl">Das neue Steuerelement, das als <see cref="DataField.Content" /> festgelegt wird.</param>
        /// <param name="dataBindingProperty">Die Eigenschaft des Steuerelements, die für die Datenbindung verwendet wird.</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// Ersetzt das <see cref="TextBox" />-Steuerelement eines <see cref="DataField" /> mit einem anderen Steuerelement und aktualisiert die Bindungen.
        /// </summary>
        /// <param name="field">Das <see cref="DataField"/>, dessen <see cref="TextBox"/> ersetzt wird.</param>
        /// <param name="newControl">Das neue Steuerelement, das als <see cref="DataField.Content" /> festgelegt wird.</param>
        /// <param name="dataBindingProperty">Die Eigenschaft des Steuerelements, die für die Datenbindung verwendet wird.</param>        
        /// <param name="bindingSetupFunction">
        ///  Eine optionale <see cref="Action"/>, die verwendet werden kann, um Parameter der neu erzeugten Bindung zu ändern, bevor diese auf <paramref name="newControl"/> angewendet wird.
        /// </param>
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty, Action<Binding> bindingSetupFunction)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }

            if (newControl == null)
            {
                throw new ArgumentNullException("newControl");
            }

            // Erstellt eine neue Bindung für alle Änderungen, die der Aufrufer durchführen möchte, indem eine bestehende Bindung kopiert und an "bindingSetupFunction" übergeben wird.
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // Feld ersetzen
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// Erstellt ein neues <see cref="Binding"/>-Objekt, indem alle Eigenschaften eines anderen <see cref="Binding"/>-Objekts kopiert werden.
        /// </summary>
        /// <param name="binding"><see cref="Binding"/>, von der die Eigenschaftswerte kopiert werden</param>
        /// <returns>Ein neues <see cref="Binding"/>-Objekt.</returns>
        private static Binding CreateCopy(this Binding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException("binding");
            }

            Binding newBinding = new Binding()
            {
                BindsDirectlyToSource = binding.BindsDirectlyToSource,
                Converter = binding.Converter,
                ConverterParameter = binding.ConverterParameter,
                ConverterCulture = binding.ConverterCulture,
                Mode = binding.Mode,
                NotifyOnValidationError = binding.NotifyOnValidationError,
                Path = binding.Path,
                UpdateSourceTrigger = binding.UpdateSourceTrigger,
                ValidatesOnExceptions = binding.ValidatesOnExceptions
            };

            if (binding.ElementName != null)
            {
                newBinding.ElementName = binding.ElementName;
            }
            else if (binding.RelativeSource != null)
            {
                newBinding.RelativeSource = binding.RelativeSource;
            }
            else
            {
                newBinding.Source = binding.Source;
            }

            return newBinding;
        }
    }
}
