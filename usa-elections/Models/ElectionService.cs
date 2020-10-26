using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infragistics.Samples
{
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
        public int CreationYear { get; set; }
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
            PartyColors.Add("Democrat",             "#008DFF");
            PartyColors.Add("Democrat-Leaning",     "#4BADFC");
            PartyColors.Add("Republican",           "#FF0808");
            PartyColors.Add("Republican-Leaning",   "#F47575");
            PartyColors.Add("NationalRepublican", "#8E5EE9"); // 1832
            PartyColors.Add("DemocraticRepublican_1", "#8E5EE9"); //1824
            PartyColors.Add("DemocraticRepublican_2", "#8E5EE9"); // 1824
            PartyColors.Add("DemocraticRepublican_3", "#8E5EE9"); // 1824
            PartyColors.Add("DemocraticRepublican_4", "#8E5EE9"); // 1824
           
            PartyColors.Add("Libertarian",          "#F7AC22");
            PartyColors.Add("Green",               "#0CDE23");
            PartyColors.Add("Independent",          "#8E5EE9");
            PartyColors.Add("Other",                "#C6C6C6");
            //TODO set colors
            PartyColors.Add("Reform", "#8E5EE9");
            PartyColors.Add("AmericanInd", "#8E5EE9"); // 1968
            PartyColors.Add("StatesRights", "#8E5EE9"); // 1948
            PartyColors.Add("Progressive", "#8E5EE9"); // 1948
            PartyColors.Add("Socialist", "#8E5EE9"); // 1932
            PartyColors.Add("Populist", "#8E5EE9"); // 1892
            PartyColors.Add("Prohibition", "#8E5EE9"); // 1892
            PartyColors.Add("Greenback", "#0CDE23"); // 1880
            PartyColors.Add("SouthernDemocrat", "#8E5EE9"); // 1860
            PartyColors.Add("ConstitutionalUnion", "#8E5EE9"); // 1860
            PartyColors.Add("WhigAmerican", "#8E5EE9"); // 1856
            PartyColors.Add("Whig", "#8E5EE9"); // 1852
            PartyColors.Add("FreeSoil", "#8E5EE9"); // 1852
            PartyColors.Add("Liberty", "#8E5EE9"); // 1844
            PartyColors.Add("AntiMasonic", "#8E5EE9"); // 1832

            PartyColors.Add("Federalist_1", "#8E5EE9"); // 1804
            PartyColors.Add("Federalist_2", "#8E5EE9"); // 1804
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
            StateLocations = new Dictionary<string, StateLocation>();

            Console.WriteLine("Locations loading... ");
            var locationCSV = await Http.GetStringAsync("config/state-locations.csv");
            //Console.WriteLine(locationCSV);
            //Console.WriteLine("Locations loading... done ");

            Console.WriteLine("Locations parsing... ");
            var locationRows = locationCSV.Split('\n');
            var offsetRows = Math.Round(locationRows.Length / 2.0);
            for (int r = 1; r < locationRows.Length; r++)
            {
                var locationColumns = locationRows[r].Split(',');
                var offsetColumns = Math.Round(locationColumns.Length / 2.0);
                //if (r == 1)
                //    Console.WriteLine("Locations columns=" + locationColumns.Length);

                var loc = r + ", ";
                for (int c = 1; c < locationColumns.Length; c++)
                {
                    var id = locationColumns[c].Trim();
                    if (id != "")
                    {
                        //Console.WriteLine("Locations r=" + r + " y=" + y + " id=" + id);
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
                //Console.WriteLine(loc);

            }

            //Console.WriteLine("Locations parsing... done ");
            await Task.Delay(1);
        }

        public async Task LoadElections()
        {
            Console.WriteLine("Elections loading... ");
            var elections = await Http.GetFromJsonAsync<List<Election>>("data/elections-2016.json");
            //var elections = await Http.GetFromJsonAsync<Elections[]>("/data/elections-2008-2012.json");
            //var elections = await Http.GetFromJsonAsync<Elections[]>("/data/elections.json");
            //Console.WriteLine("Elections loading... done");

            await Parse(elections);
        }
        public async Task Parse(List<Election> elections)
        {
            Console.WriteLine("Elections parsing... " + elections.Count);
            this.IsLoading = true;
             
            foreach (var election in elections)
            {
                //Console.WriteLine("Elections parsing... " + election.Year + " Elections");
                var electionStats = "Elections " + election.Year + ": ";
                  
                var candidates = election.Candidates;
                if (candidates != null)
                {
                    electionStats += candidates.Count + " Candidates, ";
                    //Console.WriteLine(electionName + candidates.Length + " Candidates");
                    for (int i = 0; i < candidates.Count; i++)                     
                    {
                        candidates[i].ID = i;
                        var names = candidates[i].Name.Split(" ");
                        candidates[i].NameAndParty = names[1] + " (" + candidates[i].Party + ")";
                    }
                } 

                var states = election.States;
                if (states != null)
                {
                    electionStats += states.Length + " States, ";
                    //Console.WriteLine(electionName + states.Length + " States");
                    foreach (var state in states)
                    {
                        var otherVotes = 0;
                        var otherElectors = 0;

                        var statesVotes = 0;
                        var statesElectors = 0;

                        foreach (var candidate in state.R)
                        {
                            election.TotalVotes += candidate.V;
                            election.TotalElectors += candidate.E;

                            statesVotes += candidate.V;
                            statesElectors += candidate.E;
                        }

                        for (int i = 0; i < state.R.Count; i++)
                        {
                            var candidateID = state.R[i].ID - 1;

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

                        if (state.R.Count == candidates.Count)
                        {
                            for (int i = 0; i < state.R.Count; i++)
                            {
                                state.R[i].Candidate = candidates[i];
                            }
                        }

                        ResultsByState results = new ResultsByState(state.R);
                        
                        if (results != null)
                        {
                            results.StateSymbol = state.S;
                            results.OtherPopularVotes = otherVotes;
                            results.OtherElectoralVotes = otherElectors;
                            results.OtherPercentVotes = results.OtherPopularVotes * 100.0 / statesVotes;
                            
                            //Console.WriteLine("Elections locating " + results.StateSymbol);
                            if (StateLocations.ContainsKey(results.StateSymbol))
                            {
                                var location = StateLocations[results.StateSymbol];
                                results.StateLocationX = location.X;
                                results.StateLocationY = location.Y;
                            }

                            election.ResultsByStates.Add(results);
                        }

                        //foreach (var results in state.R)
                        //{
                        //    election.Summary.PopularVotes += results.V;
                        //    election.Summary.ElectoralVotes += results.E;
                        //}
                    }

                    electionStats += election.TotalElectors + " Electors, ";
                    electionStats += election.TotalVotes + " Votes, ";
                    //Console.WriteLine(electionName + election.Summary.PopularVotes + " Popular Votes");
                    //Console.WriteLine(electionName + election.Summary.ElectoralVotes + " Electoral Votes");
                    
                    Console.WriteLine(electionStats);
                    //electionStats += "\n";
                    for (int i = 0; i < candidates.Count; i++)
                    {
                        candidates[i].TotalVotesPercent = candidates[i].TotalVotes * 100.0 / election.TotalVotes;
                        candidates[i].TotalVotesPercent = Math.Round(candidates[i].TotalVotesPercent * 10) / 10;

                        candidates[i].TotalVotesMilions = candidates[i].TotalVotes / 1000000.0;
                        candidates[i].TotalVotesMilions = Math.Round(candidates[i].TotalVotesMilions * 10) / 10;

                        candidates[i].TotalElectorsPercent = candidates[i].TotalElectors * 100.0 / election.TotalElectors;
                        candidates[i].TotalElectorsPercent = Math.Round(candidates[i].TotalElectorsPercent * 10) / 10;

                        //Console.WriteLine(candidates[i].TotalVotesPercent + " " + candidates[i].Name + " " + candidates[i].TotalVotes);
                    }

                    foreach (var item in election.ResultsByCandidates)
                    {
                        item.VotesTotalPercentage = item.V * 100.0 / election.TotalVotes;
                        item.ElectorsTotalPercentage = item.E * 100.0 / election.TotalElectors;
                    }

                    election.SortBy("V", IgniteUI.Blazor.Controls.ListSortDirection.Descending);

                    election.ResultsByCandidates = (from item in election.ResultsByCandidates orderby 
                                                    item.SortIndex ascending, item.CandidateName descending select item).ToList();

                
                }

                Candidate electionWinner = null;
                Candidate electionRunnerUp = null;


                election.Populate();

            }

            this.Elections = elections;

            //Console.WriteLine("Elections parsing... done");
            OnLoaded();
            await Task.Delay(1);
        }

        public event EventHandler<EventArgs> SamplesLoaded;
        public void OnLoaded()
        {
            this.IsLoading = false;

            this.SamplesLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
