// Datenbank_DUMMY.cs (zu Zulassungsaufgabe 24W)
// Dummy-Klasse für den Datenbankzugriff
//
// HINWEIS:
// Die Klassen hier stellen in keinster Weise eine geforderte Lösung für den Namensraum "Datenhaltung" dar!

using System;
using System.IO;
using System.Linq;
using EasyBankingMiniGuV.Datenhaltung.Transfer;

namespace EasyBankingMiniGuV.Datenhaltung.Datenbank
{


    public static class Datenbank
    {
        private static int _length = DateTime.Now.Millisecond % 5 + 1;

        private static bool Plausiprüf(ref int periodeID)
        {
            if (!IstGeladen)
                return true;

            if (periodeID == 665)
            {
                periodeID = 0;
                return false;
            }
            else if (periodeID == 666)
            {
                periodeID = 1;
                return false;
            }
            else
                return periodeID < 1 || periodeID > _length;
        }

        public static void SetLength(int length)
        {
            Console.WriteLine("\nPRESS ANY KEY\n");
            // führt in einer WPF-Anwendung zu einer Ausnahme
            Console.ReadKey();

            if (length < 1)
                throw new Exception();
            _length = length;
        }

        public static bool IstGeladen { get; set; } = false;

        public static int[] PeriodenIDs => !IstGeladen ? throw new Exception() : Enumerable.Range(1, _length).ToArray();

        public static void DatenbankAuslesen(string pfadZurDatenbank)
        {
            if (!File.Exists(pfadZurDatenbank))
                throw new Exception();

            IstGeladen = true;
        }

        public static Periode Periode(int periodeID) => Plausiprüf(ref periodeID) ? throw new Exception() : new(periodeID,
                                                                                                                $"Periode {periodeID}",
                                                                                                                new DateTime(2021 + periodeID, periodeID + 1, periodeID + 1),
                                                                                                                new DateTime(2022 + periodeID, periodeID + 2, periodeID + 2));

        public static Zinsüberschuss Zinsüberschuss(int periodeID) => Plausiprüf(ref periodeID) ? throw new Exception() : new(periodeID,
                                                                                                                              periodeID,
                                                                                                                              periodeID == 0 ? 0M : 100M + periodeID,
                                                                                                                              periodeID == 0 ? 0M : 20M + periodeID,
                                                                                                                              periodeID == 0 ? 0M : 30M + periodeID);

        public static Betriebsaufwendungen Betriebsaufwendungen(int periodeID) => Plausiprüf(ref periodeID) ? throw new Exception() : new(periodeID,
                                                                                                                                          periodeID,
                                                                                                                                          periodeID == 0 ? 0M : 40M + periodeID,
                                                                                                                                          periodeID == 0 ? 0M : 50M + periodeID,
                                                                                                                                          periodeID == 0 ? 0M : 60M + periodeID);

        public static Steuern Steuern(int periodeID) => Plausiprüf(ref periodeID) ? throw new Exception() : new(periodeID,
                                                                                                                periodeID,
                                                                                                                periodeID == 0 ? 0M : 70M + periodeID,
                                                                                                                periodeID == 0 ? 0M : 80M + periodeID);
    }
}