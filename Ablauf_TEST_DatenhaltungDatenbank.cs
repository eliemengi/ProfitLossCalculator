// Program_TEST_Datenhaltung.cs (zu zulassungsaufgabe 24W)
// Test des Namensraums 'Datenhaltung'
// Konsolenanwendung

/*
 * WICHTIG:
 * Die Datenbank 'Bank.accdb' muss im selben Ordner stehen
 * wie die ausführbare Datei, oder die Konstante '_LOCATION'
 * muss angepasst werden.
 * 
 */


using System;
using System.Linq;
using System.Reflection;
using EasyBankingMiniGuV.Datenhaltung.Transfer;
using EasyBankingMiniGuV.Datenhaltung.Datenbank;
using static Testing.TestSupport;


namespace EasyBankingMiniGuV.TestDatenhaltungDatenbank.Ablauf
{

    class Program
    {
        /// <summary>
        /// Verzeichnis, in dem die Datenbank steht
        /// </summary>
        private const string _LOCATION = @".\";

        /// <summary>
        /// Name der Datenbank
        /// </summary>
        private const string _DBNAME = "EasyBankingDatenbank-24W.accdb";

        /// <summary>
        /// Klasse für eine Zeile der Vorgabetabellen
        /// </summary>
        /// <typeparam name="T">Typ der Vorgabe (d.h. jeweilige Transferklasse)</typeparam>
        private class VorgabeZeile<T>
        {
            /// <summary>
            /// Nummer der Periode (in den Austauschobjekten i.d.R. nicht enthalten)
            /// </summary>
            public int PeriodeID { get; }

            /// <summary>
            /// restliche Vorgabezeile (Spalten entsprechen den Eigenschaften der jeweiligen Transferklasse)
            /// </summary>
            public T Zeile { get; }

            /// <summary>
            /// Konstruktor
            /// </summary>
            /// <param name="periodeID">Nummer der Periode (in den Austauschobjekten i.d.R. nicht enthalten)</param>
            /// <param name="zeile">restliche Vorgabezeile (Spalten entsprechen den Eigenschaften der jeweiligen Transferklasse)</param>
            public VorgabeZeile(int periodeID, T zeile)
            {
                PeriodeID = periodeID;
                Zeile = zeile;
            }

            /// <summary>
            /// Vergleich dieser Vorgabezeile mit der korrespondierenden Zeile der Datenbank
            /// </summary>
            /// <param name="vergleichsZeile">korrespondierende Zeile der Datenbank</param>
            public void Vergleichen(T vergleichsZeile)
            {
                foreach (PropertyInfo pi in typeof(T).GetProperties())
                {
                    MethodInfo mi = pi.GetGetMethod();
                    Console.Write(pi.Name + ": ");
                    CompareAndPrint(mi.Invoke(Zeile, null), mi.Invoke(vergleichsZeile, null));
                }
            }
        }

        /// <summary>
        /// Delegatentyp für Methoden, die aus der Datenbank Zeilen abrufen
        /// </summary>
        /// <typeparam name="T">Typ des Austauschobjekts der Zeile</typeparam>
        /// <param name="periodenID">Nummer der Periode der Zeile</param>
        /// <returns>abgerufene Tabellenzeile</returns>
        private delegate T AbgerufeneZeile<T>(int periodenID);

        /// <summary>
        /// Methode zum Abgleich der Vorgaben mit den Werten aus der Datenbank
        /// </summary>
        /// <typeparam name="T">Typ des Austauschobjektes</typeparam>
        /// <param name="vorgaben">Array mit Vorgabezeilen</param>
        /// <param name="zeileAbrufen">zum Typ korrespondierende Methode zum Abruf einer Zeile aus der Datenbank</param>
        private static void TestDurchführen<T>(VorgabeZeile<T>[] vorgaben, AbgerufeneZeile<T> zeileAbrufen)
        {
            int maxID = -1;
            object dummy;

            Console.WriteLine("\n- " + typeof(T).Name + " -\n");
            foreach (VorgabeZeile<T> vorgabeZeile in vorgaben)
            {
                int periodeID = vorgabeZeile.PeriodeID;
                if (periodeID > maxID)
                    maxID = periodeID;
                Console.WriteLine(periodeID.ToString());
                T datenZeile = zeileAbrufen(periodeID);
                vorgabeZeile.Vergleichen(datenZeile);
            }
            ProvokeException(() => dummy = zeileAbrufen(0));
            ProvokeException(() => dummy = zeileAbrufen(maxID + 1));
        }

        /// <summary>
        /// Hauptmethode
        /// </summary>
        static void Main()
        {

            PrintHeading2("Laden der Datenbank");

            // Test der Ausnahmen vor Laden der Datenbank
            object dummy;
            PL();  CompareAndPrint(Datenbank.IstGeladen, false);
            PL();  ProvokeException(() => dummy = Datenbank.PeriodenIDs);
            PL();  ProvokeException(() => dummy = Datenbank.Periode(1));
            PL();  ProvokeException(() => dummy = Datenbank.Betriebsaufwendungen(1));
            PL();  ProvokeException(() => dummy = Datenbank.Steuern(1));
            PL();  ProvokeException(() => dummy = Datenbank.Zinsüberschuss(1));

            // Test der Plausibilitätsprüfungen
            PL();  ProvokeException(() => Datenbank.DatenbankAuslesen(null));
            PL();  ProvokeException(() => Datenbank.DatenbankAuslesen(" "));

            // Test, ob der Pfad auch wirklich genutzt wird
            PL();  ProvokeException(() => Datenbank.DatenbankAuslesen("AA:cf4rfnu"));

            // Datenbank laden
            PL();  Datenbank.DatenbankAuslesen(_LOCATION + _DBNAME);


            PrintHeading2("Testen der Eigenschaften");

            Console.WriteLine("IstGeladen");
            // richtiger Wert (true)
            PL();  CompareAndPrint(Datenbank.IstGeladen, true);
            // kein öffentlicher set-Accessor
            PL();  IsNull(typeof(Datenbank).GetProperty("IstGeladen").GetSetMethod());

            Console.WriteLine("PeriodenIDs");
            // richtiger Wert (true)
            PL();  CompareAndPrint(Enumerable.SequenceEqual(Datenbank.PeriodenIDs, new int[] { 1, 2, 3 }), true);
            // kein öffentlicher set-Accessor
            PL();  IsNull(typeof(Datenbank).GetProperty("PeriodenIDs").GetSetMethod());
            // Array der Perioden muss Kopie sein (Zuweisung darf gekapseltes Array nicht ändern)
            Datenbank.PeriodenIDs[2] = 222;
            PL();  CompareAndPrint(Datenbank.PeriodenIDs[2], 3);


            PrintHeading2("Testen der Methoden");

            // für jede Tabelle in der Datenbank: Vorgabe erstellen und mit geladener Tabelle vergleichen

            VorgabeZeile<Periode>[] vorgabePeriode = new VorgabeZeile<Periode>[]
            {
                new VorgabeZeile<Periode>(1, new Periode(1, "Geschäftsjahr 2021", new DateTime(2021, 1, 1), new DateTime(2021, 12, 31))),
                new VorgabeZeile<Periode>(2, new Periode(2, "Geschäftsjahr 2022", new DateTime(2022, 1, 1), new DateTime(2022, 12, 31))),
                new VorgabeZeile<Periode>(3, new Periode(3, "Geschäftsjahr 2023", new DateTime(2023, 1, 1), new DateTime(2023, 12, 31)))
            };
            PL();  TestDurchführen<Periode>(vorgabePeriode, Datenbank.Periode);

            VorgabeZeile<Betriebsaufwendungen>[] vorgabeBetriebsaufwendungen = new VorgabeZeile<Betriebsaufwendungen>[]
            {
                new VorgabeZeile<Betriebsaufwendungen>(1, new Betriebsaufwendungen(1, 1, 64174M, 40275M, 39643M)),
                new VorgabeZeile<Betriebsaufwendungen>(2, new Betriebsaufwendungen(2, 2, 65750M, 40275M, 40126M)),
                new VorgabeZeile<Betriebsaufwendungen>(3, new Betriebsaufwendungen(3, 3, 66499M, 40275M, 40077M))
            };
            PL();  TestDurchführen<Betriebsaufwendungen>(vorgabeBetriebsaufwendungen, Datenbank.Betriebsaufwendungen);

            VorgabeZeile<Steuern>[] vorgabeSteuern = new VorgabeZeile<Steuern>[]
            {
                new VorgabeZeile<Steuern>(1, new Steuern(1, 1, 0M, 10M)),
                new VorgabeZeile<Steuern>(2, new Steuern(2, 2, 0M,  3M)),
                new VorgabeZeile<Steuern>(3, new Steuern(3, 3, 0M,  2M))
            };
            PL();  TestDurchführen<Steuern>(vorgabeSteuern, Datenbank.Steuern);

            VorgabeZeile<Zinsüberschuss>[] vorgabeZinsen = new VorgabeZeile<Zinsüberschuss>[]
            {
                new VorgabeZeile<Zinsüberschuss>(1, new Zinsüberschuss(1, 1, 201453M, 24187M,  3M)),
                new VorgabeZeile<Zinsüberschuss>(2, new Zinsüberschuss(2, 2, 228822M, 24842M, 21M)),
                new VorgabeZeile<Zinsüberschuss>(3, new Zinsüberschuss(3, 3, 231435M, 24701M,  1M))
            };
            PL();  TestDurchführen<Zinsüberschuss>(vorgabeZinsen, Datenbank.Zinsüberschuss);


            PrintHeading2("ERGEBNIS");

            PrintResult();

            Console.ReadKey();
        }
    }
}
