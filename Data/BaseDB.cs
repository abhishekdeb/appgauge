using System;
using System.Data;
using System.Data.SqlClient;

namespace AppGauge.Data
{
    /// <summary>
    /// This class is the Low-Level DataBase Processor. This actually doesn't hold any sql queries, rather, the way it should execute the queries.
    /// </summary>
    public class BaseDB : IDisposable
    {
        private SqlConnection con;
        private SqlDataAdapter da;
        private SqlCommand cmd;
        private DataTable dt;
        private string _SQL;
        private string _ConnectionString;

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        /// <exception cref="System.Exception">Invalid ConnectionString. It should >10 and <500 Characters.</exception>
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set
            {
                if (value != null || value.Length > 10 || value.Length < 500)
                {
                    this._ConnectionString = value;
                    Initialize();  
                }
                else
                    throw new Exception("Invalid ConnectionString. It should >10 and <500 Characters.");
            }
        }

        /// <summary>
        /// Gets or sets the SQL Query.
        /// </summary>
        /// <value>
        /// The SQL.
        /// </value>
        /// <exception cref="System.Exception">Null Query Provided</exception>
        public string SQL
        {
            get { return _SQL; }
            set
            {
                if (value != null && value.Length<1000)
                    _SQL = value;
                else
                    throw new Exception("Null Query Provided");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDB"/> class.
        /// </summary>
        /// <param name="CONNECTION_STRING">The CONNECTIO n_ STRING.</param>
        protected BaseDB(string CONNECTION_STRING=null)
        {
            if (CONNECTION_STRING != null)
            {
                ConnectionString = CONNECTION_STRING;
                           
            }
        }

        /// <summary>
        /// Initializes this instance i.e. Creates a Connection Object.
        /// </summary>
        /// <remarks>No Need to call this explicitly</remarks>
        /// <exception cref="System.Exception">Database Connection Error</exception>
        private void Initialize()
        {
            con = new SqlConnection(ConnectionString);
            if (!TestConnection())
                throw new Exception("Database Connection Error");
            
        }

        /// <summary>
        /// Connects this instance to DB.
        /// </summary>
        /// <returns>True if Connected</returns>
        protected bool Connect()
        {
            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        /// <returns>Returns true if success</returns>
        protected bool Disconnect()
        {
            if (con.State == ConnectionState.Open)
                con.Close();

            return true;
        }

        /// <summary>
        /// Tests the connection to the DB.
        /// </summary>
        /// <returns>True if connection successful</returns>
        protected bool TestConnection()
        {
            try
            {
                Connect();
            }
            catch
            {
                return false;
            }
            finally
            {
                Disconnect();
            }
            return true;
        }


        /// <summary>
        /// Modifies the specified SQL query. Use Insert, Update and Delete Commands.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <param name="ReturnLastID">if set to <c>true</c> [return last ID].</param>
        /// <returns>Last Inserted ID if Auto-Increment is on.</returns>
        protected int Modify(string SqlQuery, bool ReturnLastID = false)
        {
            SQL = SqlQuery;
            cmd = new SqlCommand(SQL, con);
            if (Connect())
            {
                if (ReturnLastID == true)
                    return (int)cmd.ExecuteNonQuery();  // Last Inserted ID
                else
                    cmd.ExecuteNonQuery();
                Disconnect();

                return 0; // Null Rows
            }
            else
                return -1; // Error
        }

        /// <summary>
        /// Fetches the specified SQL query.
        /// </summary>
        /// <param name="SqlQuery">The SQL query.</param>
        /// <returns>Result as DataTable</returns>
        protected DataTable Fetch(string SqlQuery)
        {
            SQL = SqlQuery;
            da = new SqlDataAdapter(SQL, con);
            dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
                return null;

            return dt;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            dt.Dispose();
            da.Dispose();
            cmd.Dispose();
            this.Dispose();
        }
    }
}
