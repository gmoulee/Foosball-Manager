using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {

        private IMongoCollection<Match> mcoll;

        public MatchController()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("FoosballDb");
            mcoll = database.GetCollection<Match>("Matches");
        }
        [HttpGet]
        public ActionResult<List<Match>> Get()
        {
           
            List<Match> list = mcoll.Find(match => true).ToList();
            list.Reverse();
            return list;
        }

        [HttpGet("{id:length(24)}", Name = "GetMatch")]
        public ActionResult<Match> Get(String id)
        {
           return mcoll.Find<Match>(match => match.Id == id).FirstOrDefault();
        }

        [HttpPost]
        public ActionResult<Match> Create()
        {
            Match match = new Match();
            Console.WriteLine("creating new match");
            match.Player1 = match.Player2 = 0;
            match.Set1 = match.Set2 = match.Set3 = match.Winner = null;
            //match.Time = DateTime.Now;
            match.Time = DateTime.Now.ToString("dd MM yyyy HH mm ss");
            mcoll.InsertOne(match);

            return CreatedAtRoute("GetMatch", new { id = match.Id.ToString() }, match);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult<String> Update(string id, String goal)
        {
            
            // mcoll.ReplaceOne(match => match.Id == id, matchIn);
            Match match = mcoll.Find<Match>(matchIn => matchIn.Id == id).FirstOrDefault();
            if(match.Set3 != null)
            {
                return "Match Over";
            }
            if(match == null)
            {
                return "Match Not Found";
            }
            else
            {
                if (goal.Equals("p1"))
                {
                    match.Player1 += 1;                  
                   // UpdateMatch(match);      
                    
                }
                else if(goal.Equals("p2"))
                {
                    match.Player2 += 1;                   
                   // UpdateMatch(match);
                    
                }
                if(match.Player1 == 10 || match.Player2 == 10)
                {
                    CheckWinner(match);
                    match.Player1 = match.Player2 = 0;
                }
                mcoll.ReplaceOne(matchIn => matchIn.Id == id, match);
                return "Successfully Updated";
            }
        }
        
       
        public void CheckWinner(Match match)
        {
            if(match.Player1 == 10)
            {
                if(match.Set1 == null)
                {
                    match.Winner = "player 1";
                    match.Set1 = match.Player1 + "-" + match.Player2;
                }
                else if (match.Set2 == null)
                {
                    match.Winner = "player 1";
                    match.Set2 = match.Player1 + "-" + match.Player2;
                }
                else
                {
                    match.Winner = "player 1";
                    match.Set3 = match.Player1 + "-" + match.Player2;
                }
            }
            else if(match.Player2 == 10)
            {
                if (match.Set1 == null)
                {
                    match.Winner = "player 2";
                    match.Set1 = match.Player1 + "-" + match.Player2;
                }
                else if (match.Set2 == null)
                {
                    match.Winner = "player 2";
                    match.Set2 = match.Player1 + "-" + match.Player2;
                }
                else
                {
                    match.Winner = "player 2";
                    match.Set3 = match.Player1 + "-" + match.Player2;
                }
            }
        }

        public void UpdateMatch(Match match)
        {
            if (match.Set1.Equals(null))
            {
                match.Set1 = match.Player1.ToString() + "-" + match.Player2.ToString();
                
            }
            else if (match.Set2.Equals(null))
            {
                match.Set2 = match.Player1.ToString() + "-" + match.Player2.ToString();
            }
            else if(match.Set3.Equals(null) && match.Winner.Equals(null))
            {
                match.Set3 = match.Player1.ToString() + "-" + match.Player2.ToString();
            }
         
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult<String> Delete(string id)
        {
            
            mcoll.DeleteOne(match => match.Id == id);
            return "Successfully Deleted";
        }
    }

}
