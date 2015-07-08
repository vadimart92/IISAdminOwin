using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace IISAdmin.Owin.DAL {

	public interface ICloneable<T> : ICloneable where T : class {
		T TypedClone();
	}
	
	public class Select : ICloneable<Select> {
		private int _topRows = -1;
		private bool _isDistinct;
		private readonly List<string> _selectStatementList = new List<string>();
		private readonly List<string> _preSelectStatementList = new List<string>();
		private readonly List<string> _fromStatementList = new List<string>();
		private readonly List<string> _whereStatementList = new List<string>();

		public Select(bool isDistinct = false) {
			_isDistinct = isDistinct;
		}

		public override string ToString() {
			var select = _selectStatementList.Aggregate((a, b) => a + "," + Environment.NewLine + "\t" + b);
			var from = _fromStatementList.Aggregate((a, b) => a + "," + Environment.NewLine + "\t" + b);
			var where = _whereStatementList.Aggregate((a, b) => a + "," + Environment.NewLine + "\t" + b);
			var preSelect = string.Format("{0}{1}", _isDistinct? "DISTINCT " : String.Empty, _topRows > 0? string.Format("TOP {0}", _topRows):String.Empty);
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
			Contract.Requires(selectPart != null, "selectPart != null");
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
			Contract.Requires(select != null, "select != null");
			return select.ToString();
		}

		public Select Distinct(bool value = true) {
			_isDistinct = value;
			return this;
		}

		public Select Columns(params string[] content) {
			Contract.Requires(content != null, "content != null");
			_selectStatementList.AddRange(content); 
			return this;
		}
		public Select Top(int rows) {
			Contract.Requires(rows > 0);
			_topRows = rows;
			return this;
		}

		public Select From(params string[] content) {
			Contract.Requires(content != null, "content != null");
			_fromStatementList.AddRange(content);
			return this;
		}
		public Select Where(params string[] content) {
			Contract.Requires(content != null, "content != null");
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
			res.Top(_topRows);
			res.From(_fromStatementList.ToArray());
			res.Where(_whereStatementList.ToArray());
			res.Distinct(_isDistinct);
			return res;
		}

		#endregion
	}
}
