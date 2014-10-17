'
'
' 이 파일은 이 프로젝트에 적용되는 SuppressMessage 특성을 유지 관리하기 위해 
' 코드 분석에서 사용하는 파일입니다.
' 프로젝트 수준 비표시 오류(Suppression)는 대상이 없거나 특정 대상에 지정되며 
' 네임스페이스, 형식, 멤버 등으로 범위가 지정됩니다.
'
' 이 파일에 비표시 오류(Suppression)를 추가하려면 오류 목록에서 
' 메시지를 마우스 오른쪽 단추로 클릭하고 "메시지 표시 안 함"을 가리킨 후 
' "프로젝트 비표시 오류(Suppression) 파일"을 클릭합니다.
' 이 파일에 수동으로 비표시 오류(Suppression)를 추가할 필요가 없습니다.

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope:="member", Target:="$safeprojectname$.Web.User.#Global_System_Security_Principal_IIdentity_Name")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope:="type", Target:="$safeprojectname$.Web.UserRegistrationContext+IUserRegistrationServiceContract")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Datas", Scope:="member", Target:="$safeprojectname$.Web.UserRegistrationContext.#RegistrationDatas")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="$safeprojectname$.Web.UserRegistrationContext.#GetUsersQuery()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope:="member", Target:="$safeprojectname$.Web.UserRegistrationContext.#CreateUser($safeprojectname$.Web.RegistrationData,System.String,System.Action`1<System.ServiceModel.DomainServices.Client.InvokeOperation`1<$safeprojectname$.Web.CreateUserStatus>>,System.Object)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope:="member", Target:="$safeprojectname$.Web.User.#Global_System_Security_Principal_IIdentity_AuthenticationType")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="$safeprojectname$.Web.RegistrationData.#ToLoginParameters()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.Web.RegistrationData.#PasswordConfirmationAccessor")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.Web.RegistrationData.#PasswordAccessor")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="$safeprojectname$.Web.AuthenticationContext+IAuthenticationServiceContract.#EndLogout(System.IAsyncResult)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="$safeprojectname$.Web.AuthenticationContext+IAuthenticationServiceContract.#EndLogin(System.IAsyncResult)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="$safeprojectname$.Web.AuthenticationContext+IAuthenticationServiceContract.#BeginLogout(System.AsyncCallback,System.Object)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="$safeprojectname$.Web.AuthenticationContext+IAuthenticationServiceContract.#BeginLogin(System.String,System.String,System.Boolean,System.String,System.AsyncCallback,System.Object)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope:="type", Target:="$safeprojectname$.Web.AuthenticationContext+IAuthenticationServiceContract")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="$safeprojectname$.Web.AuthenticationContext.#LogoutQuery()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="$safeprojectname$.Web.AuthenticationContext.#LoginQuery(System.String,System.String,System.Boolean,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="$safeprojectname$.Web.AuthenticationContext.#GetUserQuery()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.RegistrationForm.#RegisterForm_AutoGeneratingField(System.Object,System.Windows.Controls.DataFormAutoGeneratingFieldEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.RegistrationForm.#RegisterButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.RegistrationForm.#CreateComboBoxWithSecurityQuestions()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.RegistrationForm.#CancelButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.RegistrationForm.#BackToLogin_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginStatus.#LogoutButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginStatus.#LoginButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginStatus.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="$safeprojectname$.LoginUI.LoginStatus")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginRegistrationWindow.#NavigateToLogin()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginRegistrationWindow.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="$safeprojectname$.LoginUI.LoginRegistrationWindow")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginInfo.#ToLoginParameters()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginInfo.#PasswordAccessor")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginInfo.#CurrentLoginOperation")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="LogIn", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginInfo.#CanLogIn")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="$safeprojectname$.LoginUI.LoginInfo")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginForm.#RegisterNow_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginForm.#LoginForm_KeyDown(System.Object,System.Windows.Input.KeyEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginForm.#LoginForm_AutoGeneratingField(System.Object,System.Windows.Controls.DataFormAutoGeneratingFieldEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginForm.#LoginButton_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.LoginUI.LoginForm.#CancelButton_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="$safeprojectname$.LoginUI.LoginForm")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope:="member", Target:="$safeprojectname$.TargetNullValueConverter.#Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId:="string", Scope:="member", Target:="$safeprojectname$.StringFormatValueConverter.#.ctor(System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="$safeprojectname$.ApplicationResources.#Errors")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="$safeprojectname$.ApplicationResources.#Strings")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.MainPage.#ContentFrame_NavigationFailed(System.Object,System.Windows.Navigation.NavigationFailedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.MainPage.#ContentFrame_Navigated(System.Object,System.Windows.Navigation.NavigationEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="$safeprojectname$.MainPage.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.ErrorWindow.#OKButton_Click(System.Object,System.Windows.RoutedEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="$safeprojectname$.ErrorWindow.#.ctor(System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope:="member", Target:="$safeprojectname$.DataFieldExtensions.#ReplaceTextBox(System.Windows.Controls.DataField,System.Windows.FrameworkElement,System.Windows.DependencyProperty,System.Action`1<System.Windows.Data.Binding>)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.App.#Application_UnhandledException(System.Object,System.Windows.ApplicationUnhandledExceptionEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="$safeprojectname$.App.#Application_Startup(System.Object,System.Windows.StartupEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="namespace", Target:="$safeprojectname$.LoginUI")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="$safeprojectname$.Controls")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Scope:="member", Target:="$safeprojectname$.Web.User.#Global_System_Security_Principal_IPrincipal_Identity")> 

