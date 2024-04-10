using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static BazaApp_lb4.MainWindow;

namespace BazaApp_lb4
{
    public class bdOperation
    {
        //Class for Grid
        public class StudentInGrid
        {
            public int uID { get; set; }
            public string Lastname { get; set; }
            public int Math { get; set; }
        }

        public string bdNamePath;

        public void LogStudents(ItemCollection items)
        {
            {
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=" + bdNamePath + ";Version=3;");

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
                    items.Add(data);
                }
                //закрытие соединения с базой данных
                m_dbConnection.Close();
            }
        }

        public void DeleteStudent(int uID)
        {
            //подключаемся к базе
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=" + bdNamePath + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();
            string sqlDel = "DELETE FROM Students WHERE uID = " + uID + ";" +
                    "DELETE FROM Grades WHERE uID = " + uID;
            SQLiteCommand commandDel = new SQLiteCommand(sqlDel, m_dbConnection);
            commandDel.ExecuteReader();

            m_dbConnection.Close();
        }

        public void AddStudent(int uID, int Math, string Lastname)
        {
            //подключаемся к базе
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=" + bdNamePath + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();


            string sqlInsert = "INSERT INTO Students (uID, Lastname) VALUES (" + uID + ", '" + Lastname + "');" +
                        "INSERT INTO Grades (uID, Math) VALUES (" + uID + ", " + Math + ")";
            SQLiteCommand commandInsert = new SQLiteCommand(sqlInsert, m_dbConnection);
            commandInsert.ExecuteReader();

            m_dbConnection.Close();
        }

        public void EditStudent(int OLDuID, int NEWuID, int Math, string Lastname)
        {
            //подключаемся к базе
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=" + bdNamePath + ";Version=3;");

            //открытие соединения с базой данных
            m_dbConnection.Open();

            string sqlUpdate = "UPDATE Students SET Lastname = '" + Lastname + "', uID = "
                    + OLDuID + " WHERE uID = " + NEWuID
                    + ";UPDATE Grades SET Math = " + Math + ", uID = " + OLDuID + " WHERE uID = " + NEWuID + ";";
            SQLiteCommand commandUpdate = new SQLiteCommand(sqlUpdate, m_dbConnection);
            commandUpdate.ExecuteReader();

            m_dbConnection.Close();

        }
    }
}
