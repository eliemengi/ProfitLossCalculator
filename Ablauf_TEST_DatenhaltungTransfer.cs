// Program_TEST_DatenhaltungTransfer.cs (zu Zulassungsaufgabe 24W)
// Test des Namensraums 'Datenhaltung.Transfer'
// Konsolenanwendung

using System;
using EasyBankingMiniGuV.Datenhaltung.Transfer;
using static Testing.TestSupport;

namespace EasyBankingMiniGuV.TestDatenhaltungTransfer.Ablauf
{
    class Program
    {
        /// <summary>
        /// Hauptmethode
        /// </summary>
        static void Main()
        {
            #region Klasse Periode

            PrintHeading1("Klasse Periode");

            // vorgegebene Werte
            const int periodeId = 5;
            DateTime periodeBeginn = new DateTime(2023, 1, 1);
            DateTime periodeEnde = new DateTime(2023, 12, 31);
            const string periodeBezeichnung = "Periode 5";

            PrintHeading2("Objekt anlegen");
            Periode periode = new Periode(periodeId, periodeBezeichnung, periodeBeginn, periodeEnde);

            PrintHeading2("Test auf korrekte Werte der Eigenschaften");
            PL(); CompareAndPrint(periode.Id, periodeId);
            PL(); CompareAndPrint(periode.Bezeichnung, periodeBezeichnung);
            PL(); CompareAndPrint(periode.Beginn, periodeBeginn);
            PL(); CompareAndPrint(periode.Ende, periodeEnde);

            PrintHeading2("Test auf Nicht-Änderbarkeit der Eigenschaften");
            PL(); TestForNonwriteableProperties<Periode>();

            PrintHeading2("Test auf überschriebenes 'ToString'");
            PL(); CompareAndPrint(periode.GetType().GetMethod("ToString").DeclaringType, typeof(Periode));

            PrintHeading2("Test der Konstruktor-Plausibilitätsprüfungen");
            PL(); ProvokeException(() => new Periode(0, periodeBezeichnung, periodeBeginn, periodeEnde));
            PL(); DenyException(() => new Periode(1, periodeBezeichnung, periodeBeginn, periodeEnde));
            PL(); ProvokeException(() => new Periode(-10, periodeBezeichnung, periodeBeginn, periodeEnde));
            PL(); ProvokeException(() => new Periode(periodeId, null, periodeBeginn, periodeEnde));
            PL(); ProvokeException(() => new Periode(periodeId, "", periodeBeginn, periodeEnde));
            PL(); ProvokeException(() => new Periode(periodeId, " ", periodeBeginn, periodeEnde));
            PL(); ProvokeException(() => new Periode(periodeId, periodeBezeichnung, periodeEnde, periodeBeginn));

            PrintHeading2("Test der überschriebenen Methoden aus object");
            ObjectTest<Periode>(new Periode[]
            {
                periode,
                new Periode(periodeId, periodeBezeichnung, periodeBeginn, periodeEnde),
                new Periode(1, periodeBezeichnung, periodeBeginn, periodeEnde),
                new Periode(periodeId, "Periode 1", periodeBeginn, periodeEnde),
                new Periode(periodeId, periodeBezeichnung, new DateTime(2022, 1, 1), periodeEnde),
                new Periode(periodeId, periodeBezeichnung, periodeBeginn, new DateTime(2024, 12, 31))
            });

            #endregion

            #region Klasse Betriebsaufwendungen

            PrintHeading1("Klasse Betriebsaufwendungen");

            // vorgegebene Werte
            const int betriebsaufwendungenId = 3;
            const decimal personal = 20000M;
            const decimal werbeausgaben = 30000M;
            const decimal verwaltung = 40000M;

            PrintHeading2("Objekt anlegen");
            Betriebsaufwendungen betriebsaufwendungen = new Betriebsaufwendungen(betriebsaufwendungenId,
                                                                                 periodeId,
                                                                                 personal,
                                                                                 werbeausgaben,
                                                                                 verwaltung);

            PrintHeading2("Test auf korrekte Werte der Eigenschaften");
            PL(); CompareAndPrint(betriebsaufwendungen.Id, betriebsaufwendungenId);
            PL(); CompareAndPrint(betriebsaufwendungen.PeriodeId, periodeId);
            PL(); CompareAndPrintDecimal(betriebsaufwendungen.Personal, personal);
            PL(); CompareAndPrintDecimal(betriebsaufwendungen.Werbeausgaben, werbeausgaben);
            PL(); CompareAndPrintDecimal(betriebsaufwendungen.Verwaltung, verwaltung);

            PrintHeading2("Test auf Nicht-Änderbarkeit der Eigenschaften");
            PL(); TestForNonwriteableProperties<Betriebsaufwendungen>();

            PrintHeading2("Test auf überschriebenes 'ToString'");
            PL(); CompareAndPrint(betriebsaufwendungen.GetType().GetMethod("ToString").DeclaringType, typeof(Betriebsaufwendungen));

            PrintHeading2("Test der Konstruktor-Plausibilitätsprüfungen");
            PL(); ProvokeException(() => new Betriebsaufwendungen(0,
                                                                  periodeId,
                                                                  personal,
                                                                  werbeausgaben,
                                                                  verwaltung));
            PL(); DenyException(() => new Betriebsaufwendungen(1,
                                                               periodeId,
                                                               personal,
                                                               werbeausgaben,
                                                               verwaltung));
            PL(); ProvokeException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                                  0,
                                                                  personal,
                                                                  werbeausgaben,
                                                                  verwaltung));
            PL(); DenyException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                               1,
                                                               personal,
                                                               werbeausgaben,
                                                               verwaltung));
            PL(); ProvokeException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                                  periodeId,
                                                                  -1M,
                                                                  werbeausgaben,
                                                                  verwaltung));
            PL(); DenyException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                               periodeId,
                                                               0M,
                                                               werbeausgaben,
                                                               verwaltung));
            PL(); ProvokeException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                                  periodeId,
                                                                  personal,
                                                                  -1M,
                                                                  verwaltung));
            PL(); DenyException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                               periodeId,
                                                               personal,
                                                               0M,
                                                               verwaltung));
            PL(); ProvokeException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                                  periodeId,
                                                                  personal,
                                                                  werbeausgaben,
                                                                  -1M));
            PL(); DenyException(() => new Betriebsaufwendungen(betriebsaufwendungenId,
                                                               periodeId,
                                                               personal,
                                                               werbeausgaben,
                                                               0M));

            PrintHeading2("Test der überschriebenen Methoden aus object");
            ObjectTest<Betriebsaufwendungen>(new Betriebsaufwendungen[]
            {
                betriebsaufwendungen,
                new Betriebsaufwendungen(betriebsaufwendungenId, periodeId, personal, werbeausgaben, verwaltung),
                new Betriebsaufwendungen(1, periodeId, personal, werbeausgaben, verwaltung),
                new Betriebsaufwendungen(betriebsaufwendungenId, 1, personal, werbeausgaben, verwaltung),
                new Betriebsaufwendungen(betriebsaufwendungenId, periodeId, 0M, werbeausgaben, verwaltung),
                new Betriebsaufwendungen(betriebsaufwendungenId, periodeId, personal, 0M, verwaltung),
                new Betriebsaufwendungen(betriebsaufwendungenId, periodeId, personal, werbeausgaben, 0M)
            });

            #endregion

            #region Klasse Steuern

            PrintHeading1("Klasse Steuern");

            // vorgegebene Werte
            const int steuernId = 3;
            const decimal erträgeAufwendungen = 50000M;
            const decimal verlustvortrag = 60000M;

            PrintHeading2("Objekt anlegen");
            Steuern steuern = new Steuern(steuernId, periodeId, erträgeAufwendungen, verlustvortrag);

            PrintHeading2("Test auf korrekte Werte der Eigenschaften");
            PL(); CompareAndPrint(steuern.Id, steuernId);
            PL(); CompareAndPrint(steuern.PeriodeId, periodeId);
            PL(); CompareAndPrintDecimal(steuern.ErträgeAufwendungen, erträgeAufwendungen);
            PL(); CompareAndPrintDecimal(steuern.Verlustvortrag, verlustvortrag);

            PrintHeading2("Test auf Nicht-Änderbarkeit der Eigenschaften");
            PL(); TestForNonwriteableProperties<Steuern>();

            PrintHeading2("Test auf überschriebenes 'ToString'");
            PL(); CompareAndPrint(steuern.GetType().GetMethod("ToString").DeclaringType, typeof(Steuern));

            PrintHeading2("Test der Konstruktor-Plausibilitätsprüfungen");
            PL(); ProvokeException(() => new Steuern(0, periodeId, erträgeAufwendungen, verlustvortrag));
            PL(); DenyException(() => new Steuern(1, periodeId, erträgeAufwendungen, verlustvortrag));
            PL(); ProvokeException(() => new Steuern(steuernId, 0, erträgeAufwendungen, verlustvortrag));
            PL(); DenyException(() => new Steuern(steuernId, 1, erträgeAufwendungen, verlustvortrag));
            PL(); ProvokeException(() => new Steuern(steuernId, periodeId, erträgeAufwendungen, -1M));
            PL(); DenyException(() => new Steuern(steuernId, periodeId, erträgeAufwendungen, 0M));

            PrintHeading2("Test der überschriebenen Methoden aus object");
            ObjectTest<Steuern>(new Steuern[]
            {
                steuern,
                new Steuern(steuernId, periodeId, erträgeAufwendungen, verlustvortrag),
                new Steuern(1, periodeId, erträgeAufwendungen, verlustvortrag),
                new Steuern(steuernId, 1, erträgeAufwendungen, verlustvortrag),
                new Steuern(steuernId, periodeId, 0M, verlustvortrag),
                new Steuern(steuernId, periodeId, erträgeAufwendungen, 0M)
            });

            #endregion

            #region Klasse Zinsüberschuss

            PrintHeading1("Klasse Zinsüberschuss");

            // vorgegebene Werte
            const int zinsüberschussId = 3;
            const decimal zinsüberschussBrutto = 70000M;
            const decimal wertberichtigungenAbschreibungen = 80000M;
            const decimal risikovorsorge = 90000M;

            PrintHeading2("Objekt anlegen");
            Zinsüberschuss zinsüberschuss = new Zinsüberschuss(zinsüberschussId,
                                                               periodeId,
                                                               zinsüberschussBrutto,
                                                               wertberichtigungenAbschreibungen,
                                                               risikovorsorge);

            PrintHeading2("Test auf korrekte Werte der Eigenschaften");
            PL(); CompareAndPrint(zinsüberschuss.Id, zinsüberschussId);
            PL(); CompareAndPrint(zinsüberschuss.PeriodeId, periodeId);
            PL(); CompareAndPrintDecimal(zinsüberschuss.ZinsüberschussBrutto, zinsüberschussBrutto);
            PL(); CompareAndPrintDecimal(zinsüberschuss.WertberichtigungenAbschreibungen, wertberichtigungenAbschreibungen);
            PL(); CompareAndPrintDecimal(zinsüberschuss.Risikovorsorge, risikovorsorge);

            PrintHeading2("Test auf Nicht-Änderbarkeit der Eigenschaften");
            PL(); TestForNonwriteableProperties<Zinsüberschuss>();

            PrintHeading2("Test auf überschriebenes 'ToString'");
            PL(); CompareAndPrint(zinsüberschuss.GetType().GetMethod("ToString").DeclaringType, typeof(Zinsüberschuss));

            PrintHeading2("Test der Konstruktor-Plausibilitätsprüfungen");
            PL(); ProvokeException(() => new Zinsüberschuss(0,
                                                            periodeId,
                                                            zinsüberschussBrutto,
                                                            wertberichtigungenAbschreibungen,
                                                            risikovorsorge));
            PL(); DenyException(() => new Zinsüberschuss(1,
                                                         periodeId,
                                                         zinsüberschussBrutto,
                                                         wertberichtigungenAbschreibungen,
                                                         risikovorsorge));
            PL(); ProvokeException(() => new Zinsüberschuss(zinsüberschussId,
                                                            0,
                                                            zinsüberschussBrutto,
                                                            wertberichtigungenAbschreibungen,
                                                            risikovorsorge));
            PL(); DenyException(() => new Zinsüberschuss(zinsüberschussId,
                                                         1,
                                                         zinsüberschussBrutto,
                                                         wertberichtigungenAbschreibungen,
                                                         risikovorsorge));
            PL(); ProvokeException(() => new Zinsüberschuss(zinsüberschussId,
                                                            periodeId,
                                                            -1M,
                                                            wertberichtigungenAbschreibungen,
                                                            risikovorsorge));
            PL(); DenyException(() => new Zinsüberschuss(zinsüberschussId,
                                                         periodeId,
                                                         0M,
                                                         wertberichtigungenAbschreibungen,
                                                         risikovorsorge));
            PL(); ProvokeException(() => new Zinsüberschuss(zinsüberschussId,
                                                            periodeId,
                                                            zinsüberschussBrutto,
                                                            -1M,
                                                            risikovorsorge));
            PL(); DenyException(() => new Zinsüberschuss(zinsüberschussId,
                                                         periodeId,
                                                         zinsüberschussBrutto,
                                                         0M,
                                                         risikovorsorge));

            PrintHeading2("Test der überschriebenen Methoden aus object");
            ObjectTest<Zinsüberschuss>(new Zinsüberschuss[]
            {
                zinsüberschuss,
                new Zinsüberschuss(zinsüberschussId,
                                   periodeId,
                                   zinsüberschussBrutto,
                                   wertberichtigungenAbschreibungen,
                                   risikovorsorge),
                new Zinsüberschuss(1,
                                   periodeId,
                                   zinsüberschussBrutto,
                                   wertberichtigungenAbschreibungen,
                                   risikovorsorge),
                new Zinsüberschuss(zinsüberschussId,
                                   1,
                                   zinsüberschussBrutto,
                                   wertberichtigungenAbschreibungen,
                                   risikovorsorge),
                new Zinsüberschuss(zinsüberschussId,
                                   periodeId,
                                   0M,
                                   wertberichtigungenAbschreibungen,
                                   risikovorsorge),
                new Zinsüberschuss(zinsüberschussId,
                                   periodeId,
                                   zinsüberschussBrutto,
                                   0M,
                                   risikovorsorge),
                new Zinsüberschuss(zinsüberschussId,
                                   periodeId,
                                   zinsüberschussBrutto,
                                   wertberichtigungenAbschreibungen,
                                   0M)
            });

            #endregion

            PrintHeading1("ERGEBNIS");

            PrintResult();

            Console.ReadKey();
        }
    }
}
