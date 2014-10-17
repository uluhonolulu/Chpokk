namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     <see cref="DataField"/>에서 작업을 수행하기 위한 확장 메서드를 제공합니다.
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// <see cref="DataField" />의 <see cref="TextBox" /> 컨트롤을 다른 컨트롤로 바꾸고 바인딩을 업데이트합니다.
        /// </summary>
        /// <param name="field"><see cref="TextBox"/>가 표시되는 <see cref="DataField"/>입니다.</param>
        /// <param name="newControl"><see cref="DataField.Content" />로 설정할 새 컨트롤입니다.</param>
        /// <param name="dataBindingProperty">데이터를 바인딩하는 데 사용되는 컨트롤 속성입니다.</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// <see cref="DataField" />의 <see cref="TextBox" /> 컨트롤을 다른 컨트롤로 바꾸고 바인딩을 업데이트합니다.
        /// </summary>
        /// <param name="field"><see cref="TextBox"/>가 표시되는 <see cref="DataField"/>입니다.</param>
        /// <param name="newControl"><see cref="DataField.Content" />로 설정할 새 컨트롤입니다.</param>
        /// <param name="dataBindingProperty">데이터를 바인딩하는 데 사용되는 컨트롤 속성입니다.</param>        
        /// <param name="bindingSetupFunction">
        ///  새로 생성된 바인딩이 <paramref name="newControl"/>에 적용되기 전에 바인딩에 대한 매개 변수를 변경하는 데 사용할 수 있는 선택적 <see cref="Action"/>입니다.
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

            // 기존 바인딩을 복사한 후 호출자가 수행하려는 변경에 대한 bindingSetupFunction으로 전송하여 새 바인딩을 구성합니다.
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // 필드 바꾸기
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// 다른 <see cref="Binding"/> 개체에서 모든 속성을 복사하여 새 <see cref="Binding"/> 개체를 만듭니다.
        /// </summary>
        /// <param name="binding">속성 값을 복사할 <see cref="Binding"/>입니다.</param>
        /// <returns>새 <see cref="Binding"/> 개체입니다.</returns>
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
