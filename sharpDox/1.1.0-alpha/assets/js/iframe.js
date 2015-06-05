var allExpanded = false;

$(window).unload(function () {
    showLoader();
});

$(document).ready(function () {
    $('body').layout({
        defaults: {
            paneClass: "ui-inner",
            closable: false,
            resizable: false
        }
    });

	$('a').filter(function() {
	   return this.hostname && this.hostname !== location.hostname;
	}).attr("target","_blank");
	
    $('.dropdown-menu a').click(function () {
        $($(this).attr('href')).effect("highlight", {}, 3000);
        window.location.hash = $(this).attr('href');
        slideDown($(window.location.hash).children()[0]);
        postUrl();
    });

    $('#sd-class-diagram a, #sd-sequence-diagram a').click(function () {
        window.location = $(this).attr('xlink:href');
        $(window.location.hash).effect("highlight", {}, 3000);
        slideDown($(window.location.hash).children()[0]);
        postUrl();
        return false;
    });

    $('.member-header').click(function () {
		
		//prevent scrolling
        var id = $(this).parent().attr('id');
        $(this).parent().attr('id', id + "-tmp");
		
		//if not slided down
		var icon = $($($(this).children()[0]).children()[0]);
		if (icon.hasClass('icon-caret-right')) {      
			window.location = "#" + id;
			postUrl();
		}
	
        toggleSlide(this);
		
        //set to original id
        $(this).parent().attr('id', id);
    });

    $('#expand-all-button').click(function () {
        if (allExpanded) {
            allExpanded = false;
            $('.member-header').each(function (index) {
                slideUp(this);
            });
        }
        else {
            allExpanded = true;
            $('.member-header').each(function (index) {
                slideDown(this);
            });
        }
    });

    $('#print-button').click(function () {
        $('.ui-inner-center').print();
    });
	
    postUrl();
	hideLoader();
	highlightMember();
});

function slideUp(header) {
    $(header).next().slideUp();
    var icon = $($($(header).children()[0]).children()[0]);
    icon.removeClass('icon-caret-down');
    icon.addClass('icon-caret-right');
}

function slideDown(header) {
    $(header).next().slideDown();
    var icon = $($($(header).children()[0]).children()[0]);
    icon.removeClass('icon-caret-right');
    icon.addClass('icon-caret-down');
}

function toggleSlide(header) {
    var body = $(header).next();
    body.slideToggle();

    var icon = $($($(header).children()[0]).children()[0]);
    if (icon.hasClass('icon-caret-right')) {
        icon.removeClass('icon-caret-right');
        icon.addClass('icon-caret-down');
    }
    else {
        icon.removeClass('icon-caret-down');
        icon.addClass('icon-caret-right');
    }
}

function highlightMember(){
	if (window.location.hash != "") {
		$(window.location.hash).effect("highlight", {}, 3000); 
		
		slideDown($(window.location.hash).children()[0]);
	}
}

function postUrl() {
	var splittedUrl = window.location.toString().split('/');
	var site = splittedUrl[splittedUrl.length - 2] + "/" + splittedUrl[splittedUrl.length - 1];
	var splittedSite = site.split('#');
	var siteUrl = '#' + splittedSite[0].substring(0, splittedSite[0].length - 5);

	if (splittedSite.length == 2) {
		siteUrl += '?' + splittedSite[1];
	}
	
	if(siteUrl != "#article/home"){
		parent.postMessage(siteUrl, '*');
	}
}

function showLoader() {
    parent.postMessage('showLoader', '*');
}

function hideLoader() {
    parent.postMessage('hideLoader', '*');
}