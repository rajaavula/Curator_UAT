function GetRowID()
{
	try { return GrdMain.GetRowKey(GrdMain.GetFocusedRowIndex()); }
	catch (ex) { return null; }
}
