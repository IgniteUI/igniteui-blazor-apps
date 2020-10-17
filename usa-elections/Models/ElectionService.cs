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
            var locationCSV = await Http.GetStringAsync("/config/state-locations.csv");
            //Console.WriteLine(locationCSV);
            Console.WriteLine("Locations loading... done ");

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

            Console.WriteLine("Locations parsing... done ");
            await Task.Delay(1);
        }

        public async Task LoadElections()
        {
            Console.WriteLine("Elections loading... ");
            var elections = await Http.GetFromJsonAsync<List<Election>>("/data/elections-2016.json");
            //var elections = await Http.GetFromJsonAsync<Elections[]>("/data/elections-2008-2012.json");
            //var elections = await Http.GetFromJsonAsync<Elections[]>("/data/elections.json");
            Console.WriteLine("Elections loading... done");

            await Parse(elections);
        }
        public async Task Parse(List<Election> elections)
        {
            Console.WriteLine("Elections parsing... " + elections.Count);
            this.IsLoading = true;
             
            foreach (var election in elections)
            {
                //Console.WriteLine("Elections parsing... " + election.Year + " Elections");
                //var electionYear = election.Year;
                var electionStats = "Elections " + election.Year + ": ";
                //var electionStats = electionName;
                 
                var candidates = election.Candidates;
                if (candidates != null)
                {
                    electionStats += candidates.Count + " Candidates, ";
                    //Console.WriteLine(electionName + candidates.Length + " Candidates");
                    for (int i = 0; i < candidates.Count; i++)                     
                    {
                        candidates[i].ID = i;                       
                    }
                }
                //var allVotes = 0;
                //var allElectors = 0;
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
                        #region ResultsByState
                        //if (state.R.Length == 1)
                        //{
                        //    state.R[0].Candidate = candidates[0];
                        //    results = new ResultsByState(state.R[0]);
                        //}
                        //else if (state.R.Length == 2)
                        //{
                        //    state.R[0].Candidate = candidates[0];
                        //    state.R[1].Candidate = candidates[1];
                        //    results = new ResultsByState(state.R[0], state.R[1]);
                        //}
                        //else if (state.R.Length == 3)
                        //{
                        //    state.R[0].Candidate = candidates[0];
                        //    state.R[1].Candidate = candidates[1];
                        //    state.R[2].Candidate = candidates[2];
                        //    results = new ResultsByState(state.R[0], state.R[1], state.R[2]);
                        //}
                        //else if (state.R.Length == 4)
                        //{
                        //    state.R[0].Candidate = candidates[0];
                        //    state.R[1].Candidate = candidates[1];
                        //    state.R[2].Candidate = candidates[2];
                        //    state.R[3].Candidate = candidates[3];
                        //    results = new ResultsByState(state.R[0], state.R[1], state.R[2], state.R[3]);
                        //}
                        //else if (state.R.Length == 5)
                        //{
                        //    state.R[0].Candidate = candidates[0];
                        //    state.R[1].Candidate = candidates[1];
                        //    state.R[2].Candidate = candidates[2];
                        //    state.R[3].Candidate = candidates[3];
                        //    state.R[4].Candidate = candidates[4];
                        //    results = new ResultsByState(state.R[0], state.R[1], state.R[2], state.R[3], state.R[4]);
                        //}
                        //else if (state.R.Length == 6)
                        //{
                        //    state.R[0].Candidate = candidates[0];
                        //    state.R[1].Candidate = candidates[1];
                        //    state.R[2].Candidate = candidates[2];
                        //    state.R[3].Candidate = candidates[3];
                        //    state.R[4].Candidate = candidates[4];
                        //    state.R[5].Candidate = candidates[5];
                        //    results = new ResultsByState(state.R[0], state.R[1], state.R[2], state.R[3], state.R[4], state.R[5]);
                        //    //results.C1 = state.R[0];
                        //    //results.C2 = state.R[1];
                        //}
                        #endregion

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
                        candidates[i].TotalVotesPercent    = candidates[i].TotalVotes * 100.0 / election.TotalVotes;
                        candidates[i].TotalElectorsPercent = candidates[i].TotalElectors * 100.0 / election.TotalElectors;

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

                    //candidates = candidates.SortBy("V", IgniteUI.Blazor.Controls.ListSortDirection.Descending);
                    //foreach (var item in election.ResultsByCandidates)
                    //{
                    //    for (int i = 0; i < candidates.Count; i++)
                    //    { 
                    //        if (item.Candidate.ID == candidates[i].ID)
                    //        {
                    //            //item.VotesTotalPercentage    = candidates[i].TotalVotesPercent;
                    //            //item.ElectorsTotalPercentage = candidates[i].TotalElectorsPercent;
                    //            item.SortIndex = i;
                    //            item.CandidateSort = (i+1) + ". " + item.CandidateName;
                    //            break;
                    //        }
                    //    }
                    //}
                }

                //election.ResultsByCandidate = 
                //    (from item in election.ResultsByCandidate
                //    orderby item.E descending, item.State descending, item.V descending
                //     select item).ToList();


            }

            this.Elections = elections;

            Console.WriteLine("Elections parsing... done");
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
