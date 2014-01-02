using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChpokkWeb.Features.CustomerDevelopment.TrialSignup;
using Microsoft.AspNet.SignalR;
using StructureMap;

namespace ChpokkWeb.Infrastructure {
	public class StructureMapResolver : DefaultDependencyResolver {
		private readonly IContainer _container;

		public StructureMapResolver(IContainer container) {
			_container = container;
		}

		public override object GetService(Type serviceType) {
			object service = null;
			if (!serviceType.IsAbstract && !serviceType.IsInterface && serviceType.IsClass) {
				// Concrete type resolution
				if (serviceType == typeof(UserHub)) {
					var test = _container.TryGetInstance<UserHub>();
					Console.WriteLine(test);
				}
				try {
					service = _container.GetInstance(serviceType);
				}
				catch (Exception exception) {
					throw new InvalidOperationException("Error creating an instance of type " + serviceType, exception);
				}
			}
			else {
				// Other type resolution with base fallback
				service = _container.TryGetInstance(serviceType) ?? base.GetService(serviceType);
			}
			return service;
		}

		public override IEnumerable<object> GetServices(Type serviceType) {
			var objects = _container.GetAllInstances(serviceType).Cast<object>();
			return objects.Concat(base.GetServices(serviceType));
		}
	}
}