var avl, as;

$(function () {

	avl = $('#available'); as = $('#assigned');

	avl.on('click', 'a', function (e) {
		e.preventDefault();
		var t = $(e.currentTarget), p = t.closest('.assign');
		p.removeClass('phover').find('a').text('Remove');
		as.append(p);
		SetAssignedOnes();
	});

	as.on('click', 'a', function (e) {
		e.preventDefault();
		var t = $(e.currentTarget), p = t.closest('.assign');
		p.removeClass('phover').find('a').text('Add');
		avl.append(p);
		SetAssignedOnes();
	});

	$('.assign').hover(function (e) { $(e.currentTarget).addClass('phover'); }, function (e) { $(e.currentTarget).removeClass('phover'); });

});

function Save()
{
	$('#assign-issave').val(true);
	Submit();
}

function SetAssignedOnes()
{
	var il = '', pl = as.find('.assign');
	for (var i = 0; i < pl.length; i++) { il += ((i == 0 ? '' : ',') + pl[i].id); }
	$('#assign-userobjectids').val(il);
}

function SubmitForm()
{
	ShowLoadingPanel();
	Submit();
}

function ChangeGroup()
{
    ShowLoadingPanel();

    setTimeout(function ()
    {
        Submit();
    }, 800);
}

function ChangeMemberStore()
{

}