class PercentageCalculator { //extends SummaryCalculator {

    constructor(propertyName) {
        this.propertyName = propertyName;
    }

    get displayName() {
        return "USA";
    }
    counter; //: number;
    propertyName; //: number;

    beginCalculation(dataSource, b) {
        //super.beginCalculation(dataSource, b);
        this.counter = 0;
        //console.log("beginCalculation " + this.counter);
    }

    endCalculation() { //: : ISummaryResult {

        //console.log("endCalculation " + this.propertyName + "=" + this.counter);
        var summary = {
            summaryIndex: 1,
            value: this.counter,  
            operand: 5, // Custom
            propertyName: "VotesPerStatePercentage" 
            //propertyName: this.propertyName, //"VotesTotalPercentage"
        };
        return summary; 
    }

    aggregate(dataItem) { 
        //this.counter += dataItem.V;
        if (dataItem[this.propertyName] !== undefined) {

            this.counter += dataItem[this.propertyName] //.VotesTotalPercentage;
        }
    }
}

function onCalculateVotesTotalPercentage(grid, args) {
    //console.log("onCalculateVotesTotalPercentage");
    args.calculator = new PercentageCalculator("VotesTotalPercentage");
}
igRegisterScript("onCalculateVotesTotalPercentage", onCalculateVotesTotalPercentage, false);

//function onCalculateElectorsTotalPercentage(grid, args) {
//    console.log("onCalculateTotalPercentage");
//    args.calculator = new PercentageCalculator("ElectorsTotalPercentage");
//}
//igRegisterScript("onCalculateElectorsTotalPercentage", onCalculateElectorsTotalPercentage, false);


class StateCalculator { //extends SummaryCalculator {
    get displayName() { return "State"; }
    counter; //: number;

    beginCalculation(dataSource, b) {
        //super.beginCalculation(dataSource, b);
        this.counter = 0;
        //console.log("beginCalculation " + this.counter);
    }

    endCalculation() { //: : ISummaryResult {
        //console.log("endCalculation " + this.counter);
        var summary = {
            summaryIndex: 1,
            value: this.counter, // / 136669237,
            operand: 5, // Custom
            propertyName: "State" //
        };
        return summary;
    }

    aggregate(dataItem) {
        if (dataItem.V > 0) {
            this.counter++;
        }
    }
}

function onCalculateState(grid, args) {
    //console.log("onCalculateState");
    args.calculator = new StateCalculator();
}

igRegisterScript("onCalculateState", onCalculateState, false);

class PartyCalculator { //extends SummaryCalculator {
    get displayName() { return "CandidateParty"; }
   
    partyName;

    beginCalculation(dataSource, b) {
        //super.beginCalculation(dataSource, b);
        this.partyName = "";
        //console.log("beginCalculation ");
    }

    endCalculation() { //: : ISummaryResult {
        //console.log("endCalculation " + this.partyName);
        var summary = {
            summaryIndex: 1,
            value: this.partyName, 
            operand: 5, // Custom
            propertyName: "CandidateParty" //
        };
        return summary;
    }

    aggregate(dataItem) {
        if (this.partyName == "" && dataItem.CandidateParty) {
            this.partyName = dataItem.CandidateParty;
        }
    }
}

function onCalculateParty(grid, args) {
    //console.log("onCalculateState");
    args.calculator = new PartyCalculator();
}

igRegisterScript("onCalculateParty", onCalculateParty, false);

