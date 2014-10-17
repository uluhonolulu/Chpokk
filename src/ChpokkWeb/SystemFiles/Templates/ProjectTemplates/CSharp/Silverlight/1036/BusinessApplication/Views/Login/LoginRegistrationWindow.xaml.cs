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
    /// Classe <see cref="ChildWindow"/> qui contrôle le processus d'inscription.
    /// </summary>
    public partial class LoginRegistrationWindow : ChildWindow
    {
        private IList<OperationBase> possiblyPendingOperations = new List<OperationBase>();

        /// <summary>
        /// Crée une nouvelle instance <see cref="LoginRegistrationWindow"/>.
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
        /// Initialise le <see cref="VisualStateManager"/> pour ce composant en le plaçant dans l'état « AtLogin ».
        /// </summary>
        private void GoToInitialState(object sender, EventArgs eventArgs)
        {
            this.LayoutUpdated -= this.GoToInitialState;
            VisualStateManager.GoToState(this, "AtLogin", true);
        }

        /// <summary>
        /// Garantit que l'état visuel et le focus sont corrects lors de l'ouverture de la fenêtre.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            this.NavigateToLogin();
        }

        /// <summary>
        /// Met à jour le titre de la fenêtre en fonction du panneau (inscription/connexion) actuellement affiché.
        /// </summary>
        private void UpdateTitle(object sender, EventArgs eventArgs)
        {
            this.Title = (this.registrationForm.Visibility == Visibility.Visible) ?
                ApplicationStrings.RegistrationWindowTitle :
                ApplicationStrings.LoginWindowTitle;
        }

        /// <summary>
        /// Avertit la fenêtre <see cref="LoginRegistrationWindow"/> qu'elle peut se fermer uniquement si <paramref name="operation"/> est terminée ou peut être annulée.
        /// </summary>
        /// <param name="operation">Opération en attente à surveiller</param>
        public void AddPendingOperation(OperationBase operation)
        {
            this.possiblyPendingOperations.Add(operation);
        }

        /// <summary>
        /// Entraîne le passage de <see cref="VisualStateManager"/> dans l'état « AtLogin ».
        /// </summary>
        public virtual void NavigateToLogin()
        {
            VisualStateManager.GoToState(this, "AtLogin", true);
            this.loginForm.SetInitialFocus();
        }

        /// <summary>
        /// Entraîne le passage de <see cref="VisualStateManager"/> dans l'état « AtRegistration ».
        /// </summary>
        public virtual void NavigateToRegistration()
        {
            VisualStateManager.GoToState(this, "AtRegistration", true);
            this.registrationForm.SetInitialFocus();
        }

        /// <summary>
        /// Empêche la fermeture de la fenêtre pendant que des opérations sont en cours
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