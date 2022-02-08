function clearFilters() {
    gridOptions.api.setFilterModel(null);
}

function dateFormatter(params) {
    if (params.value != null) return moment(params.value).format('MMMM D, YYYY');
    else return params.value;
}
//search:
function onFilterTextBoxChanged() {
    gridOptions.api.setQuickFilter(document.getElementById('filter-text-box').value);
}

function editColumns(arrayColumns) {
    arrayColumns.forEach(function (item, index, array) {
        this.gridOptions.columnApi.getColumn(item).getColDef().editable = true;
    })
}

function stopEditColumns(arrayColumns) {
    arrayColumns.forEach(function (item, index, array) {
        this.gridOptions.columnApi.getColumn(item).getColDef().editable = false;
    })
}

function exportCSV(domainName) {
    var columnWidth = 100;
    var params = {
        columnWidth: parseFloat(columnWidth),
        sheetName: domainName,
        fileName: domainName,
        exportMode: "xml",
        suppressTextAsCDATA: true,
        rowHeight: 30,
        headerRowHeight: 40,
    };
    gridOptions.api.exportDataAsCsv(params);
}