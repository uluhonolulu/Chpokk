﻿using System;
using System.Collections.Generic;
using System.IO;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Testing;
using Gallio.Runner;
using Gallio.Runtime;
using Gallio.Runtime.ConsoleSupport;
using Gallio.Runtime.Logging;
using Gallio.Runtime.ProgressMonitoring;
using MbUnit.Framework;
using Shouldly;
using Action = Gallio.Common.Action;
using FubuCore;

namespace Chpokk.Tests.Testing {
	[TestFixture]
	public class RunningTests : BaseCommandTest<FakeConsoleContext> {
		[Test]
		public void OutputShouldContainFailedTests() {
			WebConsole.Log.ShouldContain(s => s.StartsWith("[failed] Test SomeTests/FailingTests/JustFails")); 
		}

		[Test]
		public void OutputShouldContainPassedTests() {
			WebConsole.Log.ShouldContain(s => s.StartsWith("[passed] Test SomeTests/PassingTest/ImEmpty")); //should contain passed tests
		}

		[Test]
		public void WhatsTheType() {
			var manager = RuntimeAccessor.ServiceLocator.Resolve<ITestRunnerManager>();
			var type = manager.GetType();
			Console.WriteLine(type.FullName);
		}

		public override void Act() {
			var testAssemblies = new[] {@"D:\Projects\Chpokk\src\SomeTests\bin\Debug\SomeTests.dll"};
			var webConsole = WebConsole;
			var tester = Context.Container.Get<Tester>();
			tester.RunTheTests(webConsole, testAssemblies);
			Console.WriteLine();
			Console.WriteLine("Here's the log:");
			webConsole.Log.Each((s, i) => Console.WriteLine(i.ToString() + ". " + s));			
		}

		private FakeGallioConsole WebConsole {
			get { return ((FakeGallioConsole) Context.Container.Get<IRichConsole>()); }
		}
	}

	public class FakeConsoleContext: SimpleConfiguredContext {
		protected override void ConfigureFubuRegistry(ChpokkWeb.ConfigureFubuMVC registry) {
			base.ConfigureFubuRegistry(registry);
			registry.Services(serviceRegistry => serviceRegistry.ReplaceService<IRichConsole, FakeGallioConsole>());
		}
	}

		public class FakeGallioConsole: IRichConsole {
			public FakeGallioConsole() { 
				SyncRoot = new object();
				Width = 80;
				Out = Console.Out;
				//Error = Console.Error;
			}
			public void ResetColor() {}
			public void SetFooter(Action showFooter, Action hideFooter) {}
			public void Write(char c) {}
			public void Write(string str) {}
			public void WriteLine() {
				Console.WriteLine();
			}
			public void WriteLine(string str) {
				Console.WriteLine(str);
				Log.Add(str + " -- " + ForegroundColor);
			}
			public object SyncRoot { get; private set; }
			public bool IsCancelationEnabled { get; set; }
			public bool IsCanceled { get; set; }
			public bool IsRedirected { get; private set; }
			public TextWriter Error { get; private set; }
			public TextWriter Out { get; private set; }
			public ConsoleColor ForegroundColor { get; set; }
			public int CursorLeft { get; set; }
			public int CursorTop { get; set; }
			public string Title { get; set; }
			public int Width { get; private set; }
			public bool FooterVisible { get; set; }
			public event EventHandler Cancel;

			public IList<string> Log = new List<string>();
		}	
	

}
