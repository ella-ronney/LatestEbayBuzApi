using ebayBuzApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ebayBuzApi.Helpers
{
    public static class DBHelper
    {
        public static bool NullCheckForIdListIds(List<string> idList)
        {
            if (idList == null || idList.Count() < 1)
            {
                return false;
            }
            return true;
        }

        public static bool ConvertStringToPosInt(string value, ref int itemId)
        {
            return int.TryParse(value, out itemId);
        }
    }
}
