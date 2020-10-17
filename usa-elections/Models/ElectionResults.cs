using IgniteUI.Blazor.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infragistics.Samples
{
    public class Election
    {
        // Election's Year, e.g. 2020
        public int Year { get; set; }

        public string HasVotes { get; set; }
        //public Candidate[] Candidates { get; set; }
        public List<Candidate> Candidates { get; set; }

        public StateResults[] States { get; set; }
        
        public List<ResultsByState> ResultsByStates { get; set; }
        public List<CandidateResult> ResultsByCandidates { get; set; }

        public int TotalVotes { get; set; }
        public int TotalElectors { get; set; }

        public Election()
        {
            TotalVotes = 0;
            TotalElectors = 0;

            ResultsByStates = new List<ResultsByState>();

            ResultsByCandidates = new List<CandidateResult>();
        }

        public List<CandidateResult> SortBy(string field, ListSortDirection direction)
        {
            //Console.WriteLine("SortBy " + field + " " + direction);

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

                    //Console.WriteLine(sortItems[i].SortIndex + " " + sortItems[i].Name + " = " + sortItems[i].TotalStates + ", old: " + Candidates[i].Name);
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
        // Candinate's ID, e.g. 1
        public int ID { get; set; }
        // Candinate's Name 
        public string Name { get; set; }
        // Candinate's Party, e.g. "Democrat"
        public string Party { get; set; }
        public string Image { get; set; }
        public string TermStart { get; set; }
        public string TermEnd { get; set; }

        public int SortIndex { get; set; }
        public int TotalStates { get; set; }
        public int TotalElectors { get; set; }
        public int TotalVotes { get; set; }
        public double TotalVotesPercent { get; set; }
        public double TotalElectorsPercent { get; set; }

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

        public double VotesPerStatePercentage { get; set; }
        public double VotesTotalPercentage { get; set; }

        public double ElectorsPerStatePercentage { get; set; }
        public double ElectorsTotalPercentage { get; set; }

        public int SortIndex { get; set; }
        public string State { get; set; }
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

    public class ResultsByState
    {
        public CandidateResult C1 { get; set; }
        public CandidateResult C2 { get; set; }

        public ResultsByState()
        {
            C1PopularVotes = 0;
            C1ElectoralVotes = 0;

            C2PopularVotes = 0;
            C2ElectoralVotes = 0;

            C3PopularVotes = 0;
            C3ElectoralVotes = 0;

            C4PopularVotes = 0;
            C4ElectoralVotes = 0;

            C5PopularVotes = 0;
            C5ElectoralVotes = 0;

            C6PopularVotes = 0;
            C6ElectoralVotes = 0;

            OtherName = "Other";
            OtherPopularVotes = 0;
            OtherElectoralVotes = 0;
        }

        #region ResultsByState
        //public ResultsByState(CandidateResult c1)
        //   : this(c1, null, null, null, null, null)
        //{ }

        //public ResultsByState(CandidateResult c1, CandidateResult c2)
        //    : this(c1, c2, null, null, null, null)
        //{ }

        //public ResultsByState(CandidateResult c1, CandidateResult c2, CandidateResult c3)
        //    : this(c1, c2, c3, null, null, null)
        //{ }

        //public ResultsByState(CandidateResult c1, CandidateResult c2, CandidateResult c3, CandidateResult c4)
        //    : this(c1, c2, c3, c4, null, null)
        //{ }

        //public ResultsByState(CandidateResult c1, CandidateResult c2, CandidateResult c3, CandidateResult c4, CandidateResult c5)
        //    : this(c1, c2, c3, c4, c5, null)
        //{ }

        //public ResultsByState(CandidateResult c1, CandidateResult c2, CandidateResult c3, CandidateResult c4, CandidateResult c5, CandidateResult c6)
        //    : this(new List<CandidateResult> { c1, c2, c3, c4, c5, c6 })
        //{ }
        #endregion

        public ResultsByState(List<CandidateResult> candidates)
        {
            //C1 = c1;
            //C2 = c2;
            C1 = new CandidateResult();
            C2 = new CandidateResult();

            OtherName = "Other";
            OtherPopularVotes = 0;
            OtherElectoralVotes = 0;
            OtherPercentVotes = 0;

            if (candidates.Count < 2) return;

            var c1 = candidates[0];
            C1Name = c1.Candidate.Name;
            C1PopularVotes = c1.V;
            C1ElectoralVotes = c1.E;

            var c2 = candidates[1];
            C2Name = c2.Candidate.Name;
            C2PopularVotes = c2.V;
            C2ElectoralVotes = c2.E;

            if (c1.E >= c2.E)
            { 
                this.WinnerCandidate    = c1.Candidate.Name;
                this.WinnerParty        = c1.Candidate.Party;
                this.WinnerVotes        = c1.V;
                this.WinnerElectors     = c1.E;
                this.WinnerPercentage   = c1.VotesPerStatePercentage;

                this.RunnerUpCandidate  = c2.Candidate.Name;
                this.RunnerUpParty      = c2.Candidate.Party;
                this.RunnerUpVotes      = c2.V;
                this.RunnerUpElectors   = c2.E;
                this.RunnerUpPercentage = c2.VotesPerStatePercentage;
            }
            else
            {
                this.WinnerCandidate    = c2.Candidate.Name;
                this.WinnerParty        = c2.Candidate.Party;
                this.WinnerVotes        = c2.V;
                this.WinnerElectors     = c2.E;
                this.WinnerPercentage   = c2.VotesPerStatePercentage;

                this.RunnerUpCandidate  = c1.Candidate.Name;
                this.RunnerUpParty      = c1.Candidate.Party;
                this.RunnerUpVotes      = c1.V;
                this.RunnerUpElectors   = c1.E;
                this.RunnerUpPercentage = c1.VotesPerStatePercentage;
            }

            //C2Name = c1 == null ? "" : c2.Candidate.Name;
            //C2PopularVotes = c2 == null ? 0 : c2.V;
            //C2ElectoralVotes = c2 == null ? 0 : c2.E;

            //C3Name = c1 == null ? "" : c3.Candidate.Name;
            //C3PopularVotes = c3 == null ? 0 : c3.V;
            //C3ElectoralVotes = c3 == null ? 0 : c3.E;

            //C4Name = c1 == null ? "" : c4.Candidate.Name;
            //C4PopularVotes = c4 == null ? 0 : c4.V;
            //C4ElectoralVotes = c4 == null ? 0 : c5.E;

            //C5Name = c1 == null ? "" : c5.Candidate.Name;
            //C5PopularVotes = c5 == null ? 0 : c5.V;
            //C5ElectoralVotes = c5 == null ? 0 : c5.E;

            //C6Name = c1 == null ? "" : c6.Candidate.Name;
            //C6PopularVotes = c6 == null ? 0 : c6.V;
            //C6ElectoralVotes = c6 == null ? 0 : c6.E;

            //C1PercentVotes = c1 == null ? 0 : c1.VotesPerStatePercentage;
            //C2PercentVotes = c2 == null ? 0 : c2.VotesPerStatePercentage;
            //C3PercentVotes = c3 == null ? 0 : c3.VotesPerStatePercentage;
            //C4PercentVotes = c4 == null ? 0 : c4.VotesPerStatePercentage;
            //C5PercentVotes = c5 == null ? 0 : c5.VotesPerStatePercentage;
            //C6PercentVotes = c6 == null ? 0 : c6.VotesPerStatePercentage;
        }

        public int TotalVotes { get { return WinnerVotes; } }
        public int TotalElectors { get { return WinnerElectors; } }
         
        public bool HasPartialWinner { get; set; }
        public string WinnerCandidate { get; set; }
        public string WinnerParty { get; set; }
        public int    WinnerVotes { get; set; }
        public int    WinnerElectors { get; set; }
        public double WinnerPercentage { get; set; }

        public string RunnerUpCandidate { get; set; }
        public string RunnerUpParty { get; set; }
        public int    RunnerUpVotes { get; set; }
        public int    RunnerUpElectors { get; set; }
        public double RunnerUpPercentage { get; set; } 
        
        public string StateSymbol { get; set; }
        public string StateName { get; set; }
        public bool StateHeldElections { get; set; }

        public double StateLocationX { get; set; }
        public double StateLocationY { get; set; }

        public double StateCenterX { get; set; }
        public double StateCenterY { get; set; }
        public bool StateHasLabelBox { get; set; }

        public double C1PercentVotes { get; set; }
        public double C2PercentVotes { get; set; }
        public double C3PercentVotes { get; set; }
        public double C4PercentVotes { get; set; }
        public double C5PercentVotes { get; set; }
        public double C6PercentVotes { get; set; }
        public double OtherPercentVotes { get; set; }

        // 1st candinate's name, Popular votes and Electoral Votes
        public string C1Name { get; set; }
        public    int C1PopularVotes { get; set; } 
        public    int C1ElectoralVotes { get; set; }

        public string C2Name { get; set; }
        public    int C2PopularVotes { get; set; }
        public    int C2ElectoralVotes { get; set; }
        
        public string OtherName { get; set; }
        public    int OtherPopularVotes { get; set; } 
        public    int OtherElectoralVotes { get; set; }

        public string C3Name { get; set; }
        public    int C3PopularVotes { get; set; }
        public    int C3ElectoralVotes { get; set; }
        
        public string C4Name { get; set; }
        public    int C4PopularVotes { get; set; }
        public    int C4ElectoralVotes { get; set; }

        public string C5Name { get; set; }
        public    int C5PopularVotes { get; set; }
        public    int C5ElectoralVotes { get; set; }
        
        public string C6Name { get; set; }
        public    int C6PopularVotes { get; set; }
        public    int C6ElectoralVotes { get; set; }
    }
}
