using System.IO;
using System.Text;
using System.Threading.Tasks;
using Arractas;
using Chpokk.Tests.Exploring;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class LoadingProjectData : BaseQueryTest<PhysicalCodeFileContext, IntelData> {
		private const string CODE = "code";

		public override IntelData Act() {
			var loader = Context.Container.Get<IntelDataLoader>();
			return loader.CreateIntelData(Context.ProjectPath, Context.FilePath, CODE);
		}

		[Test]
		public void LoadsThisPath() {
			Result.CodeFilePath.ShouldBe(Context.FilePath);
		}

		[Test]
		public void LoadsCurrentCode() {
			Result.Code.ShouldBe(CODE);
		}

		[Test]
		public void OtherSourcesShouldNotIncludeCurrentSource() {
			Result.OtherContent.ShouldNotContain(CODE);
		}

		[Test]
		public void ReferencePathsShouldIncludeMscorlib() {
			Result.ReferencePaths.ShouldContain(s => s.EndsWith("mscorlib.dll") && File.Exists(s));
		}
	}
}
