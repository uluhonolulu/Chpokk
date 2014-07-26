using FubuMVC.Core;

namespace ChpokkWeb.Infrastructure {
	public class DummyModel : IDontNeedActionsModel {}

	public interface IDontNeedActionsModel {}

	[UrlPattern("404")]
	public class A404Model: IDontNeedActionsModel {}
}