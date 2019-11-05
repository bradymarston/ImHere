using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Shared
{
    public class InputTime : InputDate<DateTime>
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)

        {
            builder.OpenElement(0, "input");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "type", "time");
            builder.AddAttribute(3, "class", CssClass);
            builder.AddAttribute(4, "value", BindConverter.FormatValue(CurrentValueAsString));
            builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
            builder.CloseElement();
        }

        protected override bool TryParseValueFromString(string value, out DateTime result, out string validationErrorMessage)
        {
            var success = BindConverter.TryConvertToDateTime(value, CultureInfo.InvariantCulture, "T", out var parsedValue);

            if (success)
            {
                result = parsedValue;
                validationErrorMessage = "";
                return true;
            }
            else
            {
                result = default;
                validationErrorMessage = $"{FieldIdentifier.FieldName} must be a time.";
                return false;
            }
        }

        protected override string FormatValueAsString(DateTime value)
        {
            return BindConverter.FormatValue(value, "HH:mm:ss", CultureInfo.InvariantCulture);
    }
    }
}
