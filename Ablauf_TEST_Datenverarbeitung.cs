// Program_TEST_Datenverarbeitung.cs (zu Zulassungsaufgabe 24W)
// Test des Namensraums 'Datenverarbeitung'
// Konsolenanwendung

// WICHTIG: 
// 1. Klasse GuV erstellen
// 2. Klasse GuV testen
// 3. Klasse GuVDaten erstellen
// 4. Kommentarzeichen Zeile 16 entfernen
// 5. Klassen GuV und GuVDaten testen
// 6. Klasse GuVRechnung erstellen
// 7. Kommentarzeichen Zeile 17 entfernen
// 8. Klassen GuV, GuVRechnung, GuVVergleich testen

#define TEST_GUV
//#define TEST_GUVDATEN
//#define TEST_GUVRECHNUNG

using System;
using EasyBankingMiniGuV.Datenhaltung.Transfer;
using EasyBankingMiniGuV.Datenhaltung.Datenbank;
using EasyBankingMiniGuV.Datenverarbeitung;
using static Testing.TestSupport;

namespace EasyBankingMiniGuV.TestDatenverarbeitung.Ablauf
{
    class Program
    {

#if TEST_GUVRECHNUNG

        static private decimal Vergleich(decimal wertAP, decimal wertVP) => (wertAP - wertVP) * 100M / wertVP;

        static private void GuVRechnungVergleich(int id, GuVDaten sollAP, GuVDaten sollVP, GuVDaten sollVergleich)
        {
            Console.WriteLine($"\nPeriode {id}\n");

            GuVDaten istAP;
            GuVDaten istVP;
            GuVDaten istVergleich;

            GuVRechnung.Rechnen(id, out istAP, out istVP, out istVergleich);

            Console.WriteLine("aktuelle Periode:");
            TestEqualProperties(sollAP, istAP);

            Console.WriteLine("\nVorperiode:");
            TestEqualProperties(sollVP, istVP);

            Console.WriteLine("\nVergleich:");
            TestEqualProperties(sollVergleich, istVergleich);

            Console.WriteLine();
        }

#endif

        /// <summary>
        /// Hauptmethode
        /// </summary>
        static void Main()
        {
            #region Test für Klasse GuV

            PrintHeading1("Test Klasse GuV");

#if TEST_GUV

            const decimal SMALL = -1E-20M;
            decimal zinsüberschussNetto;
            decimal betriebsaufwendungen;
            decimal ergebnisGewöhnlicheGeschäftstätigkeit;
            decimal zwischensumme;
            decimal ertragssteuer;
            decimal periodenüberschussFehlbetrag;

            PrintHeading2("Test Plausibilitätsprüfungen");

            PL(); ProvokeException(() => GuV.BerechneGuV(SMALL, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, out _, out _, out _, out _, out _, out _));
            PL(); ProvokeException(() => GuV.BerechneGuV(0.0M, SMALL, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, out _, out _, out _, out _, out _, out _));
            PL(); ProvokeException(() => GuV.BerechneGuV(0.0M, 0.0M, 0.0M, SMALL, 0.0M, 0.0M, 0.0M, 0.0M, out _, out _, out _, out _, out _, out _));
            PL(); ProvokeException(() => GuV.BerechneGuV(0.0M, 0.0M, 0.0M, 0.0M, SMALL, 0.0M, 0.0M, 0.0M, out _, out _, out _, out _, out _, out _));
            PL(); ProvokeException(() => GuV.BerechneGuV(0.0M, 0.0M, 0.0M, 0.0M, 0.0M, SMALL, 0.0M, 0.0M, out _, out _, out _, out _, out _, out _));
            PL(); ProvokeException(() => GuV.BerechneGuV(0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, SMALL, out _, out _, out _, out _, out _, out _));

            PrintHeading2("Test der korrekten Berechung 1");

            GuV.BerechneGuV(5300M,
                            200M,
                            100M,
                            300M,
                            600M,
                            100M,
                            1000M,
                            500M,
                            out zinsüberschussNetto,
                            out betriebsaufwendungen,
                            out ergebnisGewöhnlicheGeschäftstätigkeit,
                            out zwischensumme,
                            out ertragssteuer,
                            out periodenüberschussFehlbetrag);

            PL(); CompareAndPrintDecimal(zinsüberschussNetto, 5000M);
            PL(); CompareAndPrintDecimal(betriebsaufwendungen, 1000M);
            PL(); CompareAndPrintDecimal(ergebnisGewöhnlicheGeschäftstätigkeit, 4000M);
            PL(); CompareAndPrintDecimal(zwischensumme, 4500M);
            PL(); CompareAndPrintDecimal(ertragssteuer, 1350.0M);
            PL(); CompareAndPrintDecimal(periodenüberschussFehlbetrag, 3650.0M);

            PrintHeading2("Test der korrekten Berechung 2");

            GuV.BerechneGuV(2100M,
                            300M,
                            -200M,
                            600M,
                            200M,
                            200M,
                            -1100M,
                            100M,
                            out zinsüberschussNetto,
                            out betriebsaufwendungen,
                            out ergebnisGewöhnlicheGeschäftstätigkeit,
                            out zwischensumme,
                            out ertragssteuer,
                            out periodenüberschussFehlbetrag);

            PL(); CompareAndPrintDecimal(zinsüberschussNetto, 2000M);
            PL(); CompareAndPrintDecimal(betriebsaufwendungen, 1000M);
            PL(); CompareAndPrintDecimal(ergebnisGewöhnlicheGeschäftstätigkeit, 1000M);
            PL(); CompareAndPrintDecimal(zwischensumme, -200M);
            PL(); CompareAndPrintDecimal(ertragssteuer, 0.0M);
            PL(); CompareAndPrintDecimal(periodenüberschussFehlbetrag, -100.0M);

#else

            ErrorMessage("Keine Prüfung der Klasse 'GuV'!");

#endif
            #endregion

            #region Test für Klasse GuVDaten

            PrintHeading1("Test Klasse GuVDaten");

#if TEST_GUVDATEN

            GuVDaten daten = new GuVDaten
            {
                ZinsüberschussBrutto = 1010M,
                WertberichtigungenAbschreibungen = 1020,
                Risikovorsorge = 1030M,
                ZinsüberschussNetto = 1040M,
                Personal = 1050M,
                Werbeausgaben = 1060M,
                Verwaltung = 1070M,
                Betriebsaufwendungen = 1080M,
                ErtraegeAufwendungen = 1090M,
                Verlustvortrag = 1100M,
                ErgebnisGewöhnlicheGeschäftstätigkeit = 1110M,
                Zwischensumme = 1120M,
                Ertragssteuer = 1130M,
                PeriodenüberschussFehlbetrag = 1140M
            };

            PL(); CompareAndPrintDecimal(daten.ZinsüberschussBrutto, 1010M);
            PL(); CompareAndPrintDecimal(daten.WertberichtigungenAbschreibungen, 1020M);
            PL(); CompareAndPrintDecimal(daten.Risikovorsorge, 1030M);
            PL(); CompareAndPrintDecimal(daten.ZinsüberschussNetto, 1040M);
            PL(); CompareAndPrintDecimal(daten.Personal, 1050M);
            PL(); CompareAndPrintDecimal(daten.Werbeausgaben, 1060M);
            PL(); CompareAndPrintDecimal(daten.Verwaltung, 1070M);
            PL(); CompareAndPrintDecimal(daten.Betriebsaufwendungen, 1080M);
            PL(); CompareAndPrintDecimal(daten.ErtraegeAufwendungen, 1090M);
            PL(); CompareAndPrintDecimal(daten.Verlustvortrag, 1100M);
            PL(); CompareAndPrintDecimal(daten.ErgebnisGewöhnlicheGeschäftstätigkeit, 1110M);
            PL(); CompareAndPrintDecimal(daten.Zwischensumme, 1120M);
            PL(); CompareAndPrintDecimal(daten.Ertragssteuer, 1130M);
            PL(); CompareAndPrintDecimal(daten.PeriodenüberschussFehlbetrag, 1140M);

#else

            ErrorMessage("Keine Prüfung der Klasse 'GuVDaten'!");

#endif

            #endregion

            #region Test für Klasse GuVRechnung

            PrintHeading1("Test Klasse GuVRechnung");

#if TEST_GUVRECHNUNG

            PrintHeading2("Test der Plausibilitätsprüfungen");

            Datenbank.SetLength(3);
            Datenbank.IstGeladen = false;
            PL(); ProvokeException(() => GuVRechnung.Rechnen(2, out _, out _, out _));
            Datenbank.IstGeladen = true;
            PL(); DenyException(() => GuVRechnung.Rechnen(2, out _, out _, out _));
            PL(); ProvokeException(() => GuVRechnung.Rechnen(-1, out _, out _, out _));
            PL(); ProvokeException(() => GuVRechnung.Rechnen(0, out _, out _, out _));
            PL(); ProvokeException(() => GuVRechnung.Rechnen(1, out _, out _, out _));
            PL(); ProvokeException(() => GuVRechnung.Rechnen(4, out _, out _, out _));

            PrintHeading2("Test der Eigenschaften und rechnerischen Richtigkeit");

            PL(); GuVRechnungVergleich(2,
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = 102M,
                                           WertberichtigungenAbschreibungen = 22M,
                                           Risikovorsorge = 32M,
                                           ZinsüberschussNetto = 48M,
                                           Personal = 42M,
                                           Werbeausgaben = 52M,
                                           Verwaltung = 62M,
                                           Betriebsaufwendungen = 156M,
                                           ErtraegeAufwendungen = 72M,
                                           Verlustvortrag = 82M,
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = -108M,
                                           Zwischensumme = -118M,
                                           Ertragssteuer = 0M,
                                           PeriodenüberschussFehlbetrag = -36M
                                       },
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = 101M,
                                           WertberichtigungenAbschreibungen = 21M,
                                           Risikovorsorge = 31M,
                                           ZinsüberschussNetto = 49M,
                                           Personal = 41M,
                                           Werbeausgaben = 51M,
                                           Verwaltung = 61M,
                                           Betriebsaufwendungen = 153M,
                                           ErtraegeAufwendungen = 71M,
                                           Verlustvortrag = 81M,
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = -104M,
                                           Zwischensumme = -114M,
                                           Ertragssteuer = 0M,
                                           PeriodenüberschussFehlbetrag = -33M
                                       },
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = Vergleich(102M, 101M),
                                           WertberichtigungenAbschreibungen = Vergleich(22M, 21M),
                                           Risikovorsorge = Vergleich(32M, 31M),
                                           ZinsüberschussNetto = Vergleich(48M, 49M),
                                           Personal = Vergleich(42M, 41M),
                                           Werbeausgaben = Vergleich(52M, 51M),
                                           Verwaltung = Vergleich(62M, 61M),
                                           Betriebsaufwendungen = Vergleich(156M, 153M),
                                           ErtraegeAufwendungen = Vergleich(72M, 71M),
                                           Verlustvortrag = Vergleich(82M, 81M),
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = Vergleich(-108M, -104M),
                                           Zwischensumme = Vergleich(-118M, -114M),
                                           Ertragssteuer = decimal.MaxValue,
                                           PeriodenüberschussFehlbetrag = Vergleich(-36M, -33M)
                                       });

            PL(); GuVRechnungVergleich(3,
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = 103M,
                                           WertberichtigungenAbschreibungen = 23M,
                                           Risikovorsorge = 33M,
                                           ZinsüberschussNetto = 47M,
                                           Personal = 43M,
                                           Werbeausgaben = 53M,
                                           Verwaltung = 63M,
                                           Betriebsaufwendungen = 159M,
                                           ErtraegeAufwendungen = 73M,
                                           Verlustvortrag = 83M,
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = -112M,
                                           Zwischensumme = -122M,
                                           Ertragssteuer = 0M,
                                           PeriodenüberschussFehlbetrag = -39M
                                       },
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = 102M,
                                           WertberichtigungenAbschreibungen = 22M,
                                           Risikovorsorge = 32M,
                                           ZinsüberschussNetto = 48M,
                                           Personal = 42M,
                                           Werbeausgaben = 52M,
                                           Verwaltung = 62M,
                                           Betriebsaufwendungen = 156M,
                                           ErtraegeAufwendungen = 72M,
                                           Verlustvortrag = 82M,
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = -108M,
                                           Zwischensumme = -118M,
                                           Ertragssteuer = 0M,
                                           PeriodenüberschussFehlbetrag = -36M
                                       },
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = Vergleich(103M, 102M),
                                           WertberichtigungenAbschreibungen = Vergleich(23M, 22M),
                                           Risikovorsorge = Vergleich(33M, 32M),
                                           ZinsüberschussNetto = Vergleich(47M, 48M),
                                           Personal = Vergleich(43M, 42M),
                                           Werbeausgaben = Vergleich(53M, 52M),
                                           Verwaltung = Vergleich(63M, 62M),
                                           Betriebsaufwendungen = Vergleich(159M, 156M),
                                           ErtraegeAufwendungen = Vergleich(73M, 72M),
                                           Verlustvortrag = Vergleich(83M, 82M),
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = Vergleich(-112M, -108M),
                                           Zwischensumme = Vergleich(-122M, -118M),
                                           Ertragssteuer = decimal.MaxValue,
                                           PeriodenüberschussFehlbetrag = Vergleich(-39M, -36M)
                                       });

            PL(); GuVRechnungVergleich(666,
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = 101M,
                                           WertberichtigungenAbschreibungen = 21M,
                                           Risikovorsorge = 31M,
                                           ZinsüberschussNetto = 49M,
                                           Personal = 41M,
                                           Werbeausgaben = 51M,
                                           Verwaltung = 61M,
                                           Betriebsaufwendungen = 153M,
                                           ErtraegeAufwendungen = 71M,
                                           Verlustvortrag = 81M,
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = -104M,
                                           Zwischensumme = -114M,
                                           Ertragssteuer = 0M,
                                           PeriodenüberschussFehlbetrag = -33M
                                       },
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = 0M,
                                           WertberichtigungenAbschreibungen = 0M,
                                           Risikovorsorge = 0M,
                                           ZinsüberschussNetto = 0M,
                                           Personal = 0M,
                                           Werbeausgaben = 0M,
                                           Verwaltung = 0M,
                                           Betriebsaufwendungen = 0M,
                                           ErtraegeAufwendungen = 0M,
                                           Verlustvortrag = 0M,
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = 0M,
                                           Zwischensumme = 0M,
                                           Ertragssteuer = 0M,
                                           PeriodenüberschussFehlbetrag = 0M
                                       },
                                       new GuVDaten
                                       {
                                           ZinsüberschussBrutto = decimal.MaxValue,
                                           WertberichtigungenAbschreibungen = decimal.MaxValue,
                                           Risikovorsorge = decimal.MaxValue,
                                           ZinsüberschussNetto = decimal.MaxValue,
                                           Personal = decimal.MaxValue,
                                           Werbeausgaben = decimal.MaxValue,
                                           Verwaltung = decimal.MaxValue,
                                           Betriebsaufwendungen = decimal.MaxValue,
                                           ErtraegeAufwendungen = decimal.MaxValue,
                                           Verlustvortrag = decimal.MaxValue,
                                           ErgebnisGewöhnlicheGeschäftstätigkeit = decimal.MaxValue,
                                           Zwischensumme = decimal.MaxValue,
                                           Ertragssteuer = decimal.MaxValue,
                                           PeriodenüberschussFehlbetrag = decimal.MaxValue
                                       });

#else

            ErrorMessage("Keine Prüfung der Klasse 'GuVRechnung'!");

#endif
            #endregion


            Console.WriteLine("\n\n--- ERGEBNIS ---\n");

            PrintResult();

            Console.ReadKey();
        }
    }
}
