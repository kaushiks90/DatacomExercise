using DatacomConsole.Implementation;
using DatacomConsole.Interface;
using DatacomConsole.Models.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DatacomConsoleTest
{
    public class UnitTestValidationClass
    {
        IValidation val = null;
        private readonly Mock<ILogger<Validation>> _logger;
        public UnitTestValidationClass()
        {
            _logger = new Mock<ILogger<Validation>>();
            val = new Validation(_logger.Object);
        }

        [Fact]
        public void GetComapany_Should_Return_Null()
        {
            Input inputModel = new Input();
            inputModel.CompanyCode = "Airbus India";

            List<Company> companies = new List<Company>();
            companies.Add(new Company() { Code = "Datacom", Id = "DatacomX123", Name = "Datacom" });

            var result = val.GetCompany(inputModel, companies);
            Assert.Null(result.Result);

        }

        [Fact]
        public void GetComapany_Should_Return_Expected_CompanyName_Result()
        {
            Input inputModel = new Input();
            inputModel.CompanyCode = "Datacom";

            List<Company> companies = new List<Company>();
            companies.Add(new Company() { Code = "Datacom", Id = "DatacomX123", Name = "Datacom" });

            Company expectedResult = new Company() { Code = "Datacom", Id = "DatacomX123", Name = "Datacom" };

            var result = val.GetCompany(inputModel, companies);
            Assert.Equal(expectedResult.Name,result.Result.Name);

        }

        [Fact]
        public void GetPayGroups_Should_Return_Count_Zero()
        {

            Company company = new Company() { Code = "Datacom", Id = "DatacomX123", Name = "Datacom" };

            List<Paygroup> payGroups = new List<Paygroup>();
            payGroups.Add(new Paygroup() { Id = "PayGroupId1", CompanyId = "Airbus", Code = "PayGroupCode", Name = "PayGroupName" });

            var result = val.GetPaygroups(company, payGroups);
            Assert.Equal(result.Result.Count,0);

        }

        [Fact]
        public void GetPayGroups_Return_Expected_PayGroupName_Result()
        {

            Company company = new Company() { Code = "Datacom", Id = "DatacomX123", Name = "Datacom" };

            List<Paygroup> payGroups = new List<Paygroup>();
            payGroups.Add(new Paygroup() { Id = "PayGroupId1", CompanyId = "DatacomX123", Code = "PayGroupCode", Name = "PayGroupName" });

            var result = val.GetPaygroups(company, payGroups);

            Assert.Equal(payGroups[0].Name, result.Result[0].Name);

        }

        [Fact]
        public void GetPayRuns_Should_Return_Count_Zero()
        {
            Input inputModel = new Input();
            inputModel.CompanyCode = "Datacom";


            List<Paygroup> payGroups = new List<Paygroup>();
            payGroups.Add(new Paygroup() { Id = "PayGroupId1", CompanyId = "Airbus", Code = "PayGroupCode", Name = "PayGroupName" });

            List<PayRun> payRuns = new List<PayRun>();
            payRuns.Add(new PayRun() { Id = 1, PayGroupId = "PayGroupId1", PayPeriod=new PayPeriod() { StartDate=DateTime.Now,EndDate=DateTime.Now} });


            var result = val.GetPayRuns(inputModel, payGroups, payRuns);
            Assert.Equal(result.Result.Count, 0);

        }

        [Fact]
        public void GetPayRuns_Should_Return_Count_With_Empty_Input_Zero()
        {
            Input inputModel = new Input();
            List<Paygroup> payGroups = new List<Paygroup>();
            List<PayRun> payRuns = new List<PayRun>();
            var result = val.GetPayRuns(inputModel, payGroups, payRuns);
            Assert.Equal(result.Result.Count, 0);

        }

        [Fact]
        public void GetPayRuns_Should_Return_Count_One()
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            Input inputModel = new Input();
            inputModel.CompanyCode = "Datacom";
            inputModel.PayPeriodStartDate = startTime;
            inputModel.PayPeriodEndDate = endTime;



            List<Paygroup> payGroups = new List<Paygroup>();
            payGroups.Add(new Paygroup() { Id = "PayGroupId1", CompanyId = "Airbus", Code = "PayGroupCode", Name = "PayGroupName" });

            List<PayRun> payRuns = new List<PayRun>();
            payRuns.Add(new PayRun() { Id = 1, PayGroupId = "PayGroupId1", PayPeriod = new PayPeriod() { StartDate = startTime, EndDate = endTime } });


            var result = val.GetPayRuns(inputModel, payGroups, payRuns);
            Assert.Equal(result.Result.Count, 1);

        }

        [Fact]
        public void GetTimesheets_Should_Return_Count_Zero()
        {
            List<Timesheet> timeSheets = new List<Timesheet>();
            timeSheets.Add(new Timesheet() { PayRunId=1,Type="Method"});

            List<PayRun> payRuns = new List<PayRun>();
            payRuns.Add(new PayRun() { Id = 1, PayGroupId = "PayGroupId1", PayPeriod = new PayPeriod() { StartDate = DateTime.Now, EndDate = DateTime.Now } });


            var result = val.GetTimesheets(timeSheets,payRuns);
            Assert.Equal(result.Result.Count, 0);

        }

        [Fact]
        public void GetTimesheets_Should_Return_Count_One()
        {

            List<Timesheet> timeSheets = new List<Timesheet>();
            timeSheets.Add(new Timesheet() { PayRunId = 1, Type = "API" });

            List<PayRun> payRuns = new List<PayRun>();
            payRuns.Add(new PayRun() { Id = 1, PayGroupId = "PayGroupId1", PayPeriod = new PayPeriod() { StartDate = DateTime.Now, EndDate = DateTime.Now } });


            var result = val.GetTimesheets(timeSheets, payRuns);
            Assert.Equal(result.Result.Count, 1);

        }

        [Fact]
        public void Generate_Output_Should_Return_Count_One()
        {
            DateTime startTime = DateTime.Now;
            List<Timesheet> timeSheets = new List<Timesheet>();
            timeSheets.Add(new Timesheet()
            {
                PayRunId = 1,
                Type = "Method",
                Values =
                new List<TimesheetValue>()
                { new TimesheetValue() { Value = 500,EmployeeId="1",StartDate=startTime }, new TimesheetValue() 
                { Value = 600,EmployeeId="1",StartDate=startTime } }
            });
            

            
            var result = val.GenerateOutputModel(startTime,timeSheets);
            Assert.Equal(result.Result.Count, 1);

        }

        [Fact]
        public void Generate_Output_Should_Return_Exact_SumValue()
        {
            DateTime startTime = DateTime.Now;
            List<Timesheet> timeSheets = new List<Timesheet>();
            timeSheets.Add(new Timesheet()
            {
                PayRunId = 1,
                Type = "Method",
                Values =
                new List<TimesheetValue>()
                { new TimesheetValue() { Value = 500,EmployeeId="1",StartDate=startTime }, new TimesheetValue()
                { Value = 600,EmployeeId="1",StartDate=startTime } }
            });



            var result = val.GenerateOutputModel(startTime, timeSheets);
            Assert.Equal(result.Result[0].SumValue, 1100);

        }
    }
}
