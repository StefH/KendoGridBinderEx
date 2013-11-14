var KendoDataSource_Functions = {
    transport: {
        read: {
            url: '@Url.Action("GetFunctionsAsJson", "Function")',
            dataType: "json"
        }
    }
};

var KendoDataSource_SubFunctions = {
    transport: {
        read: {
            url: '@Url.Action("GetSubFunctionsAsJson", "SubFunction")',
            dataType: "json"
        }
    }
};


function KendoGridFilter_DatePicker(element) {
    element.kendoDatePicker({
        format: _DefaultDateFormat
    });
}

function KendoGridFilter_Function(element) {
    element.kendoDropDownList({
        dataSource: KendoDataSource_Functions,
        dataTextField: 'Code',
        dataValueField: 'Code',
        optionLabel: 'Select ...'
    });
}

function KendoGridFilter_SubFunction(element) {
    element.kendoDropDownList({
        dataSource: KendoDataSource_SubFunctions,
        dataTextField: 'Code',
        dataValueField: 'Code',
        optionLabel: 'Select ...'
    });
}


// kendoDS = kendo.data.DataSource
// filter = kendo filter
function KendoGrid_FixFilter(kendoDS, kendoFilter, depth) {
    if ((!kendoDS) || (!kendoFilter)) return;

    if (!depth) depth = 0;
    // console.log(depth + " - FixDatesInFilter:" + JSON.stringify(kendoFilter));

    $.each(kendoFilter.filters, function (idx, filter) {
        //console.log("filter = " + idx + " = " + JSON.stringify(filter));

        if (filter.hasOwnProperty("filters")) {
            depth++;
            KendoGrid_FixFilter(kendoDS, filter, depth)
        }
        else {
            $.each(kendoDS.schema.model.fields, function (propertyName, propertyValue) {
                if (filter.field == propertyName && propertyValue.type == 'date') {
                    filter.value = kendo.toString(filter.value, _DefaultDateFormat); // "MM/dd/yyyy"
                    // console.log("changed = " + filter.value);
                }
            });
        }
    });
}

function FixDatesInGroup(ds, group) {
    // console.log("FixDatesInGroup:" + JSON.stringify(group));
}