namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     提供扩展方法以对 <see cref="DataField"/> 执行操作。
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// 将 <see cref="DataField" /> 的 <see cref="TextBox" /> 控件替换为另一个控件并更新绑定。
        /// </summary>
        /// <param name="field">将替换其 <see cref="TextBox"/> 的 <see cref="DataField"/>。</param>
        /// <param name="newControl">将设置为 <see cref="DataField.Content" /> 的新控件。</param>
        /// <param name="dataBindingProperty">将用于数据绑定的控件属性。</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// 将 <see cref="DataField" /> 的 <see cref="TextBox" /> 控件替换为另一个控件并更新绑定。
        /// </summary>
        /// <param name="field">将替换其 <see cref="TextBox"/> 的 <see cref="DataField"/>。</param>
        /// <param name="newControl">将设置为 <see cref="DataField.Content" /> 的新控件。</param>
        /// <param name="dataBindingProperty">将用于数据绑定的控件属性。</param>        
        /// <param name="bindingSetupFunction">
        /// 一个可选 <see cref="Action"/>，可以用于在将新生成的绑定应用于 <paramref name="newControl"/> 之前更改该绑定的参数
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

            // 通过复制现有绑定并将其发送给 bindingSetupFunction，为调用方要执行的任何更改构造新绑定。
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // 替换字段
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// 通过从另一个 <see cref="Binding"/> 对象复制所有属性来创建新 <see cref="Binding"/> 对象。
        /// </summary>
        /// <param name="binding">将从其复制属性值的 <see cref="Binding"/></param>
        /// <returns>新 <see cref="Binding"/> 对象。</returns>
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
