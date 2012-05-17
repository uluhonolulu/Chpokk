using System;
using System.Collections.Generic;
using System.Text;
using FubuMVC.Core.Assets.Files;

namespace ChpokkWeb.Infrastructure
{
    public class UrlTransformationException : Exception
    {
        string _message;

        public UrlTransformationException(string message)
            : base(message)
        {
        }

        public UrlTransformationException(string contents, IEnumerable<AssetFile> files)
        {
            var message = new StringBuilder("A url was not resolved");
            foreach (var assetFile in files)
            {
                message.AppendFormat("File: {0}", assetFile.Name);
                message.AppendLine();
            }

            _message = message.ToString();
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}