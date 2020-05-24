using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Androtomist.Models.Global
{
    public class DataTableFuncs
    {
        public DataTable DatatableGroupBy(ref DataTable sourceTable, List<string> groupCols, List<string> aggrCols, string extraFilter, AGGREGATES aggrAction = AGGREGATES.SUM)
        {
            string actionString, filterString;
            List<string> filterStrings;

            try
            {
                actionString = Enum.GetName(typeof(AGGREGATES), aggrAction);

                if (groupCols.Count == 0)
                    foreach (DataColumn data_column in sourceTable.Columns)
                        if (!aggrCols.Contains(data_column.ColumnName)) groupCols.Add(data_column.ColumnName);

                DataTable data_table = DatatableDistinct(ref sourceTable, groupCols.ToArray(), extraFilter); // data_view.ToTable(True, group_column)
                aggrCols.ForEach(x => data_table.Columns.Add(x, typeof(double)));

                foreach (DataRow dataRow in data_table.Rows)
                {
                    filterStrings = new List<string>();

                    foreach (string groupCol in groupCols)
                    {
                        if (dataRow[groupCol] == DBNull.Value)
                            filterStrings.Add("[" + groupCol + "] IS NULL");
                        else if (IsNumericType(sourceTable.Columns[groupCol].DataType))
                            filterStrings.Add("[" + groupCol + "] = " + dataRow[groupCol]);
                        else if (IsTextType(sourceTable.Columns[groupCol].DataType))
                        {
                            if (!string.IsNullOrEmpty(dataRow[groupCol].ToString()))
                                filterStrings.Add("[" + groupCol + "] = '" + dataRow[groupCol] + "'");
                        }
                        else
                            filterStrings.Add("[" + groupCol + "] = '" + dataRow[groupCol] + "'");
                    }

                    filterString = "(" + string.Join(") AND (", filterStrings) + ")";

                    foreach (string aggr_column in aggrCols)
                        dataRow[aggr_column] = sourceTable.Compute(actionString + "([" + aggr_column + "])", filterString);
                }
                return data_table;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public DataTable DatatableDistinct(ref DataTable data_table, string[] FieldNames, string extraFilter = "")
        {
            object[] lastValues;
            DataTable newTable;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length - 1 + 1];
            newTable = new DataTable();

            foreach (string field in FieldNames)
                newTable.Columns.Add(field, data_table.Columns[field].DataType);

            foreach (DataRow Row in data_table.Select(extraFilter, string.Join(", ", FieldNames)))
            {
                if (!FieldValuesAreEqual(lastValues, Row, FieldNames))
                {
                    newTable.Rows.Add(CreateRowClone(Row, newTable.NewRow(), FieldNames));

                    SetLastValues(lastValues, Row, FieldNames);
                }
            }

            return newTable;
        }

        public bool FieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i <= fieldNames.Length - 1; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        public DataRow CreateRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        public void SetLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i <= fieldNames.Length - 1; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }


        public bool IsNumericType(Type type__1)
        {
            if (type__1 == null)
                return false;

            switch (Type.GetTypeCode(type__1))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    {
                        return true;
                    }

                case TypeCode.Object:
                    {
                        if (type__1.IsGenericType && type__1.GetGenericTypeDefinition() == typeof(Nullable<>))
                            return IsNumericType(Nullable.GetUnderlyingType(type__1));
                        return false;
                    }
            }
            return false;
        }

        public bool IsTextType(Type type__1)
        {
            if (type__1 == null)
                return false;

            switch (Type.GetTypeCode(type__1))
            {
                case TypeCode.String:
                case TypeCode.Char:
                    {
                        return true;
                    }

                case TypeCode.Object:
                    {
                        if (type__1.IsGenericType && type__1.GetGenericTypeDefinition() == typeof(Nullable<>))
                            return IsNumericType(Nullable.GetUnderlyingType(type__1));
                        return false;
                    }
            }
            return false;
        }

        public string DatatableToCSV(ref DataTable dataTable)
        {
            StringBuilder stringBuilder = new StringBuilder();

            IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

            stringBuilder.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                stringBuilder.AppendLine(string.Join(",", fields));
            }

            return stringBuilder.ToString();
        }

        public DataTable CSVToDataTable(string csvString)
        {
            string[] lines = csvString.Split(Environment.NewLine);
            string[] headers = lines[0].Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            foreach (string line in lines.Skip(1))
            {
                string[] rows = line.Split(',');
                if (rows.Length == dt.Columns.Count)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            dt.AcceptChanges();
            return dt;
        }


        /// <summary>
        /// Transpose columns to row, starting from given column and row. [0-basae index]
        /// </summary>
        public DataTable TransposeRows(DataTable dataTable, int startCol, int startRow)
        {
            DataTable cloneTable = new DataTable(dataTable.TableName);
            DataRow dataRow;

            for (int colHeader = 0; colHeader < startCol; colHeader++)
                cloneTable.Columns.Add(dataTable.Columns[colHeader].ColumnName, dataTable.Columns[colHeader].DataType);

            if (startRow == 0)
                cloneTable.Columns.Add("NEW_ID", typeof(string));
            else
                for (int rowHeader = 0; rowHeader < startRow; rowHeader++)
                    cloneTable.Columns.Add(dataTable.Columns[startCol + rowHeader].ColumnName, dataTable.Columns[startCol + rowHeader].DataType);

            cloneTable.Columns.Add("NEW_VAL", typeof(string));

            cloneTable.BeginLoadData();
            for (int rowData = startRow; rowData < dataTable.Rows.Count; rowData++)
            {
                for (int colData = startCol; colData < dataTable.Columns.Count; colData++)
                {
                    dataRow = cloneTable.NewRow();

                    for (int colHeader = 0; colHeader < startCol; colHeader++)
                        dataRow[colHeader] = dataTable.Rows[rowData][colHeader];

                    if (startRow == 0)
                        dataRow["NEW_ID"] = dataTable.Columns[colData].ColumnName;
                    else
                        for (int rowHeader = 0; rowHeader < startRow; rowHeader++)
                            dataRow[startCol + rowHeader] = dataTable.Rows[rowHeader][colData];

                    dataRow["NEW_VAL"] = dataTable.Rows[rowData][colData];

                    cloneTable.Rows.Add(dataRow);
                }
            }
            cloneTable.EndLoadData();
            cloneTable.AcceptChanges();

            return cloneTable;
        }

        public DataTable DuplicateRows(DataTable dataTable)
        {
            DataTable cloneTable = new DataTable(dataTable.TableName);
            DataRow dataRow;
            string[] splitVals;

            for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
            {
                cloneTable = dataTable.Clone();
                cloneTable.BeginLoadData();
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    if (dataTable.Rows[rowIndex][colIndex].ToString().Contains(";"))
                    {
                        splitVals = dataTable.Rows[rowIndex][colIndex].ToString().Split(';');
                        foreach (string splitVal in splitVals)
                        {
                            dataRow = cloneTable.NewRow();

                            foreach (DataColumn dataColumn in dataTable.Columns)
                            {
                                if (dataColumn.Ordinal == colIndex)
                                    dataRow[dataColumn.ColumnName] = splitVal;
                                else
                                    dataRow[dataColumn.ColumnName] = dataTable.Rows[rowIndex][dataColumn.ColumnName];
                            }

                            cloneTable.Rows.Add(dataRow);
                        }
                    }
                    else
                    {
                        cloneTable.ImportRow(dataTable.Rows[rowIndex]);
                    }
                }
                cloneTable.EndLoadData();
                cloneTable.AcceptChanges();

                dataTable = cloneTable.Copy();
                cloneTable.Dispose();
            }

            dataTable.AcceptChanges();
            return dataTable;
        }

        public DataTable ConvertAllCols(DataTable dataTable)
        {
            DataTable cloneTable = dataTable.Clone();

            // ' convert all the columns type to String 
            foreach (DataColumn col in cloneTable.Columns)
                col.DataType = typeof(string);

            cloneTable.Load(dataTable.CreateDataReader());

            cloneTable.AcceptChanges();
            return cloneTable;
        }


        public void ConvertColumnType(ref DataTable dt, string columnName, Type newType)
        {
            using (DataColumn dc = new DataColumn(columnName + "_new", newType))
            {
                // Add the new column which has the new type, and move it to the ordinal of the old column
                int ordinal = dt.Columns[columnName].Ordinal;
                dt.Columns.Add(dc);
                dc.SetOrdinal(ordinal);

                // Get and convert the values of the old column, and insert them into the new
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[columnName] == DBNull.Value)
                        dr[dc.ColumnName] = dr[columnName];
                    else
                        dr[dc.ColumnName] = Convert.ChangeType(dr[columnName], newType);
                }
                // Remove the old column
                dt.Columns.Remove(columnName);

                // Give the new column the old column's name
                dc.ColumnName = columnName;
            }
        }

    }
}
