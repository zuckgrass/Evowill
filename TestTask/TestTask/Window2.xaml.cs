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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            FillLabel();
        }
        string Connection = @"Data Source=DESKTOP-EN4LANA; initial Catalog=Evowill; Integrated Security=True";
        DataTable dT1=new DataTable("People");
        DataTable dT2=new DataTable("Spendings");
        public void FillLabel()
        {
            SqlConnection sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter Data1 = new SqlDataAdapter("SELECT pep.Name, SUM(Price) FROM Notes "+
                                                        "note Left join People pep ON pep.IDPerson = note.IDPeople "+
                                                        "GROUP by pep.Name "+
                                                        "ORDER BY Sum(Price) desc; ", sqlConn);
                dT1.Clear();
                Data1.Fill(dT1);
                First.Content = dT1.Rows[0][0].ToString() + "-" + dT1.Rows[0][1].ToString()+"UAH";
                Second.Content = dT1.Rows[1][0].ToString() + "-" + dT1.Rows[1][1].ToString() + "UAH";
                Third.Content = dT1.Rows[2][0].ToString() + "-" + dT1.Rows[2][1].ToString() + "UAH";
                Fourth.Content = dT1.Rows[3][0].ToString() + "-" + dT1.Rows[3][1].ToString() + "UAH";
                Fifth.Content = dT1.Rows[4][0].ToString() + "-" + dT1.Rows[4][1].ToString() + "UAH";
                SqlDataAdapter Data2 = new SqlDataAdapter("SELECT sp.SpentName, AVG(Price) FROM Notes "+
                                                        "note LEFT JOIN Spendings sp ON note.IDSpent = sp.IDspent "+
                                                        "GROUP BY SpentName "+
                                                        "ORDER BY AVG(Price) desc; ", sqlConn);
                dT2.Clear();
                Data2.Fill(dT2);
                First_Copy.Content = dT2.Rows[0][0].ToString() + "-" + dT2.Rows[0][1].ToString() + "UAH";
                Second_Copy.Content = dT2.Rows[1][0].ToString() + "-" + dT2.Rows[1][1].ToString() + "UAH";
                Third_Copy.Content = dT2.Rows[2][0].ToString() + "-" + dT2.Rows[2][1].ToString() + "UAH";
                Fourth_Copy.Content = dT2.Rows[3][0].ToString() + "-" + dT2.Rows[3][1].ToString() + "UAH";
                Fifth_Copy.Content = dT2.Rows[4][0].ToString() + "-" + dT2.Rows[4][1].ToString() + "UAH";
                Sixth_Copy.Content = dT2.Rows[5][0].ToString() + "-" + dT2.Rows[5][1].ToString() + "UAH";
                Seventh_Copy.Content = dT2.Rows[6][0].ToString() + "-" + dT2.Rows[6][1].ToString() + "UAH";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow=new MainWindow();
            Hide();
            mainWindow.Show();
        }
    }
}
