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
    /// 控制註冊程序的 <see cref="ChildWindow"/> 類別。
    /// </summary>
    public partial class LoginRegistrationWindow : ChildWindow
    {
        private IList<OperationBase> possiblyPendingOperations = new List<OperationBase>();

        /// <summary>
        /// 建立新 <see cref="LoginRegistrationWindow"/> 執行個體。
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
        /// 初始化此元件的 <see cref="VisualStateManager"/>，方法是將它置於 "AtLogin" 狀態。
        /// </summary>
        private void GoToInitialState(object sender, EventArgs eventArgs)
        {
            this.LayoutUpdated -= this.GoToInitialState;
            VisualStateManager.GoToState(this, "AtLogin", true);
        }

        /// <summary>
        /// 確保視窗開啟時可見狀態和焦點正確無誤。
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            this.NavigateToLogin();
        }

        /// <summary>
        /// 依據目前顯示的是哪個面板 (註冊 / 登入) 來更新視窗標題。
        /// </summary>
        private void UpdateTitle(object sender, EventArgs eventArgs)
        {
            this.Title = (this.registrationForm.Visibility == Visibility.Visible) ?
                ApplicationStrings.RegistrationWindowTitle :
                ApplicationStrings.LoginWindowTitle;
        }

        /// <summary>
        /// 通知 <see cref="LoginRegistrationWindow"/> 視窗它只能在 <paramref name="operation"/> 已完成或可以取消的情況下關閉。
        /// </summary>
        /// <param name="operation">要監視的暫止作業</param>
        public void AddPendingOperation(OperationBase operation)
        {
            this.possiblyPendingOperations.Add(operation);
        }

        /// <summary>
        /// 使 <see cref="VisualStateManager"/> 變更為 "AtLogin" 狀態。
        /// </summary>
        public virtual void NavigateToLogin()
        {
            VisualStateManager.GoToState(this, "AtLogin", true);
            this.loginForm.SetInitialFocus();
        }

        /// <summary>
        /// 使 <see cref="VisualStateManager"/> 變更為 "AtRegistration" 狀態。
        /// </summary>
        public virtual void NavigateToRegistration()
        {
            VisualStateManager.GoToState(this, "AtRegistration", true);
            this.registrationForm.SetInitialFocus();
        }

        /// <summary>
        /// 防止視窗在作業進行時關閉
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