// Define javascript objects here, datasource is initialized in ../Views/Shared/_KendoDataSources.cshtml
var KendoDataSource_Functions;
var KendoDataSource_SubFunctions;
var KendoDataSource_SubFunctionsByFunction;


function KendoGridFilterDatePicker(element) {
    element.kendoDatePicker({
        format: _DefaultDateFormat
    });
}

function KendoGridFilterAutoComplete(element, kendoDataSource, textField) {
    // console.log(JSON.stringify(element));
    // console.log(textField);

    element.kendoAutoComplete({
        minLength: 3,
        filter: "startswith",
        dataSource: kendoDataSource,
        dataTextField: textField
    });
}

function KendoGridFilterDropDownList_Function(element, valueField, textField, optionLabel) {
    KendoGridFilterDropDownList(element, KendoDataSource_Functions, valueField, textField, optionLabel);
}

function KendoGridFilterDropDownList_SubFunction(element, valueField, textField, optionLabel) {
    KendoGridFilterDropDownList(element, KendoDataSource_SubFunctions, valueField, textField, optionLabel);
}

function KendoGridFilterDropDownList(element, kendoDataSource, valueField, textField, optionLabel) {
    if (!valueField) valueField = "Id";
    if (!textField) textField = "Value";
    if (!optionLabel) optionLabel = "Select ...";

    element.kendoDropDownList({
        dataSource: kendoDataSource,
        dataValueField: valueField,
        dataTextField: textField,
        optionLabel: optionLabel
    });
}

// kendoDataSource = kendo.data.DataSource
// filter = kendo filter
function KendoGrid_FixFilter(kendoDataSource, kendoFilter, depth) {
    if ((!kendoDataSource) || (!kendoFilter)) return;

    if (!depth) depth = 0;
    // console.log(depth + " - FixDatesInFilter:" + JSON.stringify(kendoFilter));

    $.each(kendoFilter.filters, function (idx, filter) {
        //console.log("filter = " + idx + " = " + JSON.stringify(filter));

        if (filter.hasOwnProperty("filters")) {
            depth++;
            KendoGrid_FixFilter(kendoDataSource, filter, depth);
        }
        else {
            $.each(kendoDataSource.schema.model.fields, function (propertyName, propertyValue) {
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

function DisplayNoResultsFound(grid) {
    // Get the number of Columns in the grid
    var dataSource = grid.data("kendoGrid").dataSource;
    var colCount = grid.find('.k-grid-header colgroup > col').length;

    // If there are no results place an indicator row
    if (dataSource._view.length == 0) {
        grid.find('.k-grid-content tbody')
            .append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align:center"><b>No Results Found!</b></td></tr>');
    }

    // Get visible row count
    var rowCount = grid.find('.k-grid-content tbody tr').length;

    // If the row count is less that the page size add in the number of missing rows
    if (rowCount < dataSource._take) {
        var addRows = dataSource._take - rowCount;
        for (var i = 0; i < addRows; i++) {
            grid.find('.k-grid-content tbody').append('<tr class="kendo-data-row"><td>&nbsp;</td></tr>');
        }
    }
}