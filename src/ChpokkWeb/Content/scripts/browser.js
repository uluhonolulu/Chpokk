function build_li(item, data) {
	if (data || item.Data) {
		data = data || {};
		$.extend(data, item.Data);
	}
	var itemContainer = build_item(item, data);
	var li = $('<li/>')
				.attr('data-type', item.Type)
				.attr('data-path', item.PathRelativeToRepositoryRoot)
				.append(itemContainer);
	// handle folder path for solutions and projects
	// update: shouldn't do this -- bad for renaming
	//if (item.Data && item.Data.Folder)
	//	li.attr('data-path', item.Data.Folder);
	// append data
	if (item.Type === 'folder') {
		li.append(build_ul(item.Children, data));
	}
	return li;
}

function build_item(item, data) {
	var fileExtension = item.PathRelativeToRepositoryRoot.split('.').pop();
	// itemContainer is the tag surrounding the item's name
	var itemContainer = $('<span/>').addClass(item.Type).addClass(fileExtension).text(item.Name);
	//append all data
	if (data) {
		for (var prop in data) {
			itemContainer.data(prop, data[prop]);
		}	
	}
	// on focus, set it editable and track the old value; 
	itemContainer.dblclick(function () {
		$(this).attr('contentEditable', true).prop('oldValue', $(this).text());
	});
	// on blur, set it not editable and send the rename command to server
	itemContainer.blur(function () {
		$(this).attr('contentEditable', false);
		if ($(this).prop('oldValue') != $(this).text()) {
			rename($(this));
		}
	});
	// on Enter, let's blur
	itemContainer.keyup(function (e) {
		if (e.which == 13) {
			e.preventDefault();
			$(this).blur();
		}
	});
	itemContainer.keydown(function (e) { //at least in Chrome we need to block Enter here so that we don't break the line
		if (e.which == 13) {
			e.preventDefault();
		}
	});
	return itemContainer;
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

function rename(itemContainer) {
	var url = 'url::ChpokkWeb.Features.Exploring.Rename.RenameInputModel';
	var itemData = itemContainer.data();
	var data = $.extend({}, model, { PathRelativeToRepositoryRoot: itemContainer.parent().data('path'), NewFileName: itemContainer.text() + itemContainer.data("KeepExtension") });
	$.post(url, data, loadSolutionExplorer);
	//use 
}