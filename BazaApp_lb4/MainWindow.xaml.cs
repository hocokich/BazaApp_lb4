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
using System.Data.SQLite;
using Microsoft.Win32;
using static BazaApp_lb4.MainWindow;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace BazaApp_lb4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; } // тут будет форма
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public class StudentInGrid
        {
            public int uID { get; set; }
            public string Lastname { get; set; }
            public int Math { get; set; }
        }

        string db_name = "";

        private void LogStudents()
        {
            {
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");

                //открытие соединения с базой данных
                m_dbConnection.Open();

                //вывод учеников в лог
                string sqlShow = "SELECT Students.uID, Lastname, Math FROM Students, Grades where Students.uID=Grades.uID";
                SQLiteCommand command = new SQLiteCommand(sqlShow, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //чтение строки из data
                    var data = new StudentInGrid
                    {
                        uID = int.Parse(reader["uID"].ToString()),
                        Lastname = reader["Lastname"].ToString(),
                        Math = int.Parse(reader["Math"].ToString())
                    };
                    //добавление студента в DataGrid
                    Log.Items.Add(data);
                }

                //закрытие соединения с базой данных
                m_dbConnection.Close();
            }
        }

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Log.Items.Clear();

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //имя базы данных
            db_name = dlg.FileName;

            if (db_name != "")
            {
                LogStudents();
            }
        }

        private void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Log.SelectedIndex > -1 || db_name != null){

                WPFAddStudents add = new WPFAddStudents();

                if (add.ShowDialog() == true){
                    Log.Items.Clear();

                    //Берем данные из открывшегося окна
                    var Studentik = new StudentInGrid(){
                        uID = int.Parse(add.TextboxUID.Text),
                        Lastname = add.TextboxStudentLastname.Text,
                        Math = int.Parse(add.TextboxStudentMath.Text)
                    };

                    //добавление ученика в базу
                    BD bd = new BD();
                    bd.bdNamePath = db_name;
                    bd.AddStudent(Studentik.uID, Studentik.Math, Studentik.Lastname);

                    LogStudents();
                }
            }
        }

        private void ButtonDelStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Log.SelectedIndex > -1){

                //Взятие данных о студенте из grid
                StudentInGrid Studentik = (StudentInGrid)Log.SelectedItem;

                //удаление ученика из базы
                BD bd = new BD();
                bd.bdNamePath = db_name;
                bd.DeleteStudent(Studentik.uID);

                Log.Items.Clear();

                //Вывод списка студентов в лог
                LogStudents();
            }
        }

        private void ButtonEditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Log.SelectedIndex > -1)
            {
                //Открытие окна редактирования
                WPFAddStudents add = new WPFAddStudents();

                //Взятие данных из Grid
                StudentInGrid StudentikGrid = (StudentInGrid)Log.SelectedItem;

                //Перенос данных из Grid в окно редактирования
                add.TextboxUID.Text = StudentikGrid.uID.ToString();
                add.TextboxStudentLastname.Text = StudentikGrid.Lastname.ToString();
                add.TextboxStudentMath.Text = StudentikGrid.Math.ToString();

                //Если нажали кнопку "добавить"
                if (add.ShowDialog() == true)
                {
                    Log.Items.Clear();

                    //Берем данные из открывшегося окна
                    var StudentikAdd = new StudentInGrid()
                    {
                        uID = int.Parse(add.TextboxUID.Text),
                        Lastname = add.TextboxStudentLastname.Text,
                        Math = int.Parse(add.TextboxStudentMath.Text)
                    };

                    //редактирование студента в базе
                    BD bd = new BD();
                    bd.bdNamePath = db_name;
                    bd.EditStudent(StudentikAdd.uID, StudentikGrid.uID, StudentikAdd.Math, StudentikAdd.Lastname);

                    //Log.Items.Clear();
                    bd.ShowStudents();
                }
            }
        }
    }
}
