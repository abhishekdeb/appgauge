using System;
using System.Data;

namespace AppGauge.Data
{
    /// <summary>
    /// This class is used as a medium level DataBase Processor. This class holds all the required DB operation Queries.
    /// This class also provides custom query functions where queries can be directly inserted.
    /// This class generally eliminates the use of writing common sql queries.
    /// </summary>
    public class BaseRecord : BaseDB
    {
        private string _Table;
        private string _TKey;
        private string[] _Columns;
        private string qry;

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        /// <exception cref="System.Exception">Invalid Table Name</exception>
        public string Table
        {
            get { return _Table; }
            set
            {
                if ((value != null) && (value.Length < 100))
                    _Table = value;
                else
                    throw new Exception("Invalid Table Name");
            }
        }

        /// <summary>
        /// Gets or sets the T key.
        /// </summary>
        /// <value>
        /// The T key.
        /// </value>
        /// <exception cref="System.Exception">Invalid Primary Key</exception>
        public string TKey
        {
            get { return _TKey; }
            set
            {
                if ((value != null) && (value.Length < 100))
                    _TKey = value;
                else
                    throw new Exception("Invalid Primary Key");
            }
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        /// <exception cref="System.Exception">Invalid Columns Structure. Columns should be less than 50</exception>
        public string[] Columns
        {
            get { return _Columns; }
            set
            {
                if ((value != null))
                {
                    _Columns=new string[value.Length];
                    _Columns = value;
                }
                    
                else
                    throw new Exception("Invalid Columns Structure. Columns should be less than 50");
            }
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRecord"/> class.
        /// </summary>
        /// <param name="CONNECTION_STRING">The CONNECTIO n_ STRING.</param>
        public BaseRecord(string CONNECTION_STRING = null)
            : base(CONNECTION_STRING)
        {

        }

        /// <summary>
        /// Appends more columns to this Columns.
        /// </summary>
        /// <param name="MoreColumns">The more columns.</param>
        /// <returns>True</returns>
        public bool AppendColumns(string[] MoreColumns)
        {
            string[] tmp = new string[_Columns.Length];
            tmp = Columns;
            _Columns = new string[tmp.Length + MoreColumns.Length];
            tmp.CopyTo(Columns, 0);
            MoreColumns.CopyTo(Columns, tmp.Length - 1);

            return true;
        }

        /// <summary>
        /// Inserts the specified values as Arrays.
        /// </summary>
        /// <param name="Values">The values in Array.</param>
        /// <returns>Last inserted ID</returns>
        /// <exception cref="System.Exception">Insufficient Values Supplied. PLease check the length if the Values with the Columns</exception>
        /// <example>Insert(new string[]{"Pass","email","etc"});</example>
        public int Insert(string[] Values)
        {
            if (Values.Length == Columns.Length)
            {
                int i;

                qry = "insert into " + Table + "(" + Columns[0];
                for (i = 1; i < Columns.Length; i++)
                    qry += "," + Columns[i];
                qry += ") output inserted." + TKey + " values('" + Values[0] + "'";
                for (i = 1; i < Values.Length; i++)
                    qry += ",'" + Values[i] + "'";
                qry += ")";


                return (int)Modify(qry, true);
            }
            else
                throw new Exception("Insufficient Values Supplied. PLease check the length if the Values with the Columns");
        }

        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Values">The values.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Insufficient Values Supplied. PLease check the length if the Values with the Columns</exception>
        public bool Update(string Key, string[] Values)
        {
            if (Values.Length == Columns.Length)
            {
                int i;

                qry = "update " + Table + " set " + Columns[0] + " ='" + Values[0] + "'";
                for (i = 1; i < Columns.Length; i++)
                    qry += "," + Columns[i] + "= '" + Values[i] + "'";
                qry += " where " + _TKey + "='" + Key + "'";

                return ((int)Modify(qry) < 0) ? false : true;
            }
            else
                throw new Exception("Insufficient Values Supplied. PLease check the length of the Values with the Columns");
        }

        /// <summary>
        /// Updates the Table with Selective Columns.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="Fields">The fields.</param>
        /// <param name="Values">The values.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Insufficient Values Supplied. PLease check the length of the Values with the Fields Provided</exception>
        public bool CustomUpdate(string Key, string[] Fields, string[] Values)
        {
            if (Fields.Length == Values.Length)
            {
                int i;
                qry = "update " + Table + " set " + Fields[0] + " ='" + Values[0] + "'";
                for (i = 1; i < Fields.Length; i++)
                    qry += "," + Fields[i] + "= '" + Values[i] + "'";
                qry += " where " + _TKey + "='" + Key + "'";

                return ((int)Modify(qry) < 0) ? false : true;
            }
            else
            {
                throw new Exception("Insufficient Values Supplied. PLease check the length of the Values with the Fields Provided");
            }
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public bool Delete(string Key)
        {
            qry = "delete from " + Table + " where " + _TKey + "='" + Key + "'";

            return ((int)Modify(qry) < 0) ? false : true;
        }

        /// <summary>
        /// Views the specified key.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public DataTable View(string Key = null)
        {
            qry = "select * from " + Table;

            if (Key != null)
                qry += " where " + TKey + " ='" + Key + "'";

            return (DataTable)Fetch(qry);
        }

        /// <summary>
        /// Customs the view.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <param name="CustomColumns">The custom columns.</param>
        /// <returns>DataTable that consists the result</returns>
        /// <example>CustomView(23,new string[]{"name"});</example>
        public DataTable CustomView(string Key = null, string[] CustomColumns = null)
        {
            qry = "select ";

            if (CustomColumns != null)
            {
                qry += CustomColumns[0];
                for (int i = 1; i < CustomColumns.Length; i++)
                    qry += "," + CustomColumns[i];
            }
            else
                qry += "*";


            qry += " from " + Table;

            if (Key != null)
                qry += " where " + TKey + " ='" + Key + "'";

            return (DataTable)Fetch(qry);
        }

        /// <summary>
        /// Searches the specified search item.
        /// </summary>
        /// <param name="SearchItem">The search item.</param>
        /// <param name="Fields">The fields.</param>
        /// <returns></returns>
        public DataTable Search(string SearchItem, string[] Fields)
        {
            qry = "select * from " + Table + " where " + Fields[0] + " like '%" + SearchItem + "%'";
            for (int i = 1; i < Fields.Length; i++)
                qry += " or " + Fields[i] + " like '%" + SearchItem + "%'";

            return (DataTable)Fetch(qry);
        }

        /// <summary>
        /// Execute a Custom query.
        /// </summary>
        /// <param name="SQLQuery">The SQL query.</param>
        /// <returns>Scalar Value</returns>
        public int CustomQuery(string SQLQuery)
        {
            int i;
            i = (int)Modify(SQLQuery);
            return i;
        }

        /// <summary>
        /// Execute a Custom query with DataTable.
        /// </summary>
        /// <param name="Query">The query.</param>
        /// <param name="DATA_TABLE">The DataTable as ref.</param>
        /// <example>
        /// DataTable dt= new DataTable();
        /// CustomQuery("select * from tbl",ref dt);
        /// </example>
        public void CustomQuery(string Query, ref DataTable DATA_TABLE)
        {
            DATA_TABLE.Clear();
            DATA_TABLE = (DataTable)Fetch(Query);
        }


    }
}
