﻿using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace schooladvisor.Models
{
    public class GestioneDati
    {
        private string s; //Connectionsszszsz
        public GestioneDati(IConfiguration configuration)
        {
            s = configuration.GetConnectionString("ScuolaConnection");
        }

        public List<Trip> GetTripList()
        {
            using var con = new MySqlConnection(s);
            return con.Query<Trip>("SELECT * from trips").ToList();
        }

        public Trip GetTrip(string id)
        {
            using var con = new MySqlConnection(s);
            return con.QueryFirstOrDefault<Trip>("SELECT * FROM trips WHERE tripID = @Id", new { Id = id });
        }

        public void RegisterComment(string comment,string email,string selectedTripId)
        {
            using var con = new MySqlConnection(s);

            string sql = "INSERT INTO reviews (UserID,reviewComment,tripID) VALUES (@Email, @Comment, @TripID)";

            // Esegui l'istruzione SQL utilizzando Dapper
            con.Execute(sql, new { Email = email, Comment = comment, TripID = selectedTripId});
        }


        ////Metodi CRUD
        //public List<Studente> ListaStudenti()
        //{
        //    //apro connessione al dataBASE
        //    //Che si chiude da sola
        //    using var con = new MySqlConnection(s);
        //    return con.Query<Studente>("Select * from alunni").ToList();
        //}

        //public Studente RecuperaStudente(int id)
        //{
        //    using var con = new MySqlConnection(s);
        //    return con.Query<Studente>(@"select * 
        //                                         from alunni 
        //                                         where matricola=@matricola", new {matricola = id}).FirstOrDefault();
        //}

        //public bool InserisciStudente(Studente studente)
        //{
        //    using var con = new MySqlConnection(s);
        //    string query = @"insert into alunni(matricola, nome, classeID, cognome, nomeFile)
        //                     values(@matricola, @nome, @classeID, @cognome, nomeFile)";
        //    var param = new {
        //        matricola = studente.Matricola, 
        //        nome = studente.Nome, 
        //        classeID = studente.ClasseID,
        //        cognome = studente.Cognome,
        //        nomeFile = studente.NomeFile
        //    };
        //    bool esito;

        //    try
        //    {
        //        con.Execute(query, param);
        //        esito = true;
        //    }
        //    catch(Exception ex)
        //    {
        //        esito = false;
        //    }

        //    return esito;
        //}

        //public bool AggiornaVoto(Studente studente)
        //{
        //    using var con = new MySqlConnection(s);

        //    string query = @"UPDATE alunni SET Voto = @Voto WHERE Matricola = @Matricola";
        //    var param = new { Voto = studente.Voto, Matricola = studente.Matricola };

        //    bool esito;
        //    try
        //    {
        //        con.Execute(query, param);
        //        esito = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        esito = false;
        //    }

        //    return esito;
        //}

        //public List<Classe> GetListaClassi()
        //{
        //    using var con = new MySqlConnection(s);
        //    return con.Query<Classe>("select * from classi order by ClasseID").ToList();
        //}
    }
}
