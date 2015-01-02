using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BloodBankManagementSystem
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static SqlConnection Conn;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void TC_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ti = TC.SelectedItem as TabItem;
            if (ti != null) Title = ti.Header.ToString();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult key = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No);
            e.Cancel = (key == MessageBoxResult.No);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Conn = DatabaseConnection.GetConnection();
            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }
            string query = string.Format("select * from information");

            var cmd = new SqlCommand(query, Conn);

            var ds = new DataSet();
            var adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            Panel.DataContext = ds.Tables[0].DefaultView;
            Conn.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Name1TextBox.Text == "" || BloodgroupComboBox.Text == "" || AddressTextBox.Text == "" ||
                PhonenumberTextBox.Text == "" || CurrentcityTextBox.Text == "")
            {
                MessageBox.Show("Please fill up all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                NameTextBox.Focus();
            }

            else if (!Regex.Match(Name1TextBox.Text, @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                BloodgroupBox.Focus();
                PhonenumberTextBox.Focus();
            }

            else if (!Regex.Match(PhonenumberTextBox.Text, @"^\d{10}||\d{11}$").Success)
            {
                MessageBox.Show("Invalid Phone Number", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                AddressTextBox.Focus();
            }

            else if ( !Regex.Match(AddressTextBox.Text,
                    @"^([A-Z][a-z]+||\d{3}-[A-Z][a-z]+\s[A-Z][a-z]+,[A-Z][a-z]+,[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Address", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                CurrentcityTextBox.Focus();
            }

            else if (!Regex.Match(CurrentcityTextBox.Text, @"^[A-Z][a-z]+$").Success)
            {
                MessageBox.Show("Invalid Current City Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (Activate())
            {
                Conn = DatabaseConnection.GetConnection();
                if (Conn.State.ToString() == "Closed")
                {
                    Conn.Open();
                }

                string query = string.Format("insert into information values('{0}','{1}','{2}','{3}','{4}','{5}')",
                    IdTextBox.Text, Name1TextBox.Text, BloodgroupComboBox.SelectionBoxItem, PhonenumberTextBox.Text,
                    AddressTextBox.Text, CurrentcityTextBox.Text);

                var cmd = new SqlCommand(query, Conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data is Inserted!!", "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Conn.Close();
            }
        }


        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text == "" || BloodgroupBox.Text == "" || AddressBox.Text == "" ||
                PhonenumberBox.Text == "" || CurrentcityBox.Text == "")
            {
                MessageBox.Show("Please fill up all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                NameTextBox.Focus();
            }

            if (!Regex.Match(NameBox.Text,
                @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                BloodgroupBox.Focus();
                PhonenumberTextBox.Focus();
            }

            else if (!Regex.Match(PhonenumberBox.Text, @"^\d{10}||\d{11}$").Success)
            {
                MessageBox.Show("Invalid Phone Number", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                AddressTextBox.Focus();
            }

            else if (
                !Regex.Match(AddressBox.Text,
                    @"^([A-Z][a-z]+||\d{3}-[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+,[A-Z][a-z]+,[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Address", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                CurrentcityTextBox.Focus();
            }

            else if (!Regex.Match(CurrentcityBox.Text, @"^[A-Z][a-z]+$").Success)
            {
                MessageBox.Show("Invalid Current City Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (Activate())
            {
                Conn = DatabaseConnection.GetConnection();
                if (Conn.State.ToString() == "Closed")
                {
                    Conn.Open();
                }

                SqlCommand cmd = new SqlCommand("update information set Name=@name,Bloodgroup=@blood,Phonenumber=@phone,Address=@Address, Currentcity=@city where Id=@id",Conn);

                cmd.Parameters.AddWithValue("@id", IdComboBox.SelectionBoxItem);
                cmd.Parameters.AddWithValue("@name", NameBox.Text);
                cmd.Parameters.AddWithValue("@blood", BloodgroupBox.SelectionBoxItem);
                cmd.Parameters.AddWithValue("@phone", PhonenumberBox.Text);
                cmd.Parameters.AddWithValue("@Address", AddressBox.Text);
                cmd.Parameters.AddWithValue("@city", CurrentcityBox.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data is Updated!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Conn.Close();
            }
        }

        private void IdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (true)
            {
                Conn = DatabaseConnection.GetConnection();
                if (Conn.State.ToString() == "closed")
                {
                    Conn.Open();
                }

                SqlCommand cmd=new SqlCommand("select Name,Bloodgroup,Phonenumber,Address,Currentcity from information where Id=@id",Conn);

                cmd.Parameters.AddWithValue("@id", IdComboBox.SelectedItem);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    string sbloodgroup = reader[1].ToString();
                    string sphonenumber = reader[2].ToString();
                    string saddress = reader[3].ToString();
                    string scuurentcity = reader[4].ToString();

                    NameBox.Text = sname;
                    BloodgroupBox.Text = sbloodgroup;
                    PhonenumberBox.Text = sphonenumber;
                    AddressBox.Text = saddress;
                    CurrentcityBox.Text = scuurentcity;
                }
            }
        }


        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            Conn = DatabaseConnection.GetConnection();
            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand("delete  from information where Id=@id",Conn);

            cmd.Parameters.AddWithValue("@id", IdComboBox1.SelectionBoxItem);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Your data is deleted!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void IdComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (true)
            {
                Conn = DatabaseConnection.GetConnection();
                if (Conn.State.ToString() == "closed")
                {
                    Conn.Open();
                }

                SqlCommand cmd = new SqlCommand("select Name,Bloodgroup,Phonenumber,Address,Currentcity from information where Id=@id",Conn);

                cmd.Parameters.AddWithValue("@id", IdComboBox1.SelectedItem);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    string sbloodgroup = reader[1].ToString();
                    string sphonenumber = reader[2].ToString();
                    string saddress = reader[3].ToString();
                    string scuurentcity = reader[4].ToString();

                    NameBox1.Text = sname;
                    BloodgroupBox1.Text = sbloodgroup;
                    PhonenumberBox1.Text = sphonenumber;
                    AddressBox1.Text = saddress;
                    CurrentcityBox1.Text = scuurentcity;
                }
            }
        }

        public void GetId()
        {
            Conn = DatabaseConnection.GetConnection();
            if (Conn.State.ToString() == "Closed")
            {
                Conn.Open();
            }

            string query = string.Format("select Id from information");
            var cmd = new SqlCommand(query, Conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int sid = int.Parse(reader[0].ToString());

                IdComboBox.Items.Add(sid);
                IdComboBox1.Items.Add(sid);
                int id = sid + 1;
                IdTextBox.Text = id.ToString(CultureInfo.InvariantCulture);
            }
            Conn.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetId();
         
        }

        private void AddAdminButton_OnClick(object sender, RoutedEventArgs e)
        {
          

            if (NameTextBox.Text == string.Empty || AdminPasswordBox.Password == string.Empty)
            {
                MessageBox.Show("Enter all the fields and select position!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (
                !Regex.Match(NameTextBox.Text,
                    @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Error In Admin Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(AdminPasswordBox.Password, @"^[a-z0-9_-]{6,18}$").Success)
            {
                MessageBox.Show("Error in Password ,Password length must be 6.", "Message", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

         
            else if (Activate())
            {
                if (GetCountAdminId() > 4)
                {
                    Conn = DatabaseConnection.GetConnection();

                    if (Conn.State.ToString() == "closed")
                    {
                        Conn.Open();
                    }
                    
                    String query = string.Format("insert into admin values('{0}','{1}','{2}')",
                        AdminIdTextBox.Text, NameTextBox.Text, AdminPasswordBox.Password);
  
                    try
                    {
                      
                        SqlCommand cmd = new SqlCommand(query, Conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data is inserted Successfully!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Admin Can't be more than four!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private int GetCountAdminId()
        {
            int i = 0;

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }
            String query = string.Format("select count(Id) from admin ");
          
            try
            {            
                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i = int.Parse(reader[0].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;

        }

        private void OkayButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateAdminButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NameTextBox1.Text == "" || AdminPasswordBox1.Password == "" || AdminPasswordBox.Password == "")
            {
                MessageBox.Show("Fill up all the boxes!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (
              !Regex.Match(NameTextBox1.Text,
                  @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Error In Admin Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(AdminPasswordBox1.Password, @"^[a-z0-9_-]{6,18}$").Success)
            {
                MessageBox.Show("Error in Password ,Password length must be 6.", "Message", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            else if (!Regex.Match(NewAdminPasswordBox.Password, @"^[a-z0-9_-]{6,18}$").Success)
            {
                MessageBox.Show("Error in Password ,Password length must be 6.", "Message", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }


            else if (Activate())
            {
                Conn = DatabaseConnection.GetConnection();

                if (Conn.State.ToString() == "closed")
                {
                    Conn.Open();
                }

                SqlCommand cmd = new SqlCommand("update admin set Name=@name , Password=@pass where Id=@id",Conn);
                
                try
                {

                    cmd.Parameters.AddWithValue("@id", AdminIdComboBox.SelectionBoxItem);
                    cmd.Parameters.AddWithValue("@name", NameTextBox1.Text);
                    cmd.Parameters.AddWithValue("@pass", NewAdminPasswordBox.Password);
                        
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data is updated Successfully!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AdminIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand("select Name,Password from admin where Id=@id",Conn);
            
            try
            {
                cmd.Parameters.AddWithValue("@id", AdminIdComboBox.SelectedItem);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    string spassword = reader[1].ToString();
                   
                    NameTextBox1.Text = sname;
                    AdminPasswordBox1.Password = spassword;
                    
                }
                Conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void GetAdminId()
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }
            try
            {
                SqlCommand cmd = new SqlCommand("select Id from Admin", Conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());
                    AdminIdComboBox.Items.Add(sid);

                    int id = sid + 1;
                    AdminIdTextBox.Text = id.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            Conn = DatabaseConnection.GetConnection();

            if(Conn.State.ToString()=="closed")
            {
                Conn.Open();
            }

            string query = string.Format("select Id,Name,Password from admin");
            
            DataSet ds = new DataSet();           
            SqlDataAdapter adapter = new SqlDataAdapter();
   
            SqlCommand cmd=new SqlCommand(query,Conn);
             
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            AdminListView.DataContext = ds.Tables[0].DefaultView;  // ListView
        }

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventIdTextBox.Text == "" || EventnameTextBox.Text == "" || EventPlaceTextBox.Text == "")
            {
                MessageBox.Show("Fill all the boxes!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                Conn = DatabaseConnection.GetConnection();

                if (Conn.State.ToString() == "closed")
                {
                    Conn.Open();
                }

                string date = EventDatePicker.Text;

                string query = string.Format("insert into event values('{0}','{1}','{2}','{3}')", EventIdTextBox.Text,
                    EventnameTextBox.Text, EventPlaceTextBox.Text, date);
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Event is just created!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Conn.Close();
            }
        }

        private void EventUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventnameBox.Text == "" || EventPlaceBox.Text == "")
            {
                MessageBox.Show("Fill all the boxes!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }

            SqlCommand cmd = new SqlCommand("update event set Name=@name,Place=@place ,Date=@date where Id=@id",Conn);

            cmd.Parameters.AddWithValue("@name", EventnameBox.Text);
            cmd.Parameters.AddWithValue("@place", EventPlaceBox.Text);
            cmd.Parameters.AddWithValue("@date", EventDate.Text);
            cmd.Parameters.AddWithValue("@id", EventIdComboBox.SelectionBoxItem);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Event is just updated!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Conn.Close();

        }

        private void EventLoadButton_Click(object sender, RoutedEventArgs e)
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }
            String query = string.Format("select Id,Name,Place,Date from event");
           
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                
                SqlCommand cmd = new SqlCommand(query,Conn);
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                EventListView.DataContext = ds.Tables[0].DefaultView;  // ListView
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Conn.Close();
        }

        private void Okay1Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EventIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }

            SqlCommand cmd=new SqlCommand("select Name,Place,date from event where Id=@id",Conn);
            cmd.Parameters.AddWithValue("@id", EventIdComboBox.SelectedItem);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string sname = reader[0].ToString();
                string splace = reader[1].ToString();
                string sdate = reader[2].ToString();

                EventnameBox.Text = sname;
                EventPlaceBox.Text = splace;
                EventDate.Text = sdate;
            }
            Conn.Close();

        }

        private void GetEventId()
        {
            Conn = DatabaseConnection.GetConnection();
            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }

            SqlCommand cmd=new SqlCommand("select Id from event",Conn);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int sid = int.Parse(reader[0].ToString());

                EventIdComboBox.Items.Add(sid);

                int id = sid+1;
                EventIdTextBox.Text = id.ToString(CultureInfo.InvariantCulture);
            }

        }

        private void TabItem_Loaded_2(object sender, RoutedEventArgs e)
        {
            GetEventId();
            GetAdminId();
            GetCountAdminId();

            EventDatePicker.Text = DateTime.Today.ToString(CultureInfo.InvariantCulture);
        }
    }
}