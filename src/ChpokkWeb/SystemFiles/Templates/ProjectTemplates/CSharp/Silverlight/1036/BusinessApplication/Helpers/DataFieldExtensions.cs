namespace $safeprojectname$
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    ///     Fournit des méthodes d'extension permettant d'exécuter des opérations sur un <see cref="DataField"/>.
    /// </summary>
    public static class DataFieldExtensions
    {
        /// <summary>
        /// Remplace le contrôle <see cref="TextBox" /> d'un <see cref="DataField" /> par un autre contrôle et met à jour les liaisons.
        /// </summary>
        /// <param name="field"><see cref="DataField"/> dont <see cref="TextBox"/> sera remplacé.</param>
        /// <param name="newControl">Nouveau contrôle que vous allez définir en tant que <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Propriété du contrôle qui sera utilisée pour la liaison de données.</param>        
        public static void ReplaceTextBox(this DataField field, FrameworkElement newControl, DependencyProperty dataBindingProperty)
        {
            field.ReplaceTextBox(newControl, dataBindingProperty, null);
        }

        /// <summary>
        /// Remplace le contrôle <see cref="TextBox" /> d'un <see cref="DataField" /> par un autre contrôle et met à jour les liaisons.
        /// </summary>
        /// <param name="field"><see cref="DataField"/> dont <see cref="TextBox"/> sera remplacé.</param>
        /// <param name="newControl">Nouveau contrôle que vous allez définir en tant que <see cref="DataField.Content" />.</param>
        /// <param name="dataBindingProperty">Propriété du contrôle qui sera utilisée pour la liaison de données.</param>        
        /// <param name="bindingSetupFunction">
        ///  <see cref="Action"/> optionnelle que vous pouvez utiliser pour modifier des paramètres sur la liaison qui vient d'être créée avant qu'elle soit appliquée à <paramref name="newControl"/>
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

            // Construire une nouvelle liaison en en copiant une existante, et en l'envoyant à bindingSetupFunction pour que l'appelant puissent apporter les modifications qu'il souhaite.
            Binding newBinding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy();

            if (bindingSetupFunction != null)
            {
                bindingSetupFunction(newBinding);
            }

            // Remplacer le champ
            newControl.SetBinding(dataBindingProperty, newBinding);
            field.Content = newControl;
        }

        /// <summary>
        /// Crée un nouvel objet <see cref="Binding"/> en copiant toutes les propriétés d'un autre objet <see cref="Binding"/>.
        /// </summary>
        /// <param name="binding"><see cref="Binding"/> à partir duquel les valeurs de propriétés seront copiées</param>
        /// <returns>Nouvel objet <see cref="Binding"/>.</returns>
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
