using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infragistics.Samples
{
    public static class Logger  
    {
        public static void WriteLine(string msg)
        {
            // Console.WriteLine(msg);
        }
    }

    public class ObservableObject
    {
        public event Action OnChange;
        protected void NotifyPropertyChanged() => OnChange?.Invoke();
    }

    public class StateInfo
    {
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int Statehood { get; set; }
        public int Population { get; set; }
        public bool StateHasLabelBox { get; set; }
        //public bool Statehood { get; set; }
    }

    public class ElectionService : ObservableObject
    {
        public static Dictionary<string, StateLocation> StateLocations { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        public List<Election> Elections { get; set; }

        public static Dictionary<string, string> PartyColors { get; set; }
          
        static ElectionService()
        {           
            PartyColors = new Dictionary<string, string>();
            //PartyColors.Add("Democrat",             "#008DFF");
            //PartyColors.Add("Democrat-Leaning",     "#4BADFC");
            //PartyColors.Add("Republican",           "#FF0808");
            //PartyColors.Add("Republican-Leaning",   "#F47575");
            PartyColors.Add("Tossup",               "#959494");
            PartyColors.Add("Democrat",             "#465F98");
            PartyColors.Add("Democrat-Leaning",     "#5CAADE");
            PartyColors.Add("Republican",           "#CD433C");
            PartyColors.Add("Republican-Leaning",   "#F47575");
            //PartyColors.Add("Democrat",             "#5885EC");
            //PartyColors.Add("Democrat-Leaning",     "#4BADFC");
            //PartyColors.Add("Republican",           "#DE5E58");
            //PartyColors.Add("Republican-Leaning",   "#F47575");
            PartyColors.Add("NationalRepublican",      "#CD433C"); // 1832
            PartyColors.Add("Democratic_Republican_1", "#7A3E98"); // 1824
            PartyColors.Add("Democratic_Republican_2", "#465F98"); // 1824
            PartyColors.Add("Democratic_Republican_3", "#CD433C"); // 1824
            PartyColors.Add("Democratic_Republican_4", "#FD9B50"); // 1824

            // #CD433C #99222A #74254C #E7768F #C453C4
            // #465F98 #5CAADE #244C7D #8ACBE3 #5559AA
            // #FD9B50 #EAC451 #7A3E98 #69AA67 

            PartyColors.Add("Libertarian",          "#FD9B50");
            PartyColors.Add("Green",               "#69AA67");
            PartyColors.Add("Independent",          "#7A3E98");
            PartyColors.Add("Other",                "#B4B4B4");
           
            PartyColors.Add("Reform", "#B4B4B4");
            PartyColors.Add("StatesRights", "#FD9B50"); // 1948
            PartyColors.Add("Progressive", "#69AA67"); // 1948
            PartyColors.Add("Socialist", "#99222A"); // 1932
            PartyColors.Add("Populist", "#FD9B50"); // 1892
            PartyColors.Add("Prohibition", "#69AA67"); // 1892
            PartyColors.Add("Greenback", "#69AA67"); // 1880
            PartyColors.Add("Southern_Democrat", "#465F98"); // 1860
            PartyColors.Add("Constitutional_Union", "#FD9B50"); // 1860
            PartyColors.Add("Whig", "#7A3E98"); // 1852
            PartyColors.Add("FreeSoil", "#FD9B50"); // 1852
            PartyColors.Add("Liberty", "#FD9B50"); // 1844
            PartyColors.Add("Anti_Masonic", "#FD9B50"); // 1832
            PartyColors.Add("Federalist_1", "#69AA67"); // 1804
            PartyColors.Add("Federalist_2", "#7A3E98"); // 1804

            //PartyColors.Add("AmericanInd", "#B4B4B4"); // 1968
            //PartyColors.Add("WhigAmerican", "#7A3E98"); // 1856
        }

        private bool _IsLoading = true;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; NotifyPropertyChanged(); }
        }

        public async Task Load()
        {
            //if (this.Http == null)
            //    throw new Exception("ElectionService has no Http client setting");
            
            await LoadStateLocations();
            await LoadElections();
            await Task.Delay(1);
        }
        
        public async Task LoadStateLocations()
        {
            if (StateLocations == null)
            {
                StateLocations = new Dictionary<string, StateLocation>();

                Logger.WriteLine("Locations loading... ");

                var locationCSV = await Http.GetStringAsync("config/state-locations.csv");
                //Logger.WriteLine(locationCSV);
                //Logger.WriteLine("Locations loading... done ");

                Logger.WriteLine("Locations parsing... ");
                var locationRows = locationCSV.Split('\n');
                var offsetRows = Math.Round(locationRows.Length / 2.0);
                for (int r = 1; r < locationRows.Length; r++)
                {
                    var locationColumns = locationRows[r].Split(',');
                    var offsetColumns = Math.Round(locationColumns.Length / 2.0);
                    //if (r == 1)
                    //    Logger.WriteLine("Locations columns=" + locationColumns.Length);

                    var loc = r + ", ";
                    for (int c = 1; c < locationColumns.Length; c++)
                    {
                        var id = locationColumns[c].Trim();
                        if (id != "")
                        {
                            //Logger.WriteLine("Locations r=" + r + " y=" + y + " id=" + id);
                            if (!StateLocations.ContainsKey(id))
                            {
                                var x = (c - offsetColumns);
                                var y = (r - offsetRows) * -1;
                                StateLocations.Add(id, new StateLocation(id, x, y));
                                loc += "" + r + c + ", ";
                            }
                            else
                                throw new Exception("Duplicated ID in state-locations.csv");
                        }
                        else
                        {
                            loc += "  , ";
                        }
                        //var locationColumns = locationRows[r].Split(',');
                    }
                    //Logger.WriteLine(loc);
                }
            }

            //Logger.WriteLine("Locations parsing... done ");
            await Task.Delay(1);
        }

        public async Task LoadElections()
        {
            this.IsLoading = true;

            Logger.WriteLine("Elections loading... ");
            //var elections = await Http.GetFromJsonAsync<Elections[]>("data/elections-2008-2012.json");
            //var elections = await Http.GetFromJsonAsync<List<Election>>("data/elections-2012.json");
            //var elections = await Http.GetFromJsonAsync<List<Election>>("data/elections-2016.json");
            var elections = await Http.GetFromJsonAsync<List<Election>>("data/elections-2012-2016.json");
            //var elections = await Http.GetFromJsonAsync<List<Election>>("data/elections.json");

            await Parse(elections);
        }

        public async Task Parse(List<Election> elections)
        {
            Console.WriteLine("Elections parsing... " + elections.Count);
             
            foreach (var election in elections)
            {
                var electionID = "Elections " + election.Year + " ";
                var electionStats = electionID;
                //Console.WriteLine(electionID + "parsing");

                var candidates = election.Candidates;
                if (candidates != null)
                {
                    electionStats += candidates.Count + " Candidates, ";
                    //Logger.WriteLine(electionName + candidates.Length + " Candidates");
                    for (int i = 0; i < candidates.Count; i++)                     
                    {
                        var names = candidates[i].Name.Split(" ");
                        candidates[i].ID = i;
                        candidates[i].LastName = names[names.Length - 1];
                        candidates[i].FirstName = candidates[i].Name.Replace(" " + candidates[i].LastName, "");
                        candidates[i].NameAndParty = candidates[i].LastName + " (" + candidates[i].Party + ")";
                    }
                }

                //Console.WriteLine(electionID + "states=" + election.States.Length);
                var states = election.States;

                if (states[0].R.Count != candidates.Count)
                {
                    throw new Exception(electionID + "has states[0].R.Count != candidates.Count");
                }

                if (states != null)
                {
                    electionStats += states.Length + " States, ";
                    //Logger.WriteLine(electionName + states.Length + " States");
                    foreach (var state in states)
                    {
                        var otherVotes = 0;
                        var otherElectors = 0;

                        var statesVotes = 0;
                        var statesElectors = 0;

                        var stateLog = electionID + " " + state.S;

                        foreach (var candidate in state.R)
                        {
                            election.TotalVotes += candidate.V;
                            election.TotalElectors += candidate.E;

                            statesVotes += candidate.V;
                            statesElectors += candidate.E;
                        }

                        //Console.WriteLine(stateLog + " stats: E=" + statesElectors + " V=" + statesVotes + " C=" + state.R.Count);

                        for (int i = 0; i < state.R.Count; i++)
                        {
                            var candidateID = state.R[i].ID - 1;

                            //Console.WriteLine(stateLog + " candidate: " + candidateID );

                            state.R[i].State = state.S;
                            state.R[i].Candidate = candidates[candidateID];
                            //state.R[i].CandidateName = candidates[candidateID].Name;
                            state.R[i].VotesPerStatePercentage = (state.R[i].V * 100.0 / statesVotes);
                            state.R[i].ElectorsPerStatePercentage = (state.R[i].E * 100.0 / statesElectors);

                            var stateResults = state.R[i];
                            candidates[candidateID].TotalElectors += stateResults.E;
                            candidates[candidateID].TotalVotes += stateResults.V;
                            if (stateResults.E > 0 || stateResults.V > 0)
                            {
                                candidates[candidateID].TotalStates += 1;
                            }

                            election.ResultsByCandidates.Add(state.R[i]);

                            if (i >= 2) // non-primary candidates
                            {
                                otherVotes += stateResults.V;
                                otherElectors += stateResults.E;
                            }
                        }

                        //Console.WriteLine(stateLog + " candidates: " + election.ResultsByCandidates.Count);

                        if (state.R.Count == candidates.Count)
                        {
                            for (int i = 0; i < state.R.Count; i++)
                            {
                                state.R[i].Candidate = candidates[i];
                            }
                        }
                        //else
                        //{
                        //    Console.WriteLine(electionID + " candidates != results: " + candidates.Count + " " + state.R.Count);
                        //}

                        var results = new ResultsByState(state.R);
                        
                        if (results != null)
                        {
                            results.StateSymbol = state.S;
                            results.OtherPopularVotes = otherVotes;
                            results.OtherElectoralVotes = otherElectors;
                            results.OtherPercentVotes = results.OtherPopularVotes * 100.0 / statesVotes;
                            
                            //Logger.WriteLine("Elections locating " + results.StateSymbol);
                            if (StateLocations.ContainsKey(results.StateSymbol))
                            {
                                var location = StateLocations[results.StateSymbol];
                                results.StateLocationX = location.X;
                                results.StateLocationY = location.Y;
                            }
                            else
                            {
                                throw new Exception(electionID + "has unknown StateSymbol=" + results.StateSymbol);
                            }

                            election.ResultsByStates.Add(results);
                        }

                        //foreach (var results in state.R)
                        //{
                        //    election.Summary.PopularVotes += results.V;
                        //    election.Summary.ElectoralVotes += results.E;
                        //}
                    }

                    //Console.WriteLine(electionID + " ResultsByStates " + election.ResultsByStates.Count);

                    //Logger.WriteLine(electionName + election.Summary.PopularVotes + " Popular Votes");
                    //Logger.WriteLine(electionName + election.Summary.ElectoralVotes + " Electoral Votes");
                    
                    //electionStats += "\n";
                    for (int i = 0; i < candidates.Count; i++)
                    {
                        candidates[i].TotalVotesPercent = candidates[i].TotalVotes * 100.0 / election.TotalVotes;
                        candidates[i].TotalVotesPercent = Math.Round(candidates[i].TotalVotesPercent * 10) / 10;

                        candidates[i].TotalVotesMilions = candidates[i].TotalVotes / 1000000.0;
                        candidates[i].TotalVotesMilions = Math.Round(candidates[i].TotalVotesMilions * 10) / 10;

                        candidates[i].TotalElectorsPercent = candidates[i].TotalElectors * 100.0 / election.TotalElectors;
                        candidates[i].TotalElectorsPercent = Math.Round(candidates[i].TotalElectorsPercent * 10) / 10;

                        //Logger.WriteLine(candidates[i].TotalVotesPercent + " " + candidates[i].Name + " " + candidates[i].TotalVotes);
                    }

                    foreach (var item in election.ResultsByCandidates)
                    {
                        item.VotesTotalPercentage = item.V * 100.0 / election.TotalVotes;
                        item.ElectorsTotalPercentage = item.E * 100.0 / election.TotalElectors;
                    }

                    //Console.WriteLine(electionID + " Sort Candidates");

                    election.SortBy("V", IgniteUI.Blazor.Controls.ListSortDirection.Descending);

                    //Console.WriteLine(electionID + " Sort ResultsByCandidates");

                    election.ResultsByCandidates = (from item in election.ResultsByCandidates orderby 
                                                    item.SortIndex ascending, item.CandidateName descending select item).ToList();

                    
                }

                //Console.WriteLine(electionID + "populating");
                election.Populate();

                electionStats += election.TotalElectors + " Electors, ";
                electionStats += election.TotalVotes + " Votes, ";
                //Console.WriteLine(electionStats);
            }

            for (int i = 0; i < elections.Count; i++)
            {
                elections[i].Index = i;
            }

            Console.WriteLine("Elections parsing... done");
            this.Elections = elections;

            OnLoaded();
            await Task.Delay(1);
        }

        public event EventHandler<EventArgs> Loaded;

        public void OnLoaded()
        {
            this.IsLoading = false;

            this.Loaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
