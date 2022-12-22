
function RefreshGridWithArgs(grid) {

    var args = FormatDate(FromDate.GetDate()) + '~' + FormatDate(ToDate.GetDate());

    grid.PerformCallback(args);
}
