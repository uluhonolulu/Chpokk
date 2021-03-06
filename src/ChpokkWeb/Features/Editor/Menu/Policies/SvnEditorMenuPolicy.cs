﻿using System.IO;
using ChpokkWeb.Features.Remotes.SVN;
using ChpokkWeb.Features.RepositoryManagement;

namespace ChpokkWeb.Features.Editor.Menu.Policies {
	public class SvnEditorMenuPolicy: VersionControlEditorMenuPolicy {
		private readonly SvnDetectionPolicy _detectionPolicy;
		public SvnEditorMenuPolicy(SvnDetectionPolicy detectionPolicy) {
			_detectionPolicy = detectionPolicy;
		}

		public override bool Matches(string repositoryPath) {
			return _detectionPolicy.Matches(repositoryPath);
		}
	}
}