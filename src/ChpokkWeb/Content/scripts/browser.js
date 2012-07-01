function build_li(item) {
	var li = $('<li/>')
				.attr('data-type', item.Type)
				.attr('data-path', item.PathRelativeToRepositoryRoot)
				.append($('<span/>').addClass(item.Type).text(item.Name));
	if (item.Type === 'folder') {
		li.append(build_ul(item.Children));
	}
	return li;
}

function build_ul(items) {
	var ul = $('<ul/>');
	$.each(items, function () {
		ul.append(build_li(this));
	});
	return ul;
}

function initTreeView(selector) {
	$('li[data-type="file"]').click(function () {
		window.location.hash = $(this).attr('data-path');
	});
	$(selector).treeview({ collapsed: true });
}