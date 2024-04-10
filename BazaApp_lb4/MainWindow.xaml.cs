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
using static BazaApp_lb4.bdOperation;

namespace BazaApp_lb4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string db_name = "";

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Tab.Items.Clear();

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //имя базы данных
            db_name = dlg.FileName;

            if (db_name != "")
            {
                bdOperation bd = new bdOperation();
                bd.bdNamePath= db_name;
                bd.LogStudents(Tab.Items);
            }
        }

        private void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Tab.SelectedIndex > -1 || db_name != null)
            {
                WPFAddStudents add = new WPFAddStudents();

                if (add.ShowDialog() == true)
                {
                    Tab.Items.Clear();

                    //Берем данные из открывшегося окна
                    var Studentik = new StudentInGrid()
                    {
                        uID = int.Parse(add.TextboxUID.Text),
                        Lastname = add.TextboxStudentLastname.Text,
                        Math = int.Parse(add.TextboxStudentMath.Text)
                    };

                    //добавление ученика в базу
                    bdOperation bd = new bdOperation();
                    bd.bdNamePath = db_name;
                    bd.AddStudent(Studentik.uID, Studentik.Math, Studentik.Lastname);

                    //Вывести в grid
                    bd.LogStudents(Tab.Items);
                }
            }
        }

        private void ButtonDelStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Tab.SelectedIndex > -1)
            {

                //Взятие данных о студенте из grid
                StudentInGrid Studentik = (StudentInGrid)Tab.SelectedItem;

                //удаление ученика из базы
                bdOperation bd = new bdOperation();
                bd.bdNamePath = db_name;
                bd.DeleteStudent(Studentik.uID);

                Tab.Items.Clear();

                //Вывод списка студентов в лог
                bd.LogStudents(Tab.Items);
            }
        }

        private void ButtonEditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Tab.SelectedIndex > -1)
            {
                //Открытие окна редактирования
                WPFAddStudents add = new WPFAddStudents();

                //Взятие данных из Grid
                var StudentikGrid = (StudentInGrid)Tab.SelectedItem;

                //Перенос данных из Grid в окно редактирования
                add.TextboxUID.Text = StudentikGrid.uID.ToString();
                add.TextboxStudentLastname.Text = StudentikGrid.Lastname.ToString();
                add.TextboxStudentMath.Text = StudentikGrid.Math.ToString();

                //Если нажали кнопку "добавить"
                if (add.ShowDialog() == true)
                {
                    Tab.Items.Clear();

                    //Берем данные из открывшегося окна
                    var StudentikAdd = new StudentInGrid()
                    {
                        uID = int.Parse(add.TextboxUID.Text),
                        Lastname = add.TextboxStudentLastname.Text,
                        Math = int.Parse(add.TextboxStudentMath.Text)
                    };

                    //редактирование студента в базе
                    bdOperation bd = new bdOperation();
                    bd.bdNamePath = db_name;
                    bd.EditStudent(StudentikAdd.uID, StudentikGrid.uID, StudentikAdd.Math, StudentikAdd.Lastname);

                    bd.LogStudents(Tab.Items);
                }
            }
        }
    }
}
