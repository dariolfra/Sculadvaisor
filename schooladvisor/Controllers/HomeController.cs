using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using schooladvisor.Models;
using System.Text.Json;
using System.Net;
using schooladvisor.Filters;

using Firebase.Auth;
using Firebase.Auth.Providers;
using Org.BouncyCastle.Tls;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MySqlX.XDevAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

//parcheggio tilde che porcaccio iddio non ce l'ho sulla tastiera
// ----->  ~  <-----

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISession _session;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private GestioneDati gestione;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor contextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _configuration = configuration;
            _session = contextAccessor.HttpContext.Session;
            _hostingEnvironment = hostingEnvironment;

            gestione = new GestioneDati(_configuration);

            contextAccessor.HttpContext.Items["utente"] = _session.GetString("utente");
        }

        public IActionResult Index()
        {
            var trips = gestione.GetLastTrips();
            return View(trips);
        }

        public IActionResult Uscite()
        {
            var trips = gestione.GetTripList();
            return View(trips);
        }
        public IActionResult CommentaUscita(string selectedTripId)
        {
            var trip = gestione.GetTrip(selectedTripId);
            return View(trip);
        }
        [HttpPost]
        public async Task<IActionResult> CommentaUscita(string comment,string username,string selectedTripId,string rating)
        {
            gestione.RegisterComment(comment,username,selectedTripId,rating);
            return View("ConfermaCommento");
        }
        public async Task<IActionResult> ProvaTelegram()
        {
            HttpClient client = new HttpClient();
            string apikey = "7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw";
            string chatID = "521459468";
            string text = "*ODIO I FROCI*";

            Uri uri = new Uri(@"https://api.telegram.org/bot7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw/sendMessage?chat_id=521459468&parse_mode=MarkdownV2&text="+text);

            var response = await client.GetAsync(uri);
            ViewData["esito"] = "Successo = " + response.IsSuccessStatusCode;
            return View("Index");
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

        public IActionResult LoginFirebase()
        {
            return View(new System.Net.NetworkCredential());
        }

        [HttpPost]
        public async Task<IActionResult> LoginFirebase(System.Net.NetworkCredential credential, string selectedTripId)
        {
            var config = new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyAGbFPtTBpeRY-2YAVwz9dBHE16W-COPg0",
                AuthDomain = "schooladvisor-b0a51.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                }
            };

            var client = new FirebaseAuthClient(config);

            string email = credential.UserName;
            string password = credential.Password;

            UserCredential userCredential = null;
            try
            {
                userCredential = await client.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch (Exception ex) { }

            if (userCredential != null)
            {
                _session.SetString("utente", "admin");
                if (!string.IsNullOrEmpty(selectedTripId))
                {
                    return RedirectToAction("CommentaUscita", new { selectedTripId = selectedTripId, email = email});
                }
                else
                {
                    var auth = User.Identity.IsAuthenticated;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewData["errore"] = "Credenziali errate!";
                return View(new System.Net.NetworkCredential());
            }
        }
        public IActionResult VisualizzaCommenti(string selectedTripId)
        {
            var trip = gestione.GetTrip(selectedTripId);
            var approvedComments = gestione.GetApprovedComments(selectedTripId);

            ViewData["ApprovedComments"] = approvedComments;

            return View(trip);
        }

        public IActionResult AccessoNegato()
        {
            return View();
        }

        [OnlyAdmin]
        public IActionResult AggiungiUscita()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AggiungiUscita(IFormFile file, string tripName, DateTime tripDate, string tripDescription)
        {
            if (file != null && file.Length > 0)
            {
                var uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "img");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Genera un nome univoco per il file caricato
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Imposta il percorso relativo del file per il viaggio
                var relativeFilePath = Path.Combine("/img", fileName);

                Trip t = new Trip() { image = relativeFilePath, tripDate = tripDate, tripDescription = tripDescription, tripName = tripName };
                gestione.AddTrip(t);
            }

            return View("ConfermaAggiunta");
        }

        public IActionResult RisultatiRicerca(string search)
        {
            List<Trip> searchResults = gestione.SearchTrips(search);
            ViewBag.SearchText = search;
            return View(searchResults);
        }
        public IActionResult Logout()
        {
            var trips = gestione.GetLastTrips();
            _session.SetString("utente", "utente");
            return RedirectToAction("Index",trips);
        }

        [OnlyAdmin]
        public IActionResult ModificaUscita(string selectedTripID)
        {
            var trip = gestione.GetTrip(selectedTripID);
            return View(trip);
        }
        [HttpPost]
        public async Task<IActionResult> ModificaUscita(IFormFile file, string tripName, DateTime tripDate, string tripDescription,string selectedTripID)
        {
            Trip t = new Trip() {tripDate = tripDate, tripDescription = tripDescription, tripName = tripName, tripID = Convert.ToInt32(selectedTripID) };

            if (file != null && file.Length > 0)
            {
                var uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "img");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Genera un nome univoco per il file caricato
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Imposta il percorso relativo del file per il viaggio
                var relativeFilePath = Path.Combine("/img", fileName);

                t.image = relativeFilePath;
            }
            gestione.EditTrip(t);

            var trips = gestione.GetLastTrips();
            return RedirectToAction("Index",trips);
        }
    }
}