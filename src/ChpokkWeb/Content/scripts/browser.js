function build_li(item, data) {
	var li = $('<li/>')
				.data('type', item.Type)
				.data('path', item.PathRelativeToRepositoryRoot)
				.append($('<span/>').addClass(item.Type).text(item.Name));
	// append data
	if (data || item.Data) {
		data = data || {};
		$.extend(data, item.Data);
		for (var prop in data) {
			li.data(prop, data[prop]);
		}
	}
	if (item.Type === 'folder') {
		li.append(build_ul(item.Children, data));
	}
	return li;
}

function build_ul(items, data) {
	var ul = $('<ul/>');
	$.each(items, function () {
		ul.append(build_li(this, data));
	});
	return ul;
}

function initTreeView(selector) {
	$('li[data-type="file"]').click(function () {
		window.location.hash = $(this).attr('data-path');
	});
	$(selector).treeview({ collapsed: true });
}