// Klaudio Harhulla
// Matr.Nr.: 92102

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EasyBankingMiniGuV.Datenhaltung.Datenbank;
using EasyBankingMiniGuV.Datenverarbeitung;

namespace Datenpräsentation
{
    /// <summary>
    /// Hauptfenster der Anwendung zur Anzeige der Gewinn- und Verlustrechnung.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Liste der IDs der verfügbaren Perioden.
        /// Diese IDs werden aus der Datenbank geladen und dienen zur Identifikation der Perioden.
        /// </summary>
        private List<int> periodenIds;

        /// <summary>
        /// Konstruktor der MainWindow-Klasse.
        /// Initialisiert die Benutzeroberfläche und lädt die Perioden für die Auswahlbox.
        /// </summary>
        /// <exception cref="Exception">
        /// Kann ausgelöst werden, falls die Initialisierung der Oberfläche fehlschlägt.
        /// </exception>
        public MainWindow()
        {
            InitializeComponent();
            LadePerioden();
        }

        /// <summary>
        /// Lädt die verfügbaren Perioden aus der Datenbank und befüllt die Auswahlbox.
        /// Diese Methode ruft die Perioden-IDs aus der Datenbank ab, formatiert sie
        /// als Text und weist sie der ComboBox zur Auswahl zu.
        /// </summary>
        /// <exception cref="Exception">
        /// Wird ausgelöst, wenn ein Fehler beim Zugriff auf die Datenbank oder beim Laden der Perioden auftritt.
        /// </exception>
        private void LadePerioden()
        {
            try
            {
                if (!Datenbank.IstGeladen)
                    Datenbank.IstGeladen = true;

                periodenIds = Datenbank.PeriodenIDs.ToList();
                var perioden = periodenIds
                    .Select(id => Datenbank.Periode(id)) 
                    .Select(p => $"{p.Id}: {p.Bezeichnung} ({p.Beginn:dd.MM.yyyy} - {p.Ende:dd.MM.yyyy})") 
                    .ToList();

                PeriodenAuswahl.ItemsSource = perioden;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Perioden: {ex.Message}");
            }
        }

        /// <summary>
        /// Event-Handler für die Auswahl einer Periode in der ComboBox.
        /// Diese Methode wird ausgelöst, wenn der Benutzer eine Periode auswählt.
        /// Anschließend wird die Gewinn- und Verlustrechnung für die ausgewählte Periode berechnet.
        /// </summary>
        /// <param name="sender">Das Objekt, das das Event auslöst (in diesem Fall die ComboBox).</param>
        /// <param name="e">Enthält Informationen zum Event, z. B. die ausgewählte Periode.</param>
        /// <exception cref="FormatException">
        /// Wird ausgelöst, wenn die ausgewählte Periode-ID nicht im richtigen Format vorliegt.
        /// </exception>
        private void PeriodenAuswahl_Leiste(object sender, SelectionChangedEventArgs e)
        {
            if (PeriodenAuswahl.SelectedItem != null)
            {
                try
                {
                    int periodeId = int.Parse(PeriodenAuswahl.SelectedItem.ToString().Split(':')[0]);

                    BerechneGuV(periodeId);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show($"Fehler beim Verarbeiten der ausgewählten Periode: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Berechnet die Gewinn- und Verlustrechnung für eine ausgewählte Periode.
        /// Diese Methode ruft die Berechnungslogik der GuV-Rechnung auf und erstellt
        /// anschließend eine Liste von Zeilenobjekten zur Darstellung im DataGrid.
        /// </summary>
        /// <param name="periodeId">Die ID der ausgewählten Periode.</param>
        /// <exception cref="Exception">
        /// Wird ausgelöst, wenn ein Fehler bei der Berechnung der GuV-Daten auftritt.
        /// </exception>
        private void BerechneGuV(int periodeId)
        {
            try
            {
                GuVRechnung.Rechnen(periodeId, out var aktuelleDaten, out var vorperiodeDaten, out _);

                var guvDaten = new List<GuVZeilen>
                {
                    new GuVZeilen("Zinsüberschuss (Brutto)", vorperiodeDaten.ZinsüberschussBrutto, aktuelleDaten.ZinsüberschussBrutto, 1),
                    new GuVZeilen("./. Wertberichtigungen, Abschreibungen", vorperiodeDaten.WertberichtigungenAbschreibungen, aktuelleDaten.WertberichtigungenAbschreibungen, 2),
                    new GuVZeilen("+./. Zuführungen zu Risikovorsorge", vorperiodeDaten.Risikovorsorge, aktuelleDaten.Risikovorsorge, 3),
                    new GuVZeilen("= Zinsüberschuss (Netto)", vorperiodeDaten.ZinsüberschussNetto, aktuelleDaten.ZinsüberschussNetto, 4),
                    new GuVZeilen("./. Personal", vorperiodeDaten.Personal, aktuelleDaten.Personal, 5),
                    new GuVZeilen("./. Werbeausgaben", vorperiodeDaten.Werbeausgaben, aktuelleDaten.Werbeausgaben, 6),
                    new GuVZeilen("./. Verwaltung", vorperiodeDaten.Verwaltung, aktuelleDaten.Verwaltung, 7),
                    new GuVZeilen("= Betriebsaufwendungen", vorperiodeDaten.Betriebsaufwendungen, aktuelleDaten.Betriebsaufwendungen, 8),
                    new GuVZeilen("= Ergebnis aus gewöhnl. Geschäftstätigkeit", vorperiodeDaten.ErgebnisGewöhnlicheGeschäftstätigkeit, aktuelleDaten.ErgebnisGewöhnlicheGeschäftstätigkeit, 9),
                    new GuVZeilen("./. Ausßerordentliche Erträge/Aufwendungen", vorperiodeDaten.ErtraegeAufwendungen, aktuelleDaten.ErtraegeAufwendungen, 10),
                    new GuVZeilen("./. Ertragssteuer", vorperiodeDaten.Ertragssteuer, aktuelleDaten.Ertragssteuer, 11),
                    new GuVZeilen("= Periodenüberschuss / -fehlbetrag", vorperiodeDaten.PeriodenüberschussFehlbetrag, aktuelleDaten.PeriodenüberschussFehlbetrag, 12)
                };

                GuVDatenGrid.ItemsSource = guvDaten;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler bei der Berechnung der GuV-Daten: {ex.Message}");
            }
        }

        /// <summary>
        /// Event-Handler, der beim Laden einer Zeile im DataGrid ausgelöst wird.
        /// Färbt bestimmte Zeilen im DataGrid basierend auf den Beschreibungen hellblau.
        /// </summary>
        /// <param name="sender">Das DataGrid, das das Event auslöst.</param>
        /// <param name="e">Event-Argumente, die die geladene Zeile enthalten.</param>
        /// <exception cref="Exception">
        /// Kann ausgelöst werden, falls die Zeilenfärbung fehlschlägt.
        /// </exception>
        private void GuVDatenRaster_ZeilenLaden(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var rowData = e.Row.Item as GuVZeilen;

                if (rowData != null && (
                    rowData.Beschreibung == "= Zinsüberschuss (Netto)" ||
                    rowData.Beschreibung == "= Betriebsaufwendungen" ||
                    rowData.Beschreibung == "= Ergebnis aus gewöhnl. Geschäftstätigkeit" ||
                    rowData.Beschreibung == "= Periodenüberschuss / -fehlbetrag"))
                {
                    e.Row.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightBlue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Zeile: {ex.Message}");
            }
        }
    }
}
