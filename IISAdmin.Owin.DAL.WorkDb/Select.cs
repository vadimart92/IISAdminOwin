using System;
using System.Collections.Generic;
using System.Linq;

namespace IISAdmin.Owin.DAL {

	public interface ICloneable<T> : ICloneable where T : class {
		T TypedClone();
	}
	
	public class Select : ICloneable<Select> {
		private readonly List<string> _selectStatementList = new List<string>();
		private readonly List<string> _preSelectStatementList = new List<string>();
		private readonly List<string> _fromStatementList = new List<string>();
		private readonly List<string> _whereStatementList = new List<string>();

		public override string ToString() {
			var preSelect = _preSelectStatementList.Aggregate((a, b) => a + "," + Environment.NewLine + "\t" + b);
			var select = _selectStatementList.Aggregate((a, b) => a + "," + Environment.NewLine + "\t" + b);
			var from = _fromStatementList.Aggregate((a, b) => a + "," + Environment.NewLine + "\t" + b);
			var where = _whereStatementList.Aggregate((a, b) => a + "," + Environment.NewLine + "\t" + b);
			var res = String.Format("SELECT {1}{0}{2}{0}FROM {3}{0}WHERE {4}", NormilizeSelect(Environment.NewLine, preSelect, select, from, where));
			return res;
		}

		private object[] NormilizeSelect(params string[] selectParts) {
			object[] result = new object[selectParts.Length];
			for (int index = 0; index < selectParts.Length; index++) {
				var selectPart = selectParts[index];
				var res = NormilizeSelectPart(selectPart, ' ');
				result[index] = NormilizeSelectPart(res, '\t');
			}
			return result;
		}

		private string NormilizeSelectPart(string selectPart, char normilizeChar) {
			var searchString = new String(normilizeChar,2);
			var replaceString = new String(normilizeChar, 1);
			bool normilized = false;
			do {
				normilized = selectPart.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) == -1;
				if (!normilized) {
					selectPart = selectPart.Replace(searchString, replaceString);
				}
			} while (!normilized);
			return selectPart;
		}

		public static implicit operator string(Select select) {
			return select.ToString();
		}
		public Select Columns(params string[] content) {
			_selectStatementList.AddRange(content);
			return this;
		}
		public Select Top(params string[] content) {
			_preSelectStatementList.AddRange(content);
			return this;
		}
		public Select From(params string[] content) {
			_fromStatementList.AddRange(content);
			return this;
		}
		public Select Where(params string[] content) {
			_whereStatementList.AddRange(content);
			return this;
		}

		#region Члены ICloneable<Select>

		public object Clone() {
			return TypedClone();
		}
		
		public Select TypedClone() {
			var res = new Select();
			res.Columns(_selectStatementList.ToArray());
			res.Top(_preSelectStatementList.ToArray());
			res.From(_fromStatementList.ToArray());
			res.Where(_whereStatementList.ToArray());
			return res;
		}

		#endregion
	}
}
