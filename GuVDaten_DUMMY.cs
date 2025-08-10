// GuVDaten_DUMMY.cs (zu Zulassungsaufgabe 24W)

namespace EasyBankingMiniGuV.Datenverarbeitung
{
    public record GuVDaten(decimal ZinsüberschussBrutto,
                           decimal WertberichtigungenAbschreibungen,
                           decimal Risikovorsorge,
                           decimal ZinsüberschussNetto,
                           decimal Personal,
                           decimal Werbeausgaben,
                           decimal Verwaltung,
                           decimal Betriebsaufwendungen,
                           decimal ErtraegeAufwendungen,
                           decimal Verlustvortrag,
                           decimal ErgebnisGewöhnlicheGeschäftstätigkeit,
                           decimal Zwischensumme,
                           decimal Ertragssteuer,
                           decimal PeriodenüberschussFehlbetrag);
}