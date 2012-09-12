function build_li(item) {
	var li = $('<li/>')
				.data('type', item.Type)
				.data('path', item.PathRelativeToRepositoryRoot)
				.append($('<span/>').addClass(item.Type).text(item.Name));
	// append data
	if (item.Data) {
		for (var prop in item.Data) {
			li.data(prop, item.Data[prop]);
		}
	}
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