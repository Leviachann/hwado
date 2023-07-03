using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace StoreApp
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=.;Initial Catalog=DB name;Integrated Security=True;MultipleActiveResultSets=True";

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }
        private void LoadProducts()
        {
            try
            {
                string query = "SELECT ProductID, ProductName, Price FROM Products";
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                    connection.Open();
                    dataAdapter.Fill(dataTable);
                }

                dgProducts.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string productID = txtProductID.Text;
                string productName = txtProductName.Text;
                decimal price = decimal.Parse(txtPrice.Text);

                string query = "INSERT INTO Products (ProductID, ProductName, Price) VALUES (@ProductID, @ProductName, @Price)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@ProductName", productName);
                    command.Parameters.AddWithValue("@Price", price);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Product added successfully!");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = dgProducts.SelectedItem as DataRowView;
                if (selectedRow == null)
                {
                    MessageBox.Show("Please select a product to update.");
                    return;
                }

                string productID = txtProductID.Text;
                string productName = txtProductName.Text;
                decimal price = decimal.Parse(txtPrice.Text);

                string query = "UPDATE Products SET ProductName = @ProductName, Price = @Price WHERE ProductID = @ProductID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@ProductName", productName);
                    command.Parameters.AddWithValue("@Price", price);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Product updated successfully!");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = dgProducts.SelectedItem as DataRowView;
                if (selectedRow == null)
                {
                    MessageBox.Show("Please select a product to delete.");
                    return;
                }

                int productID = (int)selectedRow["ProductID"];

                string query = "DELETE FROM Products WHERE ProductID = @ProductID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Product deleted successfully!");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }
    }
}
