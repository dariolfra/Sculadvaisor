using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using schooladvisor.Models;
using System.Text.Json;
using System.Net;
//using schooladvisor.Filters;

using Firebase.Auth;
using Firebase.Auth.Providers;
using Org.BouncyCastle.Tls;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MySqlX.XDevAPI;

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISession _session;
        private GestioneDati gestione;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration/*, IHttpContextAccessor contextAccessor*/)
        {
            _logger = logger;
            _configuration = configuration;
            //_session = contextAccessor.HttpContext.Session;
            gestione = new GestioneDati(_configuration);

            //contextAccessor.HttpContext.Items["utente"] = _session.GetString("utente");
        }

        public IActionResult Index()
        {
            var trips = gestione.GetTripList();
            return View(trips);
        }

        //[OnlyAdmin]
        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult ProvaJson()
        //{
        //    Studente studente = new Studente()
        //    {
        //        Matricola = 1,
        //        Voto = 10,
        //        ClasseID = "5BI",
        //        Nome = "Aldo Baglio"
        //    };
        //    string json = JsonSerializer.Serialize(studente);
        //    return new JsonResult(json);
        //}

        //public IActionResult ElencoStudenti()
        //{
        //    List<Studente> studenti = gestione.ListaStudenti();
        //    return View(studenti);
        //}
        //public IActionResult DettaglioStudente(int id)
        //{
        //    Studente studente = gestione.RecuperaStudente(id);
        //    if (studente != null)
        //    {
        //        return View(studente);
        //    }
        //    else
        //    {
        //        ViewData["id"] = id;
        //        return View("NonTrovato");
        //    }
        //}
        //public IActionResult Prova()
        //{
        //    ViewData["nome"] = "Borsato Marco";
        //    return View();
        //}
        //public IActionResult Download(string id)
        //{
        //    return new VirtualFileResult("~/documents/" + id, "application/pdf");
        //}

        //// metodo che invia il form all'utente
        //// con un oggetto studente da riempiere
        ////public IActionResult AggiungiStudente()
        ////{
        ////    ViewData["listaClassi"] = gestione.GetListaClassi();
        ////    return View(new Studente());
        ////}

        //public IActionResult AggiungiStudente()
        //{
        //    ViewData["listaClassi"] = gestione.GetListaClassi();
        //    return View(new StudentePerInserimento());
        //}

        //// metodo che riceve i dati dal form
        ////[HttpPost]
        ////public IActionResult AggiungiStudente(Studente studente)
        ////{
        ////    //la pagina dello studente si crea lei il modello dello studente, perchè io gleilo specifico

        ////    bool esito = gestione.InserisciStudente(studente);
        ////    if (esito)
        ////    {
        ////        return RedirectToAction("ElencoStudenti");
        ////    }
        ////    else
        ////    {
        ////        // se lo studente ha un problema, come per esempio una matricola già inserita
        ////        // rimostro la pagina di inserimento dello studente, che mostra un errore che qualcosa è andato storto
        ////        // ModelState.AddModelError("", "Errore d'inserimento");
        ////        ViewData["errore"] = "Errore di inseriemento: dati non validi";
        ////        return View(studente);
        ////    }
        ////}

        //[HttpPost]
        //public IActionResult AggiungiStudente(StudentePerInserimento studentepi)
        //{
        //    Studente studente = new Studente()
        //    {
        //        Matricola = studentepi.Matricola,
        //        Nome = studentepi.Nome,
        //        ClasseID = studentepi.ClasseID
        //    };

        //    IFormFile file = studentepi.FileFoto;

        //    if (file != null && file.Length > 0)
        //    {
        //        string nomefile = "foto" + studentepi.Matricola + ".jpeg";
        //        studente.NomeFile = nomefile;
        //        FileStream stream = System.IO.File.Create("wwwroot/imagesStudents/" + nomefile);
        //        file.CopyTo(stream);
        //    }

        //    bool esito = gestione.InserisciStudente(studente);
        //    if (esito)
        //    {
        //        return RedirectToAction("ElencoStudenti");
        //    }
        //    else
        //    {
        //        // se lo studente ha un problema, come per esempio una matricola già inserita
        //        // rimostro la pagina di inserimento dello studente, che mostra un errore che qualcosa è andato storto
        //        // ModelState.AddModelError("", "Errore d'inserimento");
        //        ViewData["errore"] = "Errore di inseriemento: dati non validi";
        //        return View(studente);
        //    }
        //}

        //public IActionResult ModificaVoto(int id)
        //{
        //    Studente s = gestione.RecuperaStudente(id);
        //    return View(s);
        //}

        //[HttpPost]
        //public IActionResult ModificaVoto(Studente studente)
        //{
        //    bool esito = gestione.AggiornaVoto(studente);
        //    if (esito)
        //    {
        //        return RedirectToAction("ElencoStudenti");
        //    }
        //    else
        //    {
        //        // se lo studente ha un problema, come per esempio una matricola già inserita
        //        // rimostro la pagina di inserimento dello studente, che mostra un errore che qualcosa è andato storto
        //        // ModelState.AddModelError("", "Errore d'inserimento");
        //        ViewData["errore"] = "Errore di inseriemento: dati non validi";
        //        return View(studente.Matricola);
        //    }
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


        //public IActionResult LoginFirebase()
        //{
        //    return View(new System.Net.NetworkCredential());
        //}

        //[HttpPost]
        //public async Task<IActionResult> LoginFirebase(System.Net.NetworkCredential credential)
        //{
        //    var config = new FirebaseAuthConfig()
        //    {
        //        ApiKey = "AIzaSyD2xRA9Xw4sl01oH1v7_wtuc9tDzIeQPus",
        //        AuthDomain = "test-81bc6.firebaseapp.com",
        //        Providers = new FirebaseAuthProvider[]
        //        {
        //            new GoogleProvider().AddScopes("email"),
        //            new EmailProvider()
        //        }
        //    };

        //    var client = new FirebaseAuthClient(config);

        //    string email = credential.UserName;
        //    string password = credential.Password;

        //    UserCredential userCredential = null;
        //    try
        //    {
        //        userCredential = await client.SignInWithEmailAndPasswordAsync(email, password);
        //    }
        //    catch (Exception ex) { }

        //    if (userCredential != null)
        //    {
        //        _session.SetString("utente", "admin");
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ViewData["errore"] = "Credenziali errate!";
        //        return View(new System.Net.NetworkCredential());
        //    }
        //}


        //public IActionResult Login()
        //{
        //    return View(new System.Net.NetworkCredential());
        //}
        //[HttpPost]
        //public IActionResult Login(System.Net.NetworkCredential credential)
        //{
        //    string adminUsername = _configuration["Credentials:username"];
        //    string adminPassword = _configuration["Credentials:password"];

        //    string username = credential.UserName;
        //    string password = credential.Password;

        //    if (username == adminUsername && password == adminPassword)
        //    {
        //        //settare la variabile di sessione
        //        // la sessione memorizza solo stringhe
        //        // se dovessi memorizzare un oggetto => faccio un JSON
        //        _session.SetString("utente", "admin"); //chiave valore
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        //credenziali errate
        //        ViewData["errore"] = "Credenziali errate!";
        //        return View(new System.Net.NetworkCredential());
        //    }
        //}

        //public IActionResult Logout()
        //{
        //    //cancellare sessione
        //    _session.Clear();
        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> ProvaTelegram()
        {
            HttpClient client = new HttpClient();
            string apikey = "7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw";
            string chatID = "521459468";
            string text = "*ODIO I FROCI*";

            Uri uri = new Uri(@"https://api.telegram.org/bot7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw/sendMessage?chat_id=521459468&parse_mode=MarkdownV2&text="+text);

            var response = await client.GetAsync(uri);
            ViewData["esito"] = "Successo = " + response.IsSuccessStatusCode;
            return View("index.cshmtl");
        }

        public async Task<IActionResult> ProvaTelegramConPulsanti()
        {
            HttpClient client = new HttpClient();
            string apikey = "7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw";
            string chatID = "521459468";
            string text = "*ODIO I FROCI*";

            // Creazione della tastiera inline
            var keyboard = new
            {
                inline_keyboard = new[]
                {
                    new[]
                    {
                        new { text = "Conferma✅", callback_data = "approved" },
                        new { text = "Rifiuto❌", callback_data = "rejected" }
                    }
                }
            };

            var keyboardJson = JsonConvert.SerializeObject(keyboard);

            Uri uri = new Uri($"https://api.telegram.org/bot{apikey}/sendMessage?chat_id={chatID}&parse_mode=MarkdownV2&text={text}&reply_markup={keyboardJson}");

            var response = await client.GetAsync(uri);
            ViewData["esito"] = "Successo = " + response.IsSuccessStatusCode;
            return View("ConfermaCommento");
        }
    }
}