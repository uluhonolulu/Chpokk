namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     提供在 <see cref="DataField"/> 上執行作業的擴充方法。
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// 用另一個控制項取代 <see cref="DataField" />'s <see cref="TextBox" /> 控制項並且更新繫結。
        /// </summary>
        /// <param name="field">要取代其 <see cref="TextBox"/> 的 <see cref="DataField"/>。</param>
        /// <param name="newControl">您要設定成 <see cref="DataField.Content" /> 的新控制項。</param>
        /// <param name="dataBindingProperty">要使用於資料繫結之控制項的屬性。</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// 用另一個控制項取代 <see cref="DataField" />'s <see cref="TextBox" /> 控制項並且更新繫結。
        /// </summary>
        /// <param name="field">要取代其 <see cref="TextBox"/> 的 <see cref="DataField"/>。</param>
        /// <param name="newControl">您要設定成 <see cref="DataField.Content" /> 的新控制項。</param>
        /// <param name="dataBindingProperty">要使用於資料繫結之控制項的屬性。</param>        
        /// <param name="bindingSetupFunction">
        ///  選擇性 <see cref="Action"/>，可用來變更剛產生的繫結上的參數，然後再將它套用到 <paramref name="newControl"/>
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

            // 複製現有的繫結來建構新繫結，並且針對呼叫端要執行的任何變更將它傳遞給 bindingSetupFunction。
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // 取代欄位
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// 從另一個 <see cref="Binding"/> 物件複製所有屬性來建立新 <see cref="Binding"/> 物件。
        /// </summary>
        /// <param name="binding">要複製屬性值的來源 <see cref="Binding"/></param>
        /// <returns>新的 <see cref="Binding"/> 物件。</returns>
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
