// Klaudio Harhulla
// Matr.Nr.: 92102

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datenpräsentation
{
    /// <summary>
    /// Repräsentiert eine Zeile in der GuV-Tabelle.
    /// </summary>
    public class GuVZeilen
    {
        /// <summary>
        /// Beschreibung der GuV-Position.
        /// </summary>
        public string Beschreibung { get; set; }

        /// <summary>
        /// Wert der vorherigen Periode (VP).
        /// </summary>
        public string VP { get; set; }

        /// <summary>
        /// Wert der aktuellen Periode (AP).
        /// </summary>
        public string AP { get; set; }

        /// <summary>
        /// Prozentuale Veränderung im Vergleich zur VP.
        /// </summary>
        public string Prozent { get; set; }

        /// <summary>
        /// Index der Zeile in der Tabelle.
        /// </summary>
        public int ZeilenIndex { get; set; }

        /// <summary>
        /// Konstruktor für eine Zeile in der GuV-Tabelle.
        /// </summary>
        /// <param name="beschreibung">Beschreibung der Zeile.</param>
        /// <param name="vp">Wert der vorherigen Periode.</param>
        /// <param name="ap">Wert der aktuellen Periode.</param>
        /// <param name="index">Index der Zeile.</param>
        public GuVZeilen(string beschreibung, decimal vp, decimal ap, int index)
        {
            Beschreibung = beschreibung;
            VP = vp.ToString("N0");
            AP = ap.ToString("N0");
            Prozent = vp != 0 ? $"{((ap - vp) / vp * 100):F2} %" : "N/A";
            ZeilenIndex = index;
        }
    }
}
