using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.AutoScaling.Model;
using ChpokkWeb.Features.Authentication;
using ChpokkWeb.Features.CustomerDevelopment;
using ChpokkWeb.Features.MainScreen;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;

namespace ChpokkWeb.Infrastructure.Logging
{
	public class TimingBehavior : StopwatchBehavior {
		private readonly ActivityTracker _tracker;
		private readonly ICurrentChain _currentChain;
		public TimingBehavior(ActivityTracker tracker, ICurrentChain currentChain) : base(timeSpent =>
		{
			var info = currentChain.Current.ToString() + " | " +
				(currentChain.Current.FirstCall() != null? currentChain.Current.FirstCall().ToString() : "null") + " | " +
			           currentChain.OriginatingChain.ToString();
			if (timeSpent > 11) 
				tracker.Record("Timing for " + info + "> " + timeSpent);
		}) {
			_currentChain = currentChain;
			_tracker = tracker;
		}

		protected override void invoke(Action action) {
			var info = _currentChain.Current + " | " +
				(_currentChain.Current.FirstCall() != null ? _currentChain.Current.FirstCall().ToString() : "null") + " | " +
					   _currentChain.OriginatingChain;
			_tracker.Record("Calling " + info);
			base.invoke(action);
		}
	}


	[ConfigurationType(ConfigurationType.InjectNodes)]
	public class TimingConfiguration : IConfigurationAction
	{
		public void Configure(BehaviorGraph graph)
		{
			var chain = graph.BehaviorFor(typeof(MainDummyModel));
			chain.InsertFirst(Wrapper.For<TimingBehavior>());
		}
	}
}