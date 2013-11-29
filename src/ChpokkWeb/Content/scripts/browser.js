function build_li(item, data) {
	var li = $('<li/>')
				.attr('data-type', item.Type)
				.attr('data-path', item.PathRelativeToRepositoryRoot)
				.append($('<span/>').addClass(item.Type).text(item.Name));
	// handle folder path for solutions and projects
	if (item.Data && item.Data.Folder)
		li.attr('data-path', item.Data.Folder);
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
    openItemIfSingle($(selector));
}

function openItemIfSingle(tree) {
    var fileElements = tree.find('[data-type="file"]');
    if (fileElements.length == 1) {
        fileElements.click();
    }
}

function newItem(path) { //deprecated: we now reload all items
	var pathParts = path.split('\\');
	var fileName = pathParts[pathParts.length - 1];
	var folderName = path.substr(0, path.length - fileName.length - 1);
	var folderNameEscaped = folderName.replace(/\\/g, '\\\\');
	var selector = 'li[data-path="{folderName}"] > ul'.replace('{folderName}', folderNameEscaped);
	var ul = $(selector);
	var item = { "Name": fileName, "PathRelativeToRepositoryRoot": path, "Type": "file" };
	ul.append(build_li(item));
}

//amplify.subscribe('newItem', newItem);//deprecated: we now reload all items