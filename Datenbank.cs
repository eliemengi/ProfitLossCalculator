using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using EasyBankingMiniGuV.Datenhaltung.Transfer;



namespace EasyBankingMiniGuV.Datenhaltung.Datenbank
{
   public static class Datenbank
    {
        public static bool IstGeladen { get; private set; } = false;


        public static int[] PeriodenIDs
        {
            get
            {
                if (_periodeTable == null)
                {
                    throw new Exception("Die Tabelle 'Perioden' wurde nicht geladen. Bitte rufen Sie zuerst 'DatenbankAuslesen' auf.");
                }

                var ids = new int[_periodeTable.Rows.Count];
                for (int i = 0; i < _periodeTable.Rows.Count; i++)
                {
                    ids[i] = Convert.ToInt32(_periodeTable.Rows[i]["Id"]);
                }
                return (int[])ids.Clone();
            }
        }



        private static DataTable _periodeTable = null;
        private static DataTable _steuernTable = null;
        private static DataTable _zinsüberschussTable = null;
        private static DataTable _betriebsaufwendungenTable = null;

        

        public static void DatenbankAuslesen(string pfadZurDatenbank)
        {
            if (!File.Exists(pfadZurDatenbank))
                throw new Exception();

            string connectionString = "provider=Microsoft.ACE.OLEDB.12.0; data source = " + pfadZurDatenbank;

            OleDbDataAdapter adapterPeriode = new OleDbDataAdapter("SELECT * FROM Perioden", connectionString);
            OleDbDataAdapter adapterSteuern = new OleDbDataAdapter("SELECT * FROM Steuern", connectionString);
            OleDbDataAdapter adapterZinsüberschuss = new OleDbDataAdapter("SELECT * FROM Zinsueberschuss", connectionString);
            OleDbDataAdapter adapterBetriebsaufwendungen = new OleDbDataAdapter("SELECT * FROM Betriebsaufwendungen", connectionString);

            DataSet dataSet = new DataSet();
            adapterSteuern.Fill(dataSet, "Steuern");
            adapterPeriode.Fill(dataSet, "Perioden");
            adapterZinsüberschuss.Fill(dataSet, "Zinsueberschuss");
            adapterBetriebsaufwendungen.Fill(dataSet, "Betriebsaufwendungen");

            _periodeTable = dataSet.Tables["Perioden"];
            _steuernTable = dataSet.Tables["Steuern"];
            _zinsüberschussTable = dataSet.Tables["Zinsueberschuss"];
            _betriebsaufwendungenTable = dataSet.Tables["Betriebsaufwendungen"];
            IstGeladen = true;
        }

        public static Periode Periode(int id)
        {
            if (_periodeTable == null)
            {
                throw new Exception("Die Tabelle 'Periode' wurde nicht geladen.");
            }

            foreach (DataRow row in _periodeTable.Rows)
            {
                if (id == Convert.ToInt32(row["Id"]))
                {
                    return new Periode(
                        Convert.ToInt32(row["Id"]),
                        row["Bezeichnung"].ToString(),
                        Convert.ToDateTime(row["Beginn"]),
                        Convert.ToDateTime(row["Ende"])
                    );
                }
            }
            throw new Exception($"Keine Periode mit der Id {id} gefunden.");
        }


        public static Steuern Steuern(int id)
        {
            if (_steuernTable == null)
            {
                throw new Exception("Die Tabelle 'Steuern' wurde nicht geladen. Bitte rufen Sie zuerst 'DatenbankAuslesen' auf.");
            }

            foreach (DataRow row in _steuernTable.Rows)
            {
                if (id == Convert.ToInt32(row["Id"]))
                {
                    return new Steuern(
                        Convert.ToInt32(row["Id"]),
                        Convert.ToInt32(row["PeriodeId"]),
                        Convert.ToDecimal(row["ErtraegeAufwendungen"]),
                        Convert.ToDecimal(row["Verlustvortrag"])
                    );
                }
            }

            throw new Exception($"Keine Steuerdaten mit der Id {id} gefunden.");
        }


        public static Zinsüberschuss Zinsüberschuss(int id)
        {
            if (_zinsüberschussTable == null)
            {
                throw new Exception("Die Tabelle 'Zinsüberschuss' wurde nicht geladen. Bitte rufen Sie zuerst 'DatenbankAuslesen' auf.");
            }

            foreach (DataRow row in _zinsüberschussTable.Rows)
            {
                if (id == Convert.ToInt32(row["Id"]))
                {
                    return new Zinsüberschuss(
                        Convert.ToInt32(row["Id"]),
                        Convert.ToInt32(row["PeriodeId"]),
                        Convert.ToDecimal(row["ZinsueberschussBrutto"]),
                        Convert.ToDecimal(row["WertberichtigungenAbschreibungen"]),
                        Convert.ToDecimal(row["Risikovorsorge"])
                    );
                }
            }

            throw new Exception($"Kein Zinsüberschuss mit der Id {id} gefunden.");
        }


        public static Betriebsaufwendungen Betriebsaufwendungen(int id)
        {
            if (_betriebsaufwendungenTable == null)
            {
                throw new Exception("Die Tabelle 'Betriebsaufwendungen' wurde nicht geladen. Bitte rufen Sie zuerst 'DatenbankAuslesen' auf.");
            }

            foreach (DataRow row in _betriebsaufwendungenTable.Rows)
            {
                if (id == Convert.ToInt32(row["Id"]))
                {
                    return new Betriebsaufwendungen(
                        Convert.ToInt32(row["Id"]),
                        Convert.ToInt32(row["PeriodeId"]),
                        Convert.ToDecimal(row["Personal"]),
                        Convert.ToDecimal(row["Werbeausgaben"]),
                        Convert.ToDecimal(row["Verwaltung"])
                    );
                }
            }

            throw new Exception($"Keine Betriebsaufwendungen mit der Id {id} gefunden.");
        }

    }
}
