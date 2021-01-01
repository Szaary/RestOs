using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsDataManager.Library.Models
{
    public class TaskModel
    {
        public List<TaskDetailModel> SaleTypeDetails = new List<TaskDetailModel>();
        public string CashierId { get; set; }
    }
}
