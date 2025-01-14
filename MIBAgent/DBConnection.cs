﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace MIBAgent
{
    public class DBConnection
    {
        private MySqlConnection connection = null;

        //Set these to null after production and read value for excrypted text
        private string db_name = "";
        private string pwd = "";
        private string uname = "";
        private string server = "";
        private string portno = "";
        public DBConnection(string ip,string port)
        {
            var file = new StreamReader(@"../../config.yaml");
            var deserial = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();
            var config = deserial.Deserialize<dynamic>(file);
            this.db_name = config["db_name"];
            this.pwd = config["db_password"];
            this.uname = config["db_user"];
            server = ip;
            portno = port;
        }
        public DBConnection()
        {
            var file = new StreamReader(@"../../config.yaml");
            var deserial = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();
            var config = deserial.Deserialize<dynamic>(file);
            this.db_name = config["db_name"];
            this.pwd = config["db_password"];
            this.uname = config["db_user"];
            this.server = config["server"];
            this.portno = config["port"];
            string connstring = string.Format("Server={0};Port={4};Database={1};Uid={2};pwd={3}", server, db_name, uname, pwd,portno);
            connection = new MySqlConnection(connstring);
            connection.Open();
        }
     
        public bool IsConnect(){
            if (connection == null){
                    return false;
            }
            return true;
        }
        
        public MySqlConnection GetConnection(){return connection;}
        public void Close(){connection.Close();}
        public string GetDatabaseName(){return db_name;}
        public void SetDatabaseName(string value){db_name = value;}
        public void SetPassword(string value) {pwd = value;}
    }
}

/*
    var dbCon = DBConnection.Instance();
    dbCon.DatabaseName = "YourDatabase";
    if (dbCon.IsConnect())
    {
        //suppose col0 and col1 are defined as VARCHAR in the DB
        string query = "SELECT col0,col1 FROM YourTable";
        var cmd = new MySqlCommand(query, dbCon.Connection);
        var reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            string someStringFromColumnZero = reader.GetString(0);
            string someStringFromColumnOne = reader.GetString(1);
            Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
        }
        dbCon.Close();
    }
*/