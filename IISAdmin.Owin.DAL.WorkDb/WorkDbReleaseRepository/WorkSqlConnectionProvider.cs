namespace IISAdmin.Owin.DAL.WorkDbReleaseRepository {
	public class WorkSqlConnectionProvider : SqlConnectionProvider {
		public WorkSqlConnectionProvider(string connectionString)
			: base(connectionString, null) {
		}
	}
}
