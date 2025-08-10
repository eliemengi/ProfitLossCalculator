// GuVRechnung_DUMMY.cs (zu Zulassungsaufgabe 24W)

using System;
using EasyBankingMiniGuV.Datenhaltung.Datenbank;

namespace EasyBankingMiniGuV.Datenverarbeitung
{
    public static class GuVRechnung
    {
        public static void Rechnen(int aktuellePeriodeId,
                                   out GuVDaten aktuellePeriodeDaten,
                                   out GuVDaten vorperiodeDaten,
                                   out GuVDaten vergleich)
        {
            if (!Datenbank.IstGeladen)
                throw new Exception("Datenbank ist nicht geladen!");
            if (aktuellePeriodeId < 2 || aktuellePeriodeId > Datenbank.PeriodenIDs.Length)
                throw new Exception("falsche ID für Periode!");

            aktuellePeriodeDaten = new GuVDaten(1000M + aktuellePeriodeId,
                                                1010M + aktuellePeriodeId,
                                                1020M + aktuellePeriodeId,
                                                1030M + aktuellePeriodeId,
                                                1040M + aktuellePeriodeId,
                                                1050M + aktuellePeriodeId,
                                                1060M + aktuellePeriodeId,
                                                1070M + aktuellePeriodeId,
                                                1080M + aktuellePeriodeId,
                                                1090M + aktuellePeriodeId,
                                                1100M + aktuellePeriodeId,
                                                1110M + aktuellePeriodeId,
                                                1120M + aktuellePeriodeId,
                                                1130M + aktuellePeriodeId);

            vorperiodeDaten = new GuVDaten(2000M + aktuellePeriodeId - 1,
                                           2010M + aktuellePeriodeId - 1,
                                           2020M + aktuellePeriodeId - 1,
                                           2030M + aktuellePeriodeId - 1,
                                           2040M + aktuellePeriodeId - 1,
                                           2050M + aktuellePeriodeId - 1,
                                           2060M + aktuellePeriodeId - 1,
                                           2070M + aktuellePeriodeId - 1,
                                           2080M + aktuellePeriodeId - 1,
                                           2090M + aktuellePeriodeId - 1,
                                           2100M + aktuellePeriodeId - 1,
                                           2110M + aktuellePeriodeId - 1,
                                           2120M + aktuellePeriodeId - 1,
                                           2130M + aktuellePeriodeId - 1);

            if (aktuellePeriodeId % 2 == 1)
                vergleich = new GuVDaten(decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue,
                                         decimal.MaxValue);
            else
            {
                decimal factor = aktuellePeriodeId;
                vergleich = new GuVDaten(.01M * factor,
                                         .02M * factor,
                                         .03M * factor,
                                         .04M * factor,
                                         .05M * factor,
                                         .06M * factor,
                                         .07M * factor,
                                         .08M * factor,
                                         .09M * factor,
                                         .10M * factor,
                                         .11M * factor,
                                         .12M * factor,
                                         .13M * factor,
                                         .14M * factor);
            }
        }
    }
}
