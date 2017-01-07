using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice
{
    public class DecimalModelBinder : DefaultModelBinder, IModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue, new CultureInfo("en"));
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }


            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }

    }
}