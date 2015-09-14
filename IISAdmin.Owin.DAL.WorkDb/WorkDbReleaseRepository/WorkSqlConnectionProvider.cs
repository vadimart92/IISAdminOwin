namespace IISAdmin.Owin.DAL.Dapper.WorkDbReleaseRepository {
	public class WorkSqlConnectionProvider : SqlConnectionProvider {
		public WorkSqlConnectionProvider(string connectionString)
			: base(connectionString, null) {
		}
	}
}
