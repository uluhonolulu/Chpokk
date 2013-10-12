$(function () {
	var tour = new Tour({
		debug: false,
		name: 'main',
		backdrop: false,
		template: "<div class='popover tour'>          <div class='arrow'></div>          <h3 class='popover-title'></h3>          <div class='popover-content'><span/></div>          <nav class='popover-navigation'>            <div class='btn-group'>              <button class='btn btn-default' data-role='prev'>&laquo; Prev</button>              <button class='btn btn-default' data-role='next'>Next &raquo;</button>            </div>            <button class='btn btn-default' data-role='end'>End tour</button>          </nav>        </div>"
	});
	tour.addSteps([
				{
					element: '#dashDialog',
					title: "Get started",
					content: "Chpokk needs your .Net code to work with. You should upload your projects, including the *.sln and *.csproj/*.vbproj files, and referenced assemblies. Several options are supported.",
					placement: 'top'
				},
				{
					element: "#simpleProjectDialogOpener",
					title: "Create a simple project",
					content: "Create a console exe or library project using this button.",
					placement: 'right'
				},
				{
					element: "#cloneDialogOpener",
					title: "Clone a Git repository",
					content: "If your code is stored in a Git repository, clone it using this button.",
					placement: 'left'
				},
				{
					element: "#zipUploadInitiator",
					title: "Upload a zipped project",
					content: "If your code is stored on your device, zip it and upload it using this button.",
					placement: 'right'
				},
				{
					element: '#repositoryList',
					//title: "Get started",
					content: "If you have uploaded your code previously, you'll find it in this list."
				}
			]);
	//tour.start(); //let's start it on dashboard popup
})