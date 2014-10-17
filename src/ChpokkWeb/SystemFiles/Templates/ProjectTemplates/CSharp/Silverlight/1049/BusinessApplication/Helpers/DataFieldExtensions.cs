namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     Содержит методы расширения для выполнения операций над объектом <see cref="DataField"/>.
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// Заменяет элемент управления <see cref="TextBox" /> для <see cref="DataField" /> другим элементом управления и обновляет привязку.
        /// </summary>
        /// <param name="field">Объект <see cref="DataField"/>, элемент управления <see cref="TextBox"/> которого нужно заменить.</param>
        /// <param name="newControl">Новый элемент управления, который должен быть назначен в качестве <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Свойство элемента управления, которое служит для привязки данных.</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// Заменяет элемент управления <see cref="TextBox" /> для <see cref="DataField" /> другим элементом управления и обновляет привязку.
        /// </summary>
        /// <param name="field">Объект <see cref="DataField"/>, элемент управления <see cref="TextBox"/> которого нужно заменить.</param>
        /// <param name="newControl">Новый элемент управления, который должен быть назначен в качестве <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Свойство элемента управления, которое служит для привязки данных.</param>        
        /// <param name="bindingSetupFunction">
        ///  Необязательный параметр <see cref="Action"/>, который может быть использован для изменения параметров во вновь созданной привязке, прежде чем она будет применена к <paramref name="newControl"/>
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

            // Создает новую привязку путем копирования существующей и передачи ее функции bindingSetupFunction, в которой в вызывающем коде можно произвести любые необходимые изменения.
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // Замена поля
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// Создает новый объект <see cref="Binding"/>, копируя все свойства из другого объекта <see cref="Binding"/>.
        /// </summary>
        /// <param name="binding"><see cref="Binding"/>, из которого будут скопированы значения свойств</param>
        /// <returns>Новый объект <see cref="Binding"/>.</returns>
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
