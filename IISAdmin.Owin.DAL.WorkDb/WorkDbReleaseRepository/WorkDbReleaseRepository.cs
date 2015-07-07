using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DeclarativeSql;
using IISAdmin.Interfaces;
using IISAdmin.Owin.DAL.WorkDbReleaseRepository.Models;

namespace IISAdmin.Owin.DAL.WorkDbReleaseRepository {
	public class WorkDbReleaseRepository : IReleaseRepository {
		#region Fields: Private

		private readonly ISqlConnectionProvider _connectionProvider;

		#endregion
		public WorkDbReleaseRepository(ISqlConnectionProvider connectionProvider) {
			Contract.Requires(connectionProvider != null, "connectionProvider != null");
			_connectionProvider = connectionProvider;
		}

		protected IEnumerable<IRelease> GetAll(Expression<Func<WorkDbRelease, object>> expression = null) {
			var mainSelect = @"SELECT TOP 10 vr.ID, 
										  vr.Name, 
										  tb.Name as [Version], 
										  vr.IsPublished as Release, 
										  vr.CreatedOn as CreatedOn, 
										  vr.BuildFolderLink as ZipFilePath
							FROM vw_Release vr
							LEFT JOIN dbo.tbl_Build tb ON tb.ID = vr.BuildID";
			_connectionProvider.ExecuteAction((connection) => {
				
			});
			return null;
			/*
			 using (var releaseContext = new ReleaseContext()) {
				return (from release in releaseContext.Releases
						join build in releaseContext.Builds on release.BuildId equals build.Id
						select new WorkDbRelease {
							Id = release.Id,
							BuildFolderLink = release.BuildFolderLink,
							Name = release.Name,
							Release = release.IsPublished == 1,
							Version = build.Name,
							CreatedOn = release.CreatedOn
						});
			}
			 */
		}

		public void Create(IRelease entity) {
			throw new InvalidOperationException();
		}

		public IRelease Get(Guid key) {
			return GetAll(r => r.Id == key).FirstOrDefault();
		}

		public IEnumerable<IRelease> GetTopThousand() {
			return GetAll().Take(1000).ToList();
		}

		public IEnumerable<IRelease> GetTopThousand(Expression<Func<IRelease, bool>> expression) {
			return GetAll(expression);
		}

		public void Update(IRelease entity) {
			throw new InvalidOperationException();
		}

		public void Delete(Guid id) {
			throw new InvalidOperationException();
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