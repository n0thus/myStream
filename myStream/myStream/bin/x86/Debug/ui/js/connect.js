function waiting() {
	$('#form').hide();
	$("body").append("<i class='fa fa-cog fa-spin'></i>");
	$("body").append("<i class='desc'></i>")
}

function renderUsername() {
	$(".fa-cog").replaceWith("");
	$(".desc").replaceWith("");
	$('#form').show();
}


function setDescText(text) {
	$(".desc").replaceWith("<i class='desc'>"+text+"</i>")
}

/*$(document).ready(()=>{
	waiting();
	setDescText("Test en cours...")
});*/