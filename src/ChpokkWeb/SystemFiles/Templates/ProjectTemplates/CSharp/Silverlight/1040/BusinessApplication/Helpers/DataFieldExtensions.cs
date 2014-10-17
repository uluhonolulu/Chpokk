namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     Fornisce metodi di estensione per l'esecuzione di operazioni in un oggetto <see cref="DataField"/>.
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// Sostituisce il controllo <see cref="TextBox" /> di un oggetto <see cref="DataField" /> con un altro controllo e aggiorna le associazioni.
        /// </summary>
        /// <param name="field">Oggetto <see cref="DataField"/> di cui verrà sostituito il controllo <see cref="TextBox"/>.</param>
        /// <param name="newControl">Nuovo controllo che verrà impostato come <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Proprietà del controllo che verrà utilizzata per l'associazione dati.</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// Sostituisce il controllo <see cref="TextBox" /> di un oggetto <see cref="DataField" /> con un altro controllo e aggiorna le associazioni.
        /// </summary>
        /// <param name="field">Oggetto <see cref="DataField"/> di cui verrà sostituito il controllo <see cref="TextBox"/>.</param>
        /// <param name="newControl">Nuovo controllo che verrà impostato come <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Proprietà del controllo che verrà utilizzata per l'associazione dati.</param>        
        /// <param name="bindingSetupFunction">
        ///  Oggetto <see cref="Action"/> facoltativo che è possibile utilizzare per modificare i parametri nell'associazione appena generata prima che venga applicato a <paramref name="newControl"/>
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

            // Costruire una nuova associazione copiando quella esistente e inviandola a un oggetto bindingSetupFunction per le eventuali modifiche che il chiamante desidera eseguire.
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // Sostituire campo
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// Crea un nuovo oggetto <see cref="Binding"/> copiando tutte le proprietà da un altro oggetto <see cref="Binding"/>.
        /// </summary>
        /// <param name="binding">Oggetto <see cref="Binding"/> da cui verranno copiati i valori delle proprietà</param>
        /// <returns>Nuovo oggetto <see cref="Binding"/>.</returns>
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
