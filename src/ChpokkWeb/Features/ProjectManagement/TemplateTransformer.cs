using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ChpokkWeb.Features.ProjectManagement {
	public class TemplateTransformer {

		private readonly Regex _regEquality = new Regex("(?<operand1>.*)==(?<operand2>.*)");
		private readonly Regex _regNoEquality = new Regex("(?<operand1>.*)!=(?<operand2>.*)");
		private readonly Regex _regLessThan = new Regex("(?<operand1>.*)<(?<operand2>.*)");
		private readonly Regex _regLessThanOrEqual = new Regex("(?<operand1>.*)<=(?<operand2>.*)");
		private readonly Regex _regGreaterThan = new Regex("(?<operand1>.*)>(?<operand2>.*)");
		private readonly Regex _regGreaterThanOrEqual = new Regex("(?<operand1>.*)>=(?<operand2>.*)");
		private readonly Regex _ifStatementRegEx = new Regex("(?<comparison>\\$if\\$[\\s]*\\([\\s]*(?<expression>[^\\)]*)[\\s]*\\))(?<output>.*)", RegexOptions.Singleline);
		private readonly Regex _elseifRegEx = new Regex("(?<comparison>\\$elseif\\$[\\s]*\\([\\s]*(?<expression>[^\\)]*)[\\s]*\\))(?<output>.*)", RegexOptions.Singleline);
		private readonly Regex _elseRegEx = new Regex("\\$else\\$(?<output>.*)", RegexOptions.Singleline);

		public string Evaluate(string inputText, Dictionary<string, string> replacementValues) {
			//var match = new Regex("(?<comparison>\\$if\\$[\\s]*\\([\\s]*(?<expression>[^\\)]*)[\\s]*\\))(?<output>.*?)\\$endif\\$", RegexOptions.Singleline).Match(inputText);
			var processedIfInstructions = new Regex("(?<comparison>\\$if\\$[\\s]*\\([\\s]*(?<expression>[^\\)]*)[\\s]*\\))(?<output>.*?)\\$endif\\$", RegexOptions.Singleline).Replace(inputText, match1 =>
			{
				string result;
				var statement = match1.Value;
				this.Evaluate(statement, replacementValues, out result);
				return result;
			});
			return ReplaceValues(processedIfInstructions, replacementValues);
		}

		private string ReplaceValues(string inputText, Dictionary<string, string> replacementValues) {
			var result = inputText;
			foreach (var replacementValue in replacementValues) {
				result = result.Replace(replacementValue.Key, replacementValue.Value);
			}
			return result;
		}

		private bool Evaluate(string inputText, Dictionary<string, string> replacementValues, out string outputText) {
			if (!inputText.Contains("$if$")) {
				outputText = inputText;
				return true;
			}
			string input1 = inputText;
			if (input1.Contains("$elseif$"))
				input1 = input1.Substring(0, input1.IndexOf("$elseif$"));
			if (input1.Contains("$else$"))
				input1 = input1.Substring(0, input1.IndexOf("$else$"));
			if (input1.Contains("$endif$"))
				input1 = input1.Substring(0, input1.IndexOf("$endif$"));
			string str1 = inputText.Substring(input1.Length);
			string comparison1 = this._ifStatementRegEx.Match(input1).Groups["expression"].Value;
			if (comparison1.Length == 0)
				throw new Exception();
			outputText = this._ifStatementRegEx.Match(input1).Groups["output"].Value;
			if (this.EvaluateComparison(comparison1, replacementValues))
				return true;
			string str2 = str1.Substring(0, str1.Length - "$endif$".Length);
			if (str2.Length > 0) {
				List<string> list = new List<string>();
				int num1 = str2.IndexOf("$else$");
				string input2 = str2.Substring(num1);
				string str3 = str2.Substring(0, num1);
				while (str3.Contains("$elseif$")) {
					int num2 = str3.LastIndexOf("$elseif$");
					if (num2 != -1) {
						string str4 = str3.Substring(num2);
						str3 = str3.Substring(0, num2);
						list.Insert(0, str4);
					}
				}
				foreach (string input3 in list) {
					string comparison2 = this._elseifRegEx.Match(input3).Groups["expression"].Value;
					if (comparison2.Length == 0)
						throw new Exception();
					outputText = this._elseifRegEx.Match(input3).Groups["output"].Value;
					if (this.EvaluateComparison(comparison2, replacementValues))
						return true;
				}

				outputText = this._elseRegEx.Match(input2).Groups["output"].Value;
				return true;
			}
			else {
				outputText = "";
				return false;
			}
		}

		private bool EvaluateComparison(string comparison, Dictionary<string, string> replacementValues) {

			comparison = comparison.Trim();
			bool flag1 = this._regEquality.Match(comparison).Length > 0;
			bool flag2 = this._regNoEquality.Match(comparison).Length > 0;
			bool flag3 = this._regLessThan.Match(comparison).Length > 0;
			bool flag4 = this._regLessThanOrEqual.Match(comparison).Length > 0;
			bool flag5 = this._regGreaterThan.Match(comparison).Length > 0;
			bool flag6 = this._regGreaterThanOrEqual.Match(comparison).Length > 0;
			if (flag1 || flag2 || (flag3 || flag4) || (flag5 || flag6)) {
				string index1 = string.Empty;
				string index2 = string.Empty;
				if (flag1) {
					index1 = this._regEquality.Match(comparison).Groups["operand1"].Value.Trim();
					index2 = this._regEquality.Match(comparison).Groups["operand2"].Value.Trim();
				}
				else if (flag4) {
					index1 = this._regLessThanOrEqual.Match(comparison).Groups["operand1"].Value.Trim();
					index2 = this._regLessThanOrEqual.Match(comparison).Groups["operand2"].Value.Trim();
				}
				else if (flag3) {
					index1 = this._regLessThan.Match(comparison).Groups["operand1"].Value.Trim();
					index2 = this._regLessThan.Match(comparison).Groups["operand2"].Value.Trim();
				}
				else if (flag6) {
					index1 = this._regGreaterThanOrEqual.Match(comparison).Groups["operand1"].Value.Trim();
					index2 = this._regGreaterThanOrEqual.Match(comparison).Groups["operand2"].Value.Trim();
				}
				else if (flag5) {
					index1 = this._regGreaterThan.Match(comparison).Groups["operand1"].Value.Trim();
					index2 = this._regGreaterThan.Match(comparison).Groups["operand2"].Value.Trim();
				}
				else if (flag2) {
					index1 = this._regNoEquality.Match(comparison).Groups["operand1"].Value.Trim();
					index2 = this._regNoEquality.Match(comparison).Groups["operand2"].Value.Trim();
				}
				if (index1.StartsWith("$", StringComparison.Ordinal) && index1.EndsWith("$", StringComparison.Ordinal)) {
					if (!replacementValues.ContainsKey(index1))
						return false;
					index1 = replacementValues[index1];
				}
				if (index2.StartsWith("$", StringComparison.Ordinal) && index2.EndsWith("$", StringComparison.Ordinal)) {
					if (!replacementValues.ContainsKey(index2))
						return false;
					index2 = replacementValues[index2];
				}
				int num = string.CompareOrdinal(index1, index2);
				if (flag1)
					return num == 0;
				else if (flag4)
					return num <= 0;
				else if (flag3)
					return num < 0;
				else if (flag6)
					return num >= 0;
				else if (flag5)
					return num > 0;
				else
					return flag2 && num != 0;
			}
			else {
				string key1 = comparison.Trim();
				if (key1.StartsWith("!", StringComparison.Ordinal)) {
					string key2 = key1.Substring(1).Trim();
					return !replacementValues.ContainsKey(key2);
				}
				else
					return replacementValues.ContainsKey(key1);
			}
		}

	}
}