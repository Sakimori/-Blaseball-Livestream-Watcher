﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blaseball_Livestream
{
    public class SaveGame : IComparable<SaveGame>
    {
        //Default constructor
        public SaveGame() 
        {
            inningsList = new List<Inning>();
            awayPlayers = new List<Player>();
            homePlayers = new List<Player>();
            basesOccupied = new List<int>();
        }

        //Constructor with only id
        public SaveGame(string idArg)
        {
            _id = idArg;

            inningsList = new List<Inning>();
            awayPlayers = new List<Player>();
            homePlayers = new List<Player>();
            basesOccupied = new List<int>();
        }

        //Constructor with GameEvent. Initializes score.
        public SaveGame(GameEvent gameEvent)
        {
            awayScore = (int)gameEvent.awayScore;
            homeScore = (int)gameEvent.homeScore;

            if (gameEvent.topOfInning)
            {
                awayPlayers.Add(new Batter(gameEvent.batterId));
                homePlayers.Add(new Pitcher(gameEvent.pitcherId));
            }

            else
            {
                awayPlayers.Add(new Pitcher(gameEvent.pitcherId));
                homePlayers.Add(new Batter(gameEvent.batterId));
            }

            lastPitcher = new Pitcher();
            lastBatter = new Batter();

            inningsList = new List<Inning>();
            awayPlayers = new List<Player>();
            homePlayers = new List<Player>();
            basesOccupied = new List<int>();
        }

        //Constructor with GameEvent. Includes naming teams.
        public SaveGame(GameEvent gameEvent, string updateAwayTeamNickname, string updateHomeTeamNickname)
        {
            _id = gameEvent.gameId;
            awayTeamNickname = updateAwayTeamNickname;
            homeTeamNickname = updateHomeTeamNickname;

            awayScore = (int)gameEvent.awayScore;
            homeScore = (int)gameEvent.homeScore;
            
            if (gameEvent.topOfInning)
            {
                awayPlayers.Add(new Batter(gameEvent.batterId));
                homePlayers.Add(new Pitcher(gameEvent.pitcherId));
            }

            else
            {
                awayPlayers.Add(new Pitcher(gameEvent.pitcherId));
                homePlayers.Add(new Batter(gameEvent.batterId));
            }

            lastPitcher = new Pitcher();
            lastBatter = new Batter();

            inningsList = new List<Inning>();
            awayPlayers = new List<Player>();
            homePlayers = new List<Player>();
            basesOccupied = new List<int>();
        }

        //Constructor with entire game update
        public SaveGame(Game game)
        {
            _id = game._id;
            awayTeamNickname = game.awayTeamNickname;
            homeTeamNickname = game.homeTeamNickname;
            awayTeamID = game.awayTeam;
            homeTeamID = game.homeTeam;

            awayScore = game.awayScore;
            homeScore = game.homeScore;

            season = game.season;
            day = game.day;

            inningsList = new List<Inning>();
            awayPlayers = new List<Player>();
            homePlayers = new List<Player>();
            basesOccupied = new List<int>();

            if (game.topOfInning)
            {
                lastBatter = new Batter(game.awayBatter);
                lastBatter.name = game.awayBatterName;
                awayPlayers.Add(lastBatter);

                lastPitcher = new Pitcher(game.homePitcher);
                lastPitcher.name = game.homePitcherName;
                homePlayers.Add(lastPitcher);
            }
            else
            {
                lastBatter = new Batter(game.homeBatter);
                lastBatter.name = game.homeBatterName;
                homePlayers.Add(lastBatter);

                lastPitcher = new Pitcher(game.awayPitcher);
                lastPitcher.name = game.awayPitcherName;
                awayPlayers.Add(lastPitcher);
            }

            awayBatting = true;
        }

        


        public string _id { get; set; }
        [JsonProperty("inningsList")]
        public List<Inning> inningsList { get; set; }
        public int lastInning { get; set; }
        public string awayTeamNickname { get; set; }
        public string awayTeamID { get; set; }
        public string homeTeamNickname { get; set; }
        public string homeTeamID { get; set; }
        public int awayScore { get; set; }
        public int homeScore { get; set; }
        public int homeHits { get; set; }
        public int awayHits { get; set; }
        public string lastUpdate { get; set; }
        public string thisUpdate { get; set; }
        public bool gameComplete { get; set; }
        public bool topOfInning { get; set; }
        public bool lastTopOfInning { get; set; }
        public bool turnover { get; set; }
        public bool awayBatting { get; set; }
        public List<Player> awayPlayers { get; set; }
        public List<Player> homePlayers { get; set; }
        public List<int> basesOccupied { get; set; }
        public int homeRISP { get; set; }
        public int awayRISP { get; set; }
        public int homeRunsOn2Out { get; set; }
        public int awayRunsOn2Out { get; set; }
        public int homeHitsOn2Out { get; set; }
        public int awayHitsOn2Out { get; set; }
        public int awaySteals { get; set; }
        public int awayCaughtStealing { get; set; }
        public int homeSteals { get; set; }
        public int homeCaughtStealing { get; set; }
        public int season { get; set; } = 0;
        public int day { get; set; } = 0;
        public Pitcher lastPitcher { get; set; }
        public Batter lastBatter { get; set; }

        public override string ToString()
        {
            return string.Concat(awayTeamNickname, " @ ", homeTeamNickname, ", Season ", season.ToString(), " Day ", (day+1).ToString());
        }

        //Update a savegame with a game event
        public void UpdateSaveGame(GameEvent gameEvent)
        {

        }

        //public void DisplaySaveGame()
        //{
        //    string output = string.Concat("")


        //    MessageBox.Show();
        //}

        //Update a savegame with a full new game state
        public void UpdateSaveGame(Game newState)
        {
            String[] hitStrings = { "Single", "Double", "Triple", "home run", "grand slam" };

            if (thisUpdate == newState.lastUpdate) { return; } //check for unnecessary updates

            thisUpdate = newState.lastUpdate;



            //bookkeeping on day, in case it was init'd with GameEvent
            if(season == 0) { season = newState.season; }
            if(day == 0) { day = newState.day; }

            if(lastTopOfInning == newState.topOfInning && lastTopOfInning)
            {
                awayBatting = true;
            }
            else if(lastTopOfInning == newState.topOfInning && !lastTopOfInning)
            {
                awayBatting = false;
            }

            //check for turnover
            if (lastTopOfInning != newState.topOfInning)
            {
                foreach (int runnerNum in basesOccupied)
                {
                    if (runnerNum == 1 || runnerNum == 2)
                    {
                        if (awayBatting) { awayRISP += 1; }
                        else { homeRISP += 1; }
                    }
                }
                turnover = true;
            }
            else { turnover = false; }

            if (thisUpdate.Contains("Bottom of") || thisUpdate.Contains("Top of"))
            {
                if (awayBatting)
                {
                    lastBatter = new Batter(newState.awayBatter);
                    lastBatter.name = newState.awayBatterName;

                    lastPitcher = new Pitcher(newState.homePitcher);
                    lastPitcher.name = newState.homePitcherName;
                }
                else
                {
                    lastBatter = new Batter(newState.homeBatter);
                    lastBatter.name = newState.homeBatterName;

                    lastPitcher = new Pitcher(newState.awayPitcher);
                    lastPitcher.name = newState.awayPitcherName;
                }
                return;
            }

            Inning thisInning = null;

            //find inning we need, init new one if required
            try
            {
                foreach (Inning inning in inningsList)
                {
                    if (inning.number == (newState.inning + 1))
                    {
                        thisInning = inning;
                    }
                }
                if (thisInning == null)
                {
                    thisInning = new Inning();
                    thisInning.number = (newState.inning + 1);
                    inningsList.Add(thisInning);
                }
            }
            catch
            {
                thisInning = new Inning(newState.inning + 1);
                inningsList.Add(thisInning);
            }


            //find batter and pitcher in respective team lists
            Batter batter = null;
            Pitcher pitcher = null;

            if (awayBatting) 
            {
                bool found = false;
                foreach(Player player in awayPlayers)
                {
                    if(player._id == lastBatter._id)
                    {
                        batter = player as Batter;
                        found = true;
                    }
                }
                if (!found)
                {
                    batter = new Batter(lastBatter._id);
                    batter.name = lastBatter.name;
                    awayPlayers.Add(batter);
                }

                found = false;

                foreach (Player player in homePlayers)
                {
                    if (player._id == lastPitcher._id)
                    {
                        pitcher = player as Pitcher;
                        found = true;
                    }
                }
                if (!found)
                {
                    pitcher = new Pitcher(lastPitcher._id);
                    pitcher.name = lastPitcher.name;
                    homePlayers.Add(pitcher);
                }

                lastBatter = new Batter(newState.awayBatter);
                lastBatter.name = newState.awayBatterName;

                lastPitcher = new Pitcher(newState.homePitcher);
                lastPitcher.name = newState.homePitcherName;
            }
            else
            {
                bool found = false;
                foreach (Player player in homePlayers)
                {
                    if (player._id == lastBatter._id)
                    {
                        batter = player as Batter;
                        found = true;
                    }
                }
                if (!found)
                {
                    batter = new Batter(lastBatter._id);
                    batter.name = lastBatter.name;
                    homePlayers.Add(batter);
                }

                found = false;

                foreach (Player player in awayPlayers)
                {
                    if (player._id == lastPitcher._id)
                    {
                        pitcher = player as Pitcher;
                        found = true;
                    }
                }
                if (!found)
                {
                    pitcher = new Pitcher(lastPitcher._id);
                    pitcher.name = lastPitcher.name;
                    awayPlayers.Add(pitcher);
                }

                lastBatter = new Batter(newState.homeBatter);
                lastBatter.name = newState.homeBatterName;

                lastPitcher = new Pitcher(newState.awayPitcher);
                lastPitcher.name = newState.awayPitcherName;
            }
            


            //get any runs scored on play
            int scoredHome = newState.homeScore - homeScore;
            int scoredAway = newState.awayScore - awayScore;
            int scored = scoredAway + scoredHome;
            //and update runs
            homeScore = newState.homeScore;
            awayScore = newState.awayScore;
            thisInning.AddRun(scoredAway, true);
            thisInning.AddRun(scoredHome, false);


            //check for hits
            foreach (string hitString in hitStrings)
            {
                if (thisUpdate.Contains(hitString))
                {
                    batter.AddHit(scored);

                    if (newState.halfInningOuts == 2)
                    {
                        if (lastTopOfInning) { awayHitsOn2Out += 1; awayRunsOn2Out += scored; }
                        else { homeHitsOn2Out += 1; homeRunsOn2Out += scored; }
                    }

                    pitcher.pitchCount += 1;
                    pitcher.hits += 1;
                    if (hitString == "home run" || hitString == "grand slam") { batter.homeRuns += 1; pitcher.homeRuns += 1; }

                    if (lastTopOfInning) { awayHits += 1; }
                    else { homeHits += 1; }
                }       
            }

            //check for out types
            bool gotOut = false;
            if (thisUpdate.Contains("sacrifice")) { batter.AddOut(OutTypes.Sacrifice); batter.rbis += scored; gotOut = true; }
            else if (thisUpdate.Contains("fielder's choice")) { batter.AddOut(OutTypes.FieldersChoice); gotOut = true; }
            else if (thisUpdate.Contains("strikes out") || thisUpdate.Contains("struck out")) 
            { 
                batter.AddOut(OutTypes.Strikeout); gotOut = true;
                pitcher.strikeouts += 1;
            }
            else if (thisUpdate.Contains("ground out")) { batter.AddOut(OutTypes.Groundout); gotOut = true; }
            else if (thisUpdate.Contains("flyout")) { batter.AddOut(OutTypes.Flyout); gotOut = true; }
            else if (thisUpdate.Contains("double play")) { batter.AddOut(OutTypes.DoublePlay); gotOut = true; pitcher.outsRecorded += 1; }
            //add the out to pitcher stats
            if (gotOut) { pitcher.AddOut(); }

            //check for walks
            if(thisUpdate.Contains("draws a walk"))
            {
                batter.walks += 1;
                batter.plateAppearances += 1;
                pitcher.walks += 1;
                pitcher.pitchCount += 1;
            }

            //pitch with no interesting result
            if(thisUpdate.Contains("Ball.") || thisUpdate.Contains("Strike,") || thisUpdate.Contains("Foul Ball.")) { pitcher.pitchCount += 1; }

            //steal attempt
            if(thisUpdate.Contains("caught stealing"))
            {
                if (awayBatting) { awayCaughtStealing += 1; }
                else { homeCaughtStealing += 1; }
            }
            else if (thisUpdate.Contains("steals"))
            {
                if (awayBatting) { awaySteals += 1; }
                else { homeSteals += 1; }
            }


            lastTopOfInning = newState.topOfInning;
            basesOccupied = newState.basesOccupied;
        }

        public int CompareTo(SaveGame other)
        {
            return day.CompareTo(other.day);
        }
    }




}
