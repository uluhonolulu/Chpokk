namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     <see cref="DataField"/> で操作を実行するための拡張メソッドを提供します。
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// <see cref="DataField" /> の <see cref="TextBox" /> コントロールを別のコントロールに置換し、バインディングを更新します。
        /// </summary>
        /// <param name="field"><see cref="TextBox"/> が置換される <see cref="DataField"/> です。</param>
        /// <param name="newControl"><see cref="DataField.Content" /> として設定する新しいコントロールです。</param>
        /// <param name="dataBindingProperty">データ バインディングに使用されるコントロールのプロパティです。</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// <see cref="DataField" /> の <see cref="TextBox" /> コントロールを別のコントロールに置換し、バインディングを更新します。
        /// </summary>
        /// <param name="field"><see cref="TextBox"/> が置換される <see cref="DataField"/> です。</param>
        /// <param name="newControl"><see cref="DataField.Content" /> として設定する新しいコントロールです。</param>
        /// <param name="dataBindingProperty">データ バインディングに使用されるコントロールのプロパティです。</param>        
        /// <param name="bindingSetupFunction">
        ///  <paramref name="newControl"/> に適用される前に新しく生成されたバインディングでパラメーターの変更に使用できる、オプションの <see cref="Action"/> です
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

            // 既存のバインディングをコピーして新しいバインディングを構築し、呼び出し元が実行する変更の bindingSetupFunction に送信します。
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // 置換フィールド
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// 別の <see cref="Binding"/> オブジェクトからすべてのプロパティをコピーして、新しい <see cref="Binding"/> オブジェクトを作成します。
        /// </summary>
        /// <param name="binding">プロパティ値のコピー元の <see cref="Binding"/> です</param>
        /// <returns>新しい <see cref="Binding"/> オブジェクトです。</returns>
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
