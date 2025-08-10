// Betriebsaufwendungen_DUMMY.cs (zu Zulassungsaufgabe 24W)

namespace EasyBankingMiniGuV.Datenhaltung.Transfer
{
    public record Betriebsaufwendungen(int Id, int PeriodeId, decimal Personal, decimal Werbeausgaben, decimal Verwaltung);
}