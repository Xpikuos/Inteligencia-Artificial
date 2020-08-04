using System;
using System.Collections.Generic;

using System.Data.SQLite;
using System.IO;
using XudonV4NetFramework.Common;

namespace GUIXudon.Common
{
    public class DataManager
    {
        public List<string> AllHeadersIDs;
        public List<string> InputHeadersIDs;
        public List<string> OutputHeadersIDs;
        public string HeaderLineWithoutSymbols { get; set; }

        private bool _endOfLine;
        private string _lastLineReadInDataFile;

        private SQLiteConnection _sql_con;
        private StreamReader _readerDataFile;

        /// <summary>
        /// Each element of the list is a group of values separated with comas (just like a line of the Data.csv)
        /// </summary>
        private List<string> _listOfValuesToRetry;
        private string _dataFile;

        public DataManager(string dataFile)
        {
            _dataFile = dataFile;

            GetHeadersIDs(dataFile, out AllHeadersIDs, out InputHeadersIDs, out OutputHeadersIDs);

            try
            {
                _readerDataFile = new StreamReader(_dataFile);
                var headerLine = _readerDataFile.ReadLine(); //jump the header line
                TryToCreateDB();
            }
            catch(Exception)
            {
                CloseDBAndDataFile();
            }
        }

        public void CloseDBAndDataFile()
        {
            _sql_con.Close();
            _readerDataFile.Close();
        }

        public void TryToCreateDB()
        {
            var DBName = $"{Path.GetDirectoryName(_dataFile)}\\{Path.GetFileName(_dataFile).Split('.')[0]}.sqlite";

            var SQLScript = $"{Path.GetDirectoryName(_dataFile)}\\{Path.GetFileName(_dataFile).Split('.')[0]}.sql";

            if (!File.Exists(DBName)) //Path.GetFullPath(DBName)))
            {
                SQLiteConnection.CreateFile(DBName);
            }
            else
            {
                File.Delete(DBName);
                SQLiteConnection.CreateFile(DBName);
            }

            _sql_con = new SQLiteConnection(string.Format("Data Source={0};Version=3;Compress=True;", DBName));
            //("Data Source=DemoT.db;Version=3;New=False;Compress=True;");

            _sql_con.Open();

            using (var reader = new StreamReader(SQLScript))
            {
                var query = "";
                var line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    query += line;
                }
                ExecuteQuery(query);
            }
        }

        private void ExecuteQuery(string txtQuery)
        {
            using (var command = new SQLiteCommand(txtQuery, _sql_con))
            {
                command.ExecuteNonQuery();
            }
        }

        public void WriteInDB(string values)
        {
            var query = $"INSERT INTO Data ({HeaderLineWithoutSymbols}) VALUES ({values})";
            ExecuteQuery(query);
        }

        public string GetLastLineReadInDataFile()
        {
            return _lastLineReadInDataFile;
        }

        public string ReadLineInDataFile()
        {
            if (!_readerDataFile.EndOfStream)
            {
                _lastLineReadInDataFile= _readerDataFile.ReadLine();
                return _lastLineReadInDataFile;
            }
            else
            {
                _endOfLine = true;
                return null;
            }
        }

        public bool GetEndOfLine()
        {
            return _endOfLine;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereElements">Ej.: (A>5 AND A<6) OR (B>7 AND B<8) OR ...</param>
        public void FindElementsToRetry(string whereElements)
        {
            var query = $"SELECT * FROM Data WHERE {whereElements}";
            using (var command = new SQLiteCommand(query, _sql_con))
            {
                using (var reader = command.ExecuteReader())
                {
                    var lineWithValues = string.Empty;
                    while (reader.Read())
                    {
                        foreach (var header in InputHeadersIDs)
                        {
                            if (lineWithValues == string.Empty) { lineWithValues = reader[header].ToString(); }
                            else { lineWithValues = $"{lineWithValues},{reader[header].ToString()}"; }
                        }
                        _listOfValuesToRetry.Add(lineWithValues);
                    }
                }
            }
        }

        /// <summary>
        /// La primera línea del fichero de datos tiene el nombre de los pines de entrada y salida.
        /// - Si el nombre contiene el símbolo > significa que es en una entrada
        /// - Si el nombre contiene el símbolo < significa que es en una salida
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="allHeadersIDs"></param>
        /// <param name="inputHeadersIDs"></param>
        /// <param name="outputHeadersIDs"></param>
        /// <param name="separator"></param>
        private void GetHeadersIDs(string dataFile, out List<string> allHeadersIDs, out List<string> inputHeadersIDs, out List<string> outputHeadersIDs)
        {
            allHeadersIDs = null;
            inputHeadersIDs = null;
            outputHeadersIDs = null;

            using (var reader = new StreamReader(dataFile))
            {
                allHeadersIDs = new List<string>();
                inputHeadersIDs = new List<string>();
                outputHeadersIDs = new List<string>();

                var firstLine = true;
                var columnCounter = 0;

                while (!reader.EndOfStream && firstLine)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(HyperParameters.Separator);

                    foreach (var value in values)
                    {
                        if (firstLine)
                        {
                            HeaderLineWithoutSymbols = line.Replace("_i", string.Empty).Replace("_o", string.Empty);//Trim('>', '<');

                            allHeadersIDs.Add(value);
                            if (value.Contains("_i"))
                            {
                                inputHeadersIDs.Add(value.Replace("_i",string.Empty));
                            }
                            else if (value.Contains("_o"))
                            {
                                outputHeadersIDs.Add(value.Replace("_o", string.Empty));
                            }
                        }
                        columnCounter++;
                    }

                    firstLine = false;
                }
            }
        }

    }
}
