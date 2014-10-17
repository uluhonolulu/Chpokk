namespace $safeprojectname$.LoginUI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Класс <see cref="ChildWindow"/>, управляющий процессом регистрации.
    /// </summary>
    public partial class LoginRegistrationWindow : ChildWindow
    {
        private IList<OperationBase> possiblyPendingOperations = new List<OperationBase>();

        /// <summary>
        /// Создает новый экземпляр класса <see cref="LoginRegistrationWindow"/>.
        /// </summary>
        public LoginRegistrationWindow()
        {
            InitializeComponent();

            this.registrationForm.SetParentWindow(this);
            this.loginForm.SetParentWindow(this);

            this.LayoutUpdated += this.GoToInitialState;
            this.LayoutUpdated += this.UpdateTitle;
        }

        /// <summary>
        /// Инициализирует экземпляр <see cref="VisualStateManager"/> для этого компонента, переводя его в состояние "AtLogin".
        /// </summary>
        private void GoToInitialState(object sender, EventArgs eventArgs)
        {
            this.LayoutUpdated -= this.GoToInitialState;
            VisualStateManager.GoToState(this, "AtLogin", true);
        }

        /// <summary>
        /// Обеспечивает правильность визуального состояния и фокуса при открытии окна.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            this.NavigateToLogin();
        }

        /// <summary>
        /// Обновляет заголовок окна в соответствии с текущей отображаемой панелью (регистрации или входа).
        /// </summary>
        private void UpdateTitle(object sender, EventArgs eventArgs)
        {
            this.Title = (this.registrationForm.Visibility == Visibility.Visible) ?
                ApplicationStrings.RegistrationWindowTitle :
                ApplicationStrings.LoginWindowTitle;
        }

        /// <summary>
        /// Уведомляет окно <see cref="LoginRegistrationWindow"/> о том, что его закрытие возможно только после завершения или отмены операции <paramref name="operation"/>.
        /// </summary>
        /// <param name="operation">Операция в режиме ожидания для наблюдения</param>
        public void AddPendingOperation(OperationBase operation)
        {
            this.possiblyPendingOperations.Add(operation);
        }

        /// <summary>
        /// Вызывает переход <see cref="VisualStateManager"/> в состояние "AtLogin".
        /// </summary>
        public virtual void NavigateToLogin()
        {
            VisualStateManager.GoToState(this, "AtLogin", true);
            this.loginForm.SetInitialFocus();
        }

        /// <summary>
        /// Вызывает переход <see cref="VisualStateManager"/> в состояние "AtRegistration".
        /// </summary>
        public virtual void NavigateToRegistration()
        {
            VisualStateManager.GoToState(this, "AtRegistration", true);
            this.registrationForm.SetInitialFocus();
        }

        /// <summary>
        /// Предотвращает закрытие окна до завершения выполнения операции
        /// </summary>
        private void LoginWindow_Closing(object sender, CancelEventArgs eventArgs)
        {
            foreach (OperationBase operation in this.possiblyPendingOperations)
            {
                if (!operation.IsComplete)
                {
                    if (operation.CanCancel)
                    {
                        operation.Cancel();
                    }
                    else
                    {
                        eventArgs.Cancel = true;
                    }
                }
            }
        }
    }
}