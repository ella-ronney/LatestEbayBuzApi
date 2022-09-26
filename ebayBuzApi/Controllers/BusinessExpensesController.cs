using ebayBuzApi.DB;
using ebayBuzApi.Models.Expenses;
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


    }
}
