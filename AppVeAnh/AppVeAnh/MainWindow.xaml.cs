using CsvHelper;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppVeAnh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable dataTable;
        private string connectionString = "Server=HOANGNE\\SQLEXPRESS;Database=DataImportDB;User Id=sa;Password=1"; // Thay bằng chuỗi kết nối của bạn
        private string lastCreatedTableName = string.Empty;

        public MainWindow()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
            dataTable = new DataTable();
        }

        private async void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    StatusTextBlock.Text = "Importing data...";
                    if (filePath.EndsWith(".xlsx"))
                    {
                        await Task.Run(() => ImportFromExcel(filePath));
                    }
                    else if (filePath.EndsWith(".csv"))
                    {
                        await Task.Run(() => ImportFromCsv(filePath));
                    }
                    DataGrid.ItemsSource = dataTable.DefaultView;

                    StatusTextBlock.Text = "Creating new database table...";
                    lastCreatedTableName = await Task.Run(() => CreateNewDatabaseTable());

                    StatusTextBlock.Text = "Inserting data into database...";
                    await Task.Run(() => InsertDataIntoNewDatabaseTable(lastCreatedTableName));

                    StatusTextBlock.Text = $"Data imported successfully into table {lastCreatedTableName}.";
                    await LoadTablesComboBox();
                }
                catch (Exception ex)
                {
                    StatusTextBlock.Text = $"Error: {ex.Message}";
                }
            }
        }

        private void ImportFromExcel(string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rows = worksheet.Dimension.Rows;
                var columns = worksheet.Dimension.Columns;

                dataTable.Clear();
                dataTable.Columns.Clear();

                for (int col = 1; col <= columns; col++)
                {
                    string columnName = worksheet.Cells[1, col].Value.ToString();
                    dataTable.Columns.Add(SanitizeColumnName(columnName));
                }

                for (int row = 2; row <= rows; row++)
                {
                    DataRow newRow = dataTable.NewRow();
                    for (int col = 1; col <= columns; col++)
                    {
                        newRow[col - 1] = worksheet.Cells[row, col].Value;
                    }
                    dataTable.Rows.Add(newRow);
                }
            }
        }

        private void ImportFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            using (var dr = new CsvDataReader(csv))
            {
                dataTable.Clear();
                dataTable.Load(dr);

                // Sanitize column names
                foreach (DataColumn column in dataTable.Columns)
                {
                    column.ColumnName = SanitizeColumnName(column.ColumnName);
                }
            }
        }

        private string CreateNewDatabaseTable()
        {
            string newTableName = "ImportedData_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Create table with columns based on DataTable
                StringBuilder createTableCmd = new StringBuilder($"CREATE TABLE {newTableName} (Id INT IDENTITY(1,1) PRIMARY KEY, ");

                foreach (DataColumn column in dataTable.Columns)
                {
                    createTableCmd.Append($"[{column.ColumnName}] NVARCHAR(MAX), ");
                }
                createTableCmd.Length -= 2; // Remove the last comma and space
                createTableCmd.Append(");");

                using (SqlCommand cmd = new SqlCommand(createTableCmd.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            return newTableName;
        }

        private void InsertDataIntoNewDatabaseTable(string newTableName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (DataRow row in dataTable.Rows)
                {
                    StringBuilder insertCmd = new StringBuilder($"INSERT INTO {newTableName} (");
                    StringBuilder valuesCmd = new StringBuilder("VALUES (");

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        insertCmd.Append($"[{column.ColumnName}], ");
                        valuesCmd.Append($"@{column.ColumnName}, ");
                    }

                    insertCmd.Length -= 2; // Remove the last comma and space
                    valuesCmd.Length -= 2; // Remove the last comma and space
                    insertCmd.Append(") ");
                    valuesCmd.Append(")");

                    using (SqlCommand cmd = new SqlCommand(insertCmd.ToString() + valuesCmd.ToString(), conn))
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            cmd.Parameters.AddWithValue($"@{column.ColumnName}", row[column] ?? DBNull.Value);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private string SanitizeColumnName(string columnName)
        {
            // Replace spaces and special characters in column names
            return new string(columnName.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
        }

        private async void ExportFromDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (TablesComboBox.SelectedItem == null)
            {
                StatusTextBlock.Text = "Please select a table to export.";
                return;
            }

            string selectedTable = TablesComboBox.SelectedItem.ToString();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    StatusTextBlock.Text = "Exporting data from database...";
                    await Task.Run(() => ExportDataFromDatabase(selectedTable, filePath));
                    StatusTextBlock.Text = "Data exported successfully.";
                }
                catch (Exception ex)
                {
                    StatusTextBlock.Text = $"Error: {ex.Message}";
                }
            }
        }

        private void ExportDataFromDatabase(string tableName, string filePath)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {tableName}", conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                using (DataTable exportTable = new DataTable())
                {
                    adapter.Fill(exportTable);

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("DatabaseData");

                        // Add column headers
                        for (int col = 0; col < exportTable.Columns.Count; col++)
                        {
                            worksheet.Cells[1, col + 1].Value = exportTable.Columns[col].ColumnName;
                        }

                        // Add rows
                        for (int row = 0; row < exportTable.Rows.Count; row++)
                        {
                            for (int col = 0; col < exportTable.Columns.Count; col++)
                            {
                                worksheet.Cells[row + 2, col + 1].Value = exportTable.Rows[row][col];
                            }
                        }

                        package.SaveAs(new FileInfo(filePath));
                    }
                }
            }
        }

        private async Task LoadTablesComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    DataTable schemaTable = conn.GetSchema("Tables");

                    TablesComboBox.Items.Clear();
                    foreach (DataRow row in schemaTable.Rows)
                    {
                        string tableName = row[2].ToString();
                        TablesComboBox.Items.Add(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error loading tables: {ex.Message}";
            }
        }

        private async void LoadData_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "Loading data...";
            await LoadTablesComboBox();
            StatusTextBlock.Text = "Data loaded successfully.";
        }

        private async void TablesComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TablesComboBox.SelectedItem == null) return;

            string selectedTable = TablesComboBox.SelectedItem.ToString();
            await LoadTableData(selectedTable);
        }

        private async Task LoadTableData(string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {tableName}", conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        dataTable.Clear();
                        adapter.Fill(dataTable);
                        DataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error loading table data: {ex.Message}";
            }
        }

        private async void DeleteTable_Click(object sender, RoutedEventArgs e)
        {
            if (TablesComboBox.SelectedItem == null)
            {
                StatusTextBlock.Text = "Please select a table to delete.";
                return;
            }

            string selectedTable = TablesComboBox.SelectedItem.ToString();
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the table '{selectedTable}'?", "Delete Table", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();
                        using (SqlCommand cmd = new SqlCommand($"DROP TABLE {selectedTable}", conn))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    StatusTextBlock.Text = $"Table '{selectedTable}' deleted successfully.";
                    await LoadTablesComboBox();
                }
                catch (Exception ex)
                {
                    StatusTextBlock.Text = $"Error deleting table: {ex.Message}";
                }
            }
        }

        private async void UpdateTable_Click(object sender, RoutedEventArgs e)
        {
            if (TablesComboBox.SelectedItem == null)
            {
                StatusTextBlock.Text = "Please select a table to update.";
                return;
            }

            string selectedTable = TablesComboBox.SelectedItem.ToString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row.RowState == DataRowState.Modified)
                        {
                            StringBuilder updateCmd = new StringBuilder($"UPDATE {selectedTable} SET ");

                            foreach (DataColumn column in dataTable.Columns)
                            {
                                if (column.ColumnName != "Id")
                                {
                                    updateCmd.Append($"[{column.ColumnName}] = @{column.ColumnName}, ");
                                }
                            }

                            updateCmd.Length -= 2; // Remove the last comma and space
                            updateCmd.Append($" WHERE Id = @Id");

                            using (SqlCommand cmd = new SqlCommand(updateCmd.ToString(), conn))
                            {
                                foreach (DataColumn column in dataTable.Columns)
                                {
                                    cmd.Parameters.AddWithValue($"@{column.ColumnName}", row[column] ?? DBNull.Value);
                                }
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }

                StatusTextBlock.Text = $"Table '{selectedTable}' updated successfully.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error updating table: {ex.Message}";
            }
        }
    }
}
