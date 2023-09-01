using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U3W1D5_BENCHMARK
{
    internal class Contribuente
    {
        //Proprietà (le ho impostate con una validazione nel setter)
        private string _nome;
        public string Nome { 
                get => _nome ; 
                set {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                    throw new ArgumentException("Il nome non può essere vuoto o contenere solo spazi bianchi.");
                    }
                    _nome = value;
                    }
                }
        private string _cognome;
        public string Cognome { 
            get => _cognome; 
            set {
                if(string.IsNullOrWhiteSpace(value)) 
                {
                    throw new ArgumentException("Il cognome non può essere vuoto o contenere solo spazi bianchi.");
                }
                _cognome = value;
                } 
            }

        private DateTime _datanascita;
        public DateTime DataNascita
        {
            get => _datanascita;
            set
            {
                if (value > DateTime.Today)
                {
                    throw new ArgumentException("La data di nascita non può essere nel futuro.");
                }
                _datanascita = value;
                }
            }
        private string _codicefiscale; 
        public string CodiceFiscale {
            get => _codicefiscale;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 16)
                {
                    throw new ArgumentException("Il codice fiscale deve essere composto da 16 caratteri.");
                }
                _codicefiscale = value;
            }
        }
        public string Sesso { get; set; }

        public string ComuneResidenza { get; set; }
        public decimal RedditoAnnuale { get; set; }

        //Metodo

        // Il metodo calcola, come richiesto l'imposta in base al reddito annuale
        // con aliquote e scaglioni specifici. I dati sono stati tipizzati come decimal
        // e, per evitare ambiguità in fase di compilazione, marcati col suffisso specifico (m)
        public decimal CalcolaImposta()
        {
            decimal ImpostaDovuta = 0;
            if (RedditoAnnuale >= 0 && RedditoAnnuale <= 15000)
            {
                ImpostaDovuta = RedditoAnnuale * 0.23m;
            }
            else if (RedditoAnnuale > 15000 && RedditoAnnuale <= 28000)
            {
                ImpostaDovuta = 3450 + (RedditoAnnuale - 15000) * 0.27m;
            }
            else if (RedditoAnnuale > 28000 && RedditoAnnuale <= 55000)
            {
                ImpostaDovuta = 6960 + (RedditoAnnuale - 28000) * 0.38m;
            }
            else if (RedditoAnnuale > 55000 && RedditoAnnuale <= 75000)
            {
                ImpostaDovuta = 17220 + (RedditoAnnuale - 55000) * 0.41m;
            }
            else if (RedditoAnnuale > 75000)
            {
                ImpostaDovuta = 25420 + (RedditoAnnuale - 75000) * 0.43m;
            }
            return ImpostaDovuta;
        }

        //Costruttori
        // Ho creato quattro costruttori, richiamando il primo per inizializzare il secondo (e così via).
        // L'obbiettivo era di garantire flessibilità nella creazione di oggetti Contribuente con diverse
        // combinazioni di dati in ingresso, utilizzando l'overloading per evitare duplicazioni di codice.
        public Contribuente() { }
        public Contribuente (string nome, string cognome, decimal redditoannuale)
        {
            Nome = nome;
            Cognome = cognome;
            RedditoAnnuale = redditoannuale;
        }

        public Contribuente(string nome, string cognome, DateTime datanascita, string sesso, decimal redditoannuale) : this(nome, cognome, redditoannuale)
        {
            DataNascita = datanascita;
            Sesso = sesso;
        }

        public Contribuente (string nome, string cognome, DateTime datanascita, string codicefiscale, string sesso, string comuneresidenza, decimal redditoannuale) : this(nome,cognome, datanascita, sesso, redditoannuale)
        {
            CodiceFiscale = codicefiscale;
            ComuneResidenza = comuneresidenza;
        }
    }
}
