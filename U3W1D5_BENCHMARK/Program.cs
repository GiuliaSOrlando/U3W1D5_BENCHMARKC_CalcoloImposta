using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace U3W1D5_BENCHMARK
{
    internal class Program
    {
        static List<Contribuente> contribuenti  = new List<Contribuente>();
        static Regex regexCampiStringa = new Regex(@"^[a-zA-ZÀ-ÖØ-öø-ÿ]+$");

        static void Main(string[] args)
        {
            // Questa riga fornisce la codifica UTF-8 per l'output della console,
            // consentendo la visualizzazione corretta di caratteri speciali (come €).
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.WriteLine("=================================================");
                Console.WriteLine("Menu:");
                Console.WriteLine("1) Inserimento di una nuova dichiarazione di un contribuente");
                Console.WriteLine("2) Lista completa di tutti i contribuenti analizzati");
                Console.WriteLine("3) Uscita");
                Console.Write("Scelta: ");

                string scelta = Console.ReadLine();
                switch (scelta)
                {
                    case "1":
                        InserisciContribuente();
                        break;
                    case "2":
                        MostraContribuenti();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Scelta non valida. Riprova.");
                        break;
                }
            }

            
        }
        static void InserisciContribuente()

        // Ho impostato il metodo prendendo l'input dell'utente per il campo, validandolo
        // in maniera corrispondente a quanto fatto nel setter delle singole proprietà della classe.
        // Per i campi testuali ho utilizzato una regex per validare che non ci fossero valori numerici o caratteri speciali.
        // Infine li ho formattati in "Title Case" (la prima lettera maiuscola e le altre minuscole). 
        // Per i campi data di nascita e reddito ho validato i rispettivi formati (DateTime e decimal).
        // Ho anche aggiunto una condizione che impedisca l'inserimento di un reddito negativo 
        // e un paio di condizioni che limitino la data di nascita (maggiore età e data di nascita nel futuro).
        // Invece per il sesso e il codice fiscale validazione e formattazione sono specifiche (vedi sotto).

        {
            Console.WriteLine("==================================================\r\n");
            Console.WriteLine("CALCOLO DELL’IMPOSTA DA VERSARE:");

            string nome = "";
            while (string.IsNullOrWhiteSpace(nome) || !regexCampiStringa.IsMatch(nome))
            {
                Console.Write("Inserire il nome:");
                nome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("Il nome non può essere vuoto. Riprova.");
                }
                else if (!regexCampiStringa.IsMatch(nome))
                {
                    Console.WriteLine("Il nome non può contenere numeri o caratteri speciali. Riprova.");
                }
            }
            nome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nome);

            string cognome = "";
            while (string.IsNullOrWhiteSpace(cognome) || !regexCampiStringa.IsMatch(cognome))
            {
                Console.Write("Inserire il cognome:");
                cognome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cognome))
                {
                    Console.WriteLine("Il cognome non può essere vuoto. Riprova.");
                }
                else if (!regexCampiStringa.IsMatch(cognome))
                {
                    Console.WriteLine("Il cognome non può contenere numeri o caratteri speciali. Riprova.");
                }
            }
            cognome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cognome);

            DateTime datanascita;
            DateTime dataodierna = DateTime.Now;
            while (true)
            {
                Console.Write("Inserire la data di nascita:");
                string inputdata = Console.ReadLine();

                try
                {
                    datanascita = DateTime.Parse(inputdata);

                    // Gestissco l'eccezione di un'eventuale data nel futuro.
                    if (datanascita > dataodierna)
                    {
                        Console.WriteLine("La data di nascita non può essere nel futuro. Riprova.");
                        continue;
                    }

                    // Prima calcolo approssimativamente l'età, sottraendo l'anno di nascita da quello corrente,
                    // Poi nell'if, verifico se la data di nascita non è ancora avvenuta quest'anno
                    // e in tal caso correggo l'età, sottraeundo 1.

                    int eta = dataodierna.Year - datanascita.Year;
                    if (datanascita.DayOfYear > dataodierna.DayOfYear)
                    {
                        eta--;
                    }
                    if (eta < 18)
                    {
                        Console.WriteLine("Non puoi dichiarare un reddito se sei minorenne. Riprova.");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Data di nascita non valida. Riprova.");
                }
            }

            // L'input richiede un formato specifico (M/F) e tale input è preso indipendentemente
            // dalla capitalizzazione della lettera.

            string sesso;
            while (true)
            {
                Console.Write("Inserire il genere (M/F)");
                sesso = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(sesso))
                {
                    Console.WriteLine("Il genere non può essere vuoto. Riprova.");
                } else {
                    sesso = sesso.ToUpper();
                    if (sesso == "M" || sesso == "F")
                    {
                        break;
                    }
                    Console.WriteLine("Genere non valido, si prega di inserire M o F");
                }
            }

            string comuneresidenza = "";
            while (string.IsNullOrWhiteSpace(comuneresidenza) || !regexCampiStringa.IsMatch(comuneresidenza))
            {
                Console.Write("Inserire il comune di residenza:");
                comuneresidenza = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(comuneresidenza))
                {
                    Console.WriteLine("Il comune di residenza non può essere vuoto. Riprova.");
                }
                else if (!regexCampiStringa.IsMatch(comuneresidenza))
                {
                    Console.WriteLine("Il comune di residenza non può contenere numeri o caratteri speciali. Riprova.");
                }
            }
        
            comuneresidenza = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(comuneresidenza);

            // Ho utilizzato una regex per garantire che il codice fiscale inserito dall'utente
            // abbia il formato corretto (nella sua forma base). Il pattern è strutturato come segue:
            // - 6 lettere maiuscole (per il nome e cognome)
            // - 2 cifre (per l'anno di nascita)
            // - 1 lettera maiuscola (per il mese di nascita)
            // - 2 cifre (per il giorno di nascita, inclusivo del sesso)
            // - 1 lettera maiuscola 
            // - 3 lettere maiuscole (per il codice del comune di nascita)
            // - 1 lettera maiuscola (per il codice di controllo)
            // Questa regex permette solo di verificare che il formato sia verosimile.
 
            // Ho anche inserito una validazione per assicurarmi che il codice non sia nullo,
            // vuoto o contenga solo spazi bianchi e che non superi i 16 caratteri complessivi.


            string codicefiscale;
            Regex regexcodicefiscale = new Regex(@"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$", RegexOptions.IgnoreCase);
            while (true)
            {
                Console.Write("Inserire il codice fiscale:");
                codicefiscale = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(codicefiscale))
                {
                    Console.WriteLine("Il codice fiscale non può essere vuoto. Riprova.");
                }
                else if (codicefiscale.Length > 16)
                {
                    Console.WriteLine("Il codice fiscale non può essere più lungo di 16 caratteri. Riprova.");
                }
                else if (regexcodicefiscale.IsMatch(codicefiscale))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Codice fiscale non valido, si prega di inserire un codice valido (formato richiesto: ABCDEF12G34H567I)");
                }
            }

            decimal redditoannuale;
            while (true)
            {
                Console.Write("Inserire il proprio reddito annuale: €");
                string reddito = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(reddito))
                {
                    Console.WriteLine("Il reddito annuale non può essere vuoto. Riprova.");
                }
                else
                {
                    try
                    {
                        redditoannuale = decimal.Parse(reddito);
                        if (redditoannuale < 0)
                        {
                            Console.WriteLine("Il reddito annuale non può essere negativo. Riprova.");
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Reddito annuale non valido. Riprova.");
                    }
                }
            }

            Contribuente contribuente = new Contribuente(nome, cognome, datanascita, codicefiscale, sesso, comuneresidenza, redditoannuale);
            decimal impostadovuta = contribuente.CalcolaImposta();

            Console.WriteLine("==================================================\r\n");
            Console.WriteLine($"Contribuente: {nome} {cognome}");
            string genereStampato = sesso == "F" ? "nata" : "nato";
            Console.WriteLine($"{genereStampato} il: {datanascita:dd/MM/yyyy} ({sesso})");
            Console.WriteLine($"residente in: {comuneresidenza}");
            Console.WriteLine($"codice fiscale: {codicefiscale.ToUpper()}\n");
            Console.WriteLine($"Reddito dichiarato: € {redditoannuale}\n");
            Console.WriteLine($"IMPOSTA DA VERSARE: € {impostadovuta}");
            Console.ReadLine();

            contribuenti.Add(contribuente);
        }

        // Il metodo mostra la lista dei contribuenti, che è una lista statico inizializzato all'avvio del Program
        // e popolata alla fine del metodo InserisciContribuente().
        // Questo metodo verifica che la lista non sia vuota ed eventualmente stampa l'elenco dei contribuenti,
        // assegnando numeri univoci
        static void MostraContribuenti()
        {
            if(contribuenti.Count == 0)
            {
                Console.WriteLine("Nessun contribuente presente");
            } else
            {
                int numeroContribuente = 1;
                Console.WriteLine("==================================================");
                Console.WriteLine("LISTA DEI CONTRIBUENTI:");

                foreach (Contribuente con in contribuenti)
                {
                    Console.WriteLine($"Contribuente #{numeroContribuente}: {con.Nome} {con.Cognome},");
                    string genereStampato = con.Sesso == "F" ? "nata" : "nato";
                    Console.WriteLine($"{genereStampato} il: {con.DataNascita:dd/MM/yyyy} ({con.Sesso})");
                    Console.WriteLine($"residente in {con.ComuneResidenza},");
                    Console.WriteLine($"codice fiscale: {con.CodiceFiscale.ToUpper()}");
                    Console.WriteLine($"Reddito dichiarato: € {con.RedditoAnnuale}");
                    decimal impostadovuta = con.CalcolaImposta();
                    Console.WriteLine($"IMPOSTA DA VERSARE: € {impostadovuta}");
                    Console.WriteLine("==================================================");
                    numeroContribuente++;
                }
            }
        }
    }
}
