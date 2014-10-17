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
    /// <see cref="ChildWindow"/> Klasse, die den Registrierungsprozess steuert.
    /// </summary>
    public partial class LoginRegistrationWindow : ChildWindow
    {
        private IList<OperationBase> possiblyPendingOperations = new List<OperationBase>();

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="LoginRegistrationWindow"/>.
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
        /// Initialisiert <see cref="VisualStateManager"/> für diese Komponente, indem sie auf den Zustand "AtLogin" gesetzt wird.
        /// </summary>
        private void GoToInitialState(object sender, EventArgs eventArgs)
        {
            this.LayoutUpdated -= this.GoToInitialState;
            VisualStateManager.GoToState(this, "AtLogin", true);
        }

        /// <summary>
        /// Stellt sicher, dass der visuelle Zustand und der Fokus korrekt sind, wenn das Fenster geöffnet wird.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            this.NavigateToLogin();
        }

        /// <summary>
        /// Aktualisiert den Fenstertitel abhängig davon, welcher Bereich (Registrierung /Anmeldung) derzeit angezeigt wird.
        /// </summary>
        private void UpdateTitle(object sender, EventArgs eventArgs)
        {
            this.Title = (this.registrationForm.Visibility == Visibility.Visible) ?
                ApplicationStrings.RegistrationWindowTitle :
                ApplicationStrings.LoginWindowTitle;
        }

        /// <summary>
        /// Benachrichtigt das <see cref="LoginRegistrationWindow"/>-Fenster, dass es nur geschlossen werden kann, wenn <paramref name="operation"/> abgeschlossen ist, oder abgebrochen werden kann.
        /// </summary>
        /// <param name="operation">Der noch ausstehende Vorgang, der überwacht werden soll</param>
        public void AddPendingOperation(OperationBase operation)
        {
            this.possiblyPendingOperations.Add(operation);
        }

        /// <summary>
        /// Bewirkt, dass <see cref="VisualStateManager"/> auf den Zustand "AtLogin" wechselt.
        /// </summary>
        public virtual void NavigateToLogin()
        {
            VisualStateManager.GoToState(this, "AtLogin", true);
            this.loginForm.SetInitialFocus();
        }

        /// <summary>
        /// Bewirkt, dass <see cref="VisualStateManager"/> auf den Zustand "AtRegistration" wechselt.
        /// </summary>
        public virtual void NavigateToRegistration()
        {
            VisualStateManager.GoToState(this, "AtRegistration", true);
            this.registrationForm.SetInitialFocus();
        }

        /// <summary>
        /// Verhindert, dass das Fenster geschlossen wird, während noch Vorgänge ausgeführt werden
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