using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL.Common {

	public class WorkDbRelease: IRelease {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Version { get; set; }
		public bool Release { get; set; }
		public DateTime CreatedOn { get; set; }
		public string BuildFolderLink { get; set; }
	}

	public class WorkDbReleaseRepository : IReleaseRepository {
		public void Create(IRelease entity) {
			throw new NotImplementedException();
		}

		public IRelease Get(Guid key) {
			return new WorkDbRelease {
				Id = key,
				BuildFolderLink = @"\\tscrm.com\dfs-ts\Builds-7\7.4.1\7.4.1.702\SalesOmnichannel_Softkey_RUS\7.4.1.702_SalesOmnichannel_Softkey_MSSQL_RUS.zip",
				Name = "BPMonline_Sales Omnichannel_Softkey_RUS_MSSQL_7.4.1.702",
				Release = true,
				Version = "7.4.1.580",
				CreatedOn = DateTime.Now
			};
		}

		public IEnumerable<IRelease> GetAll() {
			var res = new List<IRelease>();
			for (int i = 0; i < 10; i++) {
				res.Add(new WorkDbRelease {
					Id = Guid.NewGuid(),
					Name = Guid.NewGuid().ToString()
				});
			}
			return res;
		}

		public IEnumerable<IRelease> GetAll(Expression<Func<IRelease, bool>> expression) {
			throw new NotImplementedException();
		}

		public void Update(IRelease entity) {
			throw new NotImplementedException();
		}

		public void Delete(Guid id) {
			throw new NotImplementedException();
		}

		public IRelease GetByUri(string uri) {
			var id = GetIdByUri(uri);
			return Get(id);
		}

		private Guid GetIdByUri(string uri) {
			var rg = new Regex(@"{\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b}");
			var m = rg.Match(uri);
			if (m.Captures.Count == 0) {
				throw new ArgumentOutOfRangeException("uri");
			}
			var id = m.Captures[0].Value;
			var res = new Guid(id);
			return res;
		}
	}
}
