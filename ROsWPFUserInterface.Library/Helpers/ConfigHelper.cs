﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // TODO MOVE THIS FROM CONFIG TO THE API
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = decimal.TryParse(rateText, out decimal output);

            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }
            return output;
        }
    }
}
