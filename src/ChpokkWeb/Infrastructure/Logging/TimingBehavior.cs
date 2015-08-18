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
	public class TimingBehavior : StopwatchBehavior
	{
		public TimingBehavior(ActivityTracker tracker, ICurrentChain currentChain) : base(timeSpent =>
		{
			var info = currentChain.Current.ToString() + "|" +
			           currentChain.OriginatingChain.ToString();
			if (timeSpent > 11) 
				tracker.Record("Timing for " + info + ": " + timeSpent);
		}) {}
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