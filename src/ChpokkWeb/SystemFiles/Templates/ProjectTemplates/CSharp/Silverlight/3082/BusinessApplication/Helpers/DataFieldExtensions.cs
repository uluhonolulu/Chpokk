namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     Proporciona métodos de extensión para realizar operaciones en un <see cref="DataField"/>.
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// Sustituye un control <see cref="TextBox" /> de <see cref="DataField" /> por otro control y actualiza los enlaces.
        /// </summary>
        /// <param name="field"><see cref="DataField"/> cuyo <see cref="TextBox"/> se va a sustituir.</param>
        /// <param name="newControl">Nuevo control que va a establecer como <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Propiedad del control que se utilizará para el enlace de datos.</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// Sustituye un control <see cref="TextBox" /> de <see cref="DataField" /> por otro control y actualiza los enlaces.
        /// </summary>
        /// <param name="field"><see cref="DataField"/> cuyo <see cref="TextBox"/> se va a sustituir.</param>
        /// <param name="newControl">Nuevo control que va a establecer como <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Propiedad del control que se utilizará para el enlace de datos.</param>        
        /// <param name="bindingSetupFunction">
        ///  <see cref="Action"/> opcional que se puede utilizar para cambiar parámetros en el enlace recién generado antes de aplicarlo a <paramref name="newControl"/>
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

            // Construya un nuevo enlace al copiar uno existente y enviarlo a bindingSetupFunction para cualquier cambio que el autor de la llamada desee realizar.
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // Sustituir campo
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// Crea un nuevo objeto <see cref="Binding"/> al copiar todas las propiedades de otro objeto <see cref="Binding"/>.
        /// </summary>
        /// <param name="binding"><see cref="Binding"/> del que se copiarán los valores de las propiedades</param>
        /// <returns>Nuevo objeto <see cref="Binding"/>.</returns>
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
