﻿using ebayBuzApi.DB;
using ebayBuzApi.Models;
using ebayBuzApi.Models.Car;
using ebayBuzApi.Models.Expenses;
using ebayBuzApi.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessExpensesController: Controller
    {
        private IEbayDB ebayDBRecords;
        public BusinessExpensesController(IEbayDB ebayDBRecords)
        {
            this.ebayDBRecords = ebayDBRecords;
        }

        [HttpGet]
        [Route("GetExpenseTotals")]
        public ActionResult<List<ExpenseTotals>> GetAllExpenseTotals()
        {
            return ebayDBRecords.GetAllExpensesTotals();
        }

        [HttpPost]
        [Route("AddExpense")]
        public bool AddExpense(BusinessExpenses expense)
        {
            return ebayDBRecords.AddExpense(expense);
        }

        [HttpPost]
        [Route("AddCarLog")]
        public bool AddCarRecord(CarRecords carRecord)
        {
            return ebayDBRecords.AddCarRecord(carRecord);
        }

        [HttpGet]
        [Route("YearlyCarLogs")]
        public ActionResult<List<YearlyCarLogs>> GetCarLogs()
        {
            return ebayDBRecords.GetCarLogs();
        }

        [HttpPut]
        [Route("UpdateCarLogTotalMiles")]
        public bool UpdateCarLogTotalMiles(List<YearlyCarLogs> log)
        {
            return ebayDBRecords.UpdateCarLogTotalMiles(log.First());
        }

        [HttpPost]
        [Route("GetQuarterlyReporting")]
        public bool GetQuarterlyReporting(QuarterReportingForm qForm)
        {
            return ebayDBRecords.GetQuarterlyReporting(qForm);
        }

    }
}
