// Periode_DUMMY.cs (zu Zulassungsaufgabe 24W)
//
// Hinweis: Aus einem 'record' wird automatisch eine unveränderliche Klasse erzeugt,
//          mit den Eigenschaften aus der Parameterliste.
//          hier: Die Klasse 'Periode' hat die vier Eigenschaften
//                - Id
//                - Bezeichnung
//                - Beginn
//                - Ende
//          mit den jeweils angegebenen Datentypen.

using System;

namespace EasyBankingMiniGuV.Datenhaltung.Transfer
{
    public record Periode(int Id, string Bezeichnung, DateTime Beginn, DateTime Ende);
}
