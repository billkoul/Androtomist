using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Androtomist.Models.Database
{
    public interface IDatabaseConnector : IDisposable
    {
		NpgsqlConnection Connection { get; }

		DatabaseProcedures DatabaseProcedures { get; }

		DataTable SelectSQL(string sql, string tableName);
		DataTable SelectSQL(string sql, string tableName, string param1, string param2);

		long InsertSQL(string sql, string returnCol = "", string tableName = "");
		long InsertSQL(string tableName, List<string> columnNames, List<string> columnValues, string returnCol = "");
		int UpdateSQL(string sql);
		int UpdateSQL(string sql, string parameter);

		int UpdateSQL(string tableName, List<string> columnNames, List<string> columnValues, string where);
		int UpdateSQL(string tableName, string columnName, string columnValue, string where);
		int DeleteSQL(string sql);

		//int SaveData(ref DataTable dataTable);

		long GetId(string tableName, string columnName);
	}
}
