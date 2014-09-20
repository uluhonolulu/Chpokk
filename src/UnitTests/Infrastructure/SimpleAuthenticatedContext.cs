namespace UnitTests.Infrastructure {
	public class SimpleAuthenticatedContext: SimpleConfiguredContext {
		public override void Create() {
			base.Create();
			this.FakeSecurityContext.UserName = "ulu";
		}

		public override void Dispose() {
			base.Dispose();
			this.FakeSecurityContext.UserName = null;
		}
	}
}
