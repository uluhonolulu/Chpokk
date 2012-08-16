using System;
using System.Linq;
using FubuCore.Util;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Urls;

namespace ChpokkWeb.Infrastructure {

	public interface IModelUrlResolver {
		string GetUrlForInputModelName(string modelType);
	}
	public class ModelUrlResolutionCache : IModelUrlResolver {
		static Cache<string, string> _inputModelTypeCache;

		public ModelUrlResolutionCache(IUrlRegistry urlRegistry, BehaviorGraph graph) {
			if (_inputModelTypeCache == null)
				_inputModelTypeCache = new Cache<string, string>(inputModel => {
				                                                               	var inputType = Type.GetType(inputModel)
				                                                               	                ??
				                                                               	                graph.Routes.Where(act => act.Input != null
				                                                               	                                          &&
				                                                               	                                          act.Input.InputType.Name == inputModel)
				                                                               	                	.First().Input.InputType;
				                                                               	var parameters = new RouteParameters();
				                                                               	parameters["ItemId"] = string.Empty;
				                                                               	return urlRegistry.UrlFor(inputType, parameters);
				});
		}

		public string GetUrlForInputModelName(string modelType) {
			return _inputModelTypeCache[modelType];
		}
	}
}