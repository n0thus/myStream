function setOnline() {
	$(".status").replaceWith('<li class="status"><img src="../img/status_online.png"  draggable="false"><span class="statusSpan">Online</span></li>')
	$(".viewers").show();
	$(".time").show();
	$(".newFollowers").show();
	$(".newSubscribers").show();
}

function setOffline() {
	$(".status").replaceWith('<li class="status"><img src="../img/status_offline.png"  draggable="false"><span class="statusSpan">Offline</span></li>')
	$(".viewers").hide();
	$(".time").hide();
	$(".newFollowers").hide();
	$(".newSubscribers").hide();
}


function updateElements(viewers, time, nFollowers, nSubscribers) {
	$(".viewersSpan").replaceWith("<span class='viewersSpan'>"+viewers+"<span>");


	$(".timeSpan").replaceWith('<span class="timeSpan">'+time+'</span>');

	$(".newFollowersSpan").replaceWith("<span class='newFollowersSpan'>"+nFollowers+"<span>");

	$(".newSubscribersSpan").replaceWith("<span class='newSubscribersSpan'>"+nSubscribers+"<span>");
}



function addCommas(nStr)
{
	nStr += '';
	x = nStr.split(',');
	x1 = x[0];
	x2 = x.length > 1 ? ',' + x[1] : '';
	var rgx = /(\d+)(\d{3})/;
	while (rgx.test(x1)) {
		x1 = x1.replace(rgx, '$1' + '.' + '$2');
	}
	return x1 + x2;
}

$(document).ready(() => {
	setOffline();
});