using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Npgsql;

namespace Androtomist.Models.Database
{
	public class DatabaseConnector : IDatabaseConnector
	{
		private NpgsqlConnection _conn;
		public DatabaseSettings DatabaseSettings { get; }
		public IConfiguration Configuration { get; }

		public DatabaseProcedures DatabaseProcedures { get; }

		public DatabaseConnector(IOptions<DatabaseSettings> databaseSettings, IConfiguration configuration, string conName = "DefaultConnection")
		{
			DatabaseSettings = databaseSettings.Value;
			this.Configuration = configuration;
			ConnOpen(conName);

			DatabaseProcedures = new DatabaseProcedures(ref _conn);
		}

		public NpgsqlConnection Connection => _conn;

		#region "BASIC_FUNCS"

		private void ConnOpen(string conName = "DefaultConnection")
		{
			try
			{
				if (_conn == null)
					_conn = new NpgsqlConnection(Configuration.GetConnectionString(conName));

				if (_conn.State != ConnectionState.Open)
					_conn.Open();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void ConnClose()
		{
			if (_conn != null && _conn.State == ConnectionState.Open)
			{
				_conn.Close();
				_conn.Dispose();
			}
			else
			{
				_conn?.Dispose();
			}

			_conn = null;
		}

		#endregion

		#region "SELECT"

		public DataTable SelectSQL(string sql, string tableName)
		{
			DataTable dt = new DataTable(tableName);
			NpgsqlDataReader dr = null;
			NpgsqlCommand pgCommand = null;

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand
				{
					Connection = _conn,
					CommandText = sql,
					CommandType = CommandType.Text,
					CommandTimeout = 500
				};

				dr = pgCommand.ExecuteReader();

				dt.Load(dr);
			}
			catch
			{
			}
			finally
			{
				dr?.Close();
				dr?.Dispose();
				pgCommand?.Dispose();
			}

			return dt;
		}

		public DataTable SelectSQL(string sql, string tableName, string param1, string param2)
		{
			NpgsqlCommand pgCommand = null;
			DataTable dt = new DataTable(tableName);
			NpgsqlDataReader dr = null;
			System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand
				{
					Connection = _conn,
					CommandTimeout = 500,
					CommandText = sql
				};

				pgCommand.Parameters.AddWithValue(":param1", param1);
				pgCommand.Parameters.AddWithValue(":param2", param2);


				dr = pgCommand.ExecuteReader();

				dt.Load(dr);

			}
			catch
			{
			}
			finally
			{
				dr?.Close();
				dr?.Dispose();
				pgCommand?.Dispose();
			}


			return dt;
		}

		#endregion

		#region "INSERT"

		public long InsertSQL(string tableName, List<string> cols, List<string> vals, string returnCol = "")
		{
			NpgsqlCommand pgCommand = null;
			long returnId = 0;
			DataTable dt = new DataTable(tableName);
			System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

			var columnNames = cols.ToArray();
			var columnValues = vals.ToArray();

			columnValues = columnValues.ToArray();

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand
				{
					Connection = _conn,
					CommandTimeout = 500,
					CommandText = "INSERT INTO " + tableName + "(" + string.Join(",", columnNames) + ") VALUES (:" +
								  string.Join(",:", columnNames) + ")"
				};

				//clean data follows
				for (int i = 0; i < columnNames.Length; i++)
				{


					if (columnValues[i] != null)
						columnValues[i] = columnValues[i].Replace("'", "");

					if (columnValues[i] == null)
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], DBNull.Value);
					}
					else if (columnValues[i] != null && double.TryParse(columnValues[i].ToString(), out double numeric))
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], numeric);
					}
					else if (columnValues[i] != null && int.TryParse(columnValues[i].ToString(), out int number))
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], number);
					}
					else if (columnValues[i] != null && DateTime.TryParse(columnValues[i].ToString(), out DateTime datetime))
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], datetime);
					}
					else
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], columnValues[i]);
					}
				}

				if (!string.IsNullOrWhiteSpace(returnCol) && !string.IsNullOrWhiteSpace(tableName))
				{
					pgCommand.CommandText += "; select currval(pg_get_serial_sequence('" + tableName + "', '" + returnCol.ToLower() + "'));";
				}

				NpgsqlDataReader dr;
				try
				{
					dr = pgCommand.ExecuteReader();
				}
				catch (Exception ex)
				{
					throw new Exception("Error while saving data: " + ex);
				}
				dt.Load(dr);

				string id = dt.Rows[0][0].ToString();
				long.TryParse(id, out returnId);
			}
			catch
			{
				returnId = 0;
			}
			finally
			{
				pgCommand?.Dispose();
			}


			return returnId;
		}

		public long InsertSQL(string sql, string returnCol = "", string tableName = "")
		{
			NpgsqlCommand pgCommand = null;
			long returnId = 0;
			DataTable dt = new DataTable(tableName);

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand { Connection = _conn, CommandTimeout = 500, CommandText = sql };

				if (!string.IsNullOrWhiteSpace(returnCol) && !string.IsNullOrWhiteSpace(tableName))
				{
					pgCommand.CommandText += "; select currval(pg_get_serial_sequence('" + tableName + "', '" + returnCol.ToLower() + "'));";
				}

				NpgsqlDataReader dr = null;
				try
				{
					dr = pgCommand.ExecuteReader();
				}
				catch (Exception ex)
				{
					throw new Exception("Error while saving data: " + ex);
				}
				dt.Load(dr);

				string id = dt.Rows[0][0].ToString();
				long.TryParse(id, out returnId);
			}
			catch
			{
				returnId = 0;
			}
			finally
			{
				pgCommand?.Dispose();
			}


			return returnId;
		}


		public long GetId(string tableName, string columnName)
		{
			long id = 0;

			DataTable dataTable = SelectSQL("SELECT MAX(" + columnName + ") AS MAXID FROM " + tableName + "", tableName);

			if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["MAXID"] != DBNull.Value)
				id = Convert.ToInt64(dataTable.Rows[0]["MAXID"]);

			return id + 1;
		}
		#endregion

		#region "UPDATE"

		public int UpdateSQL(string tableName, string columnName, string columnValue, string where)
		{
			return UpdateSQL(tableName, new List<string>() { columnName }, new List<string>() { columnValue }, where);
		}

		public int UpdateSQL(string tableName, List<string> cols, List<string> vals, string where)
		{
			NpgsqlCommand pgCommand = null;
			int rowsAffected = 0;
			System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
			var columnNames = cols.ToArray();
			var columnValues = vals.ToArray();

			List<string> sql = new List<string>();
			for (int i = 0; i < columnNames.Length; i++)
			{
				if (columnValues[i] != null)
					columnValues[i] = columnValues[i].Replace("'", "");
				if (columnValues[i] != null && columnValues[i].Length > 0 && columnValues[i].Trim().First().Equals('\'')) columnValues[i] = columnValues[i].Substring(1, columnValues[i].Length - 2).Replace("'", "''");
				if (columnValues[i] != null && DateTime.TryParse(columnValues[i], out DateTime _) && !double.TryParse(columnValues[i], out double _v))
					sql.Add(columnNames[i] + " = CAST(:" + columnNames[i] + " AS timestamp)");
				else
					sql.Add(columnNames[i] + " = :" + columnNames[i]);
			}

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand
				{
					Connection = _conn,
					CommandTimeout = 500,
					CommandText = "UPDATE " + tableName + " SET " + string.Join(", ", sql.ToArray()) + " WHERE " +
								  @where
				};


				//clean data follows - i need this
				for (int i = 0; i < columnNames.Length; i++)
				{
					if (columnValues[i] == null)
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], DBNull.Value);
						continue;
					}
					else if (columnValues[i] != null && double.TryParse(columnValues[i].ToString(), out double numeric))
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], numeric);
						continue;
					}
					else if (columnValues[i] != null && int.TryParse(columnValues[i].ToString(), out int number))
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], number);
						continue;
					}
					else if (columnValues[i] != null && DateTime.TryParse(columnValues[i].ToString(), out DateTime datetime))
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], datetime);
						continue;
					}
					else
					{
						pgCommand.Parameters.AddWithValue(columnNames[i], columnValues[i]);
					}
				}

				rowsAffected = pgCommand.ExecuteNonQuery();
			}
			catch
			{
				rowsAffected = 0;
			}
			finally
			{
				pgCommand?.Dispose();
			}

			return rowsAffected;
		}

		public int UpdateSQL(string sql)
		{
			NpgsqlCommand pgCommand = null;
			int rowsAffected = 0;

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand { Connection = _conn, CommandTimeout = 500, CommandText = sql };

				rowsAffected = pgCommand.ExecuteNonQuery();
			}
			catch
			{
				rowsAffected = 0;
			}
			finally
			{
				pgCommand?.Dispose();
			}

			return rowsAffected;
		}

		public int UpdateSQL(string sql, string parameter)
		{
			NpgsqlCommand pgCommand = null;
			int rowsAffected = 0;

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand { Connection = _conn, CommandTimeout = 500, CommandText = sql };

				if (parameter != null)
					parameter = parameter.Replace("'", "");

				if (parameter == null)
				{
					pgCommand.Parameters.AddWithValue(":parameter", DBNull.Value);
				}
				else if (parameter != null && double.TryParse(parameter.ToString(), out double numeric))
				{
					pgCommand.Parameters.AddWithValue(":parameter", numeric);
				}
				else if (parameter != null && int.TryParse(parameter.ToString(), out int number))
				{
					pgCommand.Parameters.AddWithValue(":parameter", number);
				}
				else if (parameter != null && DateTime.TryParse(parameter.ToString(), out DateTime datetime))
				{
					pgCommand.Parameters.AddWithValue(":parameter", datetime);
				}
				else
				{
					pgCommand.Parameters.AddWithValue(":parameter", parameter);
				}

				rowsAffected = pgCommand.ExecuteNonQuery();
			}
			catch
			{
				rowsAffected = 0;
			}
			finally
			{
				pgCommand?.Dispose();
			}

			return rowsAffected;
		}

		#endregion


		#region "DELETE"

		public int DeleteSQL(string sql)
		{
			NpgsqlCommand pgCommand = null;
			int dbRowsAffected = 0;

			try
			{
				ConnOpen();

				pgCommand = new NpgsqlCommand
				{
					Connection = _conn,
					CommandTimeout = 500,
					CommandText = sql
				};
				dbRowsAffected = pgCommand.ExecuteNonQuery();
			}
			catch
			{
				dbRowsAffected = 0;
			}
			finally
			{
				pgCommand?.Dispose();
			}

			return dbRowsAffected;
		}

		#endregion

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (disposedValue) return;
			if (disposing)
			{
				// TODO: dispose managed state (managed objects).
			}

			ConnClose();

			disposedValue = true;
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}