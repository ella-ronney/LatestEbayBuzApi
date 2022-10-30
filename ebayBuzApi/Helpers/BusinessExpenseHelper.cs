using ebayBuzApi.Models.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Helpers
{
    public static class BusinessExpenseHelper
    {
        public static List<ExpenseTotals>  SetExpenseTotals( List<BusinessExpenses> expenses)
        {
            ExpenseTotals exTotals = new ExpenseTotals();
            foreach(BusinessExpenses expense in expenses)
            {
                switch (expense.expenseCategory)
                {
                    case "Supplies":
                        exTotals.supplies += expense.cost;
                        break;
                    case "Subscriptions":
                        exTotals.subscriptions += expense.cost;
                        break;
                    case "Office Space (Rent & Insurance)":
                        exTotals.officeSpace += expense.cost;
                        break;
                    case "Utilities":
                        exTotals.utilities += expense.cost;
                        break;
                    case "Food":
                        exTotals.food += expense.cost;
                        break;
                    case "Gas":
                        exTotals.gas += expense.cost;
                        break;
                    case "Car Expenses (Insurance & Maintance)":
                        exTotals.car += expense.cost;
                        break;
                    case "Misc Business Expenses":
                        exTotals.miscExpenses += expense.cost;
                        break;
                    case "Travel":
                        exTotals.travel += expense.cost;
                        break;
                    case "Inventory":
                        exTotals.inventory += expense.cost;
                        break;
                    default:
                        Console.WriteLine("error");
                        break;
                }
            }
            List<ExpenseTotals> exTotalsList = new List<ExpenseTotals>() { exTotals };
            return exTotalsList;
        }
    }
}
