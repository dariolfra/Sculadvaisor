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
using Microsoft.AspNetCore.Http;
using schooladvisor.TelegramBot;

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISession _session;
        private GestioneDati gestione;
        private TelegramBot bot;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor contextAccessor, TelegramBot b)
        {
            _logger = logger;
            _configuration = configuration;
            _session = contextAccessor.HttpContext.Session;
            gestione = new GestioneDati(_configuration);

            bot = b;
            bot.GD = gestione;

            contextAccessor.HttpContext.Items["utente"] = _session.GetString("utente");
        }

        public IActionResult Index()
        {
            var trips = gestione.GetTripList();
            return View(trips);
        }
        public IActionResult Uscite()
        {
            var trips = gestione.GetTripList();
            return View(trips);
        }
        public IActionResult CommentaUscita(string selectedTripId,string email)
        {
            ViewData["email"] = email;
            var trip = gestione.GetTrip(selectedTripId);
            return View(trip);
        }
        [HttpPost]
        public async Task<IActionResult> CommentaUscita(string comment,string email,string selectedTripId,string rating)
        {
            await bot.SendMessage(new Review() { reviewComment = comment, reviewRating = Convert.ToInt32(rating), reviewState = "sent", tripID = Convert.ToInt32(selectedTripId), userID = email });
            return View("ConfermaCommento");
        }

        //public async Task<IActionResult> ProvaTelegram()
        //{
        //    HttpClient client = new HttpClient();
        //    string apikey = "7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw";
        //    string chatID = "521459468";
        //    string text = "*ODIO I FROCI*";

        //    Uri uri = new Uri(@"https://api.telegram.org/bot7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw/sendMessage?chat_id=521459468&parse_mode=MarkdownV2&text="+text);

        //    var response = await client.GetAsync(uri);
        //    ViewData["esito"] = "Successo = " + response.IsSuccessStatusCode;
        //    return View("Index");
        //}
        //public async Task<IActionResult> ProvaTelegramConPulsanti()
        //{
        //    HttpClient client = new HttpClient();
        //    string apikey = "7102056047:AAEvMieOy6ZfDYCjNwnV8df36gTAQR0liIw";
        //    string chatID = "521459468";
        //    string text = "*ODIO I FROCI*";

        //    // Creazione della tastiera inline
        //    var keyboard = new
        //    {
        //        inline_keyboard = new[]
        //        {
        //            new[]
        //            {
        //                new { text = "Conferma✅", callback_data = "approved" },
        //                new { text = "Rifiuto❌", callback_data = "rejected" }
        //            }
        //        }
        //    };

        //    var keyboardJson = JsonConvert.SerializeObject(keyboard);

        //    Uri uri = new Uri($"https://api.telegram.org/bot{apikey}/sendMessage?chat_id={chatID}&parse_mode=MarkdownV2&text={text}&reply_markup={keyboardJson}");

        //    var response = await client.GetAsync(uri);
        //    ViewData["esito"] = "Successo = " + response.IsSuccessStatusCode;
        //    return View("ConfermaCommento");
        //}


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
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewData["errore"] = "Credenziali errate!";
                return View(new System.Net.NetworkCredential());
            }
        }
    }
}