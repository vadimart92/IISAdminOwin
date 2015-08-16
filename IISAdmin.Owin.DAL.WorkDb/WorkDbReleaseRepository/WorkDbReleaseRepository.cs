using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Dapper;
using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL.Dapper.WorkDbReleaseRepository.Models;

namespace IISAdmin.Owin.DAL.Dapper.WorkDbReleaseRepository {
	public class WorkDbReleaseRepository : IReleaseRepository {
		#region Fields: Private

		private readonly ISqlConnectionProvider _connectionProvider;

		#endregion
		public WorkDbReleaseRepository(ISqlConnectionProvider connectionProvider) {
			Contract.Requires(connectionProvider != null, "connectionProvider != null");
			_connectionProvider = connectionProvider;
		}

		protected Select GetMainSelect() {
			return new Select().Top(1)
				.Columns(@"vr.ID, 
						vr.Name, 
						tb.Name as [Version], 
						vr.IsPublished as Release, 
						vr.CreatedOn as CreatedOn, 
						vr.BuildFolderLink as ZipFilePath")
				.From(@"vw_Release vr
						LEFT JOIN dbo.tbl_Build tb ON tb.ID = vr.BuildID");
		}

		public void Create(Release entity) {
			throw new InvalidOperationException();
		}

		public Release Get(Guid key) {
			var query = GetMainSelect()
				.Where("vr.ID = @releaseId");
			return _connectionProvider.ExecuteAction((connection) => {
				return connection.Query<WorkDbRelease>(query, new { releaseId = key }).FirstOrDefault();
			});
		}

		public IEnumerable<Release> GetTopThousand() {
			var query = GetMainSelect().Top(1000);
			return _connectionProvider.ExecuteAction((connection) => connection.Query<WorkDbRelease>(query));
		}

		public IEnumerable<Release> GetTopThousand(string nameLike) {
			var query = GetMainSelect().Top(1000).Where(@"vr.Name LIKE @releaseName");
			return _connectionProvider.ExecuteAction((connection) => connection.Query<WorkDbRelease>(query, new {releaseName = nameLike}));
		}

		public IEnumerable<Release> Get(Expression<Func<Release, bool>> expression) {
			throw new InvalidOperationException();
		}

	    public void Update(Guid key, Action<Release> action) {
	        throw new NotImplementedException();
	    }
        
		public void Delete(Guid key) {
			throw new InvalidOperationException();
		}

		public Release GetByUri(string uri) {
			var id = GetIdByUri(uri);
			return Get(id);
		}

		private Guid GetIdByUri(string uri) {
			Guid res;
			if (Guid.TryParse(uri, out res)) {
				return res;
			}
			var rg = new Regex(@"{\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b}");
			var m = rg.Match(uri);
			if (m.Captures.Count == 0) {
				throw new ArgumentOutOfRangeException("uri");
			}
			var id = m.Captures[0].Value;
			res = new Guid(id);
			return res;
		}
	}
}