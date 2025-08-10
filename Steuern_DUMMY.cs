// Steuern_DUMMY.cs (zu Zulassungsaufgabe 24W)

namespace EasyBankingMiniGuV.Datenhaltung.Transfer
{
    public record Steuern(int Id, int PeriodeId, decimal ErträgeAufwendungen, decimal Verlustvortrag);
}