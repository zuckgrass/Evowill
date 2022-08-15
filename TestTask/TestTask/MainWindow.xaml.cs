using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowDataTable();
            InitComboBox();
        }
        string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=Evowill; Integrated Security=True";
        DataTable dT = new DataTable("Notesdt");
        DataTable dT1 = new DataTable("Peopledt");
        DataTable dT2 = new DataTable("Spendingsdt");
        DataTable dT3 = new DataTable("Avgdt");
        DataTable dT4 = new DataTable("Percentdt");
       
        public void ShowDataTable()
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data = new SqlDataAdapter("SELECT per.Name AS [Ім'я], sp.SpentName AS [Вид витрати], Price AS [Ціна], CONVERT(varchar,Date,104) AS [Дата] FROM Notes " +
                                                        "notes Left Join People per ON per.IDPerson = notes.IDPeople " +
                                                        "Left join Spendings sp ON sp.IDspent = notes.IDSpent; ", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
            }
            sqlConn.Close();
        }

        public void InitComboBox()
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data1 = new SqlDataAdapter("SELECT Name FROM People; ", sqlConn);
                dT1.Clear();
                Data1.Fill(dT1);
                SqlDataAdapter Data2 = new SqlDataAdapter("SELECT SpentName FROM Spendings; ", sqlConn);
                dT2.Clear();
                Data2.Fill(dT2);
            }
            int Len = dT1.Rows.Count;
            UserNamesBox.Items.Clear();
            for (int i = 0; i < Len; i++)
            {
                UserNamesBox.Items.Add(dT1.Rows[i][0].ToString());
                UserNamesBox1.Items.Add(dT1.Rows[i][0].ToString());
            }
            Len = dT2.Rows.Count;
            SpendBox.Items.Clear();
            for (int i = 0; i < Len; i++)
            {
                SpendBox.Items.Add(dT2.Rows[i][0].ToString());
                SpendBox1.Items.Add(dT2.Rows[i][0].ToString());
            }
            sqlConn.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (UserNamesBox.SelectedIndex == -1)
            {
                MessageBox.Show("Виберіть будь ласка ім'я вище!");
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM Notes WHERE IDPeople=(SELECT IDPerson FROM People WHERE Name='"+ UserNamesBox.Text+ "');", sqlConn);
                    dT.Clear();
                    Data.Fill(dT);
                    dataGrid.ItemsSource = dT.DefaultView;
                }
                sqlConn.Close();
                ShowDataTable();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (UserNamesBox.SelectedIndex == -1 || SpendBox.SelectedIndex==-1 || PriceBox.Text== "Введіть ціну" || PriceBox.Text =="")
            {
                MessageBox.Show("Виберіть будь ласка усі дані вище!");
            }
            else if (DateBox.Text != "Введіть дату" && DateBox.Text!="")
            {
                try
                {
                    DateTime.Parse(DateBox.Text);
                    SqlConnection sqlConn = new SqlConnection(Connection);
                    sqlConn.Open();
                    if (sqlConn.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter Data = new SqlDataAdapter("INSERT INTO Notes (IDNote, IDPeople, IDSpent, Price, Date) " +
                                                                "VALUES((SELECT MAX(IDNote) FROM Notes) + 1, " +
                                                                "(SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "'), " +
                                                                "(SELECT IDspent FROM Spendings WHERE SpentName = '" + SpendBox.Text + "'), " +
                                                                PriceBox.Text + ", '" + DateBox.Text + "'); ", sqlConn);
                        dT.Clear();
                        Data.Fill(dT);
                        dataGrid.ItemsSource = dT.DefaultView;
                    }
                    sqlConn.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Дата введена неправильно");
                }
            }
            else
            {
                try
                {
                    int.Parse(PriceBox.Text);
                    SqlConnection sqlConn = new SqlConnection(Connection);
                    sqlConn.Open();
                    if (sqlConn.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter Data = new SqlDataAdapter("INSERT INTO Notes (IDNote, IDPeople, IDSpent, Price) " +
                                                                "VALUES((SELECT MAX(IDNote) FROM Notes) + 1, " +
                                                                "(SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "'), " +
                                                                "(SELECT IDspent FROM Spendings WHERE SpentName = '" + SpendBox.Text + "'), " +
                                                                PriceBox.Text + "); ", sqlConn);
                        dT.Clear();
                        Data.Fill(dT);
                        dataGrid.ItemsSource = dT.DefaultView;
                    }
                    sqlConn.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ціна введена неправильно");
                }
            }
            ShowDataTable();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(UserNamesBox1.SelectedIndex == -1 || SpendBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Введіть усі дані!");
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                if (sqlConn.State == System.Data.ConnectionState.Open)
                {
                    SqlDataAdapter Data = new SqlDataAdapter("SELECT AVG(Price) FROM Notes "+
                                            "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '"+ UserNamesBox1.Text + "') " +
                                            "AND IDSpent = (SELECT IDSpent FROM Spendings WHERE SpentName = '"+ SpendBox1.Text + "');", sqlConn);
                    dT3.Clear();
                    Data.Fill(dT3);
                    Avg.Content = dT3.Rows[0][0].ToString()+"UAH";
                    if (Avg.Content == "UAH")
                    {
                        Avg.Content = "Дані відсутні";
                    }
                    SqlDataAdapter Data1 = new SqlDataAdapter("SELECT Cast(Cast(one AS decimal(18,2))/Cast(su as decimal(18,2))*100 as integer) FROM("+
                                                            "SELECT SUM(Price) one FROM Notes "+
                                                            "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '"+ UserNamesBox1.Text + "') "+
                                                            "AND IDSpent = (SELECT IDSpent FROM Spendings WHERE SpentName = '"+ SpendBox1.Text + "'))"+
                                                            "a CROSS JOIN(SELECT SUM(Price) su FROM Notes "+
                                                            "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '"+ UserNamesBox1.Text + "')) b; ", sqlConn);
                    dT4.Clear();
                    Data1.Fill(dT4);
                    Percent.Content = dT4.Rows[0][0].ToString() + "%";
                    if (Percent.Content == "%")
                    {
                        Percent.Content = "Дані відсутні";
                    }
                }
                sqlConn.Close();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data = new SqlDataAdapter("DELETE FROM Notes;", sqlConn);
                dT.Clear();
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
            }
            sqlConn.Close();
            ShowDataTable();
        }

        private void Stat_Click(object sender, RoutedEventArgs e)
        {
            Window1 stat = new Window1();
            Hide();
            stat.Show();
        }

    }
}
