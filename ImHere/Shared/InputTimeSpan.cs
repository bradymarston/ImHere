using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Shared
{
    public class InputTimeSpan : InputBase<TimeSpan>
    {
        protected override bool TryParseValueFromString(string value, out TimeSpan result, out string validationErrorMessage)
        {
            double numericValue = 0;

            if (!double.TryParse(value, out numericValue))
            {
                validationErrorMessage = "Requires a numeric value in hours.";
                result = TimeSpan.FromSeconds(0);
                return false;
            }

            validationErrorMessage = "";
            result = TimeSpan.FromHours(numericValue);

            return true;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "type", "number");
            builder.AddAttribute(3, "class", CssClass);
            builder.AddAttribute(4, "value", BindConverter.FormatValue(CurrentValueAsString));
            builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
            builder.CloseElement();
        }

        protected override string FormatValueAsString(TimeSpan value)
        {
            return value.TotalHours.ToString();
        }
    }
}
