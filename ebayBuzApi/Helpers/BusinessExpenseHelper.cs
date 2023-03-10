using ebayBuzApi.Models;
using ebayBuzApi.Models.Expenses;
using ebayBuzApi.Models.FormModels;
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

        public enum DropOffLocation { 
            RedmondUSPS,
            BellevueUSPS,
        }

        public static float GetDropOffLocationDistanceTraveled(string location, DateTime startDate, DateTime? endDate, bool satDropOff)
        {
            if (String.IsNullOrEmpty(location) || startDate == null)
                return 0f;

            int days = 1;
            if (endDate != null)
            {
                days = GetDropOffDays(startDate, (DateTime)endDate, satDropOff);
            }

            location = location.Replace(" ", "");
            DropOffLocation loc;
            if(Enum.TryParse(location, out loc))
            {
                switch (loc)
                {
                    case DropOffLocation.RedmondUSPS:
                        return days*3.2f;
                    case DropOffLocation.BellevueUSPS:
                        return days*11.6f;
                    default:
                        return 0.0f;
                }
            }
            return 0.0f;
        }

        public static CarLogRecords MapCarRecordsToCarLogRecords(CarRecords record)
        {
            return new CarLogRecords
            {
                car = record.car,
                destination = record.destination,
                distanceTraveled = record.distanceTraveled,
                endDate = record.endDate,
                purpose = record.purpose,
                startDate = record.startDate
            };
        }

        private static int GetDropOffDays(DateTime startDate, DateTime endDate, bool satDropOff)
        {
            int days = (int)endDate.Subtract(startDate).TotalDays;
            if (satDropOff)
                return Enumerable.Range(0, days + 1).Select(x => startDate.AddDays(x)).Count(x => x.DayOfWeek != DayOfWeek.Sunday);
            else
                return Enumerable.Range(0, days + 1).Select(x => startDate.AddDays(x)).Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);

        }

    }
}
