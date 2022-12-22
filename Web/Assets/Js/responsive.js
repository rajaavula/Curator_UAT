function OnSubmit() {
	ShowLoadingPanel();
}

function Submit(msg) {
	if (document.forms.length < 1) return;
	ShowLoadingPanel(msg); document.forms[0].submit();
}

function InitialiseSelectBoxes() {
	$('.ctx-dropdown').select2();
}

function InitialiseKeepAlive() {
	setInterval(function () {
		$.ajax({
			cache: false,
			url: '/Home/Home/KeepAlive',
			data: { id: ModelID },
			success: function (json) {
				if (json && json.OK) return;

				window.location = '/Home';
			}
		});
	}, 60000); /* 5 minutes */
}

/* General utilities */
function GetUnique() {
	const date = new Date();

	const year = date.getFullYear().toString();
	const month = date.getMonth().toString();
	const day = date.getDay().toString();
	const hour = date.getHours().toString();
	const minute = date.getMinutes().toString();
	const second = date.getSeconds().toString();
	const millisecond = date.getMilliseconds();

	return `${year}${month}${day}${hour}${minute}${second}${millisecond}`;
}

function ShowLoadingPanel(message) {
	message = message ? message : 'Loading...';
	$('#loading-message').text(message);
	$('#loading-overlay').removeClass('d-none').addClass('d-flex');
}

function HideLoadingPanel() {
	$('#loading-overlay').addClass('d-none').removeClass('d-flex');
}

/* Events */
$('form').submit(OnSubmit);

/* Initialise */
InitialiseSelectBoxes();

InitialiseKeepAlive();
