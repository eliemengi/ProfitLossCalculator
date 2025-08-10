// Zinsüberschuss_DUMMY.cs (zu Zulassungsaufgabe 24W)

namespace EasyBankingMiniGuV.Datenhaltung.Transfer
{
    public record Zinsüberschuss(int Id, int PeriodeId, decimal ZinsüberschussBrutto, decimal WertberichtigungenAbschreibungen, decimal Risikovorsorge);
}