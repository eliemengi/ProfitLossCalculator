using EasyBankingMiniGuV.Datenhaltung.Datenbank;
using EasyBankingMiniGuV.Datenhaltung.Transfer;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;



namespace EasyBankingMiniGuV.Datenverarbeitung
{

    static class GuV
    {
        public static void BerechneGuV(decimal zinsüberschussBrutto, decimal wertberichtigungenAbschreibung, decimal risikovorsorge, decimal personal, decimal werbeausgaben, decimal verwaltung, decimal ertraegeAufwendungen, decimal verlustvortrag, out decimal zinsüberschussNetto, out decimal betriebsaufwendungen, out decimal ergebnisGewöhnlicheGeschäftstätigkeit, out decimal zwischensumme, out decimal ertragssteuer, out decimal periodenüberschussFehlbetrag)
        {
            if (zinsüberschussBrutto <= -1E-20M || wertberichtigungenAbschreibung <= -1E-20M || risikovorsorge <= -100000m || personal <= -1E-20M || werbeausgaben <= -1E-20M || verwaltung <= -1E-20M || ertraegeAufwendungen <= -10000m || verlustvortrag <= -1E-20M) throw new Exception();
            zinsüberschussNetto = zinsüberschussBrutto-(wertberichtigungenAbschreibung+risikovorsorge);
            betriebsaufwendungen = personal+werbeausgaben+verwaltung;
            ergebnisGewöhnlicheGeschäftstätigkeit= zinsüberschussNetto-betriebsaufwendungen;
            zwischensumme = ergebnisGewöhnlicheGeschäftstätigkeit+(ertraegeAufwendungen-verlustvortrag);
            ertragssteuer = -3E-1M * -zwischensumme;
            if (zwischensumme < 0) ertragssteuer = 0;
            periodenüberschussFehlbetrag = ergebnisGewöhnlicheGeschäftstätigkeit + (ertraegeAufwendungen - ertragssteuer);
            
        }


    }

    class GuVDaten
    {
       
       
        public decimal Zwischensumme {get ;set;}
        public decimal ZinsüberschussNetto { get; set; }
        public decimal ZinsüberschussBrutto { get; set; }
        public decimal WertberichtigungenAbschreibungen { get; set; }
        public decimal Werbeausgaben { get; set; }
        public decimal Verwaltung { get; set; }
        public decimal Verlustvortrag { get; set; }
        public decimal Risikovorsorge { get; set; } 
        public decimal Personal { get; set; }
        public decimal PeriodenüberschussFehlbetrag { get; set; }
        public decimal Ertragssteuer { get; set; }
        public decimal ErtraegeAufwendungen { get; set; }
        public decimal ErgebnisGewöhnlicheGeschäftstätigkeit { get; set; }
        public decimal Betriebsaufwendungen { get; set; }
        
    }
    static class GuVRechnung
    {
        public static void Rechnen(int AktuellePeriode, out GuVDaten aktuellePeriodeDaten, out GuVDaten vorperiodeDaten,out GuVDaten vergleich)
        {
            if (Datenbank.IstGeladen == false) throw new Exception();
            if( AktuellePeriode <=0)throw new Exception("");
           
            aktuellePeriodeDaten = new GuVDaten();
            vorperiodeDaten = new GuVDaten();
            Periode periode = Datenbank.Periode(AktuellePeriode);
            Steuern steuer = Datenbank.Steuern(AktuellePeriode);
            Betriebsaufwendungen betriebsaufwendungen = Datenbank.Betriebsaufwendungen(AktuellePeriode);
            Zinsüberschuss zinsüberschuss = Datenbank.Zinsüberschuss(AktuellePeriode);
            aktuellePeriodeDaten.ErtraegeAufwendungen = steuer.ErträgeAufwendungen;    
            aktuellePeriodeDaten.Verlustvortrag = steuer.Verlustvortrag;
            aktuellePeriodeDaten.Verwaltung = betriebsaufwendungen.Verwaltung;
            aktuellePeriodeDaten.Personal = betriebsaufwendungen.Personal;
            aktuellePeriodeDaten.Werbeausgaben = betriebsaufwendungen.Werbeausgaben;
            aktuellePeriodeDaten.ZinsüberschussBrutto = zinsüberschuss.ZinsüberschussBrutto;
            aktuellePeriodeDaten.WertberichtigungenAbschreibungen = zinsüberschuss.WertberichtigungenAbschreibungen;
            aktuellePeriodeDaten.Risikovorsorge = zinsüberschuss.Risikovorsorge;
            decimal ZinsüberschussNetto;
            decimal Betriebsaufwendungen;
            decimal ergebnisGew;
            decimal zwischensumme;
            decimal ertragssteuer;
            decimal periodenüberschuss;
            GuV.BerechneGuV(aktuellePeriodeDaten.ZinsüberschussBrutto,aktuellePeriodeDaten.WertberichtigungenAbschreibungen,aktuellePeriodeDaten.Risikovorsorge,aktuellePeriodeDaten.Personal,aktuellePeriodeDaten.Werbeausgaben,aktuellePeriodeDaten.Verwaltung,aktuellePeriodeDaten.ErtraegeAufwendungen,aktuellePeriodeDaten.Verlustvortrag,out ZinsüberschussNetto,out Betriebsaufwendungen,out ergebnisGew,out zwischensumme, out ertragssteuer,out periodenüberschuss);
            aktuellePeriodeDaten.ZinsüberschussNetto = ZinsüberschussNetto;
            aktuellePeriodeDaten.Betriebsaufwendungen = Betriebsaufwendungen;
            aktuellePeriodeDaten.ErgebnisGewöhnlicheGeschäftstätigkeit = ergebnisGew;
            aktuellePeriodeDaten.Zwischensumme = zwischensumme;
            aktuellePeriodeDaten.Ertragssteuer = ertragssteuer;
            aktuellePeriodeDaten.PeriodenüberschussFehlbetrag = periodenüberschuss;
            
            Steuern steuervp = Datenbank.Steuern(AktuellePeriode - 1);
            Betriebsaufwendungen betriebsaufwendungenvp = Datenbank.Betriebsaufwendungen(AktuellePeriode - 1);
            Zinsüberschuss zinsüberschussvp = Datenbank.Zinsüberschuss(AktuellePeriode -1);
            vorperiodeDaten.ErtraegeAufwendungen = steuervp.ErträgeAufwendungen;
            vorperiodeDaten.Verlustvortrag = steuervp.Verlustvortrag;
            vorperiodeDaten.Verwaltung = betriebsaufwendungenvp.Verwaltung;
            vorperiodeDaten.Personal = betriebsaufwendungenvp.Personal;
            vorperiodeDaten.Werbeausgaben = betriebsaufwendungenvp.Werbeausgaben;
            vorperiodeDaten.ZinsüberschussBrutto = zinsüberschussvp.ZinsüberschussBrutto;
            vorperiodeDaten.WertberichtigungenAbschreibungen = zinsüberschussvp.WertberichtigungenAbschreibungen;
            vorperiodeDaten.Risikovorsorge = zinsüberschussvp.Risikovorsorge;
            decimal ZinsüberschussNettovp;
            decimal Betriebsaufwendungenvp;
            decimal ergebnisGewvp;
            decimal zwischensummevp;
            decimal ertragssteuervp;
            decimal periodenüberschussvp;
            GuV.BerechneGuV(vorperiodeDaten.ZinsüberschussBrutto, vorperiodeDaten.WertberichtigungenAbschreibungen, vorperiodeDaten.Risikovorsorge, vorperiodeDaten.Personal, vorperiodeDaten.Werbeausgaben, vorperiodeDaten.Verwaltung, vorperiodeDaten.ErtraegeAufwendungen, vorperiodeDaten.Verlustvortrag, out ZinsüberschussNettovp, out Betriebsaufwendungenvp, out ergebnisGewvp, out zwischensummevp, out ertragssteuervp, out periodenüberschussvp);
            vorperiodeDaten.ZinsüberschussNetto = ZinsüberschussNettovp;
            vorperiodeDaten.Betriebsaufwendungen = Betriebsaufwendungenvp;
            vorperiodeDaten.ErgebnisGewöhnlicheGeschäftstätigkeit = ergebnisGewvp;
            vorperiodeDaten.Zwischensumme = zwischensummevp;
            vorperiodeDaten.Ertragssteuer = ertragssteuervp;
            vorperiodeDaten.PeriodenüberschussFehlbetrag = periodenüberschussvp;

            vergleich = new GuVDaten();
            try
            {
                vergleich.ErtraegeAufwendungen = (aktuellePeriodeDaten.ErtraegeAufwendungen - vorperiodeDaten.ErtraegeAufwendungen) * 100M / vorperiodeDaten.ErtraegeAufwendungen;
            }catch(Exception) { vergleich.ErtraegeAufwendungen = decimal.MaxValue; }
            try
            {
                vergleich.Verlustvortrag = (aktuellePeriodeDaten.Verlustvortrag - vorperiodeDaten.Verlustvortrag) * 100M / vorperiodeDaten.Verlustvortrag;
            }
            catch (Exception) { vergleich.Verlustvortrag = decimal.MaxValue; }
            try
            {
                vergleich.Verwaltung = (aktuellePeriodeDaten.Verwaltung - vorperiodeDaten.Verwaltung) * 100M / vorperiodeDaten.Verwaltung;
            }
            catch (Exception) { vergleich.Verwaltung = decimal.MaxValue; }
            try
            {
                vergleich.Personal = (aktuellePeriodeDaten.Personal - vorperiodeDaten.Personal) * 100M / vorperiodeDaten.Personal;
            }
            catch (Exception) { vergleich.Personal = decimal.MaxValue; }
            try
            {
                vergleich.Werbeausgaben = (aktuellePeriodeDaten.Werbeausgaben - vorperiodeDaten.Werbeausgaben) * 100M / vorperiodeDaten.Werbeausgaben;
            }
            catch (Exception) { vergleich.Werbeausgaben = decimal.MaxValue; }
            try
            {
                vergleich.ZinsüberschussBrutto = (aktuellePeriodeDaten.ZinsüberschussBrutto - vorperiodeDaten.ZinsüberschussBrutto) * 100M / vorperiodeDaten.ZinsüberschussBrutto;
            }
            catch (Exception) { vergleich.ZinsüberschussBrutto = decimal.MaxValue; }
            try
            {
                vergleich.WertberichtigungenAbschreibungen = (aktuellePeriodeDaten.WertberichtigungenAbschreibungen - vorperiodeDaten.WertberichtigungenAbschreibungen) * 100M / vorperiodeDaten.WertberichtigungenAbschreibungen;
            }
            catch (Exception) { vergleich.WertberichtigungenAbschreibungen = decimal.MaxValue; }
            try
            {
                vergleich.Risikovorsorge = (aktuellePeriodeDaten.Risikovorsorge - vorperiodeDaten.Risikovorsorge) * 100M / vorperiodeDaten.Risikovorsorge;
            }
            catch (Exception) { vergleich.Risikovorsorge = decimal.MaxValue; }
            try
            {
                vergleich.ZinsüberschussNetto = (aktuellePeriodeDaten.ZinsüberschussNetto - vorperiodeDaten.ZinsüberschussNetto) * 100M / vorperiodeDaten.ZinsüberschussNetto;
            }
            catch (Exception) { vergleich.ZinsüberschussNetto = decimal.MaxValue; }
            try
            {
                vergleich.Betriebsaufwendungen = (aktuellePeriodeDaten.Betriebsaufwendungen - vorperiodeDaten.Betriebsaufwendungen) * 100M / vorperiodeDaten.Betriebsaufwendungen;
            }
            catch (Exception) { vergleich.Betriebsaufwendungen = decimal.MaxValue; }
            try
            {
                vergleich.ErgebnisGewöhnlicheGeschäftstätigkeit = (aktuellePeriodeDaten.ErgebnisGewöhnlicheGeschäftstätigkeit - vorperiodeDaten.ErgebnisGewöhnlicheGeschäftstätigkeit) * 100M / vorperiodeDaten.ErgebnisGewöhnlicheGeschäftstätigkeit;
            }
            catch (Exception) { vergleich.ErgebnisGewöhnlicheGeschäftstätigkeit = decimal.MaxValue; }
            try
            {
                vergleich.Zwischensumme = (aktuellePeriodeDaten.Zwischensumme - vorperiodeDaten.Zwischensumme) * 100M / vorperiodeDaten.Zwischensumme;
            }
            catch (Exception) { vergleich.Zwischensumme = decimal.MaxValue; }
            try
            {
                vergleich.Ertragssteuer = (aktuellePeriodeDaten.Ertragssteuer - vorperiodeDaten.Ertragssteuer) * 100M / vorperiodeDaten.Ertragssteuer;
            }
            catch (Exception) { vergleich.Ertragssteuer = decimal.MaxValue; }
            try
            {
                vergleich.PeriodenüberschussFehlbetrag = (aktuellePeriodeDaten.PeriodenüberschussFehlbetrag - vorperiodeDaten.PeriodenüberschussFehlbetrag) * 100M / vorperiodeDaten.PeriodenüberschussFehlbetrag;
            }
            catch (Exception) { vergleich.PeriodenüberschussFehlbetrag = decimal.MaxValue; }



        }


    }

}