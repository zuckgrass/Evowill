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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            InitComboBox();
        }
        string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=Evowill; Integrated Security=True";
        DataTable dT=new DataTable("Statistics");
        DataTable dT1 = new DataTable("Peopledt");
        DataTable dT2 = new DataTable("Spendingsdt");
        DataTable dT3 = new DataTable("MoneyAmount");
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
            }
            Len = dT2.Rows.Count;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow stat = new MainWindow();
            Hide();
            stat.Show();
        }

        private void ForAll_Click(object sender, RoutedEventArgs e)
        {
            if((Day.Text== "Введіть день" || Day.Text=="") && (Month.Text== "Введіть номер місяця" || Month.Text=="") && (Year.Text== "Введіть рік" || Year.Text==""))
            {
                MessageBox.Show("Введіть хоча б рік");
                return;
            }
            else if((Day.Text == "Введіть день" || Day.Text == "") && (Month.Text == "Введіть номер місяця" || Month.Text == "") && (Year.Text != "Введіть рік" || Year.Text != ""))
            {
                try
                {
                    int.Parse(Year.Text);
                    SqlConnection sqlConn1 = new SqlConnection(Connection);
                    sqlConn1.Open();
                    if (sqlConn1.State == System.Data.ConnectionState.Open)
                    {
                        dT.Clear();
                        SqlDataAdapter Data1 = new SqlDataAdapter("SELECT  coalesce(Cast(Round(Cast(one AS decimal(18,2))/Cast(su as decimal(18,2))*100 ,2) as decimal(18,2)),0) " +
                                                                    "FROM Spendings sp " +
                                                                    "Left join(SELECT SUM(Price) one, IDSpent " +
                                                                              "FROM Notes " +
                                                                              "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                                    "AND Year(Date) = " + Year.Text +
                                                                              "Group by IDSpent " +
                                                                            ")a ON sp.IDspent = a.IDSpent " +
                                                                    "CROSS JOIN(SELECT SUM(Price) su " +
                                                                                 "FROM Notes " +
                                                                               "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                                     "AND Year(Date)= " + Year.Text +
                                                                              ") b; ", sqlConn1);
                        Data1.Fill(dT);
                        dT3.Clear();
                        SqlDataAdapter Data = new SqlDataAdapter("SELECT coalesce(SUM(Price),0) " +
                                                                "FROM Notes " +
                                                            "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                    "AND Year(Date)= " + Year.Text + "; ", sqlConn1);
                        Data.Fill(dT3);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Рік введений неправильно");
                    return;
                }
            }
            else if((Day.Text == "Введіть день" || Day.Text == "") && (Month.Text != "Введіть номер місяця" || Month.Text != "") && (Year.Text != "Введіть рік" || Year.Text != ""))
            {
                try
                {
                    int.Parse(Year.Text);
                    int.Parse(Month.Text);
                    SqlConnection sqlConn = new SqlConnection(Connection);
                    sqlConn.Open();
                    if (sqlConn.State == System.Data.ConnectionState.Open)
                    {
                        dT.Clear();
                        SqlDataAdapter Data1 = new SqlDataAdapter("SELECT  coalesce(Cast(Round(Cast(one AS decimal(18,2))/Cast(su as decimal(18,2))*100 ,2) as decimal(18,2)),0) " +
                                                                    "FROM Spendings sp " +
                                                                    "Left join(SELECT SUM(Price) one, IDSpent " +
                                                                              "FROM Notes " +
                                                                              "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                                    "AND Year(Date) = " + Year.Text + " AND Month(Date)=" + Month.Text +
                                                                              "Group by IDSpent " +
                                                                            ")a ON sp.IDspent = a.IDSpent " +
                                                                    "CROSS JOIN(SELECT SUM(Price) su " +
                                                                                 "FROM Notes " +
                                                                               "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                                     "AND Year(Date)= " + Year.Text + " AND Month(Date)=" + Month.Text +
                                                                              ") b; ", sqlConn);
                        Data1.Fill(dT);
                        dT3.Clear();
                        SqlDataAdapter Data = new SqlDataAdapter("SELECT coalesce(SUM(Price),0) " +
                                                                "FROM Notes " +
                                                            "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                    "AND Year(Date)= " + Year.Text + " AND Month(Date)=" + Month.Text + "; ", sqlConn);
                        Data.Fill(dT3);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Рік або місяць введені неправильно");
                    return;
                }
            }
            else
            {
                try
                {
                    int.Parse(Year.Text);
                    int.Parse(Month.Text);
                    int.Parse(Day.Text);
                    SqlConnection sqlConn = new SqlConnection(Connection);
                    sqlConn.Open();
                    if (sqlConn.State == System.Data.ConnectionState.Open)
                    {
                        dT.Clear();
                        SqlDataAdapter Data1 = new SqlDataAdapter("SELECT  coalesce(Cast(Round(Cast(one AS decimal(18,2))/Cast(su as decimal(18,2))*100 ,2) as decimal(18,2)),0) " +
                                                                    "FROM Spendings sp " +
                                                                    "Left join(SELECT SUM(Price) one, IDSpent " +
                                                                              "FROM Notes " +
                                                                              "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                                    "AND Year(Date) = " + Year.Text + " AND Month(Date)=" + Month.Text + " AND Day(Date)=" + Day.Text +
                                                                              "Group by IDSpent " +
                                                                            ")a ON sp.IDspent = a.IDSpent " +
                                                                    "CROSS JOIN(SELECT SUM(Price) su " +
                                                                                 "FROM Notes " +
                                                                               "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                                     "AND Year(Date)= " + Year.Text + " AND Month(Date)=" + Month.Text + " AND Day(Date)=" + Day.Text +
                                                                              ") b; ", sqlConn);
                        Data1.Fill(dT);
                        dT3.Clear();
                        SqlDataAdapter Data = new SqlDataAdapter("SELECT coalesce(SUM(Price),0) " +
                                                                "FROM Notes " +
                                                            "WHERE IDPeople = (SELECT IDPerson FROM People WHERE Name = '" + UserNamesBox.Text + "') " +
                                                                    "AND Year(Date)= " + Year.Text + " AND Month(Date)=" + Month.Text + "AND Day(Date)=" + Day.Text + "; ", sqlConn);
                        Data.Fill(dT3);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Якийсь з параметрів уведений неправильно");
                    return;
                }
            }
            Food.Content = dT.Rows[0][0].ToString() + "%";
            Entertain.Content = dT.Rows[1][0].ToString() + "%";
            Shopping.Content = dT.Rows[2][0].ToString() + "%";
            Cosmetics.Content = dT.Rows[3][0].ToString() + "%";
            Hygiene.Content = dT.Rows[4][0].ToString() + "%";
            Peace.Content = dT.Rows[5][0].ToString() + "%";
            Gadget.Content = dT.Rows[6][0].ToString() + "%";
            MoneyAmount.Content=dT3.Rows[0][0].ToString()+"UAH";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            Hide();
            window2.Show();
        }
    }
}
