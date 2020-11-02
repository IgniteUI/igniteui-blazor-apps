using IgniteUI.Blazor.Controls;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace Infragistics.Samples
{
    public enum ElectionDisplayMode
    {
        Electoral = 0,
        Popular = 1,
        Percent = 2,
        None = 4
    }

    public class Election
    {
        // Election's Year, e.g. 2020
        public int Year { get; set; }
        public int Index { get; set; }

        public string Title { get; set; }
        public string HasVotes { get; set; }
        //public Candidate[] Candidates { get; set; }
        public List<Candidate> Candidates { get; set; }

        public StateResults[] States { get; set; }

        public List<ResultsByState> ResultsByStates { get; set; }
        public List<CandidateResult> ResultsByCandidates { get; set; }

        public Candidate Winner { get; set; }
        public Candidate Looser { get; set; }

        public Candidate CandidateLeft { get; set; }
        public Candidate CandidateRight { get; set; }
        public Candidate CandidateOther { get; set; }

        public List<Candidate> StackedCandidates { get; set; }
        public List<Candidate> OtherCandidates { get; set; }
        public List<string> CandidateNames { get; set; }
        public List<string> PartyNames { get; set; }
        public List<string> PartyColors { get; set; }
        public string PartyColorsInString { get; set; }

        public List<Candidate> CandidateOverlap { get; set; }
        //public List<ElectionTreeItem> Tree { get; set; }
        public List<ResultsByState> Tree { get; set; }
        
        public int TotalVotes { get; set; }
        public int TotalElectors { get; set; }

        public double IntervalElectors { get; set; }
        public double IntervalVotes { get; set; }

        public Election()
        {
            TotalVotes = 0;
            TotalElectors = 0;

            ResultsByStates = new List<ResultsByState>();

            Candidates = new List<Candidate>();
            OtherCandidates = new List<Candidate>();

            ResultsByCandidates = new List<CandidateResult>();
        } 

        public void Populate()
        {
            if (this.Candidates == null || this.Candidates.Count < 2)            
                throw new Exception("Election has not enough candidates");

            var tree = new List<ResultsByState>();
            foreach (var state in ResultsByStates)
            {
                //if (state.StateSymbol == "CA")
                //{
                //    state.WinnerVotes = 10;
                //}
                tree.Add(state);
            }
            //tree.AddRange(this.ResultsByStates);
            
            foreach (var candidate in this.Candidates)
            {               
                if (ElectionService.PartyColors.ContainsKey(candidate.Party))
                {
                    candidate.PartyColor = ElectionService.PartyColors[candidate.Party];
                }
                else
                {
                    candidate.PartyColor = "LightGray";
                }

                candidate.PartyDisplay = candidate.Party.Replace("_", " ");
                candidate.PartyDisplay = candidate.PartyDisplay.Replace("1", "");
                candidate.PartyDisplay = candidate.PartyDisplay.Replace("1", "");
                candidate.PartyDisplay = candidate.PartyDisplay.Replace("1", "");

                var treeItem = new ResultsByState();
                treeItem.Winner = candidate;
                treeItem.WinnerParty = null;
                treeItem.WinnerNameAndParty = null;
                treeItem.WinnerName = candidate.Name;
                treeItem.StateSymbol = candidate.NameAndParty;
                treeItem.WinnerElectors = double.NaN;
                treeItem.WinnerVotes = double.NaN;
                treeItem.WinnerPercentage = double.NaN;
                tree.Add(treeItem);
            }

            this.Tree = tree;

            if (this.Candidates[0].TotalElectors > this.Candidates[1].TotalElectors)
            {
                Winner = this.Candidates[0];
                Looser = this.Candidates[1];
            }
            else
            {
                Looser = this.Candidates[0];
                Winner = this.Candidates[1];
            }

            if (Winner.Party == "Republican")
            {
                CandidateLeft  = Looser;
                CandidateRight = Winner;
            }
            else
            {
                CandidateLeft = Winner;
                CandidateRight = Looser;
            }
            Title = Year + " " + CandidateLeft.LastName + " vs " + CandidateRight.LastName;

            CandidateOther = new Candidate();
            CandidateOther.Party = "Other";
            CandidateOther.PartyDisplay = "Other";
            CandidateOther.PartyColor = "#B4B4B4"; 
            CandidateOther.Name = "Other Candidates";
            CandidateOther.LastName = "Other";
            CandidateOther.FirstName = "";

            OtherCandidates = new List<Candidate>();
            foreach (var candidate in this.Candidates)
            {
                //TODO remove
                //if (candidate.Name == "Gary Johnson")
                //    candidate.TotalElectors = 25;
                //if (candidate.Name == "Jill Stein")
                //    candidate.TotalElectors = 15;
                //if (candidate.Party == "Independent")
                //    candidate.TotalElectors = 5;
                
                if (candidate.Name != this.Winner.Name &&
                    candidate.Name != this.Looser.Name)
                {
                    OtherCandidates.Add(candidate);
                    CandidateOther.TotalVotes += candidate.TotalVotes;
                    CandidateOther.TotalVotesMilions += candidate.TotalVotesMilions;
                    CandidateOther.TotalElectors += candidate.TotalElectors;

                    if (candidate.Party == "Tossup")
                    {
                        CandidateOther.Party = "Tossup";
                        CandidateOther.PartyDisplay = "Tossup";
                        CandidateOther.Name = "Tossup States";
                        CandidateOther.LastName = "Tossup"; 
                    }
                }
            }
            CandidateOther.TotalVotesPercent = CandidateOther.TotalVotes * 100.0 / this.TotalVotes;
            CandidateOther.TotalVotesPercent = Math.Round(CandidateOther.TotalVotesPercent * 10) / 10;
            CandidateOther.TotalVotesMilions = Math.Round(CandidateOther.TotalVotesMilions * 10) / 10;
            CandidateOther.TotalElectorsPercent = CandidateOther.TotalElectors * 100.0 / this.TotalElectors;
            CandidateOther.TotalElectorsPercent = Math.Round(CandidateOther.TotalElectorsPercent * 10) / 10;

            CandidateOverlap = new List<Candidate>() { Winner };

            StackedCandidates = new List<Candidate>();
            StackedCandidates.Add(this.CandidateLeft);
            StackedCandidates.Add(this.CandidateOther);

            //foreach (var candidate in this.OtherCandidates)
            //{ 
            //    StackedCandidates.Add(candidate); 
            //} 
            StackedCandidates.Add(this.CandidateRight);

            for (int i = 0; i < this.StackedCandidates.Count; i++)
            {
                var candidate = this.StackedCandidates[i];
                if (i == 0)
                {
                    candidate.Stack = candidate.Clone();
                }
                else
                {
                    var previous = this.StackedCandidates[i - 1];
                    candidate.Stack = candidate.Clone();
                    candidate.Stack.TotalElectors += previous.Stack.TotalElectors;
                    candidate.Stack.TotalVotes += previous.Stack.TotalVotes;
                }
            }

            this.StackedCandidates = (from item in this.StackedCandidates
                                      orderby item.Stack.TotalElectors descending, 
                                      item.Name descending select item).ToList();

            CandidateNames = new List<string>();
            PartyNames = new List<string>();
            PartyColors = new List<string>();
            foreach (var candidate in this.StackedCandidates)
            {
                CandidateNames.Add(candidate.Name);
                PartyNames.Add(candidate.Party);
                PartyColors.Add(candidate.PartyColor);
            }
            PartyColorsInString = string.Join(", ", this.PartyColors);
             
            var midElectors = Math.Round(TotalElectors / 2.0 * 10) / 10;
            var midVotes    = Math.Round(TotalVotes / 2.0 / 1000000) * 1000000;

            IntervalElectors = Math.Round(midElectors / 3.0);

            //var intRoundVotes = Math.Round(TotalVotes / 6.0 / 1000000) * 1000000;
            //var intFloorVotes = Math.Floor(TotalVotes / 6.0 / 1000000) * 1000000;
            //var intPrec = Math.Abs(intRoundVotes - intFloorVotes) / 2.0;
            //intPrec = Math.Floor(intPrec / 100000.0) * 100000.0;
            //IntervalVotes = intFloorVotes + intPrec;
            IntervalVotes    = Math.Round(TotalVotes / 6.0 / 100000) * 100000;
            //IntervalVotes = TotalVotes / 6.0;
            //Logger.WriteLine("\n midVotes =" + midVotes + "\n IntervalVotes =" + IntervalVotes + "\n TotalVotes =" + TotalVotes);
        }

        public List<Candidate> GetCandidates(ElectionDisplayMode displayMode)
        {
            var candidates = new List<Candidate>();
            if (displayMode == ElectionDisplayMode.Electoral)
            {
                foreach (var c in this.StackedCandidates)
                {
                    if (c.TotalElectors > 0)
                        candidates.Add(c);
                }
                candidates = (from item in candidates
                              orderby item.Stack.TotalElectors descending,
                              item.Name descending
                              select item).ToList();
            }
            else
            {
                foreach (var c in this.StackedCandidates)
                {
                    if (c.TotalVotes > 0)
                        candidates.Add(c);
                }
                candidates = (from item in candidates
                              orderby item.Stack.TotalVotes descending,
                              item.Name descending
                              select item).ToList();
            }
            return candidates;
        }

        public List<CandidateResult> SortBy(string field, ListSortDirection direction)
        {
            //Logger.WriteLine("SortBy " + field + " " + direction);

            List<Candidate> sortItems = null;
            if (field == "VotesPerStatePercentage")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in this.Candidates orderby item.TotalVotesPercent descending, item.Name descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in this.Candidates orderby item.TotalVotesPercent ascending, item.Name descending select item).ToList();
            }
            else if (field == "V")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in this.Candidates orderby item.TotalVotes descending, item.Name descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in this.Candidates orderby item.TotalVotes ascending, item.Name descending select item).ToList();
            }
            else if (field == "E")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in this.Candidates orderby item.TotalElectors descending, item.TotalVotes descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in this.Candidates orderby item.TotalElectors ascending, item.TotalVotes descending select item).ToList();
            }
            else if (field == "State")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in this.Candidates orderby item.TotalStates descending, item.TotalVotes descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in this.Candidates orderby item.TotalStates ascending, item.TotalVotes descending select item).ToList();
            }
            else if (field == "CandidateSort")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in this.Candidates orderby item.SortIndex descending, item.Name descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in this.Candidates orderby item.SortIndex ascending, item.Name descending select item).ToList();
            }

            if (sortItems != null)
            {
                for (int i = 0; i < sortItems.Count; i++)
                {
                    sortItems[i].SortIndex = i + 1;

                    //Logger.WriteLine(sortItems[i].SortIndex + " " + sortItems[i].Name + " = " + sortItems[i].TotalStates + ", old: " + Candidates[i].Name);
                }

                foreach (var item in ResultsByCandidates)
                {
                    for (int i = 0; i < sortItems.Count; i++)
                    {
                        if (item.Candidate.ID == sortItems[i].ID)
                        {
                            //item.VotesTotalPercentage    = candidates[i].TotalVotesPercent;
                            //item.ElectorsTotalPercentage = candidates[i].TotalElectorsPercent;
                            item.SortIndex = i;
                            item.CandidateSort = (i + 1) + ". " + item.CandidateName;
                            break;
                        }
                    }
                }
            }

            return ResultsByCandidates;
        }
    }

 
    
    public class Candidate
    {
        public Candidate Clone()
        {
            var ret = new Candidate();
            ret.Party = this.Party;
            ret.PartyColor = this.PartyColor; 

            ret.Name = this.Name;
            ret.TermStart = this.TermStart;
            ret.TermEnd = this.TermEnd;
            ret.Image = this.Image;
            ret.SortIndex = this.SortIndex;
            ret.TotalStates = this.TotalStates;

            ret.TotalElectors = this.TotalElectors;
            ret.TotalElectorsPercent = this.TotalElectorsPercent;

            ret.TotalVotes = this.TotalVotes;
            ret.TotalVotesPercent = this.TotalVotesPercent;
            return ret;
        }
        // Candinate's ID, e.g. 1
        public int ID { get; set; }
        // Candinate's Name 
        public string Name { get; set; }
        // Candinate's Party, e.g. "Democrat"
        public string Party { get; set; }
        public string PartyColor { get; set; }
        public string PartyDisplay { get; set; }
        
        public string Image { get; set; }
        public string TermStart { get; set; }
        public string TermEnd { get; set; }

        public string NameAndParty { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int SortIndex { get; set; }
        public int TotalStates { get; set; }
        public int TotalElectors { get; set; }
        public int TotalVotes { get; set; }
        public double TotalVotesPercent { get; set; }
        public double TotalVotesMilions { get; set; }
        public double TotalElectorsPercent { get; set; }

        public Candidate Stack { get; set; }

        public Candidate()
        {
            SortIndex = -1;

            TotalStates = 0;
            TotalElectors = 0;
            TotalVotes = 0;
            TotalVotesPercent = 0;
            TotalElectorsPercent = 0;
        }
    }
    
    public class StateResults
    {
        // State's Symbol/Code, e.g. "NJ"
        public string S { get; set; }
        public string ID { get; set; }
        // Candidate results in a given state
        //public CandidateResult[] R { get; set; }
        public List<CandidateResult> R { get; set; }

    }

    public static class ElectionUtil
    {        
        public static List<CandidateResult> SortBy(this List<CandidateResult> items, string field, ListSortDirection direction)
        {
            List<CandidateResult> sortItems = null;
            if (field == "VotesPerStatePercentage")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in items orderby item.VotesPerStatePercentage descending, item.State descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in items orderby item.VotesPerStatePercentage ascending, item.State descending select item).ToList();
            }
            else if (field == "V")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in items orderby item.V descending, item.State descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in items orderby item.V ascending, item.State descending select item).ToList();
            }
            else if (field == "E")
            {
                if (direction == ListSortDirection.Descending)
                    sortItems = (from item in items orderby item.E descending, item.State descending select item).ToList();

                if (direction == ListSortDirection.Ascending)
                    sortItems = (from item in items orderby item.E ascending, item.State descending select item).ToList();
            }

            if (sortItems != null)
            {
                for (int i = 0; i < sortItems.Count; i++)
                {
                    sortItems[i].SortIndex = i + 1;
                }
            }

            return sortItems;
        }
    }

    public class CandidateResult
    {
        // Candinate ID, e.g. 1
        public int ID { get; set; }
        // Popular Votes for a candinate, e.g. 1,000
        public int V { get; set; }
        // Electoral Votes for a candinate, e.g. 5
        public int E { get; set; }
        
        public Candidate Candidate { get; set; }

        public string CandidateParty { get { return Candidate == null ? "" : Candidate.Party; } }
        public string CandidateImage { get { return Candidate == null ? "" : Candidate.Image; } }
        public string CandidateColor { get { return Candidate == null ? "" : Candidate.PartyColor; } } 

        public double VotesPerStatePercentage { get; set; }
        public double VotesTotalPercentage { get; set; }

        public double ElectorsPerStatePercentage { get; set; }
        public double ElectorsTotalPercentage { get; set; }

        public int SortIndex { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        //public string CandidateName { get; set; }
        public string CandidateName { get { return Candidate == null ? "" : Candidate.Name; } }

        public string CandidateSort { get; set; }
        //public string CandidateSort { get { return SortIndex + ". " + CandidateName; } }

        public CandidateResult()
        {
            V = -1;
            E = -1;
            State = "??";
            SortIndex = 0;
            //CandidateName = "??";

            VotesPerStatePercentage = 0;
            VotesTotalPercentage = 0;

            ElectorsPerStatePercentage = 0;
            ElectorsTotalPercentage = 0;
        }

        public void Populate()
        {

        }
    }
    
    public class StateTotals
    {
        public int ID { get; set; }
        public int PopularVotes { get; set; }
        public int ElectoralVotes { get; set; }
    }
        
    public class ResultsByCandidate
    {
        //public List<CandidateResult> Data { get; set; }
         
        public Candidate Info { get; set; }
        public int CandidateID { get { return Info.ID; } }
        public string CandidateName { get { return Info.Name; } }

        public string StateSymbol { get; set; }
        public string StateName { get; set; }
        public int    StateCount { get; set; }

        public int Votes { get; set; }
        public int Electors { get; set; }

        public double VotesPercent { get; set; }
        public double ElectorsPercent { get; set; }
        
        public ResultsByCandidate()
        {
            //Data = new List<CandidateResult>();
        }

        //public void Populate()
        //{

        //}

    }

}
