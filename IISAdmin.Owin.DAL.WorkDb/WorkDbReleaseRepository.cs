using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DomainClasses;
using IISAdmin.Interfaces;

namespace IISAdmin.Owin.DAL.Common
{
	public class WorkDbRelease : IRelease
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Version { get; set; }

		public bool Release { get; set; }

		public DateTime CreatedOn { get; set; }

		public string BuildFolderLink { get; set; }

		string ZipFilePath {
			get {
				throw new Exception();
				return string.Format(@"{0}\{1}.zip", BuildFolderLink, Name);
			}
		}
	}

	public class WorkDbReleaseRepository : IReleaseRepository
	{
		protected IQueryable<IRelease> GetAllQueryable() {
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
		}

		public void Create(IRelease entity) {
			throw new InvalidOperationException();
		}

		public IRelease Get(Guid key) {
			return GetAllQueryable().Where(r => r.Id == key).FirstOrDefault();
		}

		public IEnumerable<IRelease> GetAll() {
			return GetAllQueryable().Take(100).ToList();
		}

		public IEnumerable<IRelease> GetAll(Expression<Func<IRelease, bool>> expression) {
			return GetAllQueryable().Where(expression);
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