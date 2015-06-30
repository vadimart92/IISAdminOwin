using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using IISAdmin.Interfaces;
using DomainClasses;
using SharpCompress.Archive;
using SharpCompress.Common;

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
    }

    public class WorkDbReleaseRepository : IReleaseRepository
    {
        public void Create(IRelease entity)
        {
            //todo IISAdmin.Owin.DAL.Common.WorkDbReleaseRepository
            //throw new NotImplementedException();
           /* var archive = ArchiveFactory.Open(entity.BuildFolderLink);
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    Console.WriteLine(entry.FilePath);
                    entry.WriteToDirectory(@"C:\1", ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }
            }*/
        }

        public IRelease Get(Guid key)
        {
            //todo IISAdmin.Owin.DAL.Common.WorkDbReleaseRepository
            using (var releaseContext = new ReleaseContext())
            {
                var resultRelease = (from release in releaseContext.Releases
                    join build in releaseContext.Builds on release.BuildId equals build.Id
                    where release.Id == key
                    select new WorkDbRelease
                    {
                        Id = release.Id,
                        BuildFolderLink = release.BuildFolderLink,
                        Name = release.Name,
                        Release = release.IsPublished == 1,
                        Version = build.Name,
                        CreatedOn = release.CreatedOn
                    }).FirstOrDefault();

                return resultRelease;
            }
        }

        public IEnumerable<IRelease> GetAll()
        {
            //todo IISAdmin.Owin.DAL.Common.WorkDbReleaseRepository
            var res = new List<IRelease>();
            using (var releaseContext = new ReleaseContext())
            {
                var releases = (from release in releaseContext.Releases
                    join build in releaseContext.Builds on release.BuildId equals build.Id
                    select new WorkDbRelease
                    {
                        Id = release.Id,
                        BuildFolderLink = release.BuildFolderLink,
                        Name = release.Name,
                        Release = release.IsPublished == 1,
                        Version = build.Name,
                        CreatedOn = release.CreatedOn
                    }).Take(10);
                res.AddRange(releases);
            }
            
            return res;
        }

        public IEnumerable<IRelease> GetAll(Expression<Func<IRelease, bool>> expression)
        {
            throw new NotImplementedException();
            //todo IISAdmin.Owin.DAL.Common.WorkDbReleaseRepository
        }

        public void Update(IRelease entity)
        {
            throw new InvalidOperationException();
        }

        public void Delete(Guid id)
        {
            throw new InvalidOperationException();
        }

        public IRelease GetByUri(string uri)
        {
            var id = GetIdByUri(uri);
            return Get(id);
        }

        private Guid GetIdByUri(string uri)
        {
            var rg = new Regex(@"{\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b}");
            var m = rg.Match(uri);
            if (m.Captures.Count == 0)
            {
                throw new ArgumentOutOfRangeException("uri");
            }
            var id = m.Captures[0].Value;
            var res = new Guid(id);
            return res;
        }
    }
}
