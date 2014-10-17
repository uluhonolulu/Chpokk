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
    /// Clase <see cref="ChildWindow"/> que controla el proceso de registro.
    /// </summary>
    public partial class LoginRegistrationWindow : ChildWindow
    {
        private IList<OperationBase> possiblyPendingOperations = new List<OperationBase>();

        /// <summary>
        /// Crea una nueva instancia de <see cref="LoginRegistrationWindow"/>.
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
        /// Inicializa el <see cref="VisualStateManager"/> de este componente al establecerlo en estado "AtLogin".
        /// </summary>
        private void GoToInitialState(object sender, EventArgs eventArgs)
        {
            this.LayoutUpdated -= this.GoToInitialState;
            VisualStateManager.GoToState(this, "AtLogin", true);
        }

        /// <summary>
        /// Garantiza que el estado visual y el enfoque sean correctos al abrir la ventana.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            this.NavigateToLogin();
        }

        /// <summary>
        /// Actualiza el título de la ventana según el panel (registro o inicio de sesión) que se esté mostrando en ese momento.
        /// </summary>
        private void UpdateTitle(object sender, EventArgs eventArgs)
        {
            this.Title = (this.registrationForm.Visibility == Visibility.Visible) ?
                ApplicationStrings.RegistrationWindowTitle :
                ApplicationStrings.LoginWindowTitle;
        }

        /// <summary>
        /// Notifica a la ventana <see cref="LoginRegistrationWindow"/> que solo se puede cerrar si la <paramref name="operation"/> termina o se puede cancelar.
        /// </summary>
        /// <param name="operation">Operación pendiente que se va a supervisar</param>
        public void AddPendingOperation(OperationBase operation)
        {
            this.possiblyPendingOperations.Add(operation);
        }

        /// <summary>
        /// Hace que el <see cref="VisualStateManager"/> cambie al estado "AtLogin".
        /// </summary>
        public virtual void NavigateToLogin()
        {
            VisualStateManager.GoToState(this, "AtLogin", true);
            this.loginForm.SetInitialFocus();
        }

        /// <summary>
        /// Hace que el <see cref="VisualStateManager"/> cambie al estado "AtRegistration".
        /// </summary>
        public virtual void NavigateToRegistration()
        {
            VisualStateManager.GoToState(this, "AtRegistration", true);
            this.registrationForm.SetInitialFocus();
        }

        /// <summary>
        /// Evita que la ventana se cierre mientras hay operaciones en curso
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