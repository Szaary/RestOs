using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsWPFFlowManager.Models
{
    public class ProductTypeDisplayModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string ProductType { get; set; }




        /// <summary>
        /// /////////////////////////////////
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
