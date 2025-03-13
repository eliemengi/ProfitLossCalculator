//schiffsdatenbank.cs

using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections.Generic;
using Datenhaltung.Transfer;

namespace Datenhaltung.Datenbank
{
    static class Schiffsdatenbank
    {
        private static DataTable _tabelleSchiffe = null;
        public static void DatenbankAuslesen(string pfadZurDatenbank)
        {
            if (!File.Exists(pfadZurDatenbank)) { throw new Exception(); }

            string connectionString = "provider=Microsoft.ACE.OLEDB.12.0; data source = " + pfadZurDatenbank;

            OleDbDataAdapter adapterSchiffe = new OleDbDataAdapter("Select * from Schiffe", connectionString);
            DataSet dataSet = new DataSet();
            adapterSchiffe.Fill(dataSet, "Schiffe");
            _tabelleSchiffe = dataSet.Tables["Schiffe"];
        }

        public static Schiff Schiff(int id)
        {
            foreach (DataRow row in _tabelleSchiffe.Rows)
                if (id == Convert.ToInt32(row["ID"]))
                {
                    return new Schiff(
                        row["Name"].ToString(),
                        row["Schiffstyp"].ToString(),
                        Convert.ToInt32(row["JahrIndienststellung"]),
                        Convert.ToInt32(row["JahrAußerdienststellung"])
                        );
                }
            throw new Exception();
        }
    }
}