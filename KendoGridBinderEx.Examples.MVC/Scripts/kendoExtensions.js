// Define javascript objects here, datasource is initialized in ../Views/Shared/_KendoDataSources.cshtml
var KendoDataSource_Functions;
var KendoDataSource_SubFunctions;


function KendoGridFilter_DatePicker(element) {
    element.kendoDatePicker({
        format: _DefaultDateFormat
    });
}

function KendoGridFilter_Function(element, valueField, textField, optionLabel) {
    if (!textField) textField = "Id";
    if (!valueField) textField = "Value";
    if (!optionLabel) optionLabel = "Select ...";
    
    element.kendoDropDownList({
        dataSource: KendoDataSource_Functions,
        dataTextField: textField,
        dataValueField: valueField,
        optionLabel: optionLabel
    });
}

function KendoGridFilter_SubFunction(element, valueField, textField, optionLabel) {
    if (!textField) textField = "Id";
    if (!valueField) textField = "Value";
    if (!optionLabel) optionLabel = "Select ...";
    
    element.kendoDropDownList({
        dataSource: KendoDataSource_SubFunctions,
        dataTextField: textField,
        dataValueField: valueField,
        optionLabel: optionLabel
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