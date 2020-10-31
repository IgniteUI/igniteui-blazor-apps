using System;
using System.Collections.Generic;
using System.Linq; 

namespace Infragistics.Samples
{
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
         
        public ResultsByState(List<CandidateResult> candidates)
        {
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
                this.Winner = c1.Candidate;
                this.WinnerName = c1.Candidate.Name;
                this.WinnerNameAndParty = c1.Candidate.NameAndParty;
                this.WinnerParty = c1.Candidate.Party;
                this.WinnerVotes = c1.V;
                this.WinnerElectors = c1.E;
                this.WinnerPercentage = c1.VotesPerStatePercentage;

                this.RunnerUp = c2.Candidate;
                this.RunnerUpName = c2.Candidate.Name;
                this.RunnerUpNameAndParty = c2.Candidate.NameAndParty;
                this.RunnerUpParty = c2.Candidate.Party;
                this.RunnerUpVotes = c2.V;
                this.RunnerUpElectors = c2.E;
                this.RunnerUpPercentage = c2.VotesPerStatePercentage;
            }
            else
            {
                this.Winner = c2.Candidate;
                this.WinnerName = c2.Candidate.Name;
                this.WinnerParty = c2.Candidate.Party;
                this.WinnerNameAndParty = c2.Candidate.NameAndParty;
                this.WinnerVotes = c2.V;
                this.WinnerElectors = c2.E;
                this.WinnerPercentage = c2.VotesPerStatePercentage;

                this.RunnerUp = c1.Candidate;
                this.RunnerUpName = c1.Candidate.Name;
                this.RunnerUpNameAndParty = c1.Candidate.NameAndParty;
                this.RunnerUpParty = c1.Candidate.Party;
                this.RunnerUpVotes = c1.V;
                this.RunnerUpElectors = c1.E;
                this.RunnerUpPercentage = c1.VotesPerStatePercentage;
            }

            TotalVotes = 0;
            TotalElectors = 0;

            foreach (var c in candidates)
            {
                TotalElectors += c.E;
                TotalVotes += c.V;
            }
        }

        public double TotalVotes { get; set; }
        public double TotalElectors { get; set; }
        //public double TotalVotes { get { return WinnerVotes; } }
        //public double TotalElectors { get { return WinnerElectors; } }

        public bool HasPartialWinner { get; set; }
        public Candidate Winner { get; set; }
        public string WinnerName { get; set; }
        public string WinnerParty { get; set; }
        public string WinnerNameAndParty { get; set; }
        public double WinnerVotes { get; set; }
        public double WinnerElectors { get; set; }
        public double WinnerPercentage { get; set; }

        public Candidate RunnerUp { get; set; }
        public string RunnerUpName { get; set; }
        public string RunnerUpParty { get; set; }
        public string RunnerUpNameAndParty { get; set; }
        public double RunnerUpVotes { get; set; }
        public double RunnerUpElectors { get; set; }
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
        public int C1PopularVotes { get; set; }
        public int C1ElectoralVotes { get; set; }

        public string C2Name { get; set; }
        public int C2PopularVotes { get; set; }
        public int C2ElectoralVotes { get; set; }

        public string OtherName { get; set; }
        public int OtherPopularVotes { get; set; }
        public int OtherElectoralVotes { get; set; }

        public string C3Name { get; set; }
        public int C3PopularVotes { get; set; }
        public int C3ElectoralVotes { get; set; }

        public string C4Name { get; set; }
        public int C4PopularVotes { get; set; }
        public int C4ElectoralVotes { get; set; }

        public string C5Name { get; set; }
        public int C5PopularVotes { get; set; }
        public int C5ElectoralVotes { get; set; }

        public string C6Name { get; set; }
        public int C6PopularVotes { get; set; }
        public int C6ElectoralVotes { get; set; }
    }

}

